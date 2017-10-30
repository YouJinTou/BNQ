#include "FinalState.h"

FinalState::FinalState(std::vector<Player>& players) :
	State(players, board, pot, NoLastBettor, NoLastBettor, Street::River, 0.0)
{
}

FinalState::FinalState(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	State(players, board, pot, NoLastBettor, NoLastBettor, Street::River, 0.0, prevState)
{
}

std::shared_ptr<State> FinalState::NextState()
{
	throw std::logic_error("The shodown state cannot have a next state.");
}

StateType FinalState::Type() const
{
	return StateType::Final;
}