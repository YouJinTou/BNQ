using BNQ.Models;
using System.Collections.Generic;
using System.Linq;

namespace BNQ.Brain
{
    public class StateActionGenerator : IStateActionGenerator
    {
        private State inputState;
        private IPlayer hero;

        public StateActionGenerator(State inputState, IPlayer hero)
        {
            this.inputState = inputState;
            this.hero = hero;            
        }

        public IDictionary<StateActionPair, double> GeneratePairs()
        {
            IDictionary<StateActionPair, double> pairs =
                new Dictionary<StateActionPair, double>();
            double[] sprs = this.GetCappedSprs();
            IList<Action> allActions = new List<Action>
            {
                Action.Fold,
                Action.Check,
                Action.Call,
                Action.Bet50,
                Action.Raise50,
                Action.None
            };

            for (int spr = 0; spr < sprs.Length; spr++)
            {
                State nextState = new State(
                    this.inputState.Board, sprs[spr], this.inputState.Wager, StateType.Alive, allActions);

                this.GetStateActionPairOnBoard(pairs, nextState, allActions);
            }

            int cardsSoFar = Helper.GetDealtCardsCount(this.inputState.Board);
            int river = 5;
            bool isFlop = (cardsSoFar == 3);
            bool isRiver = (cardsSoFar == river);

            if (isRiver)
            {
                return pairs;
            }

            ICollection<ulong> possibleCards = this.GetPossibleCards(this.inputState.Board);            

            foreach (ulong card in possibleCards)
            {
                ulong nextBoard = this.inputState.Board | card;

                for (int spr = 0; spr < sprs.Length; spr++)
                {
                    State nextState = new State(
                        nextBoard, sprs[spr], this.inputState.Wager, StateType.Alive, allActions);

                    this.GetStateActionPairOnBoard(pairs, nextState, allActions);
                }

                if (isFlop)
                {
                    ICollection<ulong> possibleRiverCards = this.GetPossibleCards(nextBoard);

                    foreach (ulong riverCard in possibleRiverCards)
                    {
                        ulong riverBoard = nextBoard | riverCard;

                        for (int spr = 0; spr < sprs.Length; spr++)
                        {
                            State riverState = new State(
                                riverBoard, sprs[spr], this.inputState.Wager, StateType.Alive, allActions);

                            this.GetStateActionPairOnBoard(pairs, riverState, allActions);
                        }
                    }

                    isFlop = false;
                }
            }

            return pairs;
        }

        private IDictionary<StateActionPair, double> GetStateActionPairOnBoard(
            IDictionary<StateActionPair, double> pairs, State state, IList<Action> actions)
        {
            double stateActionValue = 0.0;

            for (int a = 0; a < actions.Count; a++)
            {
                Action action = actions[a];
                StateActionPair pair = new StateActionPair(state, action);

                pairs.Add(pair, stateActionValue);
            }

            return pairs;
        }

        private double[] GetCappedSprs()
        {
            IList<double> sprs = new List<double> { 0.5, 1, 2, 4, 8, 13, 20, 30 };
            IList<double> result = new List<double>();
            double maxSpr = this.inputState.Spr;

            foreach (double spr in sprs)
            {
                if (maxSpr >= spr)
                {
                    result.Add(spr);
                }
            }

            return result.ToArray();
        }

        private ICollection<ulong> GetPossibleCards(ulong board)
        {
            ICollection<ulong> cards = new HashSet<ulong>();
            ICollection<ulong> dealtCards = this.GetDealtCards(board);
            ulong lastCard = 2251799813685248;

            for (ulong card = 1; card <= lastCard; card *= 2)
            {
                if (!dealtCards.Contains(card))
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        private ICollection<ulong> GetDealtCards(ulong board)
        {
            ICollection<ulong> dealtCards = new HashSet<ulong>()
            {
                (ulong)this.hero.Holdings[0].First,
                (ulong)this.hero.Holdings[0].Second
            };

            for (int i = 0; i < 64; i++)
            {
                ulong bitCard = (board & ((ulong)1 << i));

                if (bitCard != 0)
                {
                    dealtCards.Add(bitCard);
                }
            }

            return dealtCards;
        }
    }
}
