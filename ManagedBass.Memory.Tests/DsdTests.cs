﻿using NUnit.Framework;
using System;
using System.IO;

namespace ManagedBass.Memory.Tests
{
    [TestFixture]
    public class DsdTests
    {
        private static readonly string Location = Path.GetDirectoryName(typeof(Tests).Assembly.Location);

        [SetUp]
        public void SetUp()
        {
            Assert.IsTrue(Loader.Load("bass"));
            Assert.IsTrue(Loader.Load("bassdsd"));
            Assert.IsTrue(BassMemory.Dsd.Load());
            Assert.IsTrue(Bass.Init());
        }

        [TearDown]
        public void TearDown()
        {
            BassMemory.Dsd.Unload();
            Bass.Free();
        }

        [TestCase("01 Sample.dsf", 7053288, BassFlags.Default)]
        [TestCase("01 Sample.dsf", 14106624, BassFlags.DSDRaw)]
        public void Test001(string fileName, long length, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            var sourceChannel = BassMemory.Dsd.CreateStream(fileName, Flags: flags | BassFlags.Decode);
            if (sourceChannel == 0)
            {
                Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
            }

            Assert.AreEqual(length, Bass.ChannelGetLength(sourceChannel));
            Assert.IsTrue(Bass.ChannelSetPosition(sourceChannel, length - 1));
            Assert.IsTrue(Bass.ChannelSetPosition(sourceChannel, length, PositionFlags.Bytes | PositionFlags.DecodeTo));
            Assert.AreEqual(length, Bass.ChannelGetPosition(sourceChannel));
            Assert.IsTrue(Bass.StreamFree(sourceChannel));
        }

        [TestCase("01 Sample.dsf", 14114908, BassFlags.Default)]
        [TestCase("01 Sample.dsf", 14114908, BassFlags.DSDRaw)]
        public void Test002(string fileName, long size, BassFlags flags)
        {
            if (string.IsNullOrEmpty(Path.GetPathRoot(fileName)))
            {
                fileName = Path.Combine(Location, "Media", fileName);
            }

            var sourceChannels = new int[10];
            for (var a = 0; a < 10; a++)
            {
                sourceChannels[a] = BassMemory.Dsd.CreateStream(fileName, Flags: flags | BassFlags.Decode);
                if (sourceChannels[a] == 0)
                {
                    Assert.Fail(string.Format("Failed to create source stream: {0}", Enum.GetName(typeof(Errors), Bass.LastError)));
                }
            }

            Assert.AreEqual(size, BassMemory.Dsd.Usage());

            for (var a = 0; a < 10; a++)
            {
                Assert.IsTrue(Bass.StreamFree(sourceChannels[a]));
            }

            Assert.AreEqual(0, BassMemory.Dsd.Usage());
        }
    }
}