using BNQ.Models;
using NUnit.Framework;

namespace BNQ.Brain.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [TestCase(Card.s5 | Card.s6 | Card.cT | Card.cJ | Card.cQ, Card.cA | Card.cK)]
        [TestCase(Card.c2 | Card.d2 | Card.dT | Card.dJ | Card.dA, Card.dK | Card.dQ)]
        [TestCase(Card.d3 | Card.s6 | Card.hT | Card.hK | Card.hA, Card.hJ | Card.hQ)]
        [TestCase(Card.dA | Card.h9 | Card.sQ | Card.sK | Card.sA, Card.sJ | Card.sT)]
        [TestCase(Card.dA | Card.dK | Card.dQ | Card.dT | Card.h3, Card.dJ | Card.s9)]
        [TestCase(Card.dA | Card.dK | Card.dQ | Card.dT | Card.dJ, Card.sJ | Card.s9)]
        [TestCase(Card.dA | Card.d2 | Card.d3 | Card.h8 | Card.sJ, Card.d4 | Card.d5)]
        [TestCase(Card.dA | Card.d2 | Card.d3 | Card.h8 | Card.sJ, Card.d4 | Card.d5)]
        [TestCase(Card.d5 | Card.d6 | Card.d7 | Card.h8 | Card.s9, Card.d3 | Card.d4)]
        [TestCase(Card.d5 | Card.d6 | Card.d7 | Card.h8 | Card.s9, Card.d8 | Card.d9)]
        public void GetHand_StraightFlush_StraightFlush(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.StraightFlush, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.s5 | Card.c7 | Card.c9 | Card.cT | Card.cJ, Card.cA | Card.cK)]
        [TestCase(Card.cJ | Card.dA | Card.sK | Card.hQ | Card.dT, Card.dK | Card.dQ)]
        [TestCase(Card.dA | Card.dK | Card.dQ | Card.dT | Card.h3, Card.sJ | Card.s9)]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        public void GetHand_NotStraightFlush_NotReturnStraightFlush(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreNotEqual(HandStrength.StraightFlush, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        public void GetHand_NotStraightFlush_NotStraightFlush(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreNotEqual(HandStrength.StraightFlush, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.sA | Card.hA | Card.s2 | Card.dK | Card.c8, Card.cA | Card.dA)]
        [TestCase(Card.c2 | Card.d2 | Card.h3 | Card.h4 | Card.h5, Card.h2 | Card.s2)]
        [TestCase(Card.c5 | Card.d5 | Card.s5 | Card.h5 | Card.dA, Card.h3 | Card.s3)]
        public void GetHand_FourOfAKind_FourOfAKind(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.FourOfAKind, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h5 | Card.c5 | Card.d8 | Card.sK | Card.cA, Card.c8 | Card.h8)]
        [TestCase(Card.h7 | Card.c7 | Card.s7 | Card.d2 | Card.sQ, Card.cJ | Card.dJ)]
        [TestCase(Card.h8 | Card.c8 | Card.d8 | Card.d2 | Card.s2, Card.dA | Card.hJ)]
        [TestCase(Card.dT | Card.sT | Card.hA | Card.c2 | Card.s4, Card.dA | Card.cT)]
        [TestCase(Card.d7 | Card.c7 | Card.h7 | Card.d8 | Card.d9, Card.dJ | Card.hJ)]
        public void GetHand_FullHouse_FullHouse(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.FullHouse, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.d7 | Card.c7 | Card.h7 | Card.d8 | Card.d9, Card.dJ | Card.dQ)]
        [TestCase(Card.hT | Card.sJ | Card.sQ | Card.dK | Card.s9, Card.s8 | Card.s7)]
        [TestCase(Card.c2 | Card.c8 | Card.cJ | Card.cK | Card.cA, Card.s8 | Card.s7)]
        [TestCase(Card.s3 | Card.s5 | Card.s8 | Card.sT | Card.sJ, Card.sQ | Card.sA)]
        public void GetHand_Flush_Flush(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.Flush, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.c5 | Card.s6 | Card.h7 | Card.dA | Card.sQ, Card.d8 | Card.h9)]
        [TestCase(Card.hT | Card.sJ | Card.hQ | Card.hK | Card.hA, Card.d8 | Card.s9)]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.cT | Card.dJ, Card.h5 | Card.sA)]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.sA | Card.h5, Card.s5 | Card.cA)]
        [TestCase(Card.c2 | Card.d2 | Card.h2 | Card.d3 | Card.s4, Card.h5 | Card.c6)]
        [TestCase(Card.h7 | Card.s7 | Card.s8 | Card.h8 | Card.s9, Card.dT | Card.hJ)]
        [TestCase(Card.hT | Card.sJ | Card.sQ | Card.dK | Card.s9, Card.d8 | Card.c7)]
        public void GetHand_Straight_Straight(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.Straight, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h7 | Card.sT | Card.d2 | Card.sA | Card.hQ, Card.c7 | Card.d7)]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        [TestCase(Card.hK | Card.sK | Card.dK | Card.s3 | Card.c7, Card.dQ | Card.sJ)]
        [TestCase(Card.cK | Card.d3 | Card.h3 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        [TestCase(Card.d2 | Card.h2 | Card.s2 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        [TestCase(Card.c3 | Card.d3 | Card.s2 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        [TestCase(Card.cA | Card.dA | Card.s2 | Card.s8 | Card.c9, Card.s3 | Card.sA)]
        public void GetHand_ThreeOfAKind_ThreeOfAKind(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.ThreeOfAKind, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, Card.d9 | Card.h7)]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, Card.cT | Card.sT)]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sA, Card.sK | Card.dJ)]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sA, Card.hA | Card.dJ)]
        public void GetHand_TwoPair_TwoPair(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.TwoPair, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, Card.d9 | Card.h8)]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, Card.cA | Card.sT)]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.cJ | Card.sA, Card.sK | Card.d8)]
        public void GetHand_OnePair_OnePair(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.OnePair, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, Card.dT | Card.h4)]
        [TestCase(Card.dQ | Card.sK | Card.h7 | Card.c5 | Card.d2, Card.cA | Card.sT)]
        [TestCase(Card.h5 | Card.s6 | Card.cT | Card.cJ | Card.s2, Card.sK | Card.d8)]
        public void GetHand_HighCard_HighCard(ulong board, ulong hand)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(HandStrength.HighCard, evaluator.GetHolding(board, hand).HandStrength);
        }

        [TestCase(Card.sT | Card.sJ | Card.sQ | Card.s3 | Card.s2, new ulong[]
        {
            (ulong)(Card.sK | Card.sA),
            (ulong)(Card.s8 | Card.s9)
        })]
        [TestCase(Card.h7 | Card.s9 | Card.d3 | Card.d4 | Card.d5, new ulong[]
        {
            (ulong)(Card.d6 | Card.d7),
            (ulong)(Card.dA | Card.d2)
        })]
        public void Evaluate_StraightFlushVsStraightFlush_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.sJ | Card.sQ | Card.s3 | Card.s2, new ulong[]
        {
            (ulong)(Card.s8 | Card.s9),
            (ulong)(Card.sK | Card.sA)
        })]
        [TestCase(Card.h7 | Card.s9 | Card.d3 | Card.d4 | Card.d5, new ulong[]
        {
            (ulong)(Card.dA | Card.d2),
            (ulong)(Card.d6 | Card.d7)
        })]
        public void Evaluate_StraightFlushVsStraightFlush_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.sJ | Card.sQ | Card.s3 | Card.s2, new ulong[]
        {
            (ulong)(Card.s8 | Card.s9),
            (ulong)(Card.s8 | Card.s9)
        })]
        [TestCase(Card.h7 | Card.s9 | Card.d3 | Card.d4 | Card.d5, new ulong[]
        {
            (ulong)(Card.d6 | Card.d7),
            (ulong)(Card.d6 | Card.d7)
        })]
        public void Evaluate_StraightFlushVsStraightFlush_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.h2 | Card.s2 | Card.s7, new ulong[]
        {
            (ulong)(Card.hT | Card.cT),
            (ulong)(Card.d2 | Card.c2)
        })]
        public void Evaluate_QuadsVsQuads_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.h2 | Card.s2 | Card.s7, new ulong[]
        {
            (ulong)(Card.d2 | Card.c2),
            (ulong)(Card.hT | Card.cT)
        })]
        public void Evaluate_QuadsVsQuads_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.hT | Card.cT | Card.s7, new ulong[]
        {
            (ulong)(Card.cK | Card.cQ),
            (ulong)(Card.sK | Card.hJ)
        })]
        public void Evaluate_QuadsVsQuads_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.hT | Card.sA | Card.sK, new ulong[]
        {
            (ulong)(Card.hA | Card.sQ),
            (ulong)(Card.hK | Card.sQ)
        })]
        public void Evaluate_BoatvsBoat_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.hT | Card.sA | Card.sK, new ulong[]
        {
            (ulong)(Card.hK | Card.sQ),
            (ulong)(Card.hA | Card.sQ)
        })]
        public void Evaluate_BoatvsBoat_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.hT | Card.sA | Card.sK, new ulong[]
        {
            (ulong)(Card.hK | Card.sQ),
            (ulong)(Card.cK | Card.sQ)
        })]
        public void Evaluate_BoatvsBoat_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.s7 | Card.s3 | Card.s4, new ulong[]
        {
            (ulong)(Card.hK | Card.sQ),
            (ulong)(Card.cK | Card.sJ)
        })]
        public void Evaluate_FlushVsFlush_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.sT | Card.dT | Card.s7 | Card.s3 | Card.s4, new ulong[]
        {
            (ulong)(Card.cK | Card.sJ),
            (ulong)(Card.hK | Card.sQ)
        })]
        public void Evaluate_FlushVsFlush_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.s2 | Card.s9 | Card.s7 | Card.sA | Card.sK, new ulong[]
        {
            (ulong)(Card.hK | Card.cQ),
            (ulong)(Card.cK | Card.hQ)
        })]
        public void Evaluate_FlushVsFlush_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.c5 | Card.s6 | Card.h7 | Card.dA | Card.sQ, new ulong[]
        {
            (ulong)(Card.d8 | Card.h9),
            (ulong)(Card.c3 | Card.h4),
            (ulong)(Card.h3 | Card.c4)
        })]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.h5 | Card.sA, new ulong[]
        {
            (ulong)(Card.s6 | Card.h7),
            (ulong)(Card.c6 | Card.dA),
            (ulong)(Card.h9 | Card.s9)
        })]
        public void Evaluate_StraightVsStraight_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.c5 | Card.s6 | Card.h7 | Card.dA | Card.sQ, new ulong[]
        {
            (ulong)(Card.c3 | Card.h4),
            (ulong)(Card.h3 | Card.c4),
            (ulong)(Card.d8 | Card.h9)
        })]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.h5 | Card.sA, new ulong[]
        {
            (ulong)(Card.c6 | Card.dA),
            (ulong)(Card.h9 | Card.s9),
            (ulong)(Card.s6 | Card.h7)
        })]
        public void Evaluate_StraightVsStraight_2(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(2, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.hT | Card.sJ | Card.hQ | Card.hK | Card.hA, new ulong[]
         {
            (ulong)(Card.d8 | Card.s9),
            (ulong)(Card.cA | Card.sK),
            (ulong)(Card.s8 | Card.c9)
         })]
        public void Evaluate_StraightVsStraight_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h7 | Card.sT | Card.d2 | Card.sA | Card.hQ, new ulong[]
        {
            (ulong)(Card.c7 | Card.d7),
            (ulong)(Card.s2 | Card.c2)
        })]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, new ulong[]
        {
            (ulong)(Card.dA | Card.d2),
            (ulong)(Card.dK | Card.c2)
        })]
        [TestCase(Card.hK | Card.sK | Card.dK | Card.s3 | Card.c7, new ulong[]
        {
            (ulong)(Card.dA | Card.sJ),
            (ulong)(Card.dQ | Card.sJ)
        })]
        public void Evaluate_TripsVsTrips_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h7 | Card.sT | Card.d2 | Card.sA | Card.hQ, new ulong[]
        {
            (ulong)(Card.s2 | Card.c2),
            (ulong)(Card.c7 | Card.d7)
        })]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, new ulong[]
        {
            (ulong)(Card.dK | Card.c2),
            (ulong)(Card.dA | Card.d2)
        })]
        [TestCase(Card.hK | Card.sK | Card.dK | Card.s3 | Card.c7, new ulong[]
        {
            (ulong)(Card.dQ | Card.sJ),
            (ulong)(Card.dA | Card.sJ)
        })]
        public void Evaluate_TripsVsTrips_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, new ulong[]
        {
            (ulong)(Card.dA | Card.d2),
            (ulong)(Card.hA | Card.c2)
        })]
        [TestCase(Card.hK | Card.sK | Card.dK | Card.dA | Card.dQ, new ulong[]
        {
            (ulong)(Card.h9 | Card.sJ),
            (ulong)(Card.h4 | Card.s7)
        })]
        public void Evaluate_TripsVsTrips_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.d9 | Card.h7),
            (ulong)(Card.s7 | Card.c3)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cT | Card.sT),
            (ulong)(Card.c9 | Card.s9)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sK, new ulong[]
        {
            (ulong)(Card.sA | Card.dJ),
            (ulong)(Card.sQ | Card.hJ)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sJ, new ulong[]
        {
            (ulong)(Card.hA | Card.dJ),
            (ulong)(Card.dK | Card.cJ)
        })]
        public void Evaluate_TwoPairVsTwoPair_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.s7 | Card.c3),
            (ulong)(Card.d9 | Card.h7)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.c9 | Card.s9),
            (ulong)(Card.cT | Card.sT)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sK, new ulong[]
        {
            (ulong)(Card.sQ | Card.hJ),
            (ulong)(Card.sA | Card.dJ)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sJ, new ulong[]
        {
            (ulong)(Card.dK | Card.cJ),
            (ulong)(Card.hA | Card.dJ)
        })]
        public void Evaluate_TwoPairVsTwoPair_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.s9 | Card.s7),
            (ulong)(Card.d9 | Card.h7)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.c9 | Card.s9),
            (ulong)(Card.d9 | Card.h9)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sK, new ulong[]
        {
            (ulong)(Card.sA | Card.hJ),
            (ulong)(Card.dA | Card.dJ)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sJ, new ulong[]
        {
            (ulong)(Card.d8 | Card.s7),
            (ulong)(Card.c8 | Card.d7)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sJ, new ulong[]
        {
            (ulong)(Card.d8 | Card.hJ),
            (ulong)(Card.c8 | Card.cJ)
        })]
        public void Evaluate_TwoPairVsTwoPair_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.d9 | Card.hQ),
            (ulong)(Card.d9 | Card.h8)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cA | Card.sT),
            (ulong)(Card.cK | Card.sT)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.cJ | Card.s9, new ulong[]
        {
            (ulong)(Card.sK | Card.d8),
            (ulong)(Card.dQ | Card.d7)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.sK | Card.dJ),
            (ulong)(Card.cK | Card.sT)
        })]
        public void Evaluate_PairVsPair_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();
            Card s = (Card)hands[0];
            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.d9 | Card.h8),
            (ulong)(Card.d9 | Card.hQ)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cK | Card.sT),
            (ulong)(Card.cA | Card.dT)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.cJ | Card.s9, new ulong[]
        {
            (ulong)(Card.dQ | Card.d7),
            (ulong)(Card.sK | Card.d2)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cK | Card.sT),
            (ulong)(Card.sK | Card.dJ)
        })]
        public void Evaluate_PairVsPair_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s5 | Card.s6, new ulong[]
        {
            (ulong)(Card.d9 | Card.h4),
            (ulong)(Card.c9 | Card.s3)
        })]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.c3 | Card.sT),
            (ulong)(Card.d4 | Card.dT)
        })]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.cJ | Card.sA, new ulong[]
        {
            (ulong)(Card.d2 | Card.d4),
            (ulong)(Card.c5 | Card.d8)
        })]
        public void Evaluate_PairVsPair_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.dQ | Card.h4),
            (ulong)(Card.dT | Card.h4)
        })]
        [TestCase(Card.dQ | Card.sK | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cA | Card.sT),
            (ulong)(Card.cJ | Card.sT)
        })]
        [TestCase(Card.h5 | Card.s6 | Card.cT | Card.cJ | Card.s2, new ulong[]
        {
            (ulong)(Card.sK | Card.d8),
            (ulong)(Card.sQ | Card.d8)
        })]
        public void Evaluate_HighCardVsHighCard_0(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(0, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.dT | Card.h4),
            (ulong)(Card.dQ | Card.h4)
        })]
        [TestCase(Card.dQ | Card.sK | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cJ | Card.sT),
            (ulong)(Card.cA | Card.sT)
        })]
        [TestCase(Card.h5 | Card.s6 | Card.cT | Card.cJ | Card.s2, new ulong[]
        {
            (ulong)(Card.sQ | Card.d8),
            (ulong)(Card.sK | Card.d8)
        })]
        public void Evaluate_HighCardVsHighCard_1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(1, evaluator.Evaluate(board, hands));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, new ulong[]
        {
            (ulong)(Card.dT | Card.h4),
            (ulong)(Card.sT | Card.c4)
        })]
        [TestCase(Card.dQ | Card.sK | Card.h7 | Card.c5 | Card.d2, new ulong[]
        {
            (ulong)(Card.cJ | Card.s3),
            (ulong)(Card.hJ | Card.s4)
        })]
        [TestCase(Card.h6 | Card.s7 | Card.cT | Card.cJ | Card.sK, new ulong[]
        {
            (ulong)(Card.s3 | Card.d4),
            (ulong)(Card.d4 | Card.h5)
        })]
        public void Evaluate_HighCardVsHighCard_Minus1(ulong board, ulong[] hands)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(-1, evaluator.Evaluate(board, hands));
        }
    }
}
