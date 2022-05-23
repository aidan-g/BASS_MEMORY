#include "buffer.h"
#include "wave_header.h"

BUFFER* read_file_buffer(const wchar_t* file, QWORD offset, QWORD length);

BUFFER* read_stream_buffer(const wchar_t* file, WAVE_HEADER wave_header, HSTREAM handle, QWORD offset, QWORD length);