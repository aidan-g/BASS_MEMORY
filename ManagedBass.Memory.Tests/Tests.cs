using NUnit.Framework;
using System;
using System.IO;

namespace ManagedBass.Memory.Tests
{
    [TestFixture]
    public class Tests
    {
        private static readonly string Location = Path.GetDirectoryName(typeof(Tests).Assembly.Location);

        [TestCase("01 Botanical Dimensions.m4a", 49062136, BassFlags.Default)]
        [TestCase("01 Botanical Dimensions.m4a", 49062136, BassFlags.Float)]
        [TestCase("02 Outer Shpongolia.m4a", 26975092, BassFlags.Default)]
        [TestCase("02 Outer Shpongolia.m4a", 26975092, BassFlags.Float)]
        public void Test001(string fileName, long length, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Init());

            try
            {
                var sourceChannel = BassMemory.CreateStream(fileName, Flags: flags);
                if (sourceChannel == 0)
                {
                    Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                }

                Assert.AreEqual(length, Bass.ChannelGetLength(sourceChannel));
                Assert.IsTrue(Bass.ChannelPlay(sourceChannel));

                global::System.Threading.Thread.Sleep(10000);

                Assert.IsTrue(Bass.ChannelSetPosition(sourceChannel, Bass.ChannelGetLength(sourceChannel) - Bass.ChannelSeconds2Bytes(sourceChannel, 10)));

                while (Bass.ChannelIsActive(sourceChannel) == PlaybackState.Playing)
                {
                    global::System.Threading.Thread.Sleep(1000);
                }

                Assert.AreEqual(length, Bass.ChannelGetPosition(sourceChannel));
                Assert.IsTrue(Bass.StreamFree(sourceChannel));
            }
            finally
            {
                Assert.IsTrue(BassMemory.Free());
                Assert.IsTrue(Bass.Free());
            }
        }

        [TestCase("01 Botanical Dimensions.m4a", 49062136, BassFlags.Default)]
        [TestCase("01 Botanical Dimensions.m4a", 49062136, BassFlags.Float)]
        [TestCase("02 Outer Shpongolia.m4a", 26975092, BassFlags.Default)]
        [TestCase("02 Outer Shpongolia.m4a", 26975092, BassFlags.Float)]
        public void Test002(string fileName, long length, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Init());

            try
            {
                var sourceChannel = BassMemory.CreateStream(Bass.CreateStream(fileName, Flags: flags | BassFlags.Decode));
                if (sourceChannel == 0)
                {
                    Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                }

                Assert.AreEqual(length, Bass.ChannelGetLength(sourceChannel));
                Assert.IsTrue(Bass.ChannelPlay(sourceChannel));

                global::System.Threading.Thread.Sleep(10000);

                Assert.IsTrue(Bass.ChannelSetPosition(sourceChannel, Bass.ChannelGetLength(sourceChannel) - Bass.ChannelSeconds2Bytes(sourceChannel, 10)));

                while (Bass.ChannelIsActive(sourceChannel) == PlaybackState.Playing)
                {
                    global::System.Threading.Thread.Sleep(1000);
                }

                Assert.AreEqual(length, Bass.ChannelGetPosition(sourceChannel));
                Assert.IsTrue(Bass.StreamFree(sourceChannel));
            }
            finally
            {
                Assert.IsTrue(BassMemory.Free());
                Assert.IsTrue(Bass.Free());
            }
        }
    }
}
