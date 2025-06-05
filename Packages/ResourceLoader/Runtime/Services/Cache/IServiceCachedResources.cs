using System.Collections.Generic;
using Models;

namespace Services.Cache
{
    public interface IServiceCachedResources
    {
        IReadOnlyList<string> GetKeysCachedResources();
        void Add(string key, ModelResource resource);
        void Remove(string key);
        bool Has(string key);
        T Get<T>(string key);
    }
}