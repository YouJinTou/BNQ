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
		double wagerToCall,
		double playerWager);
	PlayerState(
		std::shared_ptr<State> prevState,
		PlayerStrategy* strategy,
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall,
		double playerWager);
	StateType::StateType NextState();
	StateType::StateType Type() const;
	void SetValue();
	Action GetAction();
private:
	PlayerStrategy* strategy;
private:
	double ShowdownValue() const;
	bool HeroRemains() const;
};