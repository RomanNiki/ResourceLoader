using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Strategies;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ServiceCleanupResources : IServiceCleanupResources, IInitializable
    {
        private readonly List<IStrategyCleanup> _strategies;
        private const float CleanupTimerSec = 4;

        public ServiceCleanupResources(List<IStrategyCleanup> strategies)
        {
            _strategies = strategies;
        }
        
        public void Initialize()
        {
            Tick().Forget();
        }
        
        public async UniTask CleanupResources()
        {
            foreach (var strategy in _strategies)
            {
                strategy.CleanupResources();
                
                await UniTask.Yield();
            }
        }

        private async UniTaskVoid Tick()
        {
            while (Application.isPlaying)
            {
                await CleanupResources();
                
                await UniTask.WaitForSeconds(CleanupTimerSec);
            }
        }
    }
}