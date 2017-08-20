using BNQ.IO;
using BNQ.Models;

namespace BNQ.Brain
{
    public class RangeGenerator
    {
        private IValuesLoader valuesLoader;
        private double range;
        private int players;

        public RangeGenerator(IValuesLoader valuesLoader)
        {
            this.valuesLoader = valuesLoader;
            this.range = 100.00;
            this.players = 0;
        }

        public double Range
        {
            get
            {
                return this.range;
            }
            private set
            {
                this.range = value;
            }
        }

        public ulong[] GenerateHands()
        {
            return this.valuesLoader.GetRange(this.range);
        }

        public void UpdateRange(Action action, Action vsAction, Position position)
        {
            switch (position)
            {
                case Position.UTG:
                    this.Range = UTG.GetRange(action);
                    break;
                case Position.UTG1:
                    this.Range = UTG1.GetRange(players, action, vsAction);
                    break;
                case Position.MP:
                    this.Range = MP.GetRange(players, action, vsAction);
                    break;
                case Position.MP1:
                    this.Range = MP1.GetRange(players, action, vsAction);
                    break;
                case Position.HJ:
                    this.Range = HJ.GetRange(players, action, vsAction);
                    break;
                case Position.CO:
                    this.Range = CO.GetRange(players, action, vsAction);
                    break;
                case Position.BTN:
                    this.Range = BTN.GetRange(players, action, vsAction);
                    break;
                case Position.SB:
                    this.Range = SB.GetRange(players, action, vsAction);
                    break;
                case Position.BB:
                    this.Range = BB.GetRange(players, action, vsAction);
                    break;
            }

            players++;
        }
    }
}
