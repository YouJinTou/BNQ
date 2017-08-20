using Act = BNQ.Models.Action;
using System;

namespace BNQ.Brain
{
    internal class MP1
    {
        private const double TightnessFactor = 0.7;

        internal static double GetRange(int players, Act action, Act vsAction)
        {
            double range = 0.0;

            switch (vsAction)
            {
                case Act.None:
                    range = (action == Act.Call) ? 0.32 : 0.22;
                    break;
                case Act.Call:
                    range = (action == Act.Call) ? 0.23 : 0.2;
                    break;
                case Act.Raise50:
                    range = (action == Act.Call) ? 0.09 : 0.13;
                    break;
            }

            return Math.Pow(TightnessFactor, players) * range;
        }
    }
}