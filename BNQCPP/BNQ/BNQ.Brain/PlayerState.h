#pragma once

#include "State.h"
#include "PlayerStrategy.h"

class PlayerState : public State
{
public:
	PlayerState(
		PlayerStrategy* strategy,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	PlayerState(
		std::shared_ptr<State> prevState,
		PlayerStrategy* strategy,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	StateType NextState();
	StateType Type() const;
	Action GetAction();
private:
	PlayerStrategy* strategy;
};