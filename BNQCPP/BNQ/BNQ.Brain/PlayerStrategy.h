#pragma once

#include "HandEvaluator\HandEvaluator.h"

class State;

class PlayerStrategy
{
public:
	virtual void UpdateRange(State& state) = 0;
	virtual double GetShowdownValue() const = 0;
protected:
	static const omp::HandEvaluator evaluator;
protected:
	State* lastState = nullptr;
};