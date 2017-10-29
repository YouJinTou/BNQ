#pragma once

#include <vector>

#include "Action.h"
#include "ActionState.h"
#include "ChoiceState.h"
#include "State.h"
#include "StateType.h"

class OpponentState : public State
{
public:
	OpponentState(std::vector<Player>& players);
	OpponentState(std::vector<Player>& players, std::shared_ptr<State> prevState);
	std::vector<Action> Actions();
	std::shared_ptr<State> NextState();
	StateType Type() const;
private:
};