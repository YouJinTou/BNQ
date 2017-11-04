#pragma once

#include "HandEvaluator\Hand.h"

class State;

class PlayerStrategy
{
public:
	virtual void UpdateRange(const State& state) = 0;
	virtual omp::Hand GetShowdownHand() const = 0;
protected:
};