using System;
using System.Collections.Generic;
using Memory.Pools;

namespace Memory.Structs
{
    public struct ContainerMapKey<T>
    {
        private Dictionary<string, T> _map;

        public T this[string key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public int Count => _map?.Count ?? 0;

        public IReadOnlyCollection<string> GetKeys()
        {
            if (_map == null)
            {
                return Array.Empty<string>();
            }

            return _map.Keys;
        }

        public void RemoveAtKey(string key)
        {
            if (_map == null)
            {
                return;
            }

            _map.Remove(key);
        }

        public void Dispose()
        {
            if (_map == null)
            {
                return;
            }

            Clear();

            PoolDictionary<string, T>.Recycle(_map);
            _map = null;
        }

        public void Clear()
        {
            _map?.Clear();
        }

        public T GetValue(string key)
        {
            if (_map != null && _map.TryGetValue(key, out T value))
            {
                return value;
            }

            return default;
        }

        public void SetValue(string key, T getValue)
        {
            _map ??= PoolDictionary<string, T>.Spawn();
            _map[key] = getValue;
        }

        public bool TryGetValue(string key, out T value)
        {
            if (_map == null)
            {
                value = default;
                return false;
            }

            return _map.TryGetValue(key, out value);
        }

        public bool Contains(string key)
        {
            if (_map == null)
            {
                return false;
            }

            return _map.ContainsKey(key);
        }
    }
}