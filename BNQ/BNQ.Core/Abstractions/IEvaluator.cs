using BNQ.Core.Search;

namespace BNQ.Core.Abstractions
{
    public interface IEvaluator
    {
        double Evaluate(GameState state);
    }
}
