using BNQ.Models;

namespace BNQ.Brain
{
    public interface IEvaluator
    {
        int Evaluate(ulong board, ulong[] hands);

        IHolding GetHolding(ulong board, ulong hand);
    }
}
