#pragma once

#include "PlayerStrategy.h"

class VillainStrategy : public PlayerStrategy
{
public:
	void UpdateRange(const State& state);
	omp::Hand GetShowdownHand() const;
};