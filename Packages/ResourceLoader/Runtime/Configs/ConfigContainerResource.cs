using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Configs
{
    public class ConfigContainerResource : SerializedScriptableObject
    {
        public List<ContainerResource> resources = new();
    }

    [Serializable]
    [InlineProperty]
    public class ContainerResource
    {
        public string name;

        [InlineProperty]
        [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true, ShowPaging = false)]
        [OdinSerialize]
        public Dictionary<string, object> Values = new();
    }
}