namespace BNQ.Models
{
    public struct Holding : IHolding
    {
        private Card first;
        private Card second;

        public Holding(Card first, Card second)
        {
            this.first = first;
            this.second = second;
        }

        public Card First
        {
            get
            {
                return this.first;
            }
        }

        public Card Second
        {
            get
            {
                return this.second;
            }
        }
    }
}
