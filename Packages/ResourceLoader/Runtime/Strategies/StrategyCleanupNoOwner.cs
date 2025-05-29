using Services;
using Services.Cache;

namespace Strategies
{
    public class StrategyCleanupNoOwner : IStrategyCleanup
    {
        private readonly IServiceCachedResources _serviceCachedResources;
        private readonly IServiceOwnerResource _serviceOwnerResource;

        public StrategyCleanupNoOwner(IServiceCachedResources serviceCachedResources,
            IServiceOwnerResource serviceOwnerResource)
        {
            _serviceCachedResources = serviceCachedResources;
            _serviceOwnerResource = serviceOwnerResource;
        }

        public void CleanupResources()
        {
            foreach (var key in _serviceCachedResources.GetKeysCachedResources())
            {
                if (_serviceOwnerResource.GetOwnerCount(key) > 0)
                {
                    continue;
                }
                
                _serviceCachedResources.Remove(key);
            }
        }
    }
}