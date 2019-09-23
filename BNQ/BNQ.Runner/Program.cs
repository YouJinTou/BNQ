using BNQ.Core.Factories;
using BNQ.Core.Models;
using BNQ.Core.Search;
using BNQ.Core.Tools;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BNQ.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            MeasureTimes();
        }

        static void MeasureTimes()
        {
            var stopWatch = new Stopwatch();

            Evaluator.Initialize();

            while (true)
            {
                var roots = new List<GameState>();

                System.Console.Clear();

                stopWatch.Reset();

                stopWatch.Start();

                var root = StateGenerator.GenerateState();

                for (int i = 0; i < 3; i++)
                {
                    roots.Add(RunSimulation(GameState.CopyState(root), false));
                }

                System.Console.WriteLine(stopWatch.Elapsed.ToString());

                System.Console.WriteLine(roots.First().Hero);

                System.Console.WriteLine(roots.First());

                var m = 10000d;
                var averages = new Dictionary<Action, double>();

                foreach (IEnumerable<State> children in roots.Select(r => r.Children))
                {
                    foreach (GameState child in children)
                    {
                        if (averages.ContainsKey(child.LastAction))
                        {
                            averages[child.LastAction] += (child.AverageValue / children.Count());
                        }
                        else
                        {
                            averages.Add(child.LastAction, (child.AverageValue / children.Count()));
                        }
                    }
                }

                foreach (var average in averages)
                {
                    var avg = string.Format("{0:0.00000}", m * average.Value);

                    System.Console.WriteLine($"{average.Key}: {avg}");
                }

                System.Console.ReadLine();

                roots.Last().Print();

                System.Console.ReadLine();
            }
        }

        static GameState RunSimulation(GameState state = null, bool isDeterministic = false)
        {
            var root = state ?? GetStartState(isDeterministic);
            var mcts = new MCTS();

            for (int iteration = 0; iteration < 100000; iteration++)
            {
                var leaf = mcts.Select(root);

                if (leaf.IsSampled)
                {
                    mcts.Expand(leaf);
                }

                mcts.Simulate(leaf);

                mcts.Propagate(leaf);
            }

            return root;
        }

        static GameState GetStartState(bool isDeterministic)
        {
            if (!isDeterministic)
            {
                return StateGenerator.GenerateState();
            }

            var heroHand = isDeterministic ? Card.d6 | Card.d5 : BoardGenerator.GenerateHand();
            var villainHand = Card.None;
            var flop = isDeterministic ?
                Card.c3 | Card.d4 | Card.c9 : BoardGenerator.GenerateFlop(heroHand | villainHand);
            var hero = new Player(heroHand, Position.SB, 99.5, true, true);
            var villain = new Player(villainHand, Position.BB, 98.25, false);
            var state = StateFactory.CreateStartState(
                flop, Street.Flop, hero, villain, 2.25, 0.75d, Action.Bet50);

            return state;
        }
    }
}
