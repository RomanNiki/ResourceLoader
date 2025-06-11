using System.Linq;
using System.Threading;
using App.Scripts.Features.SystemResources.Core.Constants;
using Configs;
using Cysharp.Threading.Tasks;
using Handlers;
using Models;
using Openmygame.LoggerPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace App.Scripts.Features.SystemResources.Core.Handlers
{
    public class HandlerLoadAddressableResource : IHandlerLoadResource, IInitializable
    {
        private readonly ConfigContainerResource _config;
        private IContainerResources _configResource;

        public HandlerLoadAddressableResource(ConfigContainerResource config)
        {
            _config = config;
        }
        
        public void Initialize()
        {
            _configResource =
                _config.Resources.FirstOrDefault(x => x.Name == KeyCategoriesSystemResources.Addressables);
        }

        public bool CanHandle(string key)
        {
            var config = TryGetConfig();

            if (config != null)
            {
                foreach (var (keyItem, _) in config.GetValues())
                {
                    if (keyItem == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async UniTask<ModelResource> LoadResource<T>(string key, CancellationToken cancellationToken = default)
        {
            var config = TryGetConfig();

            if (config == null)
            {
                ProLogger.Exception("Cannot find config for Addressables");
                return default;
            }

            foreach (var (keyItem, item) in config.GetValues())
            {
                if (keyItem == key)
                {
                    var resource = await ProcessLoad<T>(item as AssetReference, cancellationToken);
                   
                    return new ModelResource(key, resource, () => ReleaseAddressableAsset(resource));
                }
            }

            return default;
        }
        
        private static void ReleaseAddressableAsset<T>(T obj)
        {
            Addressables.Release(obj);
        }

        private IContainerResources TryGetConfig()
        {
            return _configResource;
        }

        private async UniTask<T> ProcessLoad<T>(AssetReference item, CancellationToken token = default)
        {
            AsyncOperationHandle<T> handle = item.LoadAssetAsync<T>();

            UniTask task = handle.ToUniTask(cancellationToken: token);

            await task.SuppressCancellationThrow();

            if (token.IsCancellationRequested)
            {
                return default;
            }

            return handle.Result;
        }
    }
}