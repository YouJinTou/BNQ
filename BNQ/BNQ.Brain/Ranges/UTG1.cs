using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class UTG1
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.27 : 0.18;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.2 : 0.16;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.07 : 0.1;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}