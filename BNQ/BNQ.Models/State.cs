using System;

namespace BNQ.Models
{
    public class State
    {
        private ulong board;
        private double spr;
        private double stack;
        private double pot;
        private double wager;
        private StateType type;

        public State(ulong board, double spr, double wager, StateType type)
        {
            this.board = board;
            this.spr = this.NormalizeSpr(spr);
            this.stack = spr * 3;
            this.pot = this.stack / this.spr;
            this.wager = wager;
            this.type = type;
        }

        public ulong Board
        {
            get
            {
                return this.board;
            }
        }

        public double Wager
        {
            get
            {
                return this.wager;
            }
        }

        public StateType Type
        {
            get
            {
                return this.type;
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

        public double Pot
        {
            get
            {
                return this.pot;
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
