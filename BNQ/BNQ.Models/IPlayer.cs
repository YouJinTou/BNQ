namespace BNQ.Models
{
    public interface IPlayer
    {
        IHolding[] Holdings { get; }

        Action[] Actions { get; }
    }
}