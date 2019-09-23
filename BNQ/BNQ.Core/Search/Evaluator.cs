using BNQ.Core.Models;
using BNQ.Core.Tools;

namespace BNQ.Core.Search
{
    public static class Evaluator
    {
        private static readonly SnapCall.Evaluator evaluator;
        private static readonly System.Random random;

        static Evaluator()
        {
            evaluator = new SnapCall.Evaluator("./seven-card-table.bin", debug: false);
            random = new System.Random();
        }

        public static void Initialize()
        {
        }

        public static double Evaluate(GameState state)
        {
            //if (evaluator == null)
            //{
            //    return 0.1d;
            //}

            var heroHand = state.Hero.HoleCards | state.Board;
            //var villainHand = BoardGenerator.GenerateHand(heroHand) | state.Board;
            //var heroStrength = evaluator.Evaluate((ulong)heroHand);
            //var villainStrength = evaluator.Evaluate((ulong)villainHand);

            //if (heroStrength > villainStrength)
            //{
            //    return (state.Pot / 2);
            //}
            //else if (heroStrength == villainStrength)
            //{
            //    return 0.0d;
            //}
            //else
            //{
            //    return -(state.Pot / 2);
            //}

            var villainWins = VillainWins(state, heroHand);
            var evaluation = villainWins ? -(state.Pot / 2) : (state.Pot / 2);

            return evaluation;
        }

        private static bool VillainWins(GameState state, Card heroHand)
        {
            var heroStrength = new SnapCall.Hand((ulong)heroHand).GetSevenCardStrength(
                (ulong)heroHand, evaluator.HandRankMap);
            var chance = random.Next(1, 1001);
            var villainPlayed = state.Hero.IsToPlay;

            switch (heroStrength.HandRanking)
            {
                case SnapCall.HandRanking.HighCard:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return chance > 200;
                        case Action.Call:
                            return villainPlayed ? chance > 50 : chance > 333;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.Pair:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return chance > 500;
                        case Action.Call:
                            return villainPlayed ? chance > 150 : chance > 500;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.TwoPair:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 850 : chance > 750;
                        case Action.Call:
                            return villainPlayed ? chance > 350 : chance > 500;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.ThreeOfAKind:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 900 : chance > 800;
                        case Action.Call:
                            return villainPlayed ? chance > 700 : chance > 900;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.Straight:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 925 : chance > 750;
                        case Action.Call:
                            return villainPlayed ? chance > 650 : chance > 925;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.Flush:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 930 : chance > 875;
                        case Action.Call:
                            return villainPlayed ? chance > 725 : chance > 900;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.FullHouse:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 980 : chance > 925;
                        case Action.Call:
                            return villainPlayed ? chance > 920 : chance > 950;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.FourOfAKind:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 990 : chance > 985;
                        case Action.Call:
                            return villainPlayed ? chance > 985 : chance > 975;
                        default:
                            throw new System.InvalidOperationException();
                    }
                case SnapCall.HandRanking.StraightFlush:
                    switch (state.LastAction)
                    {
                        case Action.Check:
                            return villainPlayed ? chance > 998 : chance > 995;
                        case Action.Call:
                            return villainPlayed ? chance > 985 : chance > 975;
                        default:
                            throw new System.InvalidOperationException();
                    }
                default:
                    throw new System.InvalidOperationException();
            }
        }
    }
}
