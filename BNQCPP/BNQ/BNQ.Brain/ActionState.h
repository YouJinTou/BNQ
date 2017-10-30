#pragma once

#include "State.h"
#include "PlayerStrategy.h"

class ActionState : public State
{
public:
	ActionState() = default;
	ActionState(std::shared_ptr<State> prevState, PlayerStrategy* strategy);
	std::shared_ptr<State> NextState();
	StateType Type() const;
	Action GetAction();
private:
	PlayerStrategy* strategy;
};