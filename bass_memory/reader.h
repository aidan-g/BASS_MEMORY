#include "buffer.h"

BUFFER* read_file_buffer(const wchar_t* file, QWORD offset, QWORD length);

BUFFER* read_stream_buffer(HSTREAM handle, QWORD offset, QWORD length);