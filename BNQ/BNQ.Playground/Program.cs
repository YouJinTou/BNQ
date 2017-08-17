using Act = BNQ.Models.Action;
using BNQ.Brain;
using BNQ.Models;
using System;

namespace BNQ.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Card board = Card.hK | Card.dA | Card.sQ;
            var actions = new Act[] { Act.Bet50, Act.Check };
            var state = new State((ulong)board, 33, 0, StateType.Alive, actions);
            var hand = new IHolding[] { new Holding(Card.cK, Card.hJ) };
            var hero = new Player(hand, actions);
            var generator = new StateActionGenerator(state, hero);
            var stateActionPairs = generator.GeneratePairs();
            var sarsa = new Sarsa(state, stateActionPairs, 0.01, 1, TimeSpan.FromSeconds(10), 1000);
            var actionValues = sarsa.GetBestAction();
        }
    }
}
