using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ManagedBass.Memory.Tests
{
    [TestFixture]
    public class DsdTests
    {
        private static readonly string Location = Path.GetDirectoryName(typeof(Tests).Assembly.Location);

        [TestCase("01 Sample.dsf", 49062136, BassFlags.Default)]
        [TestCase("01 Sample.dsf", 49062136, BassFlags.DSDRaw)]
        public void Test001(string fileName, long length, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Dsd.Init());

            try
            {
                var sourceChannel = BassMemory.Dsd.CreateStream(fileName, Flags: flags);
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
                Assert.IsTrue(BassMemory.Dsd.Free());
                Assert.IsTrue(Bass.Free());
            }
        }

        [TestCase("01 Sample.dsf", 49062136, BassFlags.Default)]
        [TestCase("01 Sample.dsf", 49062136, BassFlags.DSDRaw)]
        public void Test002(string fileName, long length, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Dsd.Init());

            try
            {
                var sourceChannel = BassMemory.Dsd.CreateStream(Bass.CreateStream(fileName, Flags: flags | BassFlags.Decode));
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
                Assert.IsTrue(BassMemory.Dsd.Free());
                Assert.IsTrue(Bass.Free());
            }
        }

        [TestCase("01 Sample.dsf", BassFlags.Default)]
        [TestCase("01 Sample.dsf", BassFlags.DSDRaw)]
        public void Test003(string fileName, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Dsd.Init());

            try
            {
                var sourceChannels = new int[10];
                for (var a = 0; a < 10; a++)
                {
                    sourceChannels[a] = BassMemory.Dsd.CreateStream(fileName, Flags: flags);
                    if (sourceChannels[a] == 0)
                    {
                        Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                    }
                }

                for (var a = 0; a < 10; a++)
                {
                    Assert.IsTrue(Bass.StreamFree(sourceChannels[a]));
                }
            }
            finally
            {
                Assert.IsTrue(BassMemory.Dsd.Free());
                Assert.IsTrue(Bass.Free());
            }
        }

        [TestCase("01 Sample.dsf", BassFlags.Default)]
        [TestCase("01 Sample.dsf", BassFlags.DSDRaw)]
        public void Test004(string fileName, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Bass.Init());
            Assert.IsTrue(BassMemory.Dsd.Init());
            try
            {
                var sourceChannel = Bass.CreateStream(fileName, Flags: flags | BassFlags.Decode);
                if (sourceChannel == 0)
                {
                    Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                }

                var sourceChannels = new int[10];
                for (var a = 0; a < 10; a++)
                {
                    sourceChannels[a] = BassMemory.Dsd.CreateStream(sourceChannel, Flags: flags);
                    if (sourceChannels[a] == 0)
                    {
                        Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                    }
                }

                for (var a = 0; a < 10; a++)
                {
                    Assert.IsTrue(Bass.StreamFree(sourceChannels[a]));
                }
            }
            finally
            {
                Assert.IsTrue(BassMemory.Dsd.Free());
                Assert.IsTrue(Bass.Free());
            }
        }
    }
}