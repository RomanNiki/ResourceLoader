using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Containers
{
    internal struct ContainerItem
    {
        private readonly UniTaskCompletionSource<Texture2D> _tcs;
        public readonly DownloadHandlerTexture DownloadHandler;
        public CancellationToken CancellationToken;
        public readonly string KeyTexture;

        public ContainerItem(string keyTexture, DownloadHandlerTexture downloadHandler, CancellationToken cancellationToken)
        {
            KeyTexture = keyTexture;
            DownloadHandler = downloadHandler;
            CancellationToken = cancellationToken;
            _tcs = new UniTaskCompletionSource<Texture2D>();
        }
        
        public UniTask<Texture2D> Task => _tcs.Task;
        
        public void SetCanceled()
        {
            _tcs.TrySetResult(null);
        }

        public void CompleteLoading(Texture2D texture2D)
        {
            _tcs.TrySetResult(texture2D);
        }
    }
}