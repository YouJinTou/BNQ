#pragma once

#include "State.h"

class ChanceState : public State
{
public:
	ChanceState(std::shared_ptr<State> prevState);
	StateType Type() const;
	std::shared_ptr<State> NextState();
private:
};
