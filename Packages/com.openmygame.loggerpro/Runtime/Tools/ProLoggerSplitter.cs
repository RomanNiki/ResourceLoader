using System;
using System.Collections.Generic;
using Memory.Pools;
using UnityEngine;

namespace Openmygame.LoggerPro.Tools
{
    public static class ProLoggerSplitter
    {
        private const int MaxSizeOnePart = 789;

        [HideInCallstack]
        public static void LogSplit(Action<string> log, string message)
        {
            if (message.Length <= MaxSizeOnePart)
            {
                log.Invoke(message);
                return;
            }

            List<string> partsLog = PoolList<string>.Spawn();
            partsLog.Clear();

            GetPartsLog(message, partsLog);
            bool delimiter = partsLog.Count > 1;

            ShowDelimiter(log, delimiter, true);

            for (var i = 0; i < partsLog.Count; i++)
            {
                string partLog = partsLog[i];

                if (delimiter)
                {
                    log.Invoke($"Part {i + 1}/{partsLog.Count}: {partLog}");
                    continue;
                }

                log.Invoke(partLog);
            }

            ShowDelimiter(log, delimiter, false);

            partsLog.Clear();
            PoolList<string>.Recycle(partsLog);
        }

        private static void GetPartsLog(string message, List<string> partsLog)
        {
            ReadOnlySpan<char> span = message.AsSpan();

            int length = span.Length;
            int size = length / MaxSizeOnePart;

            if (length % MaxSizeOnePart != 0)
            {
                size++;
            }

            for (var i = 0; i < size; i++)
            {
                int indexStartPart = i * MaxSizeOnePart;
                int lengthPart = i + 1 == size ? length - indexStartPart : MaxSizeOnePart;
                partsLog.Add(span.Slice(indexStartPart, lengthPart).ToString());
            }
        }

        [HideInCallstack]
        private static void ShowDelimiter(Action<string> log, bool delimiter, bool isStart)
        {
            if (!delimiter)
            {
                return;
            }

            log.Invoke(
                $"{(isStart ? "" : "<")}================================================={(isStart ? ">" : "")}");
        }
    }
}