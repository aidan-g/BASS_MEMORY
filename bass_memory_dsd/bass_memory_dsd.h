#include "../bass_memory/bass_memory.h"

BOOL BASSMEMORYDEF(DllMain)(HANDLE dll, DWORD reason, LPVOID reserved);

const VOID* BASSMEMORYDEF(BASSplugin)(DWORD face);

HSTREAM BASSMEMORYDEF(BASS_MEMORY_DSD_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags);

QWORD BASSMEMORYDEF(BASS_MEMORY_DSD_Usage)();