#include "OpponentState.h"

OpponentState::OpponentState(std::vector<Player>& players) :
	State(players)
{
}

OpponentState::OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	State(players, prevState)
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
