using System;
using System.Collections.Generic;
using Openmygame.ConfigurableTextValue.Providers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Openmygame.ConfigurableTextValue.View
{
    [Serializable]
    [InlineProperty]
    public struct ValueTextDropdown : IEquatable<ValueTextDropdown>
    {
        [VerticalGroup]
        [HideLabel]
        [ValueDropdown(nameof(GetKeys))]
        public string value;

        [VerticalGroup]
        [HideLabel]
        [SerializeField]
        [OnValueChanged(nameof(SetupProvider))]
        public ProviderTextKeys providerTextKeysScriptable;

        private IProviderTextKeys _providerTextKeys;

        public bool Equals(ValueTextDropdown other)
        {
            return value == other.value && Equals(providerTextKeysScriptable, other.providerTextKeysScriptable);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(value) && providerTextKeysScriptable == null;
        }

        public override bool Equals(object obj)
        {
            return obj is ValueTextDropdown other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value, providerTextKeysScriptable, _providerTextKeys);
        }

        private IEnumerable<ValueDropdownItem<string>> GetKeys()
        {
            yield return new ValueDropdownItem<string>("None", string.Empty);

            SetupProvider();

            if (_providerTextKeys is null)
            {
                yield break;
            }

            foreach (string key in _providerTextKeys.GetKeys())
            {
                yield return new ValueDropdownItem<string>(key, key);
            }
        }

        private void SetupProvider()
        {
            if (providerTextKeysScriptable is IProviderTextKeys provider)
            {
                _providerTextKeys = provider;
            }
        }

        public void UpdateProvider(IProviderTextKeys provider)
        {
            _providerTextKeys = provider;
        }
    }
}