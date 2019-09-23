using BNQ.Core.Models;

namespace BNQ.Core.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsFirstToActAfterFlop(this Player player)
        {
            return player.Position == Position.BB;
        }

        public static bool IsLastToAct(this Player player, Action facingAction)
        {
            switch (facingAction)
            {
                case Action.Check:
                    return player.Position == Position.SB;
                case Action.Bet50:
                    return false;
                case Action.Call:
                    return true;
                case Action.Raise50:
                    return false;
                case Action.Fold:
                    return true;
                default:
                    throw new System.InvalidOperationException();
            }
        }

        public static bool IsPassive(this Action action)
        {
            return action == Action.Call || action == Action.Check;
        }
    }
}
