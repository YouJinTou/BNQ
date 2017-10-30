#pragma once

#include "PlayerStrategy.h"

class HeroStrategy : public PlayerStrategy
{
public:
	Action ExecuteChoice(State& state);
	void HeroStrategy::AdjustStack(Action action, Player& player, double pot, double wagerToCall);
};