﻿using BNQ.Models;
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
        public void Evaluate_StraightFlush_ReturnStraightFlush(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.StraightFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.s5 | Card.c7 | Card.c9 | Card.cT | Card.cJ, Card.cA | Card.cK)]
        [TestCase(Card.cJ | Card.dA | Card.sK | Card.hQ | Card.dT, Card.dK | Card.dQ)]
        [TestCase(Card.dA | Card.dK | Card.dQ | Card.dT | Card.h3, Card.sJ | Card.s9)]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        public void Evaluate_NotStraightFlush_NotReturnStraightFlush(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreNotEqual(Hand.StraightFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        public void Evaluate_NotStraightFlush_NotStraightFlush(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreNotEqual(Hand.StraightFlush, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.sA | Card.hA | Card.s2 | Card.dK | Card.c8, Card.cA | Card.dA)]
        [TestCase(Card.c2 | Card.d2 | Card.h3 | Card.h4 | Card.h5, Card.h2 | Card.s2)]
        [TestCase(Card.c5 | Card.d5 | Card.s5 | Card.h5 | Card.dA, Card.h3 | Card.s3)]
        public void Evaluate_FourOfAKind_ReturnFourOfAKind(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.FourOfAKind, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.h5 | Card.c5 | Card.d8 | Card.sK | Card.cA, Card.c8 | Card.h8)]
        [TestCase(Card.h7 | Card.c7 | Card.s7 | Card.d2 | Card.sQ, Card.cJ | Card.dJ)]
        [TestCase(Card.h8 | Card.c8 | Card.d8 | Card.d2 | Card.s2, Card.dA | Card.hJ)]
        [TestCase(Card.dT | Card.sT | Card.hA | Card.c2 | Card.s4, Card.dA | Card.cT)]
        public void Evaluate_FullHouse_ReturnFullHouse(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.FullHouse, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.h7 | Card.sT | Card.d2 | Card.sA | Card.hQ, Card.c7 | Card.d7)]
        [TestCase(Card.h2 | Card.s2 | Card.d4 | Card.s7 | Card.cT, Card.dA | Card.c2)]
        [TestCase(Card.hK | Card.sK | Card.dK | Card.s3 | Card.c7, Card.dQ | Card.sJ)]
        [TestCase(Card.cK | Card.d3 | Card.h3 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        [TestCase(Card.d2 | Card.h2 | Card.s2 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        [TestCase(Card.c3 | Card.d3 | Card.s2 | Card.s8 | Card.c9, Card.s3 | Card.dA)]
        public void Evaluate_ThreeOfAKind_ReturnThreeOfAKind(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.ThreeOfAKind, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.h9 | Card.d7 | Card.dJ | Card.s3 | Card.c2, Card.d9 | Card.h7)]
        [TestCase(Card.dQ | Card.sQ | Card.h7 | Card.c5 | Card.d2, Card.cT | Card.sT)]
        [TestCase(Card.h6 | Card.s6 | Card.cT | Card.dT | Card.sA, Card.sK | Card.dJ)]
        public void Evaluate_TwoPair_ReturnTwoPair(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.TwoPair, evaluator.Evaluate(board, holding));
        }

        [TestCase(Card.c5 | Card.s6 | Card.h7 | Card.dA | Card.sQ, Card.d8 | Card.h9)]
        [TestCase(Card.hT | Card.sJ | Card.hQ | Card.hK | Card.hA, Card.d8 | Card.s9)]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.cT | Card.dJ, Card.h5 | Card.sA)]
        [TestCase(Card.d2 | Card.s3 | Card.c4 | Card.sA | Card.h5, Card.s5 | Card.cA)]
        public void Evaluate_Straight_ReturnStraight(ulong board, ulong holding)
        {
            var evaluator = new Evaluator();

            Assert.AreEqual(Hand.Straight, evaluator.Evaluate(board, holding));
        }
    }
}
