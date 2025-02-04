using System;

namespace Openmygame.Infrastructure.Architecture.Containers
{
    public interface IProviderValue<out T>
    {
        T Value { get; }

        event Action<T> OnChanged;
    }
}