using Cysharp.Threading.Tasks;

namespace Services
{
    public interface IServiceCleanupResources
    {
        UniTask CleanupResources();
    }
}