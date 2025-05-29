using System;
using System.Collections.Generic;
using System.Threading;
using Containers;
using Cysharp.Threading.Tasks;
using Openmygame.LoggerPro;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Services
{
    public class ServiceLoadTexture2D : IServiceLoadTexture2D, IInitializable
    {
        private readonly Queue<ContainerItem> _queue = new();

        public void Initialize()
        {
            Load().Forget();
        }

        public UniTask<Texture2D> LoadSprite(string key, DownloadHandlerTexture downloadHandlerTexture,
            CancellationToken cancellationToken = default)
        {
            ContainerItem containerItem = new ContainerItem(key, downloadHandlerTexture, cancellationToken);
            _queue.Enqueue(containerItem);
            return containerItem.Task;
        }

        private async UniTaskVoid Load()
        {
            while (Application.isPlaying)
            {
                while (_queue.Count == 0)
                {
                    await UniTask.Yield();
                }

                ContainerItem containerItem = _queue.Dequeue();

                LoadItem(containerItem);

                await UniTask.Yield();
            }
        }

        private void LoadItem(ContainerItem containerItem)
        {
            CancellationToken cancellationToken = containerItem.CancellationToken;

            if (cancellationToken.IsCancellationRequested)
            {
                containerItem.SetCanceled();
                return;
            }

            string keyTexture = containerItem.KeyTexture;
            DownloadHandlerTexture downloadHandler = containerItem.DownloadHandler;

            Texture2D texture2D = CreateTexture2D(downloadHandler, keyTexture);

            containerItem.CompleteLoading(texture2D);
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