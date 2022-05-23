#include "buffer.h"

typedef struct {
	const wchar_t file[MAX_PATH + 1];
	QWORD position;
	BUFFER* buffer;
	DWORD handle;
} MEMORY_STREAM;

typedef HSTREAM(CALLBACK MEMORY_STREAM_HANDLER)(DWORD system, DWORD flags, const BASS_FILEPROCS *proc, void *user);

MEMORY_STREAM* memory_stream_create(const wchar_t* file, BUFFER* buffer, const MEMORY_STREAM_HANDLER* handler, DWORD flags);

void memory_stream_free(MEMORY_STREAM* stream);