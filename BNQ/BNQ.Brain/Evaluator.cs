using BNQ.Models;
using System;
using System.IO;

namespace BNQ.Brain
{
    public class Evaluator : IEvaluator
    {
        public Hand Evaluate(ulong board, ulong holding)
        {
            ulong hand = board | holding;
            //ulong hand4 = hand >> 4;
            //ulong hand8 = hand >> 8;
            //ulong hand12 = hand >> 12;
            //ulong hand16= hand >> 16;
            ulong straightFlush = hand & (hand >> 4) & (hand >> 8) & (hand >> 12) & (hand >> 16);
            //string handSt = string.Format("00: {0}\n", Convert.ToString((long)hand, 2));
            //string Hand4St = string.Format("04: {0}\n", Convert.ToString((long)hand4, 2));
            //string hand8St = string.Format("08: {0}\n", Convert.ToString((long)hand8, 2));
            //string hand12St = string.Format("12: {0}\n", Convert.ToString((long)hand12, 2));
            //string hand16St = string.Format("16: {0}\n", Convert.ToString((long)hand16, 2));

            //File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "royal.txt"),
            //    handSt + Hand4St + hand8St + hand12St + hand16St);

            ulong hand1 = hand >> 1;
            ulong hand2 = hand >> 2;
            ulong hand3 = hand >> 3;
            ulong quads = hand & (hand >> 4) & (hand >> 8) & (hand >> 12) & (hand >> 16);
            string handSt = string.Format("00: {0}\n", Convert.ToString((long)hand, 2));
            string hand1St = string.Format("01: {0}\n", Convert.ToString((long)hand1, 2));
            string hand2St = string.Format("02: {0}\n", Convert.ToString((long)hand2, 2));
            string hand3St = string.Format("03: {0}\n", Convert.ToString((long)hand3, 2));

            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "royal.txt"),
                handSt + hand1St + hand2St + hand3St);

            if (straightFlush != 0)
            {
                bool hasAce = (hand & 4222124650659840) != 0;
                bool hasTen = (hand & 64424509440) != 0;
                bool hasDeuce = (hand & 15) != 0;

                return (hasAce && hasTen) ? Hand.RoyalFlush : Hand.StraightFlush;
            }

            ulong fourOfAKind = hand & (hand >> 1) & (hand >> 2) & (hand >> 3) & 0x1111111111111111;

            if (fourOfAKind != 0)
            {
                return Hand.FourOfAKind;
            }

            ulong threeOfAKind = hand & (hand >> 1) & (hand >> 2) & 0xFFFFFFFFFFFFFFFF;

            if (threeOfAKind != 0)
            {
                return Hand.ThreeOfAKind;
            }

            ulong twoPair = hand & (hand >> 1) & 0xFFFFFFFFFFFFFFFF;

            if (twoPair != 0)
            {
                return Hand.TwoPair;
            }

            return Hand.HighCard;
        }
    }
}
