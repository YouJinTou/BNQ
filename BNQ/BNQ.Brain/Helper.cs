namespace BNQ.Brain
{
    internal static class Helper
    {
        public static int GetDealtCardsCount(ulong board)
        {
            int count = 0;

            while (board != 0)
            {
                board &= board - 1;

                count++;
            }

            return count;
        }

        public static bool IsRiver(ulong board)
        {
            return (GetDealtCardsCount(board) == 5);
        }
    }
}
