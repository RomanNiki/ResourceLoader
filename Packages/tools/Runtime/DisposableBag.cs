using System;
using System.Collections.Generic;
using Memory.Pools;

namespace Memory
{
    public struct DisposableBag : IDisposable
    {
        private List<IDisposable> _bagDisposables;
        private List<DisposableAction> _bagDisposablesActions;

        public void Add(IDisposable disposable)
        {
            _bagDisposables ??= PoolList<IDisposable>.Spawn();
            _bagDisposables.Add(disposable);
        }

        public void Add(DisposableAction disposable)
        {
            _bagDisposablesActions ??= PoolList<DisposableAction>.Spawn();
            _bagDisposablesActions.Add(disposable);
        }

        public void Dispose()
        {
            DisposeBag();
            DisposeBagActions();
        }

        private void DisposeBagActions()
        {
            if (_bagDisposablesActions == null)
            {
                return;
            }

            foreach (DisposableAction disposable in _bagDisposablesActions)
            {
                disposable.Dispose();
            }

            PoolList<DisposableAction>.Recycle(_bagDisposablesActions);
            _bagDisposablesActions = null;
        }

        private void DisposeBag()
        {
            if (_bagDisposables == null)
            {
                return;
            }

            foreach (IDisposable disposable in _bagDisposables)
            {
                disposable.Dispose();
            }

            PoolList<IDisposable>.Recycle(_bagDisposables);
            _bagDisposables = null;
        }
    }

    public struct DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action.Invoke();
        }
    }
}