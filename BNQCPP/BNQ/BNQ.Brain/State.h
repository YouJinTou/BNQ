#pragma once

#include <memory>
#include <vector>

#include "Action.h"
#include "Player.h"
#include "StateType.h"

class State
{
public:
	State() = default;
	State(std::vector<Player>& players);
	State(std::vector<Player>& players, std::shared_ptr<State> prevState);
	virtual StateType Type() const = 0;
	virtual std::shared_ptr<State> NextState() = 0;
	std::vector<Player>& Players();
	State& operator=(const State& rhs);
protected:
	std::shared_ptr<State> prevState;
	int pot;
	std::vector<Player> players;
};