#pragma once

#include <memory>
#include <vector>

#include "PlayerStrategy.h"
#include "State.h"

class StateFactory
{
public:
	static std::vector<std::shared_ptr<State> > CreateStates(std::shared_ptr<State> statePtr);
private:
	static std::vector<std::shared_ptr<State> > CreatePlayerStates(std::shared_ptr<State> statePtr);
	static std::vector<std::shared_ptr<State> > CreateChanceStates(std::shared_ptr<State> statePtr);
private:
	static std::shared_ptr<State> CreateBet50State(std::shared_ptr<State> statePtr);
	static std::shared_ptr<State> CreateCallState(std::shared_ptr<State> statePtr);
	static std::shared_ptr<State> CreateCheckState(std::shared_ptr<State> statePtr);
	static std::shared_ptr<State> CreateFoldState(std::shared_ptr<State> statePtr);
	static std::shared_ptr<State> CreateRaise50State(std::shared_ptr<State> statePtr);
private:
	static std::shared_ptr<PlayerStrategy> Strategy(bool isHero);
};