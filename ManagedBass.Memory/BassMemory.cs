using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Memory
{
    public class BassMemory
    {
        const string DllName = "bass_memory";

        static BassMemory()
        {
            Module = IntPtr.Zero;
        }

        public static IntPtr Module;

        /// <summary>
        /// Load the library.
        /// </summary>
        /// <returns></returns>
        public static bool Load()
        {
            return Loader.Load(DllName, out Module);
        }

        /// <summary>
        /// Unload the library.
        /// </summary>
        /// <returns></returns>
        public static bool Unload()
        {
            try
            {
                return Loader.Free(DllName);
            }
            finally
            {
                Module = IntPtr.Zero;
            }
        }

        [DllImport(DllName)]
        static extern bool BASS_MEMORY_Init();

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            try
            {
                return BASS_MEMORY_Init();
            }
            catch (DllNotFoundException)
            {
                if (IntPtr.Zero.Equals(Module))
                {
                    if (Load())
                    {
                        return BASS_MEMORY_Init();
                    }
                }
                return false;
            }
        }

        [DllImport(DllName)]
        static extern bool BASS_MEMORY_Free();

        /// <summary>
        /// Free.
        /// </summary>
        /// <returns></returns>
        public static bool Free()
        {
            return BASS_MEMORY_Free();
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

        public class Dsd
        {
            const string DllName = "bass_memory_dsd";

            static Dsd()
            {
                Module = IntPtr.Zero;
            }

            public static IntPtr Module;

            /// <summary>
            /// Load the library.
            /// </summary>
            /// <returns></returns>
            public static bool Load()
            {
                return Loader.Load(DllName, out Module);
            }

            /// <summary>
            /// Unload the library.
            /// </summary>
            /// <returns></returns>
            public static bool Unload()
            {
                try
                {
                    return Loader.Free(DllName);
                }
                finally
                {
                    Module = IntPtr.Zero;
                }
            }

            [DllImport(DllName)]
            static extern bool BASS_MEMORY_DSD_Init();

            /// <summary>
            /// Initialize.
            /// </summary>
            /// <returns></returns>
            public static bool Init()
            {
                try
                {
                    return BASS_MEMORY_DSD_Init();
                }
                catch (DllNotFoundException)
                {
                    if (IntPtr.Zero.Equals(Module))
                    {
                        if (Load())
                        {
                            return BASS_MEMORY_DSD_Init();
                        }
                    }
                    return false;
                }
            }

            [DllImport(DllName)]
            static extern bool BASS_MEMORY_DSD_Free();

            /// <summary>
            /// Free.
            /// </summary>
            /// <returns></returns>
            public static bool Free()
            {
                return BASS_MEMORY_DSD_Free();
            }

            [DllImport(DllName, CharSet = CharSet.Unicode)]
            static extern int BASS_MEMORY_DSD_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

            public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
            {
                return BASS_MEMORY_DSD_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
            }

            [DllImport(DllName)]
            static extern int BASS_MEMORY_DSD_StreamCreate(int handle, long offset, long length, BassFlags flags);

            public static int CreateStream(int handle, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
            {
                return BASS_MEMORY_DSD_StreamCreate(handle, Offset, Length, Flags | BassFlags.Unicode);
            }
        }
    }
}
