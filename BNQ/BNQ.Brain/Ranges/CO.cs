using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class CO
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.45 : 0.38;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.35 : 0.34;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.16 : 0.21;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}