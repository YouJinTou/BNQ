#pragma once

#include <string>

#include "PlayerStrategy.h"

class EquiprobableStrategy : public PlayerStrategy
{
public:
	double GetShowdownValue(State* statePtr) const override;	
};