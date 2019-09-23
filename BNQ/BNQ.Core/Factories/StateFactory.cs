using BNQ.Core.Extensions;
using BNQ.Core.Models;
using BNQ.Core.Search;
using BNQ.Core.Tools;

namespace BNQ.Core.Factories
{
    public static class StateFactory
    {
        public static GameState CreateStartState(
            Card board,
            Street street,
            Player hero,
            Player villain,
            double pot,
            double wagerToCall,
            Action lastAction)
        {
            var state = new GameState(
                null, 
                board, 
                street, 
                hero, 
                villain, 
                pot, 
                wagerToCall,
                lastAction);

            return state;
        }

        public static GameState CreateBet50State(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            var toPlay = hero.IsToPlay ? hero : villain;
            var bet = toPlay.Wager(parent.Pot.GetHalfPot());
            var newPot = parent.Pot + bet;
            hero.IsToPlay = !hero.IsToPlay;
            villain.IsToPlay = !villain.IsToPlay;
            var state = new GameState(
                parent,
                parent.Board,
                parent.Street,
                hero,
                villain,
                newPot,
                bet,
                Action.Bet50);

            return state;
        }

        public static GameState CreateCheckState(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            var toPlay = hero.IsToPlay ? hero : villain;
            var street = toPlay.IsLastToAct(Action.Check) ? 
                parent.Street.GetNextStreet() : parent.Street;
            hero.IsToPlay = !hero.IsToPlay;
            villain.IsToPlay = !villain.IsToPlay;
            var state = new GameState(
                parent,
                parent.Board,
                street,
                hero,
                villain,
                parent.Pot,
                0.0d,
                Action.Check);

            return state;
        }

        public static GameState CreateFoldState(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            hero.IsToPlay = !hero.IsToPlay;
            villain.IsToPlay = !villain.IsToPlay;
            var state = new GameState(
                parent,
                parent.Board,
                Street.Fold,
                hero,
                villain,
                parent.Pot,
                0.0d,
                Action.Fold);

            return state;
        }

        public static GameState CreateCallState(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            var toPlay = hero.IsToPlay ? hero : villain;
            var callAmount = toPlay.Wager(parent.WagerToCall);
            var newPot = parent.Pot + callAmount;
            var board = parent.AnyAllIn ? 
                BoardGenerator.FillToRiver(parent.Board, parent.Street, hero.HoleCards) : 
                parent.Board;
            var street = parent.AnyAllIn ? Street.Showdown : parent.Street.GetNextStreet();
            hero.IsToPlay = hero.IsFirstToActAfterFlop() ? true : !hero.IsToPlay;
            villain.IsToPlay = villain.IsFirstToActAfterFlop() ? true : !villain.IsToPlay;
            var state = new GameState(
                parent,
                board,
                street,
                hero,
                villain,
                newPot,
                0.0d,
                Action.Call);

            return state;
        }

        public static GameState CreateRaise50State(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            var toPlay = hero.IsToPlay ? hero : villain;
            var raiseAmount = PotTools.GetRaiseAmount(Wager.Half, parent.Pot, parent.WagerToCall);
            var raise = toPlay.Wager(raiseAmount);
            var wagerToCall = System.Math.Abs(raise - parent.WagerToCall);
            var newPot = parent.Pot + raise;
            hero.IsToPlay = !hero.IsToPlay;
            villain.IsToPlay = !villain.IsToPlay;
            var state = new GameState(
                parent,
                parent.Board,
                parent.Street,
                hero,
                villain,
                newPot,
                wagerToCall,
                Action.Raise50);

            return state;
        }

        public static GameState CreateAllInState(GameState parent)
        {
            var hero = new Player(parent.Hero);
            var villain = new Player(parent.Villain);
            var board = BoardGenerator.FillToRiver(parent.Board, parent.Street, hero.HoleCards);
            var state = new GameState(
                parent,
                board,
                Street.Showdown,
                hero,
                villain,
                parent.Pot,
                0.0d,
                Action.Call);

            return state;
        }
    }
}
