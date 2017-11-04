#pragma once

#include "PlayerStrategy.h"

class State;

class HeroStrategy : public PlayerStrategy
{
public:
	void UpdateRange(State& state);
	double GetShowdownValue() const;
};