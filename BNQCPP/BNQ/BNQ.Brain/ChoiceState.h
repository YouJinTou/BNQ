#pragma once

#include "State.h"

class ChoiceState : public State
{
public:
	ChoiceState(std::shared_ptr<State> prevState,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall, 
		double playerWager,
		const std::vector<Action>& actions);
	std::vector<Action>& Actions();
	StateType::StateType Type() const;
	StateType::StateType NextState();
	void SetValue();
private:
	std::vector<Action> actions;
};