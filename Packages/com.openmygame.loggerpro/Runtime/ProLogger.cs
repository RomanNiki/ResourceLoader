using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Openmygame.LoggerPro.Config;
using Openmygame.LoggerPro.Impl;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Openmygame.LoggerPro
{
    public class ProLogger
    {
        private const string Space = " ";
        public const string ConditionMacros = "PRO_LOGGER";
        private static IConfigLoggerPro _configLoggerPro;

        public static void SetConfig(IConfigLoggerPro configLoggerPro)
        {
            _configLoggerPro = configLoggerPro;
        }

        [HideInCallstack]
        [Conditional(ConditionMacros)]
        public static void Error(string value)
        {
            Debug.LogError(value);
        }

        [HideInCallstack]
        public static void Exception(Exception e)
        {
            Debug.LogException(e);
        }

        [HideInCallstack]
        public static void Exception(string value)
        {
            Exception(new Exception(value));
        }

        [HideInCallstack]
        [Conditional(ConditionMacros)]
        public static void Log(params object[] values)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < values.Length; i++)
            {
                sb.Append(values[i]);

                if (i < values.Length - 1)
                {
                    sb.Append(Space);
                }
            }

            LogForce(sb.ToString());
        }

        [HideInCallstack]
        [Conditional(ConditionMacros)]
        public static void TagLog(string tag, string value)
        {
            if (!CanLogTag(tag))
            {
                return;
            }

            InternalLogTag(tag, $"{value}");
        }

        [HideInCallstack]
        public static void ForceTagLog(string tag, string value)
        {
            InternalLogTag(tag, value);
        }

        [HideInCallstack]
        private static void InternalLogTag(string tag, string value)
        {
            LogForce($"#{tag}# {value}");
        }

        [HideInCallstack]
        private static bool CanLogTag(string tag)
        {
            return _configLoggerPro is null || _configLoggerPro.CanLogTag(tag);
        }

        [HideInCallstack]
        [Conditional(ConditionMacros)]
        public static void TagLog(string tag, params object[] values)
        {
            if (!CanLogTag(tag))
            {
                return;
            }

            var sb = new StringBuilder($"#{tag}# ");

            for (var i = 0; i < values.Length; i++)
            {
                sb.Append(values[i]);

                if (i < values.Length - 1)
                {
                    sb.Append(Space);
                }
            }

            LogForce(sb.ToString());
        }

        [HideInCallstack]
        public static string Pretty(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        [HideInCallstack]
        [Conditional(ConditionMacros)]
        public static void Warning(string value)
        {
            Debug.LogWarning(value);
        }

        [HideInCallstack]
        public static void LogForce(string value)
        {
#if UNITY_EDITOR
            LogEditor(value);
            return;
#endif

#if UNITY_IPHONE
                LogIos(value);
#endif

#if UNITY_ANDROID
            LogAndroid(value);
#endif
        }

        [HideInCallstack]
        private static void LogAndroid(string value)
        {
            ProLoggerInternalAndroid.Log(value);
        }

        [Conditional(ConditionMacros)]
        [HideInCallstack]
        private static void LogIos(string value)
        {
            ProLoggerInternalIos.Log(value);
        }

        [HideInCallstack]
        private static void LogEditor(string value)
        {
            Debug.Log(value);
        }
    }
}