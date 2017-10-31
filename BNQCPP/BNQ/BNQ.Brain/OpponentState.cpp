#include "ActionState.h"
#include "ChanceState.h"
#include "ChoiceState.h"
#include "HeroStrategy.h"
#include "OpponentState.h"
#include "VillainStrategy.h"

OpponentState::OpponentState(std::vector<Player>& players) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall)
{
}

OpponentState::OpponentState(
	std::vector<Player>& players, std::shared_ptr<State> prevState, PlayerStrategy* strategy) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState)
{
}

std::shared_ptr<State> OpponentState::NextState()
{
	if (IsClosingAction(ToAct()))
	{
		return std::make_shared<ChanceState>(std::make_shared<OpponentState>(*this));
	}

	if (NextToAct().IsHero())
	{
		return std::make_shared<ActionState>(
			std::make_shared<OpponentState>(*this), &HeroStrategy());
	}

	return std::make_shared<OpponentState>(
		players, std::make_shared<OpponentState>(*this), &VillainStrategy());
}

StateType OpponentState::Type() const
{
	return StateType::Opponent;
}
