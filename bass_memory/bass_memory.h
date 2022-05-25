#include "../bass/bass.h"

#ifndef BASSMEMORYDEF
#define BASSMEMORYDEF(f) WINAPI f
#endif

__declspec(dllexport)
BOOL BASSMEMORYDEF(BASS_MEMORY_Init)();

__declspec(dllexport)
BOOL BASSMEMORYDEF(BASS_MEMORY_Free)();

__declspec(dllexport)
HSTREAM BASSMEMORYDEF(BASS_MEMORY_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags);

__declspec(dllexport)
HSTREAM BASSMEMORYDEF(BASS_MEMORY_StreamCreate)(HSTREAM handle, QWORD offset, QWORD length, DWORD flags);

__declspec(dllexport)
QWORD BASSMEMORYDEF(BASS_MEMORY_Usage)();