#include <stdio.h>
#include "reader.h"
#include "cache.h"

#define BUFFER_BLOCK_SIZE 10000000

size_t get_file_length(FILE* file_handle) {
	size_t length;
	if (fseek(file_handle, 0L, SEEK_END) != 0) {
#if _DEBUG
		char error[256];
		strerror_s(error, sizeof(error), errno);
		printf("Error seeking file: %s\n", error);
#endif
		return -1;
	}
	length = ftell(file_handle);
	if (fseek(file_handle, 0L, SEEK_SET) != 0) {
#if _DEBUG
		char error[256];
		strerror_s(error, sizeof(error), errno);
		printf("Error seeking file: %s\n", error);
#endif
		return -1;
	}
	return length;
}

BOOL populate_file_buffer(FILE* file_handle, size_t position, const BUFFER* const buffer) {
	size_t length;
	BYTE* const file_buffer = malloc(BUFFER_BLOCK_SIZE);
	if (!file_buffer) {
#if _DEBUG
		printf("Could not allocate temp buffer.\n");
#endif
		return FALSE;
	}
	do {
		length = fread(file_buffer, sizeof(BYTE), BUFFER_BLOCK_SIZE, file_handle);
		if (ferror(file_handle)) {
#if _DEBUG
			char error[256];
			strerror_s(error, sizeof(error), errno);
			printf("Error opening file: %s\n", error);
#endif
			free(file_buffer);
			return FALSE;
		}
		if (!length) {
			free(file_buffer);
			return TRUE;
		}
		if (position + length > buffer->length) {
#if _DEBUG
			printf("Buffer capacity exceeded.");
#endif
			free(file_buffer);
			return TRUE;
		}
		buffer_write(buffer, position, length, file_buffer);
		position += length;
	} while (TRUE);
}

BUFFER* read_file_buffer(const wchar_t* const file, const size_t offset, const size_t length) {
	BUFFER* buffer;
	size_t file_length;
	FILE* file_handle;
	if (cache_acquire(file, &buffer)) {
		return buffer;
	}
	_wfopen_s(&file_handle, file, L"rb");
	if (!file_handle) {
#if _DEBUG
		char error[256];
		strerror_s(error, sizeof(error), errno);
		printf("Error opening file: %s\n", error);
#endif
		return NULL;
	}
	file_length = get_file_length(file_handle);
	if (file_length == -1) {
#if _DEBUG
		printf("Could not determine file length.\n");
#endif
		buffer = NULL;
	}
	else {
		buffer = buffer_create(file_length);
		if (buffer) {
			if (!populate_file_buffer(file_handle, 0, buffer)) {
				buffer_free(buffer);
				buffer = NULL;
			}
		}
	}
	fclose(file_handle);
	if (buffer) {
		cache_add(file, buffer);
	}
	return buffer;
}

BOOL populate_stream_buffer(const HSTREAM handle, size_t position, const BUFFER* const buffer) {
	size_t length;
	BYTE* const stream_buffer = malloc(BUFFER_BLOCK_SIZE);
	if (!stream_buffer) {
#if _DEBUG
		printf("Could not allocate temp buffer.\n");
#endif
		return FALSE;
	}
	do {
		length = BASS_ChannelGetData(handle, stream_buffer, BUFFER_BLOCK_SIZE);
		if (length == BASS_STREAMPROC_END || length == BASS_ERROR_UNKNOWN || length <= 0) {
			free(stream_buffer);
			return TRUE;
		}
		if (position + length > buffer->length) {
#if _DEBUG
			printf("Buffer capacity exceeded.");
#endif
			free(stream_buffer);
			return TRUE;
		}
		buffer_write(buffer, position, length, stream_buffer);
		position += length;
	} while (TRUE);
}

BUFFER* read_stream_buffer(const wchar_t* const file, const WAVE_HEADER* const wave_header, const HSTREAM handle, const size_t offset, const size_t length) {
	BUFFER* buffer;
	size_t stream_length;
	if (cache_acquire(file, &buffer)) {
		return buffer;
	}
	stream_length = (size_t)BASS_ChannelGetLength(handle, BASS_POS_BYTE);
	if (stream_length == -1) {
#if _DEBUG
		printf("Could not determine stream length.\n");
#endif
		buffer = NULL;
	}
	else {
		buffer = buffer_create(sizeof(WAVE_HEADER) + stream_length);
		if (buffer) {
			buffer_write(buffer, 0, sizeof(WAVE_HEADER), (BYTE*)wave_header);
			if (!populate_stream_buffer(handle, sizeof(WAVE_HEADER), buffer)) {
				buffer_free(buffer);
				buffer = NULL;
			}
		}
	}
	if (buffer) {
		cache_add(file, buffer);
	}
	return buffer;
}