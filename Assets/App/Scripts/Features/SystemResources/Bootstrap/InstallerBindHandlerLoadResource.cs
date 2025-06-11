using App.Scripts.Features.SystemResources.Core.Handlers;
using Handlers;
using Zenject;

namespace App.Scripts.Features.SystemResources.Bootstrap
{
    public class InstallerBindHandlerLoadResource : Installer<InstallerBindHandlerLoadResource>
    {
        public override void InstallBindings()
        {
            Container.Bind<IHandlerLoadResource>().To<HandlerLoadJsonFromRemote>().AsCached();
            Container.Bind(typeof(IHandlerLoadResource), typeof(IInitializable)).To<HandlerLoadAddressableResource>()
                .AsSingle();
        }
    }
}