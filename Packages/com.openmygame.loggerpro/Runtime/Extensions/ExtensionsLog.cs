using System.Diagnostics;

namespace Openmygame.LoggerPro.Extensions
{
    public static class ExtensionsLog
    {
        [Conditional(ProLogger.ConditionMacros)]
        public static void ProLog(this object obj, string message)
        {
            ProLogger.TagLog(obj.GetType().Name, message);
        }
    }
}