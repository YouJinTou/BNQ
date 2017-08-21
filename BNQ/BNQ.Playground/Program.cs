using Act = BNQ.Models.Action;
using BNQ.Brain;
using BNQ.Models;
using System;
using System.Diagnostics;
using System.Linq;
using BNQ.IO;
using System.Configuration;

namespace BNQ.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Card board = Card.hJ | Card.hT | Card.s6;
            var storagePath = ConfigurationManager.AppSettings["StoragePath"];            
            var valuesLoader = new PrecomputedValuesLoader(storagePath);
            var rangeGenerator = new RangeGenerator(valuesLoader);
            var actions = new Act[] { Act.Call, Act.Fold, Act.Raise50 };
            var hand = new IHand[] { new Hand(Card.sT, Card.cT) };
            var hero = new Player(hand, actions);            

            rangeGenerator.UpdateRange(Act.Raise50, Act.None, Position.MP);

            var state = new State((ulong)board, 12, 97, rangeGenerator.Range, 4.5, StateType.Alive, actions);
            var stateActionGenerator = new StateActionGenerator(state, hero);
            var stateActionPairs = stateActionGenerator.GeneratePairs();
            var evaluator = new Evaluator();
            var sarsa = new Sarsa(state, true, (ulong)(Card.sT | Card.cT), 
                rangeGenerator.GenerateHands(), stateActionPairs, evaluator, 0.01, 1, TimeSpan.FromSeconds(10));
            var actionValues = sarsa.GetBestAction();

            //var sw = new Stopwatch();
            //var evaluator = new Evaluator();
            //var total = 0;

            //sw.Start();

            //for (ulong i = 16; i <= 36028797018963968; i *= 2)
            //{
            //    for (ulong j = 32; j <= 36028797018963968; j *= 2)
            //    {
            //        for (ulong z = 64; z <= 36028797018963968; z *= 2)
            //        {
            //            for (ulong x = 128; x <= 36028797018963968; x *= 2)
            //            {
            //                ulong board = z | x | 256 | 512 | 1024;

            //                evaluator.GetHolding(board, i | j);

            //                total++;
            //            }
            //        }
            //    }
            //}

            //sw.Stop();

            //Console.WriteLine("Time: {0}\nTotal: {1}", sw.Elapsed, total);
        }
    }
}
