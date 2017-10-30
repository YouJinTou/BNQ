#pragma once

#include "PlayerStrategy.h"

class VillainStrategy : PlayerStrategy
{
public:
	Action ExecuteChoice(State& state) const;
};