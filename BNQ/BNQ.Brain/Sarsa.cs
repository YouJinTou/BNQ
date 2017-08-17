using BNQ.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Act = BNQ.Models.Action;

namespace BNQ.Brain
{
    public class Sarsa
    {
        private State state;
        private IDictionary<StateActionPair, double> stateActionPairs;
        private double alpha;
        private double gamma;
        private TimeSpan allowance;
        private int episodes;
        private Random rng;

        public Sarsa(
            State state, 
            IDictionary<StateActionPair, double> stateActionPairs, 
            double alpha, 
            double gamma, 
            TimeSpan allowance,
            int episodes)
        {
            this.state = state;
            this.stateActionPairs = stateActionPairs;
            this.alpha = alpha;
            this.gamma = gamma;
            this.allowance = allowance;
            this.episodes = episodes;
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

            while (this.allowance > sw.Elapsed)
            {
                KeyValuePair<StateActionPair, double> stateAction = this.GetStateAction(this.state);
                State state = stateAction.Key.State;
                Act action = stateAction.Key.Action;
                int episode = 0;

                while (episode < episodes)
                {
                    double reward;
                    State nextState = this.GetNextState(state, action, out reward);
                    KeyValuePair<StateActionPair, double> nextStateAction = this.GetStateAction(nextState);

                    this.stateActionPairs[stateAction.Key] = stateAction.Value +
                        alpha * (reward + gamma * nextStateAction.Value - stateAction.Value);

                    if (nextState.Type == StateType.HeroFolded || nextState.Type == StateType.VillainFolded)
                    {
                        episode++;
                        total++;

                        break;
                    }

                    stateAction = nextStateAction;
                    state = nextStateAction.Key.State;
                    action = nextStateAction.Key.Action;

                    episode++;
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
            var pair = this.stateActionPairs
                .Where(sap =>
                    sap.Key.State.Board == state.Board &&
                    sap.Key.State.Spr == state.Spr &&
                    state.Actions.Contains(sap.Key.Action))
                .OrderByDescending(p => p.Value)
                .ToArray();

            return pair;
        }

        private State GetNextState(State initialState, Act initialAction, out double reward)
        {
            switch (initialAction)
            {
                case Act.Bet50:
                    {
                        Act[] villainActions = new Act[] { Act.Call, Act.Fold, Act.Raise50 };
                        Act action = this.GetRandomAction(villainActions);
                        double bet = initialState.Pot / 2;
                        State state = this.GetStateAfterVillainAction(initialState, action, bet);
                        reward = this.GetReward(state, bet);

                        return state;
                    }
                case Act.Call:
                    {
                        if (Helper.IsRiver(initialState.Board))
                        {
                            double newStack = initialState.Stack - initialState.Wager;
                            double newPot = initialState.Pot + initialState.Wager;
                            double newSpr = newStack / newPot;
                            ICollection<Act> actions = new HashSet<Act> { Act.None };
                            State finalState = new State(
                                initialState.Board,
                                newSpr,
                                initialState.Wager,
                                StateType.CalledFinal,
                                actions);
                            reward = this.GetReward(finalState, initialState.Wager);

                            return finalState;
                                
                        }

                        Act[] villainActions = new Act[] { Act.Check, Act.Bet50 };
                        Act action = this.GetRandomAction(villainActions);
                        State state = this.GetStateAfterVillainAction(initialState, action, 0.0);
                        reward = this.GetReward(state, initialState.Wager);

                        return state;
                    }
                case Act.Check:
                    {
                        if (Helper.IsRiver(initialState.Board))
                        {
                            ICollection<Act> actions = new HashSet<Act> { Act.None };
                            State finalState = new State(
                                initialState.Board,
                                initialState.Spr,
                                initialState.Wager,
                                StateType.CheckedFinal,
                                actions);
                            reward = this.GetReward(finalState, 0.0);

                            return finalState;
                        }

                        Act[] villainActions = new Act[] { Act.Check, Act.Bet50 };
                        Act action = this.GetRandomAction(villainActions);
                        ulong nextBoard = this.GetNextBoard(initialState.Board);
                        State newState = new State(nextBoard, initialState.Stack, initialState.Pot, 0.0, null);
                        State state = this.GetStateAfterVillainAction(newState, action, 0.0);
                        reward = this.GetReward(state, 0.0);

                        return state;
                    }
                case Act.Fold:
                    {
                        ICollection<Act> actions = new HashSet<Act> { Act.None };
                        State state = new State(
                            initialState.Board, initialState.Spr, 0.0, StateType.HeroFolded, actions);
                        reward = this.GetReward(state, 0.0);

                        return state;
                    }
                case Act.None:
                    {
                        Act[] villainActions = new Act[] { Act.Bet50, Act.Check };
                        Act action = this.GetRandomAction(villainActions);
                        reward = 0.0;

                        return this.GetStateAfterVillainAction(initialState, action, 0.0);
                    }                   
                case Act.Raise50:
                    {
                        Act[] villainActions = new Act[] { Act.Fold, Act.Call, Act.Raise50 };
                        Act action = this.GetRandomAction(villainActions);
                        double basePot = (initialState.Pot + 2 * initialState.Wager);
                        double raise = basePot / 2;
                        State state = this.GetStateAfterVillainAction(initialState, action, raise);
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
            double spr = heroState.Spr;
            double pot = heroState.Pot;
            double stack = heroState.Stack;

            switch (villainAction)
            {
                case Act.Bet50:
                    {
                        double bet = pot / 2;
                        double newSpr = (stack - bet) / (pot + bet);
                        ICollection<Act> actions = new HashSet<Act> { Act.Call, Act.Fold, Act.Raise50 };

                        return new State(board, newSpr, bet, StateType.Alive, actions);
                    }                   
                case Act.Call:
                    {
                        double newSpr = (stack - heroWager) / (pot + heroWager);

                        if (Helper.IsRiver(board))
                        {
                            return new State(
                                board,
                                newSpr,
                                0.0,
                                StateType.CalledFinal,
                                new HashSet<Act> { Act.None });
                        }

                        ulong newBoard = this.GetNextBoard(board);
                        ICollection<Act> actions = new HashSet<Act> { Act.Check, Act.Bet50 };

                        return new State(newBoard, newSpr, 0.0, StateType.Alive, actions);
                    }
                case Act.Check:
                    {
                        if (Helper.IsRiver(board))
                        {
                            return new State(board, spr, 0.0, StateType.CheckedFinal, new HashSet<Act> { Act.None });
                        }

                        ulong newBoard = this.GetNextBoard(board);
                        ICollection<Act> actions = new HashSet<Act> { Act.Check, Act.Bet50 };

                        return new State(newBoard, spr, 0.0, StateType.Checked, actions);
                    }
                case Act.Fold:
                    {
                        return new State(board, spr, 0.0, StateType.VillainFolded, new HashSet<Act> { Act.None });
                    }
                case Act.Raise50:
                    {
                        double basePot = (pot + 2 * heroWager);
                        double raise = basePot / 2;
                        double newSpr = (stack - raise) / (basePot + raise);
                        ICollection<Act> actions = new HashSet<Act> { Act.Fold, Act.Call, Act.Raise50 };

                        return new State(board, newSpr, raise, StateType.Alive, actions);
                    }
                default:
                    return null;
            }
        }

        private ulong GetNextBoard(ulong board)
        {
            ulong lastCard = 36028797018963968;

            for (ulong card = 1; card <= lastCard; card *= 2)
            {
                bool cardExists = (board & card) == card;

                if (!cardExists)
                {
                    return (board |= card);
                }
            }

            return board |= lastCard;
        }

        private double GetReward(State stateAfterVillainAction, double amountRisked)
        {
            switch (stateAfterVillainAction.Type)
            {
                case StateType.Alive:
                    return -amountRisked;
                case StateType.CalledFinal:
                    return 0.0; // EVALUATE
                case StateType.Checked:
                    return 0.0;
                case StateType.CheckedFinal:
                    return 0.0; // EVALUATE
                case StateType.VillainFolded:
                    return stateAfterVillainAction.Pot;
                case StateType.HeroFolded:
                    return 0.0;
                default:
                    return 0.0;
            }
        }
    }
}
