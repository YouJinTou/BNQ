#include "ActionState.h"
#include "ChanceState.h"
#include "OpponentState.h"
#include "VillainStrategy.h"

ActionState::ActionState(std::shared_ptr<State> prevState, PlayerStrategy* strategy) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState),
	strategy(strategy)
{
}

std::shared_ptr<State> ActionState::NextState()
{
	if (IsClosingAction(ToAct()))
	{
		return std::make_shared<ChanceState>(std::make_shared<ActionState>(*this));
	}

	return std::make_shared<OpponentState>(
		players, std::make_shared<ActionState>(*this), &VillainStrategy());
}

StateType ActionState::Type() const
{
	return StateType::HeroAction;
}

Action ActionState::GetAction()
{
	return strategy->ExecuteChoice(*this);
}
