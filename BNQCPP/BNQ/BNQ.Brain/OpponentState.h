#pragma once

#include "State.h"

class State;

class OpponentState : public State
{
public:
	OpponentState(
		PlayerStrategy* strategy,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	OpponentState(
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
private:
	PlayerStrategy* strategy;
};