#pragma once

#include "State.h"

class PlayerState : public State
{
public:
	PlayerState(
		std::shared_ptr<State> prevState,
		StateType::StateType nextStateType,
		std::vector<Player>& players,
		std::shared_ptr<Board> board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall,
		double playerWager);
	StateType::StateType Type() const;
	void SetValue(bool isFinal = false);
private:
	double ShowdownValue();
	bool HeroRemains() const;
};