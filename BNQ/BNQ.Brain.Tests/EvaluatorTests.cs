using BNQ.Models;
using NUnit.Framework;

namespace BNQ.Brain.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [TestCase((ulong)1172526628864, (ulong)299067162755072)] // 5s6sTcJcQc, AcKc
        [TestCase((ulong)563095982309379, (ulong)37383395344384)] // 2c2dTdJdAd, KdQd
        [TestCase((ulong)1196285831413792, (ulong)4672924418048)] // 3d6sThKhAh, QhJh
        [TestCase((ulong)2964284422225920, (ulong)584115552256)] // Ad9hQsKsAs, JsTs
        [TestCase((ulong)600341938700352, (ulong)139586437120)] // AdKdQdTd3h, Jd9s
        public void Evaluate_RoyalFlush_ReturnRoyalFlush(ulong board, ulong holding)
        {
            Evaluator evaluator = new Evaluator();

            Assert.AreEqual(Hand.RoyalFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase((ulong)73283960832, (ulong)299067162755072)] // 5s7c9cTcJc, AcKc
        [TestCase((ulong)708162797699072, (ulong)37383395344384)] // JcAdKsQhTd, KdQd
        [TestCase((ulong)600341938700352, (ulong)551903297536)] // AdKdQdTd3h, Js9s
        [TestCase((ulong)563499776344098, (ulong)8704)] // Ad2d3d8hJs, 4d5d
        public void Evaluate_NotRoyalFlush_NotReturnRoyalFlush(ulong board, ulong holding)
        {
            Evaluator evaluator = new Evaluator();

            Assert.AreNotEqual(Hand.RoyalFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase((ulong)563499776344098, (ulong)8704)] // Ad2d3d8hJs, 4d5d
        public void Evaluate_StraightFlush_ReturnStraightFlush(ulong board, ulong holding)
        {
            Evaluator evaluator = new Evaluator();

            Assert.AreEqual(Hand.StraightFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase((ulong)600341938700352, (ulong)139586437120)] // AdKdQdTd3h, Jd9s
        public void Evaluate_FourOfAKind_ReturnFourOfAKind(ulong board, ulong holding)
        {
            Evaluator evaluator = new Evaluator();

            Assert.AreEqual(Hand.FourOfAKind, evaluator.Evaluate(board, holding));
        }
    }
}
