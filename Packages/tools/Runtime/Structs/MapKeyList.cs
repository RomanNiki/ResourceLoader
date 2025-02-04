using System;
using System.Collections.Generic;
using Memory.Pools;

namespace Memory.Structs
{
    public struct MapKeyList<T>
    {
        private ContainerMapKey<List<T>> _mapLists;

        public int Count => _mapLists.Count;

        public void AddItem(string key, T item)
        {
            _mapLists[key] ??= PoolList<T>.Spawn();
            _mapLists[key].Add(item);
        }

        public IReadOnlyCollection<string> GetKeys()
        {
            return _mapLists.GetKeys();
        }

        public IReadOnlyCollection<T> GetItems(string key)
        {
            List<T> list = _mapLists[key];

            if (list == null)
            {
                return Array.Empty<T>();
            }

            return list;
        }

        public void Dispose()
        {
            foreach (string key in _mapLists.GetKeys())
            {
                PoolList<T>.Recycle(_mapLists[key]);
            }

            _mapLists.Dispose();
        }
    }
}