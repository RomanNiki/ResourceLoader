using System.Collections.Generic;

namespace Services
{
    public interface IServiceCachedResources
    {
        IReadOnlyList<string> GetKeysCachedResources();
        void Add(string key, object resource);
        void Remove(string key);
        bool Has(string key);
        T Get<T>(string key);
    }
}