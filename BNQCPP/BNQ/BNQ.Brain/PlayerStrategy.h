#pragma once

#include "HandEvaluator\HandEvaluator.h"

class State;

class PlayerStrategy
{
public:
	virtual double GetShowdownValue(State* statePtr) const = 0;
protected:
	static const omp::HandEvaluator evaluator;
};