namespace BNQ.Models
{
    public struct Hand : IHand
    {
        private Card first;
        private Card second;

        public Hand(Card first, Card second)
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
