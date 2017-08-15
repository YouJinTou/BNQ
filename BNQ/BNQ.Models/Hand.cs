namespace BNQ.Models
{
    public enum Hand
    {
        HighCard = 1,
        OnePair = 2,
        TwoPair = 4,
        ThreeOfAKind = 8,
        Straight = 16,
        Flush = 32,
        FullHouse = 64,
        FourOfAKind = 128,
        StraightFlush = 256,
        RoyalFlush = 512
    }
}
