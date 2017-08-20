namespace BNQ.Models
{
    public struct Holding : IHolding
    {
        private ulong hand;
        private HandStrength handStrength;
        private int rank;

        public Holding(ulong hand, HandStrength handStrength, int rank)
        {
            this.hand = hand;
            this.handStrength = handStrength;
            this.rank = rank;
        }

        public ulong Hand
        {
            get
            {
                return this.hand;
            }
        }

        public HandStrength HandStrength
        {
            get
            {
                return this.handStrength;
            }
        }

        public int Rank
        {
            get
            {
                return this.rank;
            }
        }
    }
}
