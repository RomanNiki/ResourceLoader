using App.Scripts.Features.SystemResources.Core.Handlers;
using Handlers;
using Zenject;

namespace App.Scripts.Features.SystemResources.Bootstrap
{
    public class InstallerHandlerLoadResource : Installer<InstallerHandlerLoadResource>
    {
        public override void InstallBindings()
        {
            Container.Bind<IHandlerLoadResource>().To<HandlerLoadJsonFromRemote>().AsCached();
            Container.Bind(typeof(IHandlerLoadResource), typeof(IInitializable)).To<HandlerLoadAddressableResource>()
                .AsSingle();
        }
    }
}