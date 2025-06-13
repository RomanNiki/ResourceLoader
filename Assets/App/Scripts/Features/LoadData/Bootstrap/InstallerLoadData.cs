using App.Scripts.Features.LoadData.Services;
using Tools.Json.Services;
using Zenject;

namespace App.Scripts.Features.LoadData.Bootstrap
{
    public class InstallerLoadData : Installer<InstallerLoadData>
    {
        public override void InstallBindings()
        {
            Container.Bind<IServiceJsonSerializer>().To<ServiceJsonNewtonsoftSerializer>().AsSingle();
        }
    }
}