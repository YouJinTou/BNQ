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

StateType::StateType FinalState::NextState()
{
	return StateType::None;
}

StateType::StateType FinalState::Type() const
{
	return StateType::Final;
}