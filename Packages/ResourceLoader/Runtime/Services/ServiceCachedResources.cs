using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services
{
    public class ServiceCachedResources : IServiceCachedResources
    {
        private readonly IServiceOwnerResource _serviceOwnerResource;
        private readonly Dictionary<string, object> _cache = new();
        private readonly List<string> _keys = new List<string>();

        public ServiceCachedResources(IServiceOwnerResource serviceOwnerResource)
        {
            _serviceOwnerResource = serviceOwnerResource;
        }

        public IReadOnlyList<string> GetKeysCachedResources()
        {
            return _keys;
        }

        public void Add(string key, object resource)
        {
            _cache[key] = resource;
            
            if (_keys.Contains(key))
            {
                return;
            }

            _keys.Add(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);
        }

        public bool Has(string key)
        {
            return _cache.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var result))
            {
                switch (result)
                {
                    case T t:
                        return t;

                    case GameObject resultObject:
                        return resultObject.GetComponent<T>();
                }
            }

            return default;
        }
    }
}