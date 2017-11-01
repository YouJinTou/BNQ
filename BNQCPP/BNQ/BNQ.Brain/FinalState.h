#pragma once

#include "State.h"

class FinalState : public State
{
public:
	FinalState(
		std::shared_ptr<State> prevState,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	StateType::StateType NextState();
	StateType::StateType Type() const;
private:
};