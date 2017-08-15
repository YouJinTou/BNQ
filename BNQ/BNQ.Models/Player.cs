namespace BNQ.Models
{
    public class Player : IPlayer
    {
        private IHolding[] holdings;
        private Action[] actions;

        public Player(IHolding[] holdings, Action[] actions)
        {
            this.holdings = holdings;
            this.actions = actions;
        }

        public IHolding[] Holdings
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
