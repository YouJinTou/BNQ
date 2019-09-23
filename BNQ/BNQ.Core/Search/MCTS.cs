using System.Collections.Generic;
using System.Linq;

namespace BNQ.Core.Search
{
    public class MCTS
    {
        public State Select(State root)
        {
            return root.Select();
        }

        public void Expand(State leaf)
        {
            leaf.Expand();
        }

        public void Simulate(State leaf)
        {
            leaf.Simulate();
        }

        public void Propagate(State leaf)
        {
            leaf.Propagate();
        }

        public State GetBestNextState(State root)
        {
            return root.Children.OrderByDescending(c => c.AverageValue).First();
        }
    }
}
