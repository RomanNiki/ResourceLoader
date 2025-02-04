using System;
using System.Diagnostics;

namespace Openmygame.LoggerPro.Profiling
{
    public class ProfilingSection : IDisposable
    {
        private const string FormatLog = "For {0} elapsed miliseconds = {1}";
        private readonly string _message;
        private readonly string _tag;
        private readonly Stopwatch _stopwatch;

        public ProfilingSection(string message, string tag)
        {
            _message = message;
            _tag = tag;
            _stopwatch = Stopwatch.StartNew();
            // Profiler.BeginSample(message);
        }

        public void Dispose()
        {
            // Profiler.EndSample();
            _stopwatch.Stop();

#if ENBALE_PROGILING_LOGS
            ProLogger.ForceTagLog(_tag, string.Format(FormatLog, _message, _stopwatch.ElapsedMilliseconds.ToString()));
#else
            ProLogger.TagLog(_tag, string.Format(FormatLog, _message, _stopwatch.ElapsedMilliseconds.ToString()));
#endif
        }
    }
}