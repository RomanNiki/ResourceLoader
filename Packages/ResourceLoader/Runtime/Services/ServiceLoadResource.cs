using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Handlers;
using Openmygame.LoggerPro;

namespace Services
{
    public class ServiceLoadResource : IServiceLoadResource
    {
        private readonly IServiceOwnerResource _serviceOwnerResource;
        private readonly List<IHandlerLoadResource> _handlers;
        private readonly IServiceCachedResources _serviceCachedResources;

        public ServiceLoadResource(List<IHandlerLoadResource> handlers, IServiceCachedResources serviceCachedResources)
        {
            _handlers = handlers;
            _serviceCachedResources = serviceCachedResources;
        }

        public ServiceLoadResource(IServiceOwnerResource serviceOwnerResource)
        {
            _serviceOwnerResource = serviceOwnerResource;
        }

        public ContainerLoadItem<T> Load<T>(string key, object owner, CancellationToken cancellationToken = default)
        {
            if (TryGetCachedItem(key, owner, out ContainerLoadItem<T> cachedContainerItem))
            {
                return cachedContainerItem;
            }

            foreach (var handlerLoadResource in _handlers)
            {
                if (!handlerLoadResource.CanHandle(key))
                {
                    continue;
                }

                UniTask<T> loadTask = handlerLoadResource.LoadResource<T>(key, cancellationToken);

                _serviceOwnerResource.AddOwner(key, owner);
                ContainerLoadItem<T> containerLoadItem = new ContainerLoadItem<T>(key, owner, _serviceOwnerResource);
                LoadInternal(containerLoadItem, loadTask, cancellationToken).Forget();

                return containerLoadItem;
            }

            ProLogger.Exception("Not found handler for type " + typeof(T).Name);
            return default;
        }

        private bool TryGetCachedItem<T>(string key, object owner, out ContainerLoadItem<T> load)
        {
            load = default;

            if (_serviceCachedResources.Has(key))
            {
                T item = _serviceCachedResources.Get<T>(key);
                _serviceOwnerResource.AddOwner(key, owner);
                ContainerLoadItem<T> containerLoadItem = new ContainerLoadItem<T>(key, owner, _serviceOwnerResource);
                containerLoadItem.CompleteLoading(item);
                load = containerLoadItem;
                return true;
            }

            return false;
        }

        private async UniTaskVoid LoadInternal<T>(ContainerLoadItem<T> containerLoadItem, UniTask<T> loadTask,
            CancellationToken cancellationToken)
        {
            var result = await loadTask;

            if (cancellationToken.IsCancellationRequested)
            {
                containerLoadItem.SetCanceled();
                return;
            }

            _serviceCachedResources.Add(containerLoadItem.KeyResource, result);
            containerLoadItem.CompleteLoading(result);
        }
    }
}