using Services.Cache;
using Services.Load;
using Strategies;
using Zenject;

namespace App.Scripts.Features.SystemResources.Bootstrap
{
    public class InstallerSystemResourcesCore : Installer<InstallerSystemResourcesCore>
    {
        public override void InstallBindings()
        {
            Container.Bind<IServiceLoadTexture2D>().To<ServiceLoadTexture2D>().AsSingle();
            Container.Bind<IServiceLoadWebRequest>().To<ServiceLoadWebRequest>().AsSingle();
            Container.Bind<IServiceLoadResource>().To<ServiceLoadResource>().AsSingle();
            Container.Bind<IServiceCleanupResources>().To<ServiceCleanupResources>().AsSingle();
            Container.Bind<IServiceCachedResources>().To<ServiceCachedResources>().AsSingle();
            Container.Bind<IServiceOwnerResource>().To<ServiceOwnerResource>().AsSingle();

            BindCleanupStrategies();
        }

        private void BindCleanupStrategies()
        {
            Container.Bind<IStrategyCleanup>().To<StrategyCleanupNoOwner>().AsCached();
        }
    }
}