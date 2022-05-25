#include "../bass_memory/bass_memory.h"

__declspec(dllexport)
BOOL BASSMEMORYDEF(BASS_MEMORY_DSD_Init)();

__declspec(dllexport)
BOOL BASSMEMORYDEF(BASS_MEMORY_DSD_Free)();

__declspec(dllexport)
HSTREAM BASSMEMORYDEF(BASS_MEMORY_DSD_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags);

__declspec(dllexport)
QWORD BASSMEMORYDEF(BASS_MEMORY_DSD_Usage)();