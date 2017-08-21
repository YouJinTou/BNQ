namespace BNQ.Models
{
    public struct StateActionPair
    {
        private State state;
        private Action action;

        public StateActionPair(State state, Action action)
        {
            this.state = state;
            this.action = action;
        }

        public State State
        {
            get
            {
                return this.state;
            }
        }

        public Action Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }
    }
}
