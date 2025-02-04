using Cysharp.Threading.Tasks;

namespace Openmygame.Infrastructure.Architecture.Containers
{
    public interface IProviderValueAsync<T>
    {
        UniTask<T> Value { get; }
    }
}