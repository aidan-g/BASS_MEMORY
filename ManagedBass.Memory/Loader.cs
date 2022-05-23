using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Memory
{
    public class Loader
    {
        public static readonly string FolderName = Path.Combine(Path.GetDirectoryName(typeof(Loader).Assembly.Location), Environment.Is64BitProcess ? "x64" : "x86");

        public static readonly ConcurrentDictionary<string, IntPtr> Handles = new ConcurrentDictionary<string, IntPtr>(StringComparer.OrdinalIgnoreCase);

        public static bool Load(string fileName)
        {
            var module = default(IntPtr);
            return Load(fileName, out module);
        }

        public static bool Load(string fileName, out IntPtr module)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(FolderName, fileName);
            }
            module = Handles.GetOrAdd(fileName, key => LoadLibrary(fileName));
            if (IntPtr.Zero.Equals(module))
            {
                return false;
            }
            return true;
        }

        public static bool Free(string fileName)
        {
            var module = default(IntPtr);
            if (!Handles.TryRemove(fileName, out module))
            {
                return false;
            }
            if (IntPtr.Zero.Equals(module))
            {
                return false;
            }
            return FreeLibrary(module);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
