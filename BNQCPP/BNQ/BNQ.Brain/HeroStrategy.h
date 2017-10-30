#pragma once

#include "PlayerStrategy.h"

class HeroStrategy : PlayerStrategy
{
public:
	Action ExecuteChoice(State& state) const;
};