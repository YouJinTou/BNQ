#pragma once

#include "State.h"

class FinalState : public State
{
public:
	FinalState(std::vector<Player>& players);
	FinalState(std::vector<Player>& players, std::shared_ptr<State> prevState);
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
};