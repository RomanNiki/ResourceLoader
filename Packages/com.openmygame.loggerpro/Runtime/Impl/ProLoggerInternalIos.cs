using Openmygame.LoggerPro.Tools;
using UnityEngine;

namespace Openmygame.LoggerPro.Impl
{
    public class ProLoggerInternalIos
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void _logToiOS(string debugMessage);
#endif

        [HideInCallstack]
        public static void Log(string value)
        {
            ProLoggerSplitter.LogSplit(LogIosInternal, $"{value}\n{StackTraceUtility.ExtractStackTrace()}");
        }

        [HideInCallstack]
        private static void LogIosInternal(string value)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            _logToiOS(value);
#endif
        }
    }
}