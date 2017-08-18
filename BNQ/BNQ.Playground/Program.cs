using Act = BNQ.Models.Action;
using BNQ.Brain;
using BNQ.Models;
using System;
using System.Diagnostics;

namespace BNQ.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            //Card board = Card.hK | Card.dA | Card.sQ;
            //var actions = new Act[] { Act.Bet50, Act.Check };
            //var state = new State((ulong)board, 33, 0, StateType.Alive, actions);
            //var hand = new IHolding[] { new Holding(Card.cK, Card.hJ) };
            //var hero = new Player(hand, actions);
            //var generator = new StateActionGenerator(state, hero);
            //var stateActionPairs = generator.GeneratePairs();
            //var sarsa = new Sarsa(state, stateActionPairs, 0.01, 1, TimeSpan.FromSeconds(10), 1000);
            //var actionValues = sarsa.GetBestAction();

            var sw = new Stopwatch();
            var evaluator = new Evaluator();
            var total = 0;
            ulong holding = 512 | 1024;

            sw.Start();

            for (ulong i = 16; i <= 36028797018963968; i *= 2)
            {
                for (ulong j = 32; j <= 36028797018963968; j *= 2)
                {
                    for (ulong z = 64; z <= 36028797018963968; z *= 2)
                    {
                        for (ulong x = 128; x <= 36028797018963968; x *= 2)
                        {
                            ulong board = i | j | z | x | 256;

                            evaluator.GetHand(board, holding);

                            total++;
                        }
                    }
                }
            }

            sw.Stop();

            Console.WriteLine("Time: {0}\nTotal: {1}", sw.Elapsed, total);
        }
    }
}
