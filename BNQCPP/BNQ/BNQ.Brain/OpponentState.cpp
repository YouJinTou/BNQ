#include "OpponentState.h"

OpponentState::OpponentState(std::vector<Player>& players) :
	State(players, pot, seatToAct, street)
{
}

OpponentState::OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	State(players, pot, seatToAct, street, prevState)
{
}

std::shared_ptr<State> OpponentState::NextState()
{
	return std::shared_ptr<ChoiceState>();
}

StateType OpponentState::Type() const
{
	return StateType::Opponent;
}
