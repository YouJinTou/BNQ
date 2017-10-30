#pragma once

#include "Action.h"
#include "Player.h"
#include "State.h"

class PlayerStrategy
{
public:
	virtual Action ExecuteChoice(State& state) = 0;
	virtual void AdjustStack(Action action, Player& player, double pot, double wagerToCall) = 0;
};