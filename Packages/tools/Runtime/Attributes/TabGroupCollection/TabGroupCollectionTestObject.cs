using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools.Attributes.TabGroupCollection
{
    [HideMonoScript]
    public class TabGroupCollectionTestObject : SerializedScriptableObject
    {
        [SerializeField]
        [LabelText("Test List")]
        [HideLabel]
        [TabGroupCollection(MaximumNumberOfTabs = 5, NavigationTabAmount = 1)]
        [ListDrawerSettings(ListElementLabelName = nameof(GetElementName), ShowPaging = true, ShowIndexLabels = false)]
        private List<MyStruct> testListStructs = new();

        private string GetElementName(List<MyStruct> list, int index)
        {
            return string.IsNullOrEmpty(list[index].name) ? $"null-{index}" : list[index].name;
        }

        private struct MyStruct
        {
            public string name;
            public string value;
        }
    }
}