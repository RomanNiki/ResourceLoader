using System;
using System.Text;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Services.Interfaces;
using Unity.Collections;
using UnityEngine.Networking;

namespace Handlers
{
    public class HandlerLoadJsonFromRemote : IHandlerLoadResource
    {
        private readonly IServiceLoadWebRequest _serviceLoadWebRequest;
        private readonly IServiceJsonSerializer _serviceJsonSerializer;

        public HandlerLoadJsonFromRemote(IServiceLoadWebRequest serviceLoadWebRequest,
            IServiceJsonSerializer serviceJsonSerializer)
        {
            _serviceLoadWebRequest = serviceLoadWebRequest;
            _serviceJsonSerializer = serviceJsonSerializer;
        }

        public bool CanHandle(string key)
        {
            return Uri.TryCreate(key, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }

        public async UniTask<T> LoadResource<T>(string key, CancellationToken cancellationToken = default)
        {
            DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
            ResultLoadWebRequest resultLoadWebRequest =
                await _serviceLoadWebRequest.LoadRequest(key, cancellationToken, downloadHandlerBuffer);

            if (cancellationToken.IsCancellationRequested)
            {
                return default;
            }

            UnityWebRequest.Result status = resultLoadWebRequest.Status;

            var result = string.Empty;

            if (status == UnityWebRequest.Result.Success && resultLoadWebRequest.ResponseCode != 0)
            {
                result = GetFileAsString(downloadHandlerBuffer.nativeData);
            }

            resultLoadWebRequest.Dispose();

            downloadHandlerBuffer.Dispose();

            return _serviceJsonSerializer.Deserialize<T>(result);
        }
        
        private static string GetFileAsString(NativeArray<byte>.ReadOnly fileBytes)
        {
            return Encoding.UTF8.GetString(fileBytes);
        }

    }
}