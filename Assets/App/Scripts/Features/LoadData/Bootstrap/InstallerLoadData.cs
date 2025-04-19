using App.Scripts.Features.LoadData.Services;
using Services.Interfaces;
using Zenject;

namespace App.Scripts.Features.LoadData.Bootstrap
{
    public class InstallerLoadData : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IServiceJsonSerializer>().To<ServiceJsonNewtonsoftSerializer>().AsSingle();
        }
    }
}