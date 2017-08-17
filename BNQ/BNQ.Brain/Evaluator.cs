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

        public Hand Evaluate(ulong board, ulong holding)
        {
            ulong hand = board | holding;
            ulong acesMask = hand & this.NibbleMasks[MasksLength];
            ulong lowHand = hand | (acesMask >> 52);
            ulong straightFlush = lowHand & (lowHand >> 4) & (lowHand >> 8) & (lowHand >> 12) & (lowHand >> 16);

            if (straightFlush != 0)
            {
                return Hand.StraightFlush;
            }

            ulong popCount = hand - ((hand >> 1) & 0x5555555555555555);
            popCount = (popCount & 0x3333333333333333) + ((popCount >> 2) & 0x3333333333333333);
            bool hasTrips = false;
            int pairCount = 0;

            for (int i = 0; i < this.NibbleMasks.Length; i++)
            {
                if ((popCount & (this.NibbleMasks[i])) == this.QuadsMasks[i])
                {
                    return Hand.FourOfAKind;
                }

                if ((popCount & (this.NibbleMasks[i])) == this.TripsMasks[i])
                {
                    hasTrips = true;

                    continue;
                }

                if ((popCount & (this.NibbleMasks[i])) == this.PairMasks[i])
                {
                    pairCount++;
                }
            }

            ulong straightMask = 131072 - 1;

            switch (pairCount)
            {
                case 0:
                    return hasTrips ? Hand.ThreeOfAKind : Hand.HighCard;
                case 1:
                    return hasTrips ? Hand.FullHouse : Hand.OnePair;
                case 2:
                    return Hand.TwoPair;
                case 3:
                    return Hand.TwoPair;
            }

            return Hand.HighCard;
        }
    }
}
