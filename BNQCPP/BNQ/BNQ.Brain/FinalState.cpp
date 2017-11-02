#include "FinalState.h"

FinalState::FinalState(
	std::shared_ptr<State> prevState,
	StateType::StateType nextStateType,
	std::vector<Player>& players,
	std::shared_ptr<Board> board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall) :
	State(players, board, pot, NoLastBettor, NoLastBettor, Street::River, 0.0, 0.0, prevState, StateType::None)
{
}

StateType::StateType FinalState::Type() const
{
	return StateType::Final;
}