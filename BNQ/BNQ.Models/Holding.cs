namespace BNQ.Models
{
    public struct Holding : IHolding
    {
        private HandStrength handStrength;
        private int rank;

        public Holding(HandStrength handStrength, int rank)
        {
            this.handStrength = handStrength;
            this.rank = rank;
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
