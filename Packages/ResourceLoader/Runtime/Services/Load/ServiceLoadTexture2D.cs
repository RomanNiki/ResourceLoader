using System;
using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Openmygame.LoggerPro;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Services.Load
{
    public class ServiceLoadTexture2D : IServiceLoadTexture2D, IInitializable
    {
        private readonly Queue<ContainerItemTexture2D> _queue = new();

        public void Initialize()
        {
            Load().Forget();
        }

        public UniTask<Texture2D> LoadSprite(string key, DownloadHandlerTexture downloadHandlerTexture,
            CancellationToken cancellationToken = default)
        {
            ContainerItemTexture2D containerItemTexture2D = new ContainerItemTexture2D(key, downloadHandlerTexture, cancellationToken);
            _queue.Enqueue(containerItemTexture2D);
            return containerItemTexture2D.Task;
        }

        private async UniTaskVoid Load()
        {
            while (Application.isPlaying)
            {
                while (_queue.Count == 0)
                {
                    await UniTask.Yield();
                }

                ContainerItemTexture2D containerItemTexture2D = _queue.Dequeue();

                LoadItem(containerItemTexture2D);

                await UniTask.Yield();
            }
        }

        private void LoadItem(ContainerItemTexture2D containerItemTexture2D)
        {
            CancellationToken cancellationToken = containerItemTexture2D.CancellationToken;

            if (cancellationToken.IsCancellationRequested)
            {
                containerItemTexture2D.SetCanceled();
                return;
            }

            string keyTexture = containerItemTexture2D.KeyTexture;
            DownloadHandlerTexture downloadHandler = containerItemTexture2D.DownloadHandler;

            Texture2D texture2D = CreateTexture2D(downloadHandler, keyTexture);

            containerItemTexture2D.CompleteLoading(texture2D);
        }

        private Texture2D CreateTexture2D(DownloadHandlerTexture downloadHandler, string keyTexture)
        {
            Texture2D texture2D = downloadHandler.texture;

            try
            {
                texture2D.name = keyTexture;
            }
            catch (Exception e)
            {
                ProLogger.Exception(e);
                texture2D = null;
            }

            return texture2D;
        }
    }
}