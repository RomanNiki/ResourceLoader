using Openmygame.LoggerPro.Tools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Openmygame.LoggerPro.Impl
{
    public class ProLoggerInternalAndroid
    {
        [HideInCallstack]
        public static void Log(string value)
        {
            ProLoggerSplitter.LogSplit(Debug.Log, value);
        }
    }
}