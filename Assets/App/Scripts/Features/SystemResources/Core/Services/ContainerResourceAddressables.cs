using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [Serializable]
    public class ContainerResourceAddressables : IContainerResources
    {
        public string name;

        [InlineProperty]
        [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true, ShowPaging = false)]
        [OdinSerialize]
        public Dictionary<string, AssetReference> Values = new();

        public string Name => name;


        public List<KeyValuePair<string, object>> GetValues()
        {
            Values ??= new Dictionary<string, AssetReference>();
            List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
           
            foreach (var pair in Values)
            {
                list.Add(new KeyValuePair<string, object>(pair.Key, pair.Value));
            }

            return list;
        }
    }
}