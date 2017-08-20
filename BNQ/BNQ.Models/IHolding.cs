namespace BNQ.Models
{
    public interface IHolding
    {
        ulong Hand { get; }

        HandStrength HandStrength { get; }

        int Rank { get; }
    }
}
