#pragma once

#include "HandEvaluator\Hand.h"

#include "State.h"
#include "PlayerStrategy.h"

class PlayerState : public State
{
public:
	PlayerState(
		std::shared_ptr<State> prevState,
		StateType::StateType nextStateType,
		PlayerStrategy* strategy,
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
	Action GetAction();
private:
	PlayerStrategy* strategy;
private:
	static omp::Hand GetPlayerHand(const Player& player);
private:
	double ShowdownValue() const;
	bool HeroRemains() const;
};