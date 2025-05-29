using System;
using Cysharp.Threading.Tasks;
using Services.Interfaces;

namespace Containers
{
    public struct ContainerLoadItem<T> : ILoadable, IDisposable
    {
        private readonly UniTaskCompletionSource<T> _tcs;
        public readonly string KeyResource;
        private readonly object _owner;
        private readonly IServiceOwnerResource _serviceOwnerResource;

        internal ContainerLoadItem(string keyResource, object owner, IServiceOwnerResource serviceOwnerResource)
        {
            KeyResource = keyResource;
            _owner = owner;
            _serviceOwnerResource = serviceOwnerResource;
            _tcs = new UniTaskCompletionSource<T>();
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