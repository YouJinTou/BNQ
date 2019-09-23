using BNQ.Core.Models;

namespace BNQ.Core.Tools
{
    public static class PotTools
    {
        public static double GetRaiseAmount(Wager wager, double pot, double wagerToCall)
        {
            switch (wager)
            {
                case Wager.Half:
                    var raiseAmount = wagerToCall + (wagerToCall + pot) / 2;

                    return raiseAmount;
                default:
                    throw new System.InvalidOperationException();
            }
        }
    }
}
