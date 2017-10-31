#include "FinalState.h"

FinalState::FinalState(std::shared_ptr<State> prevState,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall) :
	State(players, board, pot, NoLastBettor, NoLastBettor, Street::River, 0.0, prevState)
{
}

StateType FinalState::NextState()
{
	return StateType::None;
}

//std::shared_ptr<State> FinalState::NextState()
//{
//	throw std::logic_error("The shodown state cannot have a next state.");
//}

StateType FinalState::Type() const
{
	return StateType::Final;
}