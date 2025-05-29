using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ServiceCachedResources : IServiceCachedResources
    {
        private readonly Dictionary<string, object> _cache = new();

        public void Add(string key, object resource)
        {
            _cache[key] = resource;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
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