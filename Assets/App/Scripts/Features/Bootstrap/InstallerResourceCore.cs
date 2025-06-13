using App.Scripts.Features.LoadData.Bootstrap;
using App.Scripts.Features.SystemResources.Bootstrap;
using Configs;
using UnityEngine;
using Zenject;

namespace App.Scripts.Features.Bootstrap
{
    public class InstallerResourceCore : MonoInstaller
    {
        [SerializeField] private ConfigContainerResource configContainerResource;

        public override void InstallBindings()
        {
            Container.Bind<ConfigContainerResource>().FromInstance(configContainerResource).AsSingle();
           
            InstallerLoadData.Install(Container);
            InstallerHandlerLoadResource.Install(Container);
            InstallerSystemResourcesCore.Install(Container);
        }
    }
}