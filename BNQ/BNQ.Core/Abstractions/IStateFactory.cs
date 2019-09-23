using BNQ.Core.Models;
using BNQ.Core.Search;

namespace BNQ.Core.Abstractions
{
    public interface IStateFactory
    {
        GameState CreateStartState(
            Card board,
            Street street,
            Player hero,
            Player villain,
            double pot,
            double wagerToCall,
            Action lastAction);

        GameState CreateBet50State(GameState parent);

        GameState CreateCheckState(GameState parent);

        GameState CreateFoldState(GameState parent);

        GameState CreateCallState(GameState parent);

        GameState CreateRaise50State(GameState parent);

        GameState CreateAllInState(GameState parent);
    }
}
