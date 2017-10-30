#pragma once

#include "PlayerStrategy.h"

class VillainStrategy : public PlayerStrategy
{
public:
	Action ExecuteChoice(State& state);
	virtual void AdjustStack(Action action, Player& player, double pot, double wagerToCall) = 0;
};