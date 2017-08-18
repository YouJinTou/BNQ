using System;
using BNQ.Models;

namespace BNQ.Brain
{
    public class Evaluator : IEvaluator
    {
        private const int MasksLength = 13;

        private readonly ulong[] NibbleMasks = new ulong[]
        {
            0xF,
            0xF0,
            0xF00,
            0xF000,
            0xF0000,
            0xF00000,
            0xF000000,
            0xF0000000,
            0xF00000000,
            0xF000000000,
            0xF0000000000,
            0xF00000000000,
            0xF000000000000,
            0xF0000000000000
        };
        private readonly ulong[] QuadsMasks = new ulong[]
        {
            0x4,
            0x40,
            0x400,
            0x4000,
            0x40000,
            0x400000,
            0x4000000,
            0x40000000,
            0x400000000,
            0x4000000000,
            0x40000000000,
            0x400000000000,
            0x4000000000000,
            0x40000000000000
        };
        private readonly ulong[] FlushMasks = new ulong[]
        {
            0x1,
            0x2,
            0x4,
            0x8
        };
        private readonly ulong[] TripsMasks = new ulong[]
        {
            0x3,
            0x30,
            0x300,
            0x3000,
            0x30000,
            0x300000,
            0x3000000,
            0x30000000,
            0x300000000,
            0x3000000000,
            0x30000000000,
            0x300000000000,
            0x3000000000000,
            0x30000000000000
        };
        private readonly ulong[] PairMasks = new ulong[]
        {
            0x2,
            0x20,
            0x200,
            0x2000,
            0x20000,
            0x200000,
            0x2000000,
            0x20000000,
            0x200000000,
            0x2000000000,
            0x20000000000,
            0x200000000000,
            0x2000000000000,
            0x20000000000000
        };

        public int Evaluate(ulong[] hands)
        {
            return -1;
        }

        public Hand GetHand(ulong board, ulong holding)
        {
            ulong hand = this.ParseHand(board, holding);
            ulong straightFlush = hand & (hand >> 4) & (hand >> 8) & (hand >> 12) & (hand >> 16);

            if (straightFlush != 0)
            {
                return Hand.StraightFlush;
            }

            ulong popCount = this.GetPopCount(hand);            
            int straightCards = 0;
            int pairCount = 0;
            bool hasTrips = false;
            int clubsCount = 0;
            int diamondsCount = 0;
            int heartsCount = 0;
            int spadesCount = 0;

            for (int i = 0; i < this.NibbleMasks.Length; i++)
            {
                ulong mask = (popCount & this.NibbleMasks[i]);

                if (mask == 0)
                {
                    if (straightCards >= 5)
                    {
                        break;
                    }

                    straightCards = 0;

                    continue;
                }

                bool isLowNibble = (i == 0);

                if (!isLowNibble)
                {
                    ulong nibble = (hand >> i * 4) & this.NibbleMasks[0];
                    clubsCount = ((nibble & this.FlushMasks[0]) != 0) ? clubsCount + 1 : clubsCount;
                    diamondsCount = ((nibble & this.FlushMasks[1]) != 0) ? diamondsCount + 1 : diamondsCount;
                    heartsCount = ((nibble & this.FlushMasks[2]) != 0) ? heartsCount + 1 : heartsCount;
                    spadesCount = ((nibble & this.FlushMasks[3]) != 0) ? spadesCount + 1 : spadesCount;
                }

                straightCards++;

                if (mask == this.QuadsMasks[i])
                {
                    return Hand.FourOfAKind;
                }

                if (mask == this.TripsMasks[i])
                {
                    hasTrips = true;

                    continue;
                }

                if (mask == this.PairMasks[i] && !isLowNibble)
                {
                    pairCount++;
                }
            }

            bool hasFlush = (clubsCount >= 5 || diamondsCount >= 5 || heartsCount >= 5 || spadesCount >= 5);
            bool hasStraight = (straightCards >= 5);

            return this.GetPairBasedHand(pairCount, hasFlush, hasStraight, hasTrips);
        }

        private ulong ParseHand(ulong board, ulong holding)
        {
            ulong hand = board | holding;
            ulong acesMask = hand & this.NibbleMasks[MasksLength];
            ulong lowNibble = (acesMask >> 52);
            hand |= lowNibble;

            return hand;
        }

        private ulong GetPopCount(ulong hand)
        {
            ulong popCount = hand - ((hand >> 1) & 0x5555555555555555);
            popCount = (popCount & 0x3333333333333333) + ((popCount >> 2) & 0x3333333333333333);

            return popCount;
        }

        private Hand GetPairBasedHand(int pairCount, bool hasFlush, bool hasStraight, bool hasTrips)
        {
            switch (pairCount)
            {
                case 0:
                    if (hasFlush)
                    {
                        return Hand.Flush;
                    }

                    if (hasStraight)
                    {
                        return Hand.Straight;
                    }

                    return hasTrips ? Hand.ThreeOfAKind : Hand.HighCard;
                case 1:
                case 2:
                case 3:
                    if (hasTrips)
                    {
                        return Hand.FullHouse;
                    }

                    if (hasFlush)
                    {
                        return Hand.Flush;
                    }

                    if (hasStraight)
                    {
                        return Hand.Straight;
                    }

                    return (pairCount == 1) ? Hand.OnePair : Hand.TwoPair;
            }

            return Hand.HighCard;
        }
    }
}
