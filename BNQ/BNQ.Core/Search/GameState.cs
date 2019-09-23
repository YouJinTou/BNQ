using BNQ.Core.Extensions;
using BNQ.Core.Factories;
using BNQ.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BNQ.Core.Search
{
    public class GameState : State
    {
        public GameState(
            GameState parent,
            Card board,
            Street street,
            Player hero,
            Player villain,
            double pot,
            double wagerToCall,
            Action lastAction)
        {
            this.Parent = parent;
            this.Board = board;
            this.Street = street;
            this.Hero = new Player(hero);
            this.Villain = new Player(villain);
            this.Pot = pot;
            this.WagerToCall = wagerToCall;
            this.LastAction = lastAction;
            this.ToPlay = this.Hero.IsToPlay ? this.Hero : this.Villain;
            this.Children = new List<State>();
        }

        public Card Board { get; set; }

        public Street Street { get; set;  }

        public Player Hero { get; set; }

        public Player Villain { get; set; }

        public Player ToPlay { get; set; }

        public double Pot { get; set; }

        public double WagerToCall { get; set; }

        public Action LastAction { get; set; }

        public Player NotToPlay => this.Hero.IsToPlay ? this.Villain : this.Hero;

        public bool IsShowdown => this.Street == Street.Showdown;

        public bool PlayerFolded => this.Street == Street.Fold;

        public bool AllAllIn => this.ToPlay.IsAllIn && this.NotToPlay.IsAllIn;

        public bool AnyAllIn => this.ToPlay.IsAllIn || this.NotToPlay.IsAllIn;

        public override State Expand()
        {
            if (this.Street.IsFinalStreet())
            {
                return this;
            }

            if (this.AllAllIn)
            {
                this.Children.Add(StateFactory.CreateAllInState(this));

                return this.Children.First();
            }

            if (this.AnyAllIn)
            {
                this.Children.Add(StateFactory.CreateCallState(this));

                this.Children.Add(StateFactory.CreateFoldState(this));

                return this.Children.First();
            }

            if (this.LastAction.IsPassive())
            {
                this.Children.Add(StateFactory.CreateBet50State(this));

                this.Children.Add(StateFactory.CreateCheckState(this));

                return this.Children.First();
            }

            this.Children.Add(StateFactory.CreateCallState(this));

            this.Children.Add(StateFactory.CreateRaise50State(this));

            this.Children.Add(StateFactory.CreateFoldState(this));

            return this.Children.First();
        }

        public override void Simulate()
        {
            var value = Simulator.Simulate(this);
            this.Value += value;
            this.Wins = value > 0 ? this.Wins + 1 : this.Wins;
            this.Visits++;
        }

        public override string ToString()
        {
            return
                $"{this.Board} | " +
                $"{this.Street} | " +
                $"{this.ToPlay.Position} | " +
                $"Last action: {this.LastAction} | " +
                $"Pot: {this.Pot} | " +
                $"To call: {this.WagerToCall} | " +
                $"Visits: {this.Visits} | " +
                $"Expected value: {this.AverageValue}";
        }

        public double GetInvestedAmount()
        {
            var investedAmount = (this.Pot - this.WagerToCall) / 2;

            return investedAmount;
        }

        public static GameState CopyState(GameState state)
        {
            var stateCopy = new GameState(
                (GameState)state.Parent,
                state.Board,
                state.Street,
                state.Hero,
                state.Villain,
                state.Pot,
                state.WagerToCall,
                state.LastAction);

            return stateCopy;
        }

        public static bool IsOver(Street street)
        {
            return street == Street.Fold || street == Street.Showdown;
        }

        public void Print(string indent = "")
        {
            System.Console.WriteLine(
                $"{indent}" +
                $"{this.Street} | " +
                $"{this.NotToPlay.Position} {this.LastAction} | " +
                $"{this.ToPlay.Position} | " +
                $"Pot: {this.Pot} | " +
                $"Visits: {this.Visits} | " +
                $"Value: {this.Value} | " +
                $"Average: {this.AverageValue}");

            foreach (GameState child in this.Children)
            {
                child.Print(indent + "-");
            }
        }
    }
}
