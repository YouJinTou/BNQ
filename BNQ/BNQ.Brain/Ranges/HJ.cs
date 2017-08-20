using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class HJ
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.37 : 0.25;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.25 : 0.23;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.14 : 0.18;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}