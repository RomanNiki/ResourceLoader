using System;
using System.Collections.Generic;
using System.Linq;
using CodeGeneration;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Tools.Attributes.TabGroupCollection;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ConfigContainerResource", menuName = "ResourceLoader/ConfigContainerResource")]
    public class ConfigContainerResource : SerializedScriptableObject
    {
        [TabGroup("Tools", "Resources", SdfIconType.Controller, TextColor = "lightyellow", Order = 0)]
        [Searchable]
        [HideLabel]
        [OdinSerialize]
        [TableList(ShowPaging = false)]
        [ListDrawerSettings(
            ListElementLabelName = nameof(GetElementName),
            ShowPaging = true,
            ShowIndexLabels = false)]
        [TabGroupCollection(NavigationTabAmount = 100, MaximumNumberOfTabs = 100)]
        public List<IContainerResources> Resources = new();

        private string GetElementName(List<IContainerResources> list, int index)
        {
            string elementName = list[index].Name;
            return string.IsNullOrEmpty(elementName) ? $"Empty [{index}]" : elementName;
        }

#if UNITY_EDITOR
        [TabGroup("Tools", "Toolbox", SdfIconType.Tools, TextColor = "green", Order = 2)]
        [TitleGroup("Tools/Toolbox/Const")]
        [HorizontalGroup("Tools/Toolbox/Const/Hor")]
        [VerticalGroup("Tools/Toolbox/Const/Hor/Ver")]
        [SerializeField]
        private ConstantGeneratorKeys constantGeneratorKeys;

        [TabGroup("Tools", "Toolbox", SdfIconType.Tools, TextColor = "green", Order = 2)]
        [TitleGroup("Tools/Toolbox/Const")]
        [HorizontalGroup("Tools/Toolbox/Const/Hor")]
        [VerticalGroup("Tools/Toolbox/Const/Hor/Ver")]
        [SerializeField]
        private ConstantGeneratorKeys constantGeneratorCategories;

        [HorizontalGroup("Tools/Toolbox/Const/Ver")]
        [Button("Generate constants", ButtonHeight = 40)]
        private void GenerateConstants()
        {
            constantGeneratorKeys.GenerateConstantFile(GetKeys(Resources));
            constantGeneratorCategories.GenerateConstantFile(GetKeyCategories(Resources));
        }

        private static List<string> GetKeyCategories(IEnumerable<IContainerResources> containersResources)
        {
            var keys = new List<string>();

            foreach (var containerResources in containersResources)
            {
                keys.Add(containerResources.Name);
            }

            return keys;
        }

        private static List<string> GetKeys(IEnumerable<IContainerResources> containersResources)
        {
            var keys = new List<string>();

            foreach (IContainerResources container in containersResources)
            {
                foreach (KeyValuePair<string, object> resource in container.GetValues())
                {
                    keys.Add(resource.Key);
                }
            }

            return keys;
        }
#endif
    }

    [Serializable]
    [InlineProperty]
    public class ContainerResource : IContainerResources
    {
        public string name;

        [InlineProperty]
        [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true, ShowPaging = false)]
        [OdinSerialize]
        public Dictionary<string, object> Values = new();

        public string Name => name;

        public List<KeyValuePair<string, object>> GetValues()
        {
            Values ??= new Dictionary<string, object>();
            return Values.ToList();
        }
    }
}