using System.Linq;
using System.Threading;
using App.Scripts.Features.SystemResources.Core.Constants;
using Configs;
using Cysharp.Threading.Tasks;
using Handlers;
using Openmygame.LoggerPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace App.Scripts.Features.SystemResources.Core.Handlers
{
    public class HandlerLoadAddressableResource : IHandlerLoadResource
    {
        private readonly ConfigContainerResource _config;

        public HandlerLoadAddressableResource(ConfigContainerResource config)
        {
            _config = config;
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

        private IContainerResources TryGetConfig()
        {
            IContainerResources config =
                _config.Resources.FirstOrDefault(x => x.Name == KeyCategoriesSystemResources.Addressables);

            return config;
        }

        public async UniTask<T> LoadResource<T>(string key, CancellationToken cancellationToken = default)
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
                    return await ProcessLoad<T>(item as AssetReference, cancellationToken);
                }
            }

            return default;
        }

        private async UniTask<T> ProcessLoad<T>(AssetReference item, CancellationToken token = default)
        {
            AsyncOperationHandle<T> handle = item.LoadAssetAsync<T>();

            UniTask task = handle.ToUniTask(cancellationToken: token);

            await task.SuppressCancellationThrow();

            if (token.IsCancellationRequested)
                return default;

            return handle.Result;
        }
    }
}