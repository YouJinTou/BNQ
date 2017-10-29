#pragma once

#include "Action.h"
#include "State.h"
#include "StateType.h"
#include "Street.h"

class ActionState : public State
{
public:
	ActionState() = default;
	ActionState(std::shared_ptr<State> prevState, Action action);
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
	Action action;
};