using BNQ.Models;
using NUnit.Framework;
using System;

namespace BQN.Models.Tests
{
    [TestFixture]
    public class BoardTests
    {
        private const Card Flop = Card.hK | Card.dA | Card.sQ;

        private IBoard GetBoard()
        {
            var flop = new Card[] { Card.dA, Card.hK, Card.sQ };
            var board = new Board(flop);

            return board;
        }

        [TestCase(new Card[] { Card.h9, Card.s7, Card.c8, Card.sK })]
        [TestCase(new Card[] { Card.h9, Card.s7 })]
        [TestCase(new Card[] { Card.h9 })]
        [TestCase(new Card[] { })]
        public void Constructor_PassInvalidNumberOfCards_ShouldFail(Card[] flop)
        {
            Board board;
            var ex = Assert.Throws<ArgumentException>(() => board = new Board(flop));

            Assert.IsTrue(ex.Message.IndexOf(
                "Either invalid number of cards or there are repeating cards.") > -1);
        }

        [TestCase(new Card[] { Card.h9, (Card)5, Card.c8 })]
        [TestCase(new Card[] { Card.h9, Card.c8, (Card)999 })]
        [TestCase(new Card[] { (Card)1001, Card.c8, Card.hJ })]
        public void Constructor_PassNonExistingCard_ShouldFail(Card[] flop)
        {
            Board board;
            var ex = Assert.Throws<ArgumentException>(() => board = new Board(flop));

            Assert.IsTrue(ex.Message.IndexOf("A non-existing card has been passed.") > -1);
        }

        [TestCase(new Card[] { Card.None, Card.dQ, Card.h3 })]
        public void Constructor_PassNoneCard_ShouldFail(Card[] flop)
        {
            Board board;
            var ex = Assert.Throws<ArgumentException>(() => board = new Board(flop));

            Assert.IsTrue(ex.Message.IndexOf("Cannot pass a None type card.") > -1);
        }

        [TestCase(new Card[] { Card.h3, Card.dQ, Card.h3 })]
        [TestCase(new Card[] { Card.dT, Card.dT, Card.h3 })]
        public void Constructor_PassRepeatingCardsForFlop_ShouldFail(Card[] flop)
        {
            Board board;
            var ex = Assert.Throws<ArgumentException>(() => board = new Board(flop));

            Assert.IsTrue(ex.Message.IndexOf(
                "Either invalid number of cards or there are repeating cards.") > -1);
        }

        [TestCase(new Card[] { Card.c2, Card.c3, Card.c4 }, Card.c2 | Card.c3 | Card.c4)]
        [TestCase(new Card[] { Card.cA, Card.dK, Card.s7 }, Card.cA | Card.dK | Card.s7)]
        public void Constructor_PassValidFlop_ShouldSetFlop(Card[] flopCards, ulong expectedFlop)
        {
            var board = new Board(flopCards);
            var flop = board.Flop;

            Assert.AreEqual(expectedFlop, flop);
        }

        [TestCase(Card.None)]
        [TestCase((Card)5)]
        [TestCase((Card)2251799813685249)]
        public void AddTurn_PassNonExistingCard_ShouldFail(Card turn)
        {
            var board = this.GetBoard();
            var ex = Assert.Throws<ArgumentException>(() => board.AddTurn(turn));

            Assert.IsTrue(ex.Message.IndexOf("Invalid card.") > -1);
        }

        [TestCase(Card.dA)]
        [TestCase(Card.hK)]
        [TestCase(Card.sQ)]
        public void AddTurn_PassARepeat_ShouldFail(Card turn)
        {
            var board = this.GetBoard();
            var ex = Assert.Throws<ArgumentException>(() => board.AddTurn(turn));

            Assert.IsTrue(ex.Message.IndexOf("Repeats are not allowed.") > -1);
        }

        [TestCase(Card.hJ, Card.hJ, Flop | Card.hJ)]
        [TestCase(Card.s8, Card.s8, Flop | Card.s8)]
        [TestCase(Card.h6, Card.h6, Flop | Card.h6)]
        public void AddTurn_PassValidTurn_ShouldSetTurn(
            Card turn, ulong expectedTurn, ulong expectedCards)
        {
            var board = this.GetBoard();

            board.AddTurn(turn);

            Assert.AreEqual(board.Turn, expectedTurn);
            Assert.AreEqual(board.Cards, expectedCards);
        }

        [TestCase(Card.None)]
        [TestCase((Card)93)]
        [TestCase((Card)2251799813685249)]
        public void AddRiver_PassNonExistingCard_ShouldFail(Card river)
        {
            var board = this.GetBoard();
            var ex = Assert.Throws<ArgumentException>(() => board.AddRiver(river));

            Assert.IsTrue(ex.Message.IndexOf("Invalid card.") > -1);
        }

        [TestCase(Card.h7, Card.dA)]
        [TestCase(Card.sT, Card.hK)]
        [TestCase(Card.d3, Card.sQ)]
        public void AddRiver_PassARepeat_ShouldFail(Card turn, Card river)
        {
            var board = this.GetBoard();

            board.AddTurn(turn);

            var ex = Assert.Throws<ArgumentException>(() => board.AddRiver(river));

            Assert.IsTrue(ex.Message.IndexOf("Repeats are not allowed.") > -1);
        }

        [TestCase(Card.hJ, Card.sT, Card.sT, Flop | Card.hJ | Card.sT)]
        [TestCase(Card.s8, Card.c3, Card.c3, Flop | Card.s8 | Card.c3)]
        [TestCase(Card.h6, Card.d6, Card.d6, Flop | Card.h6 | Card.d6)]
        public void AddRiver_PassValidRiver_ShouldSetRiver(
            Card turn, Card river, ulong expectedRiver, ulong expectedCards)
        {
            var board = this.GetBoard();

            board.AddTurn(turn);
            board.AddRiver(river);

            Assert.AreEqual(board.River, expectedRiver);
            Assert.AreEqual(board.Cards, expectedCards);
        }
    }
}
