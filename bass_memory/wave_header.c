#include "wave_header.h"

void create_pcm_header(short format, int channels, int rate, int length, WAVE_HEADER* header) {

	short sample_size;
	switch (format) {
	default:
	case PCM_FORMAT_INT:
		sample_size = 4;
		break;
	case PCM_FORMAT_FLOAT:
		sample_size = 8;
		break;
	}

	header->riff[0] = 'R';
	header->riff[1] = 'I';
	header->riff[2] = 'F';
	header->riff[3] = 'F';
	header->file_size = sizeof(WAVE_HEADER) + length;
	header->fmt[0] = 'f';
	header->fmt[1] = 'm';
	header->fmt[2] = 't';
	header->fmt[3] = ' ';
	header->fmt_size = 16;
	header->format = format;
	header->channels = channels;
	header->sample_rate = rate;
	header->byte_rate = rate * channels * sample_size;
	header->block_alignment = channels * header->byte_rate;
	header->depth = sample_size * 8;

	header->data_size = length;
}