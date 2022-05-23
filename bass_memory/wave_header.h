#include "../bass/bass.h"

typedef struct {
	char riff[4];
	int file_size;
	char wave[4];

	char fmt[4];
	int fmt_size;
	short format;
	short channels;
	int sample_rate;
	int byte_rate;
	short block_alignment;
	short depth;

	char data[4];
	int data_size;
} WAVE_HEADER;

void create_pcm_header(int channels, int rate, int length, WAVE_HEADER* header);