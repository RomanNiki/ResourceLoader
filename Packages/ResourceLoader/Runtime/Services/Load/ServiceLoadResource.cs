using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Handlers;
using Models;
using Openmygame.LoggerPro;
using Services.Cache;

namespace Services.Load
{
    public class ServiceLoadResource : IServiceLoadResource
    {
        private readonly IServiceOwnerResource _serviceOwnerResource;
        private readonly List<IHandlerLoadResource> _handlers;
        private readonly IServiceCachedResources _serviceCachedResources;
        private readonly Dictionary<string, IUniTaskSource> _activeTasks = new();

        public ServiceLoadResource(IServiceCachedResources serviceCachedResources, List<IHandlerLoadResource> handlers)
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
                var tsk = new UniTaskCompletionSource<T>();

                var containerLoadItem = CreateContainerLoad(key, tsk, owner);
                containerLoadItem.CompleteLoading(item);

                return containerLoadItem;
            }

            if (_activeTasks.TryGetValue(key, out var completionSource))
            {
                var containerLoadItem = CreateContainerLoad(key, (UniTaskCompletionSource<T>)completionSource, owner);
                return containerLoadItem;
            }

            foreach (var handlerLoadResource in _handlers)
            {
                if (!handlerLoadResource.CanHandle(key))
                {
                    continue;
                }

                UniTask<ModelResource> loadTask = handlerLoadResource.LoadResource<T>(key, cancellationToken);

                var tsk = new UniTaskCompletionSource<T>();
                var containerLoadItem = CreateContainerLoad<T>(key, tsk, owner);
               
                _activeTasks.Add(key, tsk);
              
                LoadInternal(containerLoadItem, loadTask, cancellationToken).Forget();

                return containerLoadItem;
            }

            ProLogger.Exception("Not found handler for type " + typeof(T).Name);
            return default;
        }

        private ContainerLoadItem<T> CreateContainerLoad<T>(string key, UniTaskCompletionSource<T> tsk, object owner)
        {
            _serviceOwnerResource.AddOwner(key, owner);
            ContainerLoadItem<T> containerLoadItem = new ContainerLoadItem<T>(key, owner, tsk, _serviceOwnerResource);
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

        private async UniTaskVoid LoadInternal<T>(ContainerLoadItem<T> containerLoadItem,
            UniTask<ModelResource> loadTask,
            CancellationToken cancellationToken)
        {
            var result = await loadTask;

            if (cancellationToken.IsCancellationRequested)
            {
                containerLoadItem.SetCanceled();
                return;
            }
            
            _serviceCachedResources.Add(containerLoadItem.KeyResource, result);
            _activeTasks.Remove(containerLoadItem.KeyResource);
            containerLoadItem.CompleteLoading(result.Resource);
        }
    }
}