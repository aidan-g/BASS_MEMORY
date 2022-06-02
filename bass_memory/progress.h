#pragma once
#include "../bass/bass.h"

#define PROGRESS_BEGIN -1

#define PROGRESS_END -2

typedef struct {
	const wchar_t file[MAX_PATH + 1];
	QWORD position;
	QWORD length;
} BASS_MEMORY_PROGRESS;

void progress_handler(VOID(*progress)(BASS_MEMORY_PROGRESS* progress));

void progress_begin(const wchar_t* const file);

void progress_update(const wchar_t* const file, const QWORD position, const QWORD length);

void progress_end(const wchar_t* const file);