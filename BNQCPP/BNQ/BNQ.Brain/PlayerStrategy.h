#pragma once

#include "HandEvaluator\Hand.h"
#include "HandEvaluator\HandEvaluator.h"

class State;

class PlayerStrategy
{
public:
	virtual void UpdateRange(const State& state) = 0;
	virtual omp::Hand GetShowdownHand() const = 0;
protected:
	static const omp::HandEvaluator evaluator;
protected:
	const State* lastState = nullptr;
};