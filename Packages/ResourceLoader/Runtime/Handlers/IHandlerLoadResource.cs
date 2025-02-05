using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Handlers
{
    public interface IHandlerLoadResource
    {
        bool CanLoad(string key, Type type);
        UniTask<T> LoadResource<T>(string key, CancellationToken cancellationToken = default);
    }
}