using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Services.Cache
{
    public class ServiceCachedResources : IServiceCachedResources
    {
        private readonly IServiceOwnerResource _serviceOwnerResource;
        private readonly Dictionary<string, ModelResource> _cache = new();
        private readonly List<string> _keys = new();

        public ServiceCachedResources(IServiceOwnerResource serviceOwnerResource)
        {
            _serviceOwnerResource = serviceOwnerResource;
        }

        public IReadOnlyList<string> GetKeysCachedResources()
        {
            return _keys;
        }

        public void Add(string key, ModelResource resource)
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
            if (!_cache.TryGetValue(key, out var result))
            {
                throw new System.Exception($"Key {key} was not found");
            }
            
            result = _cache[key];
            result.Destructor?.Invoke();
            
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
                switch (result.Resource)
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