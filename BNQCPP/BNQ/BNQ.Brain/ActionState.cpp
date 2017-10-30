#include "ActionState.h"
#include "OpponentState.h"
#include "PlayerStrategy.h"

ActionState::ActionState(std::shared_ptr<State> prevState, PlayerStrategy* strategy) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState),
	strategy(strategy)
{
}

std::shared_ptr<State> ActionState::NextState()
{
	return std::shared_ptr<OpponentState>();
}

StateType ActionState::Type() const
{
	return StateType::SingleAction;
}

Action ActionState::GetAction()
{
	return strategy->ExecuteChoice(*this);
}
