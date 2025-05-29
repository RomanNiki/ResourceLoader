using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Services.Load
{
    public interface IServiceLoadWebRequest
    {
        UniTask<ResultLoadWebRequest> LoadRequest(string url, CancellationToken cancellationToken,
            DownloadHandler handler);
    }
}