#ifdef _DEBUG
#include <stdio.h>
#endif

#include "bass_memory.h"
#include "cache.h"
#include "memory_stream.h"
#include "reader.h"

BOOL is_initialized = FALSE;

//I have no idea how to prevent linking against this routine in msvcrt.
//It doesn't exist on Windows XP.
//Hopefully it doesn't do anything important.
int _except_handler4_common() {
	return 0;
}

BOOL BASSMEMORYDEF(BASS_MEMORY_Init)() {
	if (is_initialized) {
		return FALSE;
	}
	is_initialized = TRUE;
#if _DEBUG
	printf("BASS MEMORY initialized.\n");
#endif
	return TRUE;
}

BOOL BASSMEMORYDEF(BASS_MEMORY_Free)() {
	if (!is_initialized) {
		return FALSE;
	}
	is_initialized = FALSE;
#if _DEBUG
	printf("BASS MEMORY released.\n");
#endif
	return TRUE;
}

HSTREAM BASSMEMORYDEF(BASS_MEMORY_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags) {
	MEMORY_STREAM* stream;
	BUFFER* buffer;
	buffer = read_file_buffer((const wchar_t*)file, offset, length);
	if (buffer) {
		stream = memory_stream_create((const wchar_t*)file, buffer, &BASS_StreamCreateFileUser, flags);
		if (stream) {
			return stream->handle;
		}
	}
	return 0;
}

HSTREAM BASSMEMORYDEF(BASS_MEMORY_StreamCreate)(HSTREAM handle, QWORD offset, QWORD length, DWORD flags) {
	MEMORY_STREAM* stream;
	BUFFER* buffer;
	wchar_t file[MAX_PATH + 1];
	WAVE_HEADER wave_header;
	if (create_wave_header(handle, &wave_header)) {
		swprintf_s(file, sizeof(file), L"%u.wav", handle);
		buffer = read_stream_buffer((const wchar_t*)file, wave_header, handle, offset, length);
		if (buffer) {
			stream = memory_stream_create((const wchar_t*)file, buffer, &BASS_StreamCreateFileUser, flags);
			if (stream) {
				return stream->handle;
			}
		}
	}
	return 0;
}

QWORD BASSMEMORYDEF(BASS_MEMORY_Usage)() {
	return cache_size();
}