#pragma once

#include <memory>
#include <vector>

#include "State.h"
#include "StateType.h"

class StateFactory
{
public:
	static std::vector<std::shared_ptr<State> > CreateStates(std::shared_ptr<State> statePtr);
private:
	static std::shared_ptr<State> CreateState(StateType stateType);
private:
	static std::vector<std::shared_ptr<State> > CreateChanceStates(std::shared_ptr<State> statePtr);
};