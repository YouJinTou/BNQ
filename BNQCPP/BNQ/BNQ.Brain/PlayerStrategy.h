#pragma once

#include "Action.h"
#include "State.h"

class PlayerStrategy
{
public:
	virtual Action ExecuteChoice(State& state) const = 0;
};