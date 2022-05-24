#ifdef _DEBUG
#include <stdio.h>
#endif

#include "../bass/bassdsd.h"

#include "bass_memory_dsd.h"
#include "../bass_memory/memory_stream.h"
#include "../bass_memory/reader.h"

BOOL is_initialized = FALSE;

//I have no idea how to prevent linking against this routine in msvcrt.
//It doesn't exist on Windows XP.
//Hopefully it doesn't do anything important.
int _except_handler4_common() {
	return 0;
}

BOOL BASSMEMORYDEF(BASS_MEMORY_DSD_Init)() {
	if (is_initialized) {
		return FALSE;
	}
	is_initialized = TRUE;
#if _DEBUG
	printf("BASS MEMORY DSD initialized.\n");
#endif
	return TRUE;
}

BOOL BASSMEMORYDEF(BASS_MEMORY_DSD_Free)() {
	if (!is_initialized) {
		return FALSE;
	}
	is_initialized = FALSE;
#if _DEBUG
	printf("BASS MEMORY DSD released.\n");
#endif
	return TRUE;
}

HSTREAM BASSMEMORYDEF(_BASS_DSD_StreamCreateFileUser)(DWORD system, DWORD flags, const BASS_FILEPROCS* procs, void* user) {
	return BASS_DSD_StreamCreateFileUser(system, flags, procs, user, 0);
}

HSTREAM BASSMEMORYDEF(BASS_MEMORY_DSD_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags) {
	MEMORY_STREAM* stream;
	BUFFER* buffer;
	buffer = read_file_buffer((const wchar_t*)file, offset, length);
	if (buffer) {
		stream = memory_stream_create((const wchar_t*)file, buffer, &_BASS_DSD_StreamCreateFileUser, flags);
		if (stream) {
			return stream->handle;
		}
	}
	return 0;
}

HSTREAM BASSMEMORYDEF(BASS_MEMORY_DSD_StreamCreate)(HSTREAM handle, QWORD offset, QWORD length, DWORD flags) {
	MEMORY_STREAM* stream;
	BUFFER* buffer;
	wchar_t file[MAX_PATH + 1];
	WAVE_HEADER wave_header;
	if (create_wave_header(handle, &wave_header)) {
		swprintf_s(file, sizeof(file), L"%u.wav", handle);
		buffer = read_stream_buffer((const wchar_t*)file, wave_header, handle, offset, length);
		if (buffer) {
			stream = memory_stream_create((const wchar_t*)file, buffer, &_BASS_DSD_StreamCreateFileUser, flags);
			if (stream) {
				return stream->handle;
			}
		}
	}
	return 0;
}