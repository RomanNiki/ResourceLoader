namespace Openmygame.LoggerPro.Profiling
{
    public static class ProfilingTool
    {
        public static ProfilingSection StartSection(string message, string tag)
        {
            return new ProfilingSection(message, tag);
        }
    }
}