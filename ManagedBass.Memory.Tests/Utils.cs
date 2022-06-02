using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ManagedBass.Memory.Tests
{
    public static class ProgressHandler
    {
        public static ConcurrentDictionary<string, IList<BassMemoryProgress>> Events = new ConcurrentDictionary<string, IList<BassMemoryProgress>>(StringComparer.OrdinalIgnoreCase);

        public static void Attach(Action<BassMemoryProgressHandler> source)
        {
            source(Progress);
            Events.Clear();
        }

        public static void Detach(Action<BassMemoryProgressHandler> source)
        {
            source(null);
            Events.Clear();
        }

        public static void Progress(ref BassMemoryProgress progress)
        {
            Events.GetOrAdd(progress.File, key => new List<BassMemoryProgress>()).Add(progress);
        }
    }
}
