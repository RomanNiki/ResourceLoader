using System;
using System.Collections.Generic;
using Openmygame.ConfigurableTextValue.Providers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Tools.ConfigurableTextValue.Providers
{
    [CreateAssetMenu(menuName = "ProviderConstKeys", fileName = "Const")]
    public class ProviderConstKeys : ProviderTextKeys
    {
        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false)]
        [SerializeField]
        private List<ConfigItemsPrefix> keysPrefixed = new();

        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false)]
        [SerializeField]
        private List<string> ids = new();

        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false)]
        [SerializeField]
        private List<ProviderTextKeys> providers = new();

        public override IEnumerable<string> GetKeys()
        {
            if (keysPrefixed != null)
            {
                foreach (ConfigItemsPrefix configItemsPrefix in keysPrefixed)
                {
                    ProviderTextKeys provider = configItemsPrefix.keysUnderPrefix;

                    if (provider != null)
                    {
                        string prefix = configItemsPrefix.prefix;

                        yield return prefix;

                        foreach (string key in provider.GetKeys())
                        {
                            yield return $"{prefix}/{key}";
                        }
                    }
                }
            }

            if (ids != null)
            {
                foreach (string id in ids)
                {
                    yield return id;
                }
            }

            if (providers != null)
            {
                foreach (ProviderTextKeys provider in providers)
                {
                    if (provider != null)
                    {
                        foreach (string key in provider.GetKeys())
                        {
                            yield return key;
                        }
                    }
                }
            }
        }

        public void UpdateIds(IEnumerable<string> values)
        {
            ids.Clear();
            ids.AddRange(values);

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        [Serializable]
        [InlineProperty]
        private struct ConfigItemsPrefix
        {
            public string prefix;
            public ProviderTextKeys keysUnderPrefix;
        }
    }
}