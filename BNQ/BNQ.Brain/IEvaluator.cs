using BNQ.Models;

namespace BNQ.Brain
{
    internal interface IEvaluator
    {
        Hand GetHand(ulong board, ulong holding);
    }
}
