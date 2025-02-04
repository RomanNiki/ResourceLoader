using System.Collections.Generic;
using UnityEngine;

namespace Openmygame.ConfigurableTextValue.Providers
{
    public abstract class ProviderTextKeys : ScriptableObject, IProviderTextKeys
    {
        public abstract IEnumerable<string> GetKeys();
    }
}