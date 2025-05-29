using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Load
{
    public interface IServiceLoadTexture2D
    {
        public UniTask<Texture2D> LoadSprite(string key, DownloadHandlerTexture downloadHandlerTexture, CancellationToken cancellationToken = default);
    }
}