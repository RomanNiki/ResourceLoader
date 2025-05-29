using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Handlers;
using Openmygame.LoggerPro;
using Services.Cache;

namespace Services.Load
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
            if (TryGetCachedItem(key, out T item))
            {
                var containerLoadItem = CreateContainerLoad<T>(key, owner);
                containerLoadItem.CompleteLoading(item);
                return containerLoadItem;
            }

            foreach (var handlerLoadResource in _handlers)
            {
                if (!handlerLoadResource.CanHandle(key))
                {
                    continue;
                }

                UniTask<T> loadTask = handlerLoadResource.LoadResource<T>(key, cancellationToken);

                var containerLoadItem = CreateContainerLoad<T>(key, owner);
                LoadInternal(containerLoadItem, loadTask, cancellationToken).Forget();

                return containerLoadItem;
            }

            ProLogger.Exception("Not found handler for type " + typeof(T).Name);
            return default;
        }

        private ContainerLoadItem<T> CreateContainerLoad<T>(string key, object owner)
        {
            _serviceOwnerResource.AddOwner(key, owner);
            ContainerLoadItem<T> containerLoadItem = new ContainerLoadItem<T>(key, owner, _serviceOwnerResource);
            return containerLoadItem;
        }

        private bool TryGetCachedItem<T>(string key, out T load)
        {
            load = default;

            if (_serviceCachedResources.Has(key))
            {
                load = _serviceCachedResources.Get<T>(key);
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