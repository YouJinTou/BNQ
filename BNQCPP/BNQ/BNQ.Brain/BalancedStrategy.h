#pragma once

#include "PlayerStrategy.h"

class BalancedStrategy : public PlayerStrategy
{
public:
	void UpdateRange(const State& state);
	omp::Hand GetShowdownHand() const;
};