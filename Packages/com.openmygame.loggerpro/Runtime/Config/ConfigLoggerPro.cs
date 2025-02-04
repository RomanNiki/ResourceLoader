using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Openmygame.LoggerPro.Config
{
    [CreateAssetMenu(menuName = "Infrastructure/LoggerPro/Config ", fileName = "LoggerPro")]
    public class ConfigLoggerPro : SerializedScriptableObject, IConfigLoggerPro
    {
        [SerializeField] private bool defaultAllTagsAllowed = true;
        [SerializeField] private bool autoFillOnUsage = true;

        [Searchable]
        [PropertyOrder(-2)]
        [DictionaryDrawerSettings(KeyLabel = "Tag", ValueLabel = "Enabled")]
        [OdinSerialize]
        private readonly Dictionary<string, bool> tagsContainerLogs = new();

        public bool CanLogTag(string tag)
        {
            if (tagsContainerLogs is null)
            {
                return true;
            }

            if (tagsContainerLogs.TryGetValue(tag, out bool value))
            {
                return value;
            }

            if (autoFillOnUsage)
            {
                UpdateTagUsage(tag);
            }

            return defaultAllTagsAllowed;
        }

        [Button("Check All")]
        [HorizontalGroup("Controls")]
        private void ReCheckAll()
        {
            UpdateEnabled(true);
        }

        [HorizontalGroup("Controls")]
        [Button("Uncheck All")]
        private void UnCheckAll()
        {
            UpdateEnabled(false);
        }

        [Button("Clear All")]
        [HorizontalGroup("Controls")]
        private void ClearAll()
        {
            tagsContainerLogs.Clear();
        }

        private void UpdateEnabled(bool value)
        {
            foreach (string keyValue in tagsContainerLogs.Keys.ToList())
            {
                tagsContainerLogs[keyValue] = value;
            }
        }

        private void UpdateTagUsage(string tag)
        {
            if (tagsContainerLogs.ContainsKey(tag))
            {
                return;
            }

            tagsContainerLogs[tag] = defaultAllTagsAllowed;
        }
    }
}