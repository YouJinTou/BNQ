using BNQ.Models;

namespace BNQ.Brain
{
    internal interface IEvaluator
    {
        Hand Evaluate(ulong board, ulong holding);
    }
}
