using Act = BNQ.Models.Action;
using BNQ.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BNQ.Brain.Tests
{
    [TestFixture]
    public class StateActionGeneratorTests
    {
        private State GetState()
        {
            var flop = new Card[] { Card.dA, Card.hK, Card.sQ };
            var board = new Board(flop);
            var state = new State(board.Cards, 100, 1.5, 0, null);

            return state;
        }

        [TestCase()]
        public void Generate_GivenFlop_ReturnStateActions()
        {
            var hand = new HashSet<IHand> { new Hand(Card.hA, Card.dK) };
            var actions = new HashSet<Act>
            {
                Act.Bet50,
                Act.Check
            };
            StateActionGenerator generator = new StateActionGenerator(
                this.GetState(), new Player(hand.ToArray(), actions.ToArray()));

            generator.GeneratePairs();
        }
    }
}
