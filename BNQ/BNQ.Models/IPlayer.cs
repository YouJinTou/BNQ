namespace BNQ.Models
{
    public interface IPlayer
    {
        IHand[] Hands { get; }

        Action[] Actions { get; }
    }
}