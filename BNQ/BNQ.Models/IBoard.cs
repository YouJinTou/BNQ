namespace BNQ.Models
{
    public interface IBoard
    {
        ulong Cards { get; }

        ulong Flop { get; }

        ulong Turn { get; }

        ulong River { get; }

        void AddTurn(Card turn);

        void AddRiver(Card river);
    }
}
