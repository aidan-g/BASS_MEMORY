# BASS_MEMORY
An in memory playback plugin for BASS with .NET bindings.

bass.dll is required for native projects.
ManagedBass is required for .NET projects.

A simple example;

```c#
Bass.Init();
BassMemory.Init();

var sourceChannel = BassMemory.CreateStream(fileName);
Bass.ChannelPlay(sourceChannel);

while (Bass.ChannelIsActive(sourceChannel) == PlaybackState.Playing)
{
	global::System.Threading.Thread.Sleep(1000);
}

Bass.StreamFree(sourceChannel);
BassMemory.Free();
Bass.Free();
```

A memory stream can also be created from an existing channel, this will buffer the data uncompressed (as PCM):

```c#
var sourceChannel = Bass.CreateStream(fileName);
var memoryChannel = BassMemory.CreateStream(sourceChannel);
```

Uncompressed channels have a size limit of ~4GB due to the internal WAVE format being 32 bit.