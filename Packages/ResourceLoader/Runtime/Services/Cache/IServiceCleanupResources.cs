using Cysharp.Threading.Tasks;

namespace Services.Cache
{
    public interface IServiceCleanupResources
    {
        UniTask CleanupResources();
    }
}