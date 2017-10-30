#pragma once

#include "State.h"

class State;

class OpponentState : public State
{
public:
	OpponentState(std::vector<Player>& players);
	OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState);
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
};