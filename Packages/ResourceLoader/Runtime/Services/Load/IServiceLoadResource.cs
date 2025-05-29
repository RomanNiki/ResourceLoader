using System.Threading;
using Containers;

namespace Services.Load
{
    public interface IServiceLoadResource
    {
        ContainerLoadItem<T> Load<T>(string key, object owner, CancellationToken cancellationToken = default);
    }
}