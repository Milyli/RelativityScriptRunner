namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
    using Milyli.ScriptRunner.Core.Tools;
    using NUnit.Framework;

    [TestFixture]
    public class TestBitmaskHelper
    {
        [TestCase(3, 4)]
        [TestCase(3, 0)]
        [TestCase(10, 5)]
        public void TestMakeMask(int len, int offset)
        {
            var mask = BitmaskHelper.MakeMask(len, offset);
            var lo = (1 << offset) - 1;
            var expectedVal = ((1 << (len + offset)) - 1) & ~lo;
            Assert.That(mask == expectedVal, string.Format("Got {0}, expected {1} for len {2}, offset {3}", mask, expectedVal, len, offset));
        }

        [Test]
        public void TestRotateRight()
        {
            // 0111 1011
            var mask = 0x7F ^ 0x04;

            // 0111 1101
            var expectedResult = 0x7F ^ 0x02;
            var result = BitmaskHelper.RotateRight(mask, 1, 7);
            Assert.That(result == expectedResult);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(8)]
        public void TestRotateLeft(int count)
        {
            var all7bits = 0x7F;
            var defaultOffset = 3;
            var defaultLen = 7;
            var markBit = 1 << defaultOffset;

            var mask = all7bits ^ markBit;
            var markBitOffset = (defaultOffset + count) % defaultLen;
            var expectedResult = 0x7F ^ (1 << markBitOffset);
            var result = BitmaskHelper.RotateLeft(mask, count, defaultLen);
            Assert.That(result == expectedResult, string.Format("Expectd {0}, Got {1}", expectedResult, result));
        }
    }
}
