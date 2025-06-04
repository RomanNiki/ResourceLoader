using System.Collections.Generic;

namespace Services.Cache
{
    public class ServiceOwnerResource : IServiceOwnerResource
    {
        private const int StartCapacity = 15;
        private readonly Dictionary<string, HashSet<object>> _serviceCachedResources = new(StartCapacity);
        
        public void AddOwner(string keyResource, object owner)
        {
            if (_serviceCachedResources.TryGetValue(keyResource, out var resource))
            {
                resource.Add(owner);
                return;
            }
            
            _serviceCachedResources[keyResource] = new HashSet<object>()
            {
                owner
            };
        }

        public void RemoveOwner(string keyResource, object owner)
        {
            _serviceCachedResources[keyResource].Remove(owner);
        }

        public int GetOwnerCount(string keyResource)
        {
            return _serviceCachedResources[keyResource].Count;
        }
    }
}