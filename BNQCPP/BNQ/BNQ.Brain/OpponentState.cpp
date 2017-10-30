#include "OpponentState.h"

OpponentState::OpponentState(std::vector<Player>& players) :
	State(players, board, pot, seatToAct, street, facingBet)
{
}

OpponentState::OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	State(players, board, pot, seatToAct, street, facingBet, prevState)
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
