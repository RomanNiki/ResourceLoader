using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Openmygame.ConfigurableTextValue.Providers
{
    [CreateAssetMenu(menuName = "ValueDropdown/ProviderListItems", fileName = "ProviderKey")]
    public class ProviderTextKeysFromList : ProviderTextKeys
    {
        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false)]
        [SerializeField]
        private List<string> ids = new();

        [ListDrawerSettings(ShowFoldout = false, ShowPaging = false)]
        [SerializeField]
        private List<ProviderTextKeys> providers = new();

        public override IEnumerable<string> GetKeys()

        {
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
    }
}