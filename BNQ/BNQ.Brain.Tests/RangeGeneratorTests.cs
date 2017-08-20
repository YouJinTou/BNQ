using BNQ.IO;
using BNQ.Models;
using Moq;
using NUnit.Framework;

namespace BNQ.Brain.Tests
{
    [TestFixture]
    public class RangeGeneratorTests
    {
        private IValuesLoader GetValuesLoader(ulong[] hands = null)
        {
            Mock<IValuesLoader> valuesLoader = new Mock<IValuesLoader>();

            valuesLoader.Setup(vl => vl.GetRange(It.IsAny<double>())).Returns(hands);

            return valuesLoader.Object;
        }

        [TestCase(Action.Call, Action.None, Position.UTG, 0.25)]
        public void RangeGenerator_PositionAndActions_CorrectRangeUTG1(
            Action action, Action vsAction, Position position, double range)
        {
            var generator = new RangeGenerator(this.GetValuesLoader());

            generator.UpdateRange(action, vsAction, position);

            Assert.AreEqual(range, generator.Range);
        }

        [TestCase(Action.Raise50, Action.Raise50, Position.SB, 0.03)]
        public void RangeGenerator_PositionAndActions_CorrectRangeSB(
            Action action, Action vsAction, Position position, double range)
        {
            var generator = new RangeGenerator(this.GetValuesLoader());

            generator.UpdateRange(Action.Raise50, Action.Raise50, Position.HJ);
            generator.UpdateRange(Action.Raise50, Action.Raise50, Position.BTN);
            generator.UpdateRange(action, vsAction, position);

            Assert.AreEqual(range, generator.Range);
        }

        [TestCase(0.026, 36)]
        public void RangeGenerator_Range_CorrectHandsCount(double range, int count)
        {
            var generator = new RangeGenerator(this.GetValuesLoader(new ulong[36]));

            ulong[] hands = generator.GenerateHands();

            Assert.AreEqual(count, hands.Length);
        }

        [Test]
        public void RangeGenerator_Range_GenerateAllStartingHands()
        {
            var generator = new RangeGenerator(this.GetValuesLoader(new ulong[1326]));

            ulong[] hands = generator.GenerateHands();

            Assert.AreEqual(1326, hands.Length);
        }
    }
}
