using BNQ.Core.Abstractions;
using BNQ.Core.Extensions;
using BNQ.Core.Models;
using BNQ.Core.Search;
using BNQ.Core.Tools;

namespace BNQ.Core.Tests.Doubles
{
    internal class SimulatorDouble
    {
        private readonly IStateFactory stateFactory;
        private readonly IEvaluator evaluator;
        private readonly IBoardGenerator generator;

        public SimulatorDouble(
            IStateFactory stateFactory, IEvaluator evaluator, IBoardGenerator generator)
        {
            this.stateFactory = stateFactory;
            this.evaluator = evaluator;
            this.generator = generator;
        }

        public double Simulate(GameState state)
        {
            var stateCopy = GameState.CopyState(state);

            if (stateCopy.IsShowdown)
            {
                if (!state.AnyAllIn)
                {
                    state.Board = this.generator.FillToRiver(
                        state.Board, Street.Flop, state.Hero.HoleCards);
                }

                return this.evaluator.Evaluate(stateCopy);
            }

            if (stateCopy.Street == Street.Fold)
            {
                return stateCopy.AverageValue;
            }

            stateCopy.Board = this.generator.FillFromTo(
                stateCopy.Board,
                Street.Flop,
                stateCopy.Street,
                stateCopy.Hero.HoleCards);
            var stateValue = 0.0d;

            while (!GameState.IsOver(stateCopy.Street))
            {
                switch (stateCopy.ToPlay.GetAction(stateCopy.WagerToCall))
                {
                    case Action.Check:
                        {
                            var isLastToAct = stateCopy.ToPlay.IsLastToAct(Action.Check);
                            stateCopy.WagerToCall = 0.0d;
                            stateCopy.LastAction = Action.Check;

                            if (isLastToAct && stateCopy.Street.ShouldShowdown())
                            {
                                stateCopy.Street = stateCopy.Street.GetNextStreet();
                                stateValue = this.evaluator.Evaluate(stateCopy);

                                break;
                            }

                            if (isLastToAct)
                            {
                                stateCopy.Board |= this.generator.GenerateCard(
                                    stateCopy.Board | stateCopy.Hero.HoleCards);
                                stateCopy.Street = stateCopy.Street.GetNextStreet();
                            }

                            stateCopy.ToPlay = isLastToAct ?
                                (stateCopy.Hero.IsFirstToActAfterFlop() ?
                                    stateCopy.Hero : stateCopy.Villain) :
                                stateCopy.Hero.IsToPlay ? stateCopy.Villain : stateCopy.Hero;
                            stateCopy.Hero.IsToPlay = false;
                            stateCopy.Villain.IsToPlay = false;
                            stateCopy.ToPlay.IsToPlay = true;

                            break;
                        }
                    case Action.Bet50:
                        {
                            var bet = stateCopy.ToPlay.Wager(stateCopy.Pot.GetHalfPot());
                            stateCopy.Pot += bet;
                            stateCopy.WagerToCall = bet;
                            stateCopy.ToPlay = stateCopy.Hero.IsToPlay ?
                                stateCopy.Villain : stateCopy.Hero;
                            stateCopy.Hero.IsToPlay = false;
                            stateCopy.Villain.IsToPlay = false;
                            stateCopy.ToPlay.IsToPlay = true;
                            stateCopy.LastAction = Action.Bet50;

                            break;
                        }
                    case Action.Call:
                        {
                            var callAmount = stateCopy.ToPlay.Wager(stateCopy.WagerToCall);
                            stateCopy.Pot += callAmount;
                            stateCopy.WagerToCall = 0.0d;
                            stateCopy.LastAction = Action.Call;

                            if (stateCopy.Street.ShouldShowdown())
                            {
                                stateCopy.Street = stateCopy.Street.GetNextStreet();
                                stateValue = this.evaluator.Evaluate(stateCopy);

                                break;
                            }

                            stateCopy.Board |= this.generator.GenerateCard(
                                stateCopy.Board | stateCopy.Hero.HoleCards);
                            stateCopy.Street = stateCopy.Street.GetNextStreet();
                            stateCopy.ToPlay = stateCopy.Hero.IsFirstToActAfterFlop() ?
                                stateCopy.Hero : stateCopy.Villain;
                            stateCopy.Hero.IsToPlay = false;
                            stateCopy.Villain.IsToPlay = false;
                            stateCopy.ToPlay.IsToPlay = true;

                            break;
                        }
                    case Action.Raise50:
                        {
                            var raiseAmount = PotTools.GetRaiseAmount(
                                Wager.Half, stateCopy.Pot, stateCopy.WagerToCall);
                            var raise = stateCopy.ToPlay.Wager(raiseAmount);
                            if (stateCopy.ToPlay.Stack <= 0)
                            {

                            }
                            stateCopy.WagerToCall = System.Math.Abs(raise - stateCopy.WagerToCall);
                            stateCopy.Pot += raise;
                            stateCopy.ToPlay = stateCopy.Hero.IsToPlay ?
                                stateCopy.Villain : stateCopy.Hero;
                            stateCopy.Hero.IsToPlay = false;
                            stateCopy.Villain.IsToPlay = false;
                            stateCopy.ToPlay.IsToPlay = true;
                            stateCopy.LastAction = Action.Raise50;

                            break;
                        }
                    case Action.Fold:
                        {
                            if (stateCopy.ToPlay.Stack <= 0)
                            {

                            }
                            stateCopy.Street = Street.Fold;
                            stateCopy.WagerToCall = 0.0d;
                            stateCopy.LastAction = Action.Fold;
                            var investedAmount = (stateCopy.Pot - stateCopy.WagerToCall) / 2;
                            stateValue =
                                stateCopy.ToPlay.IsHero ? -investedAmount : investedAmount;

                            break;
                        }
                }
            }

            return stateValue;
        }
    }
}
