#pragma once

#include "State.h"

class ChoiceState : public State
{
public:
	ChoiceState(std::shared_ptr<State> prevState, const std::vector<Action>& actions);
	std::vector<Action>& Actions();
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
	std::vector<Action> actions;
};