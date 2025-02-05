using System;
using UnityEngine.Networking;

namespace Containers
{
    public struct ResultLoadWebRequest : IDisposable
    {
        private UnityWebRequest _unityWebRequest;

        public readonly UnityWebRequest.Result Status;
        public readonly long ResponseCode;

        public ResultLoadWebRequest(UnityWebRequest unityWebRequest)
        {
            _unityWebRequest = unityWebRequest;
            Status = unityWebRequest.result;
            ResponseCode = unityWebRequest.responseCode;
        }

        public void Dispose()
        {
            _unityWebRequest?.Dispose();
            _unityWebRequest = null;
        }
    }
}