using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Handlers;
using Openmygame.LoggerPro;
using Services.Interfaces;

namespace Services
{
    public class ServiceLoadResource : IServiceLoadResource
    {
        private readonly IServiceOwnerResource _serviceOwnerResource;
        private readonly List<IHandlerLoadResource> _handlers;

        public ServiceLoadResource(List<IHandlerLoadResource> handlers)
        {
            _handlers = handlers;
        }

        public ServiceLoadResource(IServiceOwnerResource serviceOwnerResource)
        {
            _serviceOwnerResource = serviceOwnerResource;
        }

        public ContainerLoadItem<T> Load<T>(string key, object owner, CancellationToken cancellationToken = default)
        {
            foreach (var handlerLoadResource in _handlers)
            {
                if (!handlerLoadResource.KeyType.IsAssignableFrom(typeof(T)))
                {
                    continue;
                }

                var loadTask = handlerLoadResource.LoadResource<T>(key, cancellationToken);

                var containerLoadItem = new ContainerLoadItem<T>(key, owner, _serviceOwnerResource);
                LoadInternal(containerLoadItem, loadTask, cancellationToken).Forget();

                return containerLoadItem;
            }

            ProLogger.Exception("Not found handler for type " + typeof(T).Name);
            return default;
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

            containerLoadItem.CompleteLoading(result);
        }
    }
}