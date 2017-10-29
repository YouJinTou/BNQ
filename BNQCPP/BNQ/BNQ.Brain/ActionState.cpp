#include "ActionState.h"
#include "OpponentState.h"

ActionState::ActionState(std::shared_ptr<State> prevState, Action action) :
	State(players, pot, seatToAct, street, prevState),
	action(action)
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
