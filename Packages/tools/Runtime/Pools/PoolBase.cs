using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Memory.Pools
{
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolClassInterface<T> where T : class
    {
        private static readonly PoolInternalBase pool = new(null, null);

        public static TReal Spawn<TReal>() where TReal : class, new()
        {
            var instance = (T) pool.Spawn();
            if (instance == null)
            {
                return new TReal();
            }

            return (TReal) (object) instance;
        }

        public static void Recycle(ref T instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(T instance)
        {
            pool.Recycle(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolList<T>
    {
        private static readonly PoolInternalBase pool = new(() => new List<T>(), h => ((List<T>) h).Clear());

        public static List<T> Spawn()
        {
            return (List<T>) pool.Spawn();
        }

        public static void Recycle(ref List<T> instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(List<T> instance)
        {
            pool.Recycle(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolDictionary<TKey, TValue>
    {
        private static readonly PoolInternalBase pool = new(() => new Dictionary<TKey, TValue>(),
            h => ((Dictionary<TKey, TValue>) h).Clear());

        public static Dictionary<TKey, TValue> Spawn()
        {
            return (Dictionary<TKey, TValue>) pool.Spawn();
        }

        public static void Recycle(ref Dictionary<TKey, TValue> instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(Dictionary<TKey, TValue> instance)
        {
            pool.Recycle(instance);
        }
    }
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolHashSet<T>
    {
        private static readonly PoolInternalBase pool = new(() => new HashSet<T>(), h => ((HashSet<T>) h).Clear());

        public static HashSet<T> Spawn()
        {
            return (HashSet<T>) pool.Spawn();
        }

        public static void Recycle(ref HashSet<T> instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(HashSet<T> instance)
        {
            pool.Recycle(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolClass<T> where T : class, new()
    {
        public static readonly PoolInternalBase pool = new(() => new T(), null);

        public static T Spawn()
        {
            return (T) pool.Spawn();
        }

        public static void Recycle(ref T instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(T instance)
        {
            pool.Recycle(instance);
        }
    } 
    
#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolStack<T>
    {
        private static readonly PoolInternalBase pool = new(() => new Stack<T>(), h => ((Stack<T>) h).Clear());

        public static Stack<T> Spawn()
        {
            return (Stack<T>) pool.Spawn();
        }

        public static void Recycle(ref Stack<T> instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(Stack<T> instance)
        {
            pool.Recycle(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolClassCustom<T> where T : class
    {
        private static readonly PoolInternalBase poolCustom = new(null, null);

        public static TCustom Spawn<TCustom>() where TCustom : class, T, new()
        {
            object objBase = poolCustom.Spawn();
            if (objBase != null && objBase is TCustom == false)
            {
                poolCustom.Recycle(objBase);
            }

            if (objBase as TCustom == null)
            {
                return new TCustom();
            }

            return (TCustom) objBase;
        }

        public static void Recycle<TCustom>(TCustom instance) where TCustom : class, T
        {
            poolCustom.Recycle(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
	[Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
	 Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public static class PoolClassMainThread<T> where T : class, new()
    {
        private static readonly PoolInternalBaseNoStackPool pool = new(() => new T(), null);

        public static T Spawn()
        {
            return (T) pool.Spawn();
        }

        public static void Recycle(ref T instance)
        {
            pool.Recycle(instance);
            instance = null;
        }

        public static void Recycle(T instance)
        {
            pool.Recycle(instance);
        }
    }

    // public interface IPoolableSpawn
    // {
    //     void OnSpawn();
    // }
    //
    // public interface IPoolableRecycle
    // {
    //     void OnRecycle();
    // }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif
    public class PoolInternalBaseNoStackPool
    {
        public static int newAllocated;
        public static int allocated;
        public static int deallocated;
        public static int used;

        private static readonly List<PoolInternalBaseNoStackPool> list = new();

        protected Stack<object> cache = new();
        protected Func<object> constructor;
        protected Action<object> desctructor;
        protected int poolAllocated;
        protected int poolDeallocated;
        protected int poolNewAllocated;
        protected int poolUsed;

        public PoolInternalBaseNoStackPool(Func<object> constructor, Action<object> desctructor)
        {
            this.constructor = constructor;
            this.desctructor = desctructor;

            list.Add(this);
        }

        public override string ToString()
        {
            return "Allocated: " + poolAllocated + ", Deallocated: " + poolDeallocated + ", Used: " + poolUsed +
                   ", cached: " + cache.Count + ", new: " + poolNewAllocated;
        }

        public static void Clear()
        {
            List<PoolInternalBaseNoStackPool> pools = list;
            for (var i = 0; i < pools.Count; ++i)
            {
                PoolInternalBaseNoStackPool pool = pools[i];
                pool.cache.Clear();
                pool.constructor = null;
                pool.desctructor = null;
            }

            pools.Clear();
        }

        // public static T Create<T>() where T : new()
        // {
        //     var instance = new T();
        //     CallOnSpawn(instance);
        //
        //     return instance;
        // }

        // public static void CallOnSpawn<T>(T instance)
        // {
        //     if (instance is IPoolableSpawn poolable)
        //     {
        //         poolable.OnSpawn();
        //     }
        // }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Prewarm(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Recycle(Spawn());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual object Spawn()
        {
            object item = null;
            if (cache.Count > 0)
            {
                item = cache.Pop();
            }

            if (item == null)
            {
                ++newAllocated;
                ++poolNewAllocated;
            }
            else
            {
                ++used;
                ++poolUsed;
            }

            if (constructor != null && item == null)
            {
                item = constructor.Invoke();
            }

            // if (item is IPoolableSpawn poolable)
            // {
            //     poolable.OnSpawn();
            // }

            ++poolAllocated;
            ++allocated;

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recycle(object instance)
        {
            ++poolDeallocated;
            ++deallocated;

            if (desctructor != null)
            {
                desctructor.Invoke(instance);
            }

            // if (instance is IPoolableRecycle poolable)
            // {
            //     poolable.OnRecycle();
            // }

            cache.Push(instance);
        }
    }

#if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
#endif

    public class PoolInternalBase
    {
#if MULTITHREAD_SUPPORT
	    protected CCStack<object> cache = new CCStack<object>(usePool: true);
#else
        [HideInInspector] public Stack<object> cache = new();
#endif
        protected Func<object> constructor;
        protected Action<object> desctructor;

#if UNITY_EDITOR
        [HideInInspector] public Type poolType;

        [ShowInInspector]
        [PropertyOrder(0)]
        public string poolName => poolType.PrettyName();

        [ShowInInspector]
        [PropertyOrder(5)]
        public int cacheCount => cache.Count;
#endif

        [ShowInInspector]
        [PropertyOrder(1)]
        public int newAllocatedCurrent;

        [ShowInInspector]
        [PropertyOrder(2)]
        public int spawnedTotal;

        [ShowInInspector]
        [PropertyOrder(3)]
        public int despawnedTotal;

        [ShowInInspector]
        [PropertyOrder(4)]
        public int timesUsedFromCache;

        [ShowInInspector]
        [PropertyOrder(6)]
        public int newAllocatedTotal;

        public static readonly List<PoolInternalBase> list = new();

        public static int staticNewAllocated;
        public static int staticAllocated;
        public static int staticDeallocated;
        public static int staticUsed;

        public static void ClearCaches()
        {
            List<PoolInternalBase> pools = list;

            for (var i = 0; i < pools.Count; ++i)
            {
                PoolInternalBase pool = pools[i];
                pool.ClearCache();
            }
        }

        private void ClearCache()
        {
            newAllocatedCurrent -= cache.Count;
            cache.Clear();
        }

        public void ResetStat()
        {
            spawnedTotal = 0;
            despawnedTotal = 0;
            timesUsedFromCache = 0;
            newAllocatedTotal = 0;
            newAllocatedCurrent = 0;
        }

        public override string ToString()
        {
            return "Allocated: " + spawnedTotal + ", Deallocated: " + despawnedTotal + ", Used: " + timesUsedFromCache +
                   ", cached: " + cache.Count + ", new: " + newAllocatedTotal;
        }

        public PoolInternalBase(Func<object> constructor, Action<object> desctructor)
        {
            this.constructor = constructor;
            this.desctructor = desctructor;

            list.Add(this);
        }

        // public static T Create<T>() where T : new()
        // {
        //     var instance = new T();
        //     CallOnSpawn(instance);
        //
        //     return instance;
        // }

        // public static void CallOnSpawn<T>(T instance)
        // {
        //     if (instance is IPoolableSpawn poolable)
        //     {
        //         poolable.OnSpawn();
        //     }
        // }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Prewarm(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Recycle(Spawn());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual object Spawn()
        {
#if MULTITHREAD_SUPPORT
		    this.cache.TryPop(out object item);
#else
            object item = cache.Count > 0 ? cache.Pop() : null;
#endif

            if (item == null)
            {
                ++staticNewAllocated;
                ++newAllocatedTotal;
                ++newAllocatedCurrent;
            }
            else
            {
                ++staticUsed;
                ++timesUsedFromCache;
            }

            if (constructor != null && item == null)
            {
                item = constructor.Invoke();

#if UNITY_EDITOR
                if (item != null)
                {
                    poolType = item.GetType();
                }
#endif

#if ENABLE_LOG_POOL_ALLOCATE_NEW && UNITY_EDITOR
                Debug.Log($"#Pool# {poolName} allocated new object");
#endif
            }

            // if (item is IPoolableSpawn poolable)
            // {
            //     poolable.OnSpawn();
            // }

            ++spawnedTotal;
            ++staticAllocated;

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Recycle(object instance)
        {
#if UNITY_EDITOR
            if (instance != null)
            {
                poolType = instance.GetType();
            }
#endif

            ++despawnedTotal;
            ++staticDeallocated;

            if (desctructor != null)
            {
                desctructor.Invoke(instance);
            }

            // if (instance is IPoolableRecycle poolable)
            // {
            //     poolable.OnRecycle();
            // }

            cache.Push(instance);
        }
    }
}