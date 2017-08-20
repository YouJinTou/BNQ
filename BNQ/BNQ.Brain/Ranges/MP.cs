using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class MP
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.3 : 0.2;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.21 : 0.18;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.08 : 0.12;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}