using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class SB
    {
        private const double TightnessFactor = 0.5;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.8 : 0.85;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.35 : 0.09;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.07 : 0.12;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}