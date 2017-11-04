#pragma once

#include "PlayerStrategy.h"

class BalancedStrategy : public PlayerStrategy
{
public:
	void UpdateRange(State& state);
	double GetShowdownValue() const;
};