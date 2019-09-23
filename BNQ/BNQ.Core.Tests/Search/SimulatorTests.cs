using BNQ.Core.Abstractions;
using BNQ.Core.Factories;
using BNQ.Core.Models;
using BNQ.Core.Search;
using BNQ.Core.Tests.Doubles;
using Moq;
using NUnit.Framework;

namespace BNQ.Core.Tests.Search
{
    public class SimulatorTests
    {
        private Mock<IStateFactory> stateFactory;
        private Mock<IEvaluator> evaluator;
        private Mock<IBoardGenerator> generator;
        private SimulatorDouble simulator;

        [SetUp]
        public void InitializeTests()
        {
            this.stateFactory = new Mock<IStateFactory>();
            this.evaluator = new Mock<IEvaluator>();
            this.generator = new Mock<IBoardGenerator>();
            this.simulator = new SimulatorDouble(
                this.stateFactory.Object, this.evaluator.Object, this.generator.Object);
        }

        [Test]
        public void StateIsShowDown_And_NoOneAllIn_FillsBoardToRiver()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 100, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Showdown, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(
                g => g.FillToRiver(board, Street.Flop, heroHoleCards), Times.Once);
        }

        [Test]
        public void StateIsShowDown_And_NoOneAllIn_Evaluates()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 100, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Showdown, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.evaluator.Verify(g => g.Evaluate(state), Times.Once);
        }

        [Test]
        public void StateIsShowDown_And_SomeoneAllIn_DoesNotFillToRiver()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Showdown, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(g => g.FillToRiver(
                It.IsAny<Card>(), It.IsAny<Street>(), It.IsAny<Card>()), Times.Never);
        }

        [Test]
        public void StateIsShowDown_And_SomeoneAllIn_Evaluates()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Showdown, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.evaluator.Verify(g => g.Evaluate(state), Times.Once);
        }

        [Test]
        public void StreetIsFold_ReturnsAverageValue()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Fold, hero, villain, 0, 0, Action.Bet50);
            state.Visits = 2;
            state.Value = 5;

            var value = this.simulator.Simulate(state);

            Assert.AreEqual(state.AverageValue, value);
        }

        [Test]
        public void StreetIsFold_DoesNotCallGenerator()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Fold, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(g => g.FillToRiver(
                It.IsAny<Card>(), It.IsAny<Street>(), It.IsAny<Card>()), Times.Never);
        }

        [Test]
        public void StreetIsFold_DoesNotCallEvaluator()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Fold, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.evaluator.Verify(g => g.Evaluate(It.IsAny<GameState>()), Times.Never);
        }

        [Test]
        public void StreetIsFlop_FillsFromFlopToFlop()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Flop, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(g => g.FillFromTo(
                board, Street.Flop, Street.Flop, state.Hero.HoleCards), Times.Once);
        }

        [Test]
        public void StreetIsTurn_FillsFromFlopToTurn()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.Turn, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(g => g.FillFromTo(
                board, Street.Flop, Street.Turn, state.Hero.HoleCards), Times.Once);
        }

        [Test]
        public void StreetIsRiver_FillsFromFlopToRiver()
        {
            var board = Card.cA | Card.c7 | Card.c8;
            var heroHoleCards = Card.h4 | Card.dT;
            var hero = this.CreatePlayer(heroHoleCards, Position.BB, 0, true, true);
            var villain = this.CreatePlayer(Card.None, Position.SB, 100, false, false);
            var state = this.CreateState(
                board, Street.River, hero, villain, 0, 0, Action.Bet50);

            this.simulator.Simulate(state);

            this.generator.Verify(g => g.FillFromTo(
                board, Street.Flop, Street.River, state.Hero.HoleCards), Times.Once);
        }

        private GameState CreateState(
            Card board, 
            Street street, 
            Player hero, 
            Player villain,
            double pot,
            double wagerToCall,
            Action lastAction)
        {
            return new GameState(null, board, street, hero, villain, pot, wagerToCall, lastAction);
        }

        private Player CreatePlayer(
            Card holeCards, Position position, double stack, bool isToPlay, bool isHero)
        {
            return new Player(holeCards, position, stack, isToPlay, isHero);
        }
    }
}
