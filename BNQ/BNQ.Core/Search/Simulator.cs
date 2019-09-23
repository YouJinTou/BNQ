using BNQ.Core.Extensions;
using BNQ.Core.Models;
using BNQ.Core.Tools;
using System.Collections.Generic;

namespace BNQ.Core.Search
{
    internal class Simulator
    {
        public static double Simulate(GameState root)
        {
            var traces = new Queue<string>();
            var state = new GameState(
                (GameState)root.Parent,
                root.Board,
                root.Street,
                root.Hero,
                root.Villain,
                root.Pot,
                root.WagerToCall,
                root.LastAction);

            if (state.PlayerFolded)
            {
                var investedAmount = state.Hero.IsToPlay ? 
                    state.GetInvestedAmount() : -state.GetInvestedAmount();

                return investedAmount;
            }

            if (state.IsShowdown)
            {
                TryUpdateBoard(state, Street.Flop);

                return Evaluator.Evaluate(state);
            }

            state.Board = BoardGenerator.FillFromTo(
                state.Board,
                Street.Flop,
                state.Street,
                state.Hero.HoleCards);
            var stateValue = 0.0d;

            while (!GameState.IsOver(state.Street))
            {
                switch (state.ToPlay.GetAction(state.WagerToCall))
                {
                    case Action.Check:
                        {
                            var isLastToAct = state.ToPlay.IsLastToAct(Action.Check);
                            state.WagerToCall = 0.0d;
                            state.LastAction = Action.Check;

                            if (isLastToAct && state.Street.ShouldShowdown())
                            {
                                state.Street = state.Street.GetNextStreet();
                                stateValue = Evaluator.Evaluate(state);

                                break;
                            }

                            if (isLastToAct)
                            {
                                state.Board |= BoardGenerator.GenerateCard(
                                    state.Board | state.Hero.HoleCards);
                                state.Street = state.Street.GetNextStreet();
                            }

                            state.ToPlay = isLastToAct ?
                                (state.Hero.IsFirstToActAfterFlop() ?
                                    state.Hero : state.Villain) :
                                state.Hero.IsToPlay ? state.Villain : state.Hero;
                            state.Hero.IsToPlay = false;
                            state.Villain.IsToPlay = false;
                            state.ToPlay.IsToPlay = true;

                            traces.Enqueue(state.ToString());

                            break;
                        }
                    case Action.Bet50:
                        {
                            var bet = state.ToPlay.Wager(state.Pot.GetHalfPot());
                            state.Pot += bet;
                            state.WagerToCall = bet;
                            state.ToPlay = state.Hero.IsToPlay ?
                                state.Villain : state.Hero;
                            state.Hero.IsToPlay = false;
                            state.Villain.IsToPlay = false;
                            state.ToPlay.IsToPlay = true;
                            state.LastAction = Action.Bet50;

                            traces.Enqueue(state.ToString());
                            break;
                        }
                    case Action.Call:
                        {
                            var callAmount = state.ToPlay.Wager(state.WagerToCall);
                            state.Pot += callAmount;
                            state.WagerToCall = 0.0d;
                            state.LastAction = Action.Call;

                            if (state.Street.ShouldShowdown() || state.AnyAllIn)
                            {
                                state.Board = BoardGenerator.FillToRiver(
                                    state.Board, state.Street, state.Hero.HoleCards);
                                state.Street = Street.Showdown;
                                stateValue = Evaluator.Evaluate(state);

                                break;
                            }

                            state.Board |= BoardGenerator.GenerateCard(
                                state.Board | state.Hero.HoleCards);
                            state.Street = state.Street.GetNextStreet();
                            state.ToPlay = state.Hero.IsFirstToActAfterFlop() ?
                                state.Hero : state.Villain;
                            state.Hero.IsToPlay = false;
                            state.Villain.IsToPlay = false;
                            state.ToPlay.IsToPlay = true;

                            traces.Enqueue(state.ToString());
                            break;
                        }
                    case Action.Raise50:
                        {
                            var raiseAmount = PotTools.GetRaiseAmount(
                                Wager.Half, state.Pot, state.WagerToCall);
                            var raise = state.ToPlay.Wager(raiseAmount);
                            state.WagerToCall = System.Math.Abs(raise - state.WagerToCall);
                            state.Pot += raise;
                            state.ToPlay = state.Hero.IsToPlay ?
                                state.Villain : state.Hero;
                            state.Hero.IsToPlay = false;
                            state.Villain.IsToPlay = false;
                            state.ToPlay.IsToPlay = true;
                            state.LastAction = Action.Raise50;

                            traces.Enqueue(state.ToString());
                            break;
                        }
                    case Action.Fold:
                        {
                            var investedAmount = state.GetInvestedAmount();
                            state.Street = Street.Fold;
                            state.WagerToCall = 0.0d;
                            state.LastAction = Action.Fold;
                            stateValue =
                                state.ToPlay.IsHero ? -investedAmount : investedAmount;

                            break;
                        }
                }
            }

            return stateValue;
        }

        private static void TryUpdateBoard(GameState state, Street streetToStartUpdateFrom)
        {
            if (!state.AnyAllIn)
            {
                state.Board = BoardGenerator.FillToRiver(
                    state.Board, streetToStartUpdateFrom, state.Hero.HoleCards);
            }
        }
    }
}
