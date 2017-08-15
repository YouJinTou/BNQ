using System;
using BNQ.Models;

namespace BNQ.Brain
{
    public class Evaluator : IEvaluator
    {
        public Hand Evaluate(ulong board, ulong holding)
        {
            ulong hand = board | holding;
            ulong straightFlush = hand & (hand >> 4) & (hand >> 8) & (hand >> 12) & (hand >> 16);

            if (straightFlush != 0)
            {
                bool hasAce = (hand & 4222124650659840) != 0;
                bool hasDeuce = (hand & 15) != 0;

                return (hasAce && !hasDeuce) ? Hand.RoyalFlush : Hand.StraightFlush;
            }

            return Hand.OnePair;
        }
    }
}
