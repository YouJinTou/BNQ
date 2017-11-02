#pragma once

#include "Card.h"
#include "State.h"

class ChanceState : public State
{
public:
	ChanceState(
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
};
