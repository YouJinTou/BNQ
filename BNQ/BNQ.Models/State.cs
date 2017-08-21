using Act = BNQ.Models.Action;
using System;
using System.Collections.Generic;

namespace BNQ.Models
{
    public class State
    {
        private ulong board;
        private double pot;
        private double stack;
        private double villainRange;
        private double spr;
        private double wager;
        private StateType type;
        private ICollection<Act> actions;

        public State(ulong board, double pot, double stack, double villainRange, double wager, StateType type, ICollection<Act> actions)
        {
            this.board = board;
            this.pot = pot;
            this.stack = stack;
            this.villainRange = villainRange;
            this.spr = this.NormalizeSpr(stack / pot);
            this.wager = wager;
            this.type = type;
            this.actions = actions;
        }

        public void SetPot(double wager, bool heroWagering)
        {
            this.pot += wager;
            this.stack = heroWagering ? this.stack - wager : this.stack;

            this.NormalizeSpr(this.pot / this.stack);
        }

        public ulong Board
        {
            get
            {
                return this.board;
            }
            set
            {
                this.board = value;
            }
        }

        public double Pot
        {
            get
            {
                return this.pot;
            }
        }

        public double VillainRange
        {
            get
            {
                return (this.villainRange == 0.0) ? 1.0 : this.villainRange;
            }
        }

        public double Wager
        {
            get
            {
                return this.wager;
            }
            set
            {
                this.wager = value;
            }
        }

        public StateType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public ICollection<Act> Actions
        {
            get
            {
                return this.actions;
            }
            set
            {
                this.actions = value;
            }
        }

        public double Spr
        {
            get
            {
                return this.spr;
            }
        }

        public double Stack
        {
            get
            {
                return this.stack;
            }
        }

        public bool Equals(State other)
        {
            return (other != null & this.board == other.board && this.spr == other.spr);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((State)obj);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + this.board.GetHashCode();
            hash = (hash * 7) + this.spr.GetHashCode();

            return hash;
        }

        private double NormalizeSpr(double spr)
        {
            double[] sprs = new double[] { 0.5, 1, 2, 4, 8, 13, 20, 30 };
            double smallestDiff = double.MaxValue;
            int bestIndex = 0;

            for (int i = 0; i < sprs.Length; i++)
            {
                double currentDiff = Math.Abs(sprs[i] - spr);

                if (currentDiff < smallestDiff)
                {
                    smallestDiff = currentDiff;
                    bestIndex = i;
                }
            }

            return sprs[bestIndex];
        }
    }
}
