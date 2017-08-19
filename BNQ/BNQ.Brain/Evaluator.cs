using BNQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BNQ.Brain
{
    public class Evaluator : IEvaluator
    {
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

        public int Evaluate(ulong board, ulong[] hands)
        {
            int bestIndex = -1;
            int bestRank = 0;
            HandStrength bestHand = HandStrength.None;

            for (int i = 0; i < hands.Length; i++)
            {
                IHolding holding = this.GetHolding(board, hands[i]);

                if (holding.HandStrength > bestHand)
                {
                    bestIndex = i;
                    bestRank = holding.Rank;
                    bestHand = holding.HandStrength;
                }
                else if (holding.HandStrength == bestHand)
                {
                    bestIndex = (holding.Rank > bestRank) ? i : 
                        (holding.Rank == bestRank) ? -1 : bestIndex;
                }
            }

            return bestIndex;
        }

        public IHolding GetHolding(ulong board, ulong hand)
        {
            ulong originalHand = hand;
            hand = this.AccountLowAces(board, hand);
            ulong popCount = this.GetPopCount(hand);
            int cardRank = -1;
            int lowerKicker = -1;
            int higherKicker = -1;
            int flushRank = -1;
            int straightRank = -1;
            int wheelRank = -1;
            int tripsRank = -1;
            int[] pairRanks = new int[] { -1, -1, -1 };
            IList<int> allRanks = new List<int>(7);
            int straightCards = 0;
            int clubsCount = 0;
            int diamondsCount = 0;
            int heartsCount = 0;
            int spadesCount = 0;
            int pairCount = 0;
            bool hasTrips = false;
            bool isWheel = false;
            bool isStraight = false;

            ulong straightFlush = hand & (hand >> 4) & (hand >> 8) & (hand >> 12) & (hand >> 16);

            if (straightFlush != 0)
            {
                return new Holding(HandStrength.StraightFlush, this.GetStraightFlushRank(straightFlush));
            }

            for (int rank = 0; rank < this.NibbleMasks.Length; rank++)
            {
                ulong mask = (popCount & this.NibbleMasks[rank]);

                if (mask == 0)
                {
                    if (straightCards >= 5)
                    {
                        isStraight = true;
                        straightRank = isStraight ? rank : straightRank;
                        isWheel = isWheel ? true : (isStraight && rank == 4);
                        wheelRank = (isWheel && isStraight) ? rank : wheelRank;
                    }
                   
                    straightCards = 0;

                    continue;
                }

                if (mask == this.QuadsMasks[rank])
                {
                    return new Holding(HandStrength.FourOfAKind, rank + cardRank);
                }

                if (straightCards >= 5)
                {
                    isStraight = true;
                    straightRank = isStraight ? rank : straightRank;
                    isWheel = isWheel ? true : (isStraight && rank == 4);
                    wheelRank = (isWheel && isStraight) ? rank : wheelRank;
                }

                straightCards++;
                cardRank = rank;
                higherKicker = (lowerKicker != -1) ? ((originalHand & this.NibbleMasks[rank]) != 0) ? rank : -1 : higherKicker;
                lowerKicker = (lowerKicker == -1) ? ((originalHand & this.NibbleMasks[rank]) != 0) ? rank : -1 : lowerKicker;
                bool isLowNibble = (rank == 0);                

                if (!isLowNibble)
                {
                    ulong nibble = (hand >> rank * 4) & this.NibbleMasks[0];
                    int club = ((nibble & this.FlushMasks[0]) != 0) ? 1 : 0;
                    int diamond = ((nibble & this.FlushMasks[1]) != 0) ? 1 : 0;
                    int heart = ((nibble & this.FlushMasks[2]) != 0) ? 1 : 0;
                    int spade = ((nibble & this.FlushMasks[3]) != 0) ? 1 : 0;
                    bool clubsChanged = (clubsCount + club != clubsCount && clubsCount + club >= 5);
                    bool diamondsChanged = (diamondsCount + diamond != diamondsCount && diamondsCount + diamond >= 5);
                    bool heartsChanged = (heartsCount + heart != heartsCount && heartsCount + heart >= 5);
                    bool spadesChanged = (spadesCount + spade != spadesCount && spadesCount + spade >= 5);

                    if (clubsChanged || diamondsChanged || heartsChanged || spadesChanged)
                    {
                        flushRank = rank;
                    }

                    clubsCount += club;
                    diamondsCount += diamond;
                    heartsCount += heart;
                    spadesCount += spade;

                    allRanks.Add(rank);
                }

                if (mask == this.TripsMasks[rank])
                {
                    hasTrips = true;
                    tripsRank = rank;

                    continue;
                }

                if (mask == this.PairMasks[rank] && !isLowNibble)
                {
                    pairRanks[pairCount] = rank;
                    pairCount++;
                }
            }
            Card HANDVE = (Card)(originalHand | board);
            straightRank = isWheel ? wheelRank : straightRank;
            int[] orderedPairRanks = pairRanks.OrderByDescending(p => p).ToArray();
            int[] bestRanks = allRanks.OrderByDescending(r => r).Take(5).ToArray();

            return this.GetHandStrength(
                hand, 
                popCount,
                bestRanks,
                cardRank,
                lowerKicker,
                higherKicker,
                flushRank, 
                straightRank, 
                tripsRank,
                orderedPairRanks[0],
                orderedPairRanks[1],
                pairCount, 
                hasTrips);
        }

        private ulong AccountLowAces(ulong board, ulong hand)
        {
            hand |= board;
            int masksLength = 13;
            ulong acesMask = hand & this.NibbleMasks[masksLength];
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

        public int GetStraightFlushRank(ulong hand)
        {
            ulong n = 1;

            if ((hand >> 32) == 0) { n = n + 32; hand = hand << 32; }
            if ((hand >> 48) == 0) { n = n + 16; hand = hand << 16; }
            if ((hand >> 56) == 0) { n = n + 8; hand = hand << 8; }
            if ((hand >> 60) == 0) { n = n + 4; hand = hand << 4; }
            if ((hand >> 62) == 0) { n = n + 2; hand = hand << 2; }
            n = n - (hand >> 63);

            return (int)(63 - n);
        }

        private IHolding GetHandStrength(
            ulong hand, 
            ulong popCount,
            int[] bestRanks,
            int cardRank,
            int lowerKicker,
            int higherKicker,
            int flushRank,
            int straightRank,
            int tripsRank,
            int pairRank,
            int secondPairRank,
            int pairCount,
            bool hasTrips)
        {
            bool hasFlush = (flushRank != -1);
            bool hasStraight = (straightRank != -1);

            switch (pairCount)
            {
                case 0:
                    if (hasFlush)
                    {
                        return new Holding(HandStrength.Flush, flushRank);
                    }

                    if (hasStraight)
                    {
                        return new Holding(HandStrength.Straight, straightRank);
                    }

                    return hasTrips ? 
                        new Holding(HandStrength.ThreeOfAKind, tripsRank + cardRank) : 
                        new Holding(HandStrength.HighCard, cardRank);
                case 1:
                case 2:
                case 3:
                    if (hasTrips)
                    {
                        return new Holding(HandStrength.FullHouse, tripsRank + pairRank);
                    }

                    if (hasFlush)
                    {
                        return new Holding(HandStrength.Flush, flushRank);
                    }

                    if (hasStraight)
                    {
                        return new Holding(HandStrength.Straight, straightRank);
                    }

                    return (pairCount == 1) ? 
                        new Holding(HandStrength.OnePair, pairRank + 
                            (bestRanks.All(r => r > higherKicker) ? 0 : 
                            (int)(Math.Pow(2, higherKicker) + Math.Pow(2, lowerKicker)))) : 
                        new Holding(HandStrength.TwoPair, pairRank + secondPairRank + cardRank);
            }

            return new Holding(HandStrength.HighCard, cardRank);
        }
    }
}
