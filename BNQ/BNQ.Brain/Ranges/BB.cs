using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class BB
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 100.0;

            switch (vsAction)
            {
                case Act.Call:
                    range = (action == Act.Check) ? 1.0 : 0.27;
                    return range;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.38 : 0.15;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}