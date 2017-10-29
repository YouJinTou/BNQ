#include "ChoiceState.h"

ChoiceState::ChoiceState(const State& prevState, const std::vector<Action>& actions) :
	State(prevState),
	actions(actions)
{
}

std::vector<Action>& ChoiceState::Actions()
{
	return actions;
}

std::shared_ptr<State> ChoiceState::NextState()
{
	return std::make_shared<ActionState>();
}

StateType ChoiceState::Type() const
{
	return StateType::Choice;
}
