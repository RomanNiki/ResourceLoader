using System;
using System.Collections.Generic;
using System.Linq;

namespace Memory.Pools
{
    public static class ExtensionsType
    {
        private static readonly Dictionary<ItemImplementsInterface, bool> _mapImplementsInterface = new();

        public static IEnumerable<Type> GetTypes<T>()
        {
            Type typeInterface = typeof(T);

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && p.ImplementsInterface(typeInterface));

            foreach (Type t in types)
            {
                yield return t;
            }
        }

        public static string PrettyName(this Type type)
        {
            // if (type.GetGenericArguments().Length == 0)
            // {
            //     return type.Name;
            // }
            //
            // Type[] genericArguments = type.GetGenericArguments();
            // string typeDefinition = type.Name;
            // int index = typeDefinition.IndexOf("`", StringComparison.Ordinal);
            //
            //
            // string unmangledName = typeDefinition[..index];
            // return unmangledName + "<" + string.Join(",", genericArguments.Select(PrettyName)) + ">";

            // Use the type's name and format it
            string typeName = type.Name;

            // Handle generic types
            if (type.IsGenericType)
            {
                int indexOf = typeName.IndexOf('`');
                if (indexOf != -1)
                {
                    typeName = typeName.Substring(0, indexOf);
                    typeName += $"<{string.Join(", ", type.GetGenericArguments().Select(PrettyName))}>";
                }
            }

            // Optional: Handle arrays
            if (type.IsArray)
            {
                typeName = type.GetElementType().PrettyName() + "[]";
            }

            return typeName;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            var itemImplementsInterface = new ItemImplementsInterface(type, interfaceType);

            if (_mapImplementsInterface.TryGetValue(itemImplementsInterface, out bool value))
            {
                return value;
            }

            value = type.GetInterface(interfaceType.FullName) != null;
            _mapImplementsInterface[itemImplementsInterface] = value;

            return value;
        }

        private readonly struct ItemImplementsInterface : IEquatable<ItemImplementsInterface>
        {
            private readonly Type _type;
            private readonly Type _interfaceType;

            public ItemImplementsInterface(Type type, Type interfaceType)
            {
                _type = type;
                _interfaceType = interfaceType;
            }

            public bool Equals(ItemImplementsInterface other)
            {
                return Equals(_type, other._type) && Equals(_interfaceType, other._interfaceType);
            }

            public override bool Equals(object obj)
            {
                return obj is ItemImplementsInterface other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(_type, _interfaceType);
            }
        }
    }
}