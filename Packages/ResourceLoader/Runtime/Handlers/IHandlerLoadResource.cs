using System.Threading;
using Cysharp.Threading.Tasks;
using Models;

namespace Handlers
{
    public interface IHandlerLoadResource
    {
        bool CanHandle(string key);
        UniTask<ModelResource> LoadResource<T>(string key, CancellationToken cancellationToken = default);
    }
}