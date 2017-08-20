using Act = BNQ.Models.Action;

namespace BNQ.Brain
{
    internal class UTG
    {
        internal static double GetRange(Act action)
        {
            return (action == Act.Call) ? 0.25 : 0.15;
        }
    }
}
