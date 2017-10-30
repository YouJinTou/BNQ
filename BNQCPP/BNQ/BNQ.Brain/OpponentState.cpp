#include "ActionState.h"
#include "ChoiceState.h"
#include "OpponentState.h"

OpponentState::OpponentState(std::vector<Player>& players) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall)
{
}

OpponentState::OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState)
{
}

std::shared_ptr<State> OpponentState::NextState()
{
	if (NextToAct().IsHero())
	{
		return std::shared_ptr<State>();
	}

	return std::shared_ptr<ChoiceState>();
}

StateType OpponentState::Type() const
{
	return StateType::Opponent;
}
