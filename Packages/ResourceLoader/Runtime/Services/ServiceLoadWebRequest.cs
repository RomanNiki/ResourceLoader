using System;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Openmygame.LoggerPro.Extensions;
using UnityEngine.Networking;

namespace Services
{
    public class ServiceLoadWebRequest : IServiceLoadWebRequest
    {
        private const int TimeoutRequestSec = 10;

        public async UniTask<ResultLoadWebRequest> LoadRequest(string url, CancellationToken cancellationToken,
            DownloadHandler handler)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.disposeDownloadHandlerOnDispose = false;
            unityWebRequest.downloadHandler = handler;

            await LoadWebRequest(cancellationToken, unityWebRequest);

            return new ResultLoadWebRequest(unityWebRequest);
        }

        private async UniTask LoadWebRequest(CancellationToken cancellationToken,
            UnityWebRequest unityWebRequest)
        {
            try
            {
                unityWebRequest.timeout = TimeoutRequestSec;

                await unityWebRequest.SendWebRequest().ToUniTask(cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
            }
            catch (Exception e)
            {
                this.ProLog(e.Message);
            }
        }
    }
}