#pragma once

#include "PlayerStrategy.h"

class State;

class HeroStrategy : public PlayerStrategy
{
public:
	void UpdateRange(const State& state);
	omp::Hand GetShowdownHand() const;
};