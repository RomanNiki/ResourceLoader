using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Handlers
{
    public interface IHandlerLoadResource
    {
        bool CanHandle(string key);
        UniTask<T> LoadResource<T>(string key, CancellationToken cancellationToken = default);
    }
}