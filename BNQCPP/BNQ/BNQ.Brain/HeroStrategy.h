#pragma once

#include "PlayerStrategy.h"

class State;

class HeroStrategy : public PlayerStrategy
{
public:
	double GetShowdownValue(State* statePtr) const;
};