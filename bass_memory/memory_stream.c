#if _DEBUG
#include <stdio.h>
#endif

#include "memory_stream.h"
#include "cache.h"

void CALLBACK memory_stream_close(void* user) {
	MEMORY_STREAM* stream = user;
	if (cache_release(stream->file)) {
		buffer_free(stream->buffer);
		stream->buffer = NULL;
	}
	memory_stream_free(stream);
}

QWORD CALLBACK memory_stream_length(void* user) {
	MEMORY_STREAM* stream = user;
	return stream->buffer->length;
}

DWORD CALLBACK memory_stream_read(void* buffer, DWORD length, void* user) {
	MEMORY_STREAM* stream = user;
	QWORD result = stream->buffer->length - stream->position;
	if (result >= length) {
		result = length;
	}
	buffer_read(stream->buffer, stream->position, result, buffer);
	stream->position += result;
	if (result < length) {
#if _DEBUG
		printf("Buffer under run while writing to buffer.\n");
#endif
	}
	return (DWORD)result;
}

BOOL CALLBACK memory_stream_seek(QWORD offset, void* user) {
	MEMORY_STREAM* stream = user;
	if (offset >= 0 && offset <= stream->buffer->length) {
		stream->position = offset;
		return TRUE;
	}
#if _DEBUG
	printf("Seek offset is invalid: %d.\n", offset);
#endif
	return FALSE;
}

const BASS_FILEPROCS memory_stream_procs = {
	&memory_stream_close,
	&memory_stream_length,
	&memory_stream_read,
	&memory_stream_seek
};

MEMORY_STREAM* memory_stream_create(const wchar_t* file, BUFFER* buffer, MEMORY_STREAM_HANDLER* handler, DWORD flags) {
	MEMORY_STREAM* stream = calloc(sizeof(MEMORY_STREAM), 1);
	if (!stream) {
#if _DEBUG
		printf("Failed to allocate stream.\n");
#endif
		return stream;
	}
	wcscpy_s((wchar_t*)stream->file, sizeof(stream->file), file);
	stream->buffer = buffer;
	stream->handle = handler(STREAMFILE_NOBUFFER, flags, &memory_stream_procs, stream);
	if (!stream->handle) {
#if _DEBUG
		printf("Failed to create stream.\n");
#endif
		//TODO: It looks like memory_stream_close has already been called if the handler fails.
		//memory_stream_free(stream);
		return NULL;
	}
	return stream;
}

void memory_stream_free(MEMORY_STREAM* stream) {
	if (stream) {
		free(stream);
		stream = NULL;
	}
}