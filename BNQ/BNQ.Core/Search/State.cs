using BNQ.Core.Extensions;
using System;
using System.Collections.Generic;

namespace BNQ.Core.Search
{
    public abstract class State
    {
        public const double ExplorationConstant = 2.0d;

        public int Visits { get; set; }

        public bool IsSampled => this.Visits > 0;

        public int Wins { get; set; }

        public double Value { get; set; }

        public State Parent { get; protected set; }

        public bool IsLeaf => this.Parent != null && this.Children.Count == 0;

        public ICollection<State> Children { get; protected set; }

        public double AverageValue => this.IsSampled ? this.Value / this.Visits : 0.0d;

        public double GetUcb()
        {
            var exploitationTerm = (this.Visits == 0) ? 0 : this.Wins / this.Visits;
            var explorationTerm = ExplorationConstant *
                Math.Sqrt(Math.Log(this.Parent.Visits) / this.Visits);
            var ucb = exploitationTerm + explorationTerm;

            return ucb;
        }

        public State Select()
        {
            var current = this;

            while (!current.IsLeaf)
            {
                var maximumUcbChild = current.Children.GetMaxUcbState();

                if (maximumUcbChild == null)
                {
                    return current;
                }

                current = maximumUcbChild;
            }

            return current;
        }

        public abstract State Expand();

        public abstract void Simulate();

        public void Propagate()
        {
            var current = this.Parent;

            while (current != null)
            {
                current.Visits++;
                current = current.Parent;
            }
        }
    }
}
