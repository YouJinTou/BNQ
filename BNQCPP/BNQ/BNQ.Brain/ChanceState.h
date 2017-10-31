#pragma once

#include "Card.h"
#include "State.h"

class ChanceState : public State
{
public:
	ChanceState(
		std::shared_ptr<State> prevState,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	StateType Type() const;
	StateType NextState();
private:
};
