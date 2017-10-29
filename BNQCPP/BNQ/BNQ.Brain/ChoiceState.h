#pragma once

#include <vector>

#include "Action.h"
#include "ActionState.h"
#include "State.h"

class ChoiceState : public State
{
public:
	ChoiceState(const State& prevState, const std::vector<Action>& actions);
	std::vector<Action>& Actions();
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
	std::vector<Action> actions;
};