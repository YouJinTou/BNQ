using BNQ.Models;

namespace BNQ.Brain
{
    internal interface IEvaluator
    {
        IHolding GetHolding(ulong board, ulong hand);
    }
}
