#pragma once

#include "PlayerStrategy.h"

class BalancedStrategy : public PlayerStrategy
{
public:
	double GetShowdownValue(State* statePtr) const;
};