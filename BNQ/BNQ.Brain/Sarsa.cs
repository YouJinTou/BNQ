using Act = BNQ.Models.Action;
using BNQ.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BNQ.Brain
{
    public class Sarsa
    {
        private State state;
        private bool heroInPosition;
        private ulong heroHand;
        private ulong[] villainRange;
        private IDictionary<StateActionPair, double> stateActionPairs;
        private IEvaluator evaluator;
        private double alpha;
        private double gamma;
        private TimeSpan allowance;
        private Random rng;

        public Sarsa(
            State state,
            bool heroInPosition,
            ulong heroHand,
            ulong[] villainRange,
            IDictionary<StateActionPair, double> stateActionPairs,
            IEvaluator evaluator,
            double alpha,
            double gamma,
            TimeSpan allowance)
        {
            this.state = state;
            this.heroInPosition = heroInPosition;
            this.heroHand = heroHand;
            this.villainRange = villainRange;
            this.stateActionPairs = stateActionPairs;
            this.evaluator = evaluator;
            this.alpha = alpha;
            this.gamma = gamma;
            this.allowance = allowance;
            this.rng = new Random();
        }

        public Act GetBestAction()
        {
            this.Learn();

            var pairs = this.GetStateActionPairs(this.state);

            for (int i = 0; i < pairs.Length; i++)
            {
                Console.WriteLine($"{pairs[i].Key.Action.ToString()}: {pairs[i].Value}");
            }

            return pairs.First().Key.Action;
        }

        private void Learn()
        {
            Stopwatch sw = new Stopwatch();
            int total = 0;

            sw.Start();

            while (sw.Elapsed < this.allowance)
            {
                KeyValuePair<StateActionPair, double> stateAction = this.GetStateAction(this.state);
                State state = stateAction.Key.State;
                Act action = stateAction.Key.Action;

                while (true)
                {
                    double reward;
                    State nextState = this.GetNextState(state, action, out reward);

                    if (nextState.Actions.Contains(Act.None))
                    {
                        this.stateActionPairs[stateAction.Key] = stateAction.Value +
                            alpha * (reward + gamma * 0.0 - stateAction.Value);

                        total++;

                        break;
                    }

                    KeyValuePair<StateActionPair, double> nextStateAction = this.GetStateAction(nextState);

                    this.stateActionPairs[stateAction.Key] = stateAction.Value + 
                        alpha * (reward + gamma * 0.0 - stateAction.Value);

                    if (nextState.Type == StateType.HeroFolded ||
                        nextState.Type == StateType.VillainFolded ||
                        nextState.Type == StateType.CalledFinal ||
                        nextState.Type == StateType.CheckedFinal)
                    {
                        total++;

                        break;
                    }

                    stateAction = nextStateAction;
                    state = nextStateAction.Key.State;
                    action = nextStateAction.Key.Action;

                    total++;
                }
            }

            var x = this.stateActionPairs.Where(sap => sap.Value != 0).ToList();

            Console.WriteLine("Total " + total);
        }

        private KeyValuePair<StateActionPair, double> GetStateAction(State state)
        {
            var pairs = this.GetStateActionPairs(state);
            int epsilon = this.rng.Next(1, 11);

            if (epsilon != 1)
            {
                return pairs[0];
            }
            else
            {
                int indexToReturn = this.rng.Next(0, pairs.Length);

                return pairs[indexToReturn];
            }
        }

        private KeyValuePair<StateActionPair, double>[] GetStateActionPairs(State state)
        {
            var pairs = this.stateActionPairs
                .Where(sap =>
                    sap.Key.State.Board == state.Board &&
                    sap.Key.State.Spr == state.Spr &&
                    state.Actions.Contains(sap.Key.Action))
                .OrderByDescending(sap => sap.Value)
                .ToArray();

            for (int i = 0; i < pairs.Length; i++)
            {
                StateActionPair stateAction = new StateActionPair(state, pairs[i].Key.Action);

                this.stateActionPairs.Remove(pairs[i].Key);

                if (!this.stateActionPairs.ContainsKey(pairs[i].Key))
                {
                    this.stateActionPairs.Add(stateAction, pairs[i].Value);
                }
            }

            return pairs;
        }

        private State GetNextState(State currentState, Act initialAction, out double reward)
        {
            switch (initialAction)
            {
                case Act.Bet50:
                    {
                        Act[] villainActions = new Act[] { Act.Call, Act.Fold, Act.Raise50 };
                        Act action = this.GetRandomAction(villainActions);
                        double bet = currentState.Pot / 2;

                        currentState.SetPot(bet, true);

                        State state = this.GetStateAfterVillainAction(currentState, action, bet);
                        reward = this.GetReward(state, bet);

                        return state;
                    }
                case Act.Call:
                    {
                        if (Helper.IsRiver(currentState.Board))
                        {
                            currentState.SetPot(currentState.Wager, true);
                            currentState.Type = StateType.CalledFinal;
                            currentState.Actions = new HashSet<Act> { Act.None };

                            reward = this.GetReward(currentState, currentState.Wager);

                            return currentState;
                        }

                        Act[] villainActions = new Act[] { Act.Check, Act.Bet50 };
                        Act action = this.GetRandomAction(villainActions);
                        State state = this.GetStateAfterVillainAction(currentState, action, 0.0);
                        reward = this.GetReward(state, currentState.Wager);

                        return state;
                    }
                case Act.Check:
                    {
                        if (Helper.IsRiver(currentState.Board) && heroInPosition)
                        {
                            currentState.Actions = new HashSet<Act> { Act.None };
                            currentState.Type = StateType.CheckedFinal;
                            reward = this.GetReward(currentState, 0.0);

                            return currentState;
                        }

                        Act[] villainActions = new Act[] { Act.Check, Act.Bet50 };
                        Act action = this.GetRandomAction(villainActions);
                        currentState.Actions = villainActions;
                        currentState.Board = heroInPosition ? this.GetNextBoard(currentState.Board) : currentState.Board;
                        currentState.Type = StateType.Checked;
                        State state = this.GetStateAfterVillainAction(currentState, action, 0.0);
                        reward = this.GetReward(state, 0.0);

                        return state;
                    }
                case Act.Fold:
                    {
                        currentState.Actions = new HashSet<Act> { Act.None };
                        currentState.Type = StateType.HeroFolded;
                        currentState.Wager = 0.0;
                        reward = this.GetReward(currentState, 0.0);

                        return state;
                    }
                case Act.None:
                    {
                        Act[] villainActions = new Act[] { Act.Bet50, Act.Check };
                        Act action = this.GetRandomAction(villainActions);
                        reward = 0.0;

                        return this.GetStateAfterVillainAction(currentState, action, 0.0);
                    }
                case Act.Raise50:
                    {
                        Act[] villainActions = new Act[] { Act.Fold, Act.Call, Act.Raise50 };
                        Act action = this.GetRandomAction(villainActions);
                        double basePot = (currentState.Pot + 2 * currentState.Wager);
                        double raise = basePot / 2;
                        currentState.Actions = villainActions;
                        currentState.SetPot(raise, true);
                        currentState.Type = StateType.Alive;

                        State state = this.GetStateAfterVillainAction(currentState, action, raise);
                        reward = this.GetReward(state, raise);

                        return state;
                    }
                default:
                    reward = 0.0;

                    return null;
            }
        }

        private Act GetRandomAction(Act[] availableActions)
        {
            int actionIndex = this.rng.Next(0, availableActions.Length);

            return availableActions[actionIndex];
        }

        private State GetStateAfterVillainAction(
            State heroState, Act villainAction, double heroWager)
        {
            ulong board = heroState.Board;
            double range = heroState.VillainRange;
            double spr = heroState.Spr;
            double pot = heroState.Pot;
            double stack = heroState.Stack;
            double facingSize = (heroWager / (pot - heroWager));

            switch (villainAction)
            {
                case Act.Bet50:
                    {
                        double bet = pot / 2;
                        double newPot = pot + bet;
                        ICollection<Act> actions = new HashSet<Act> { Act.Call, Act.Fold, Act.Raise50 };

                        return new State(board, newPot, stack, this.RecalculateRange(0.5, range), bet, StateType.Alive, actions);
                    }
                case Act.Call:
                    {
                        double newPot = pot + heroWager;
                        double defendByCallFrequency = 0.7;
                        double newRange = this.RecalculateRange(defendByCallFrequency * facingSize, range);

                        if (Helper.IsRiver(board))
                        {
                            return new State(
                                board,
                                newPot,
                                stack,
                                newRange,
                                0.0,
                                StateType.CalledFinal,
                                new HashSet<Act> { Act.None });
                        }

                        ulong newBoard = this.GetNextBoard(board);
                        ICollection<Act> actions = new HashSet<Act> { Act.Check, Act.Bet50 };

                        return new State(newBoard, newPot, stack, newRange, 0.0, StateType.Alive, actions);
                    }
                case Act.Check:
                    {
                        if (Helper.IsRiver(board) && !heroInPosition)
                        {
                            double showdownFrequency = 0.8;
                            double finalRange = this.RecalculateRange(0.0, showdownFrequency * range);

                            return new State(board, pot, stack, finalRange, 0.0, StateType.CheckedFinal, new HashSet<Act> { Act.None });
                        }

                        ICollection<Act> actions = new HashSet<Act> { Act.Check, Act.Bet50 };
                        double checkFrequency = 0.5;
                        double checkRange = this.RecalculateRange(0.0, checkFrequency * range);

                        return new State(heroInPosition ? board : this.GetNextBoard(board), pot, stack, checkRange, 0.0, StateType.Checked, actions);
                    }
                case Act.Fold:
                    {
                        return new State(board, pot, stack, 0.0, 0.0, StateType.VillainFolded, new HashSet<Act> { Act.None });
                    }
                case Act.Raise50:
                    {
                        double basePot = (pot + 2 * heroWager);
                        double raise = basePot / 2;
                        double newPot = pot + raise;
                        double polarizationFactor = 1.5;
                        double newRange = this.RecalculateRange(polarizationFactor * facingSize, range);
                        ICollection<Act> actions = new HashSet<Act> { Act.Fold, Act.Call, Act.Raise50 };

                        return new State(board, newPot, stack, newRange, raise, StateType.Alive, actions);
                    }
                default:
                    return null;
            }
        }

        private double RecalculateRange(double betSizePercent, double villainRange)
        {
            double change = (1 - (betSizePercent / (betSizePercent + 1)));
            double newRange = villainRange * change;
            double max = change;
            double min = -change;
            double factor = 0.015;
            double randomness = factor * (this.rng.NextDouble() * (max - min) + min);

            return newRange + randomness;
        }

        private ulong GetNextBoard(ulong board)
        {
            bool cardExists = true;
            ulong card = 1;
            int firstCardIndex = 4;
            int lastCardIndex = 56;

            do
            {
                int shift = this.rng.Next(firstCardIndex, lastCardIndex);
                card = ((ulong)1 << shift);
                cardExists = (board & card) == card || (card & this.heroHand) == card;
            } while (cardExists);

            return board |= card;
        }

        private double GetReward(State state, double amountRisked)
        {
            switch (state.Type)
            {
                case StateType.Alive:
                    return -amountRisked;
                case StateType.CalledFinal:
                    return this.GetEvaluationReward(state, amountRisked);
                case StateType.Checked:
                    return 0.0;
                case StateType.CheckedFinal:
                    return this.GetEvaluationReward(state, amountRisked);
                case StateType.VillainFolded:
                    return state.Pot;
                case StateType.HeroFolded:
                    return 0.0;
                default:
                    return 0.0;
            }
        }

        private double GetEvaluationReward(State state, double amountRisked)
        {
            ulong[] hands = new ulong[] { this.heroHand, this.GetVillainHand(state.Board, state.VillainRange) };
            int winner = this.evaluator.Evaluate(state.Board, hands);
            bool isSplit = (winner == -1);
            bool heroWins = (winner == 0);

            return isSplit ? (state.Pot / 2) :
                heroWins ? state.Pot : -amountRisked;
        }

        private ulong GetVillainHand(ulong board, double range)
        {
            IList<IHolding> hands = new List<IHolding>();

            for (int i = 0; i < this.villainRange.Length; i++)
            {
                hands.Add(this.evaluator.GetHolding(board, this.villainRange[i]));
            }

            hands.OrderByDescending(h => h.HandStrength + h.Rank);

            double valuePortion = 0.7;
            double bluffPortion = 0.3;
            int valueHandsCount = (int)(range * valuePortion * hands.Count);
            int bluffHandsCount = (int)(range * bluffPortion * hands.Count);
            IList<IHolding> finalHands = new List<IHolding>();

            for (int i = 0, j = hands.Count - 1; i < hands.Count; i++, j--)
            {
                if (i < valueHandsCount)
                {
                    finalHands.Add(hands[i]);
                }

                if (i < bluffHandsCount)
                {
                    finalHands.Add(hands[j]);
                }
            }

            int randomHandIndex = this.rng.Next(0, finalHands.Count);

            return finalHands[randomHandIndex].Hand;
        }
    }
}
