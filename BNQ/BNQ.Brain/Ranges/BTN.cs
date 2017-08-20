using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class BTN
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.5 : 0.7;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.38 : 0.45;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.18 : 0.23;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}