namespace BNQ.Models
{
    public class Player : IPlayer
    {
        private IHand[] holdings;
        private Action[] actions;

        public Player(IHand[] holdings, Action[] actions)
        {
            this.holdings = holdings;
            this.actions = actions;
        }

        public IHand[] Hands
        {
            get
            {
                return this.holdings;
            }
        }

        public Action[] Actions
        {
            get
            {
                return this.actions;
            }
        }
    }
}
