using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Memory
{
    public class BassMemory
    {
        const string DllName = "bass_memory";

        public static int Module = 0;

        public static bool Load(string folderName = null)
        {
            if (Module == 0)
            {
                var fileName = default(string);
                if (!string.IsNullOrEmpty(folderName))
                {
                    fileName = Path.Combine(folderName, DllName);
                }
                else
                {
                    fileName = Path.Combine(Loader.FolderName, DllName);
                }
                Module = Bass.PluginLoad(string.Format("{0}.{1}", fileName, Loader.Extension));
            }
            return Module != 0;
        }

        public static bool Unload()
        {
            if (Module != 0)
            {
                if (!Bass.PluginFree(Module))
                {
                    return false;
                }
                Module = 0;
            }
            return true;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MEMORY_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_MEMORY_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        [DllImport(DllName)]
        static extern int BASS_MEMORY_StreamCreate(int handle, long offset, long length, BassFlags flags);

        public static int CreateStream(int handle, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_MEMORY_StreamCreate(handle, Offset, Length, Flags | BassFlags.Unicode);
        }

        [DllImport(DllName)]
        static extern long BASS_MEMORY_Usage();

        public static long Usage()
        {
            return BASS_MEMORY_Usage();
        }

        public class Dsd
        {
            const string DllName = "bass_memory_dsd";

            public static int Module = 0;

            public static bool Load(string folderName = null)
            {
                if (Module == 0)
                {
                    var fileName = default(string);
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        fileName = Path.Combine(folderName, DllName);
                    }
                    else
                    {
                        fileName = Path.Combine(Loader.FolderName, DllName);
                    }
                    Module = Bass.PluginLoad(string.Format("{0}.{1}", fileName, Loader.Extension));
                }
                return Module != 0;
            }

            public static bool Unload()
            {
                if (Module != 0)
                {
                    if (!Bass.PluginFree(Module))
                    {
                        return false;
                    }
                    Module = 0;
                }
                return true;
            }

            [DllImport(DllName, CharSet = CharSet.Unicode)]
            static extern int BASS_MEMORY_DSD_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

            public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
            {
                return BASS_MEMORY_DSD_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
            }

            [DllImport(DllName)]
            static extern long BASS_MEMORY_DSD_Usage();

            public static long Usage()
            {
                return BASS_MEMORY_DSD_Usage();
            }
        }
    }
}
