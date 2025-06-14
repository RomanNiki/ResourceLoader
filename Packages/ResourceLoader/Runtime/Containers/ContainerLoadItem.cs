using System;
using Cysharp.Threading.Tasks;
using Services.Cache;
using Services.Load;

namespace Containers
{
    public struct ContainerLoadItem<T> : ILoadable, IDisposable
    {
        public readonly string KeyResource;
        private readonly object _owner;
        private readonly UniTaskCompletionSource<T> _tcs;
        private readonly IServiceOwnerResource _serviceOwnerResource;

        internal ContainerLoadItem(string keyResource, object owner, UniTaskCompletionSource<T> completionSource, IServiceOwnerResource serviceOwnerResource)
        {
            KeyResource = keyResource;
            _owner = owner;
            _tcs = completionSource;
            _serviceOwnerResource = serviceOwnerResource;
        }

        public UniTaskStatus Status => Task.Status;
        public UniTask<T> Task => _tcs.Task;

        public void CompleteLoading(object loaded)
        {
            _tcs.TrySetResult((T)loaded);
        }

        public void SetCanceled()
        {
            _tcs.TrySetResult(default);
            _serviceOwnerResource.RemoveOwner(KeyResource, _owner);
        }

        public void Dispose()
        {
            _serviceOwnerResource.RemoveOwner(KeyResource, _owner);
        }
    }
}