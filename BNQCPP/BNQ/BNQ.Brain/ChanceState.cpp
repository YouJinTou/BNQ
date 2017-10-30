#include "ChanceState.h"

ChanceState::ChanceState(std::shared_ptr<State> prevState) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState)
{
}

StateType ChanceState::Type() const
{
	return StateType::Chance;
}

std::shared_ptr<State> ChanceState::NextState()
{
	return std::shared_ptr<State>();
}
