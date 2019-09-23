using System;

namespace BNQ.Core.Models
{
    public class Player
    {
        public Player(Player player)
            : this(player.HoleCards, player.Position, player.Stack, player.IsToPlay, player.IsHero)
        {
        }

        public Player(
            Card holeCards, Position position, double stack, bool isToPlay, bool isHero = false)
        {
            this.HoleCards = holeCards;
            this.Position = position;
            this.Stack = stack;
            this.HasFolded = false;
            this.IsToPlay = isToPlay;
            this.IsHero = isHero;
        }

        public Card HoleCards { get; }

        public Position Position { get; }

        public double Stack { get; private set; }

        public bool HasFolded { get; set; }

        public bool IsToPlay { get; set; }

        public bool IsHero { get; }

        public bool IsAllIn => this.Stack <= 0.0d;

        public double Wager(double amount)
        {
            var toWager = Math.Min(amount, this.Stack);
            this.Stack -= toWager;

            return toWager;
        }

        public Action GetAction(double wagerToCall)
        {
            var random = new Random();
            var actions = (wagerToCall == 0) ? GetCheckedToActions() : GetActionVsWager(wagerToCall);

            return actions[random.Next(0, actions.Length)];
        }

        public override string ToString()
        {
            var status = this.HasFolded ? "Folded" : "Active";

            return $"{this.Position} | {this.HoleCards} | {this.Stack} | {status}";
        }

        public Action[] GetCheckedToActions()
        {
            return new Action[]
            {
                Action.Bet50,
                Action.Check
            };
        }

        public Action[] GetActionVsWager(double wagerToCall)
        {
            if (wagerToCall > this.Stack)
            {
                return new Action[]
                {
                    Action.Call,
                    Action.Fold
                };
            }

            return new Action[]
            {
                Action.Call,
                Action.Fold,
                Action.Raise50
            };
        }
    }
}
