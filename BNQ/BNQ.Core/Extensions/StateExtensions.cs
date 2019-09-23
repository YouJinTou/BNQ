using BNQ.Core.Search;
using System.Collections.Generic;

namespace BNQ.Core.Extensions
{
    internal static class StateExtensions
    {
        public static State GetMaxUcbState(this IEnumerable<State> states)
        {
            var maxUcb = double.MinValue;
            var maxState = default(State);

            foreach (var state in states)
            {
                var ucb = state.GetUcb();

                if (maxUcb < ucb)
                {
                    maxUcb = ucb;
                    maxState = state;
                }
            }

            return maxState;
        }
    }
}
