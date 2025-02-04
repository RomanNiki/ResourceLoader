using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Handlers
{
    public interface IHandlerLoadResource
    {
        Type KeyType { get; }
        UniTask<T> LoadResource<T>(string key, CancellationToken cancellationToken = default);
    }
}