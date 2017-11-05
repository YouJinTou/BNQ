#pragma once

#include "HandEvaluator\Hand.h"
#include "HandEvaluator\HandEvaluator.h"

#include "Action.h"
#include "Card.h"
#include "Holding.h"
#include "Position.h"
#include "PlayerStrategy.h"

class State;

class Player
{
public:
	Player() = default;
	Player(Position::Position seat, double stack, PlayerStrategy* strategy);
	Player(Position::Position seat, double stack, bool isHero, Holding holding, PlayerStrategy* strategy);
	Position::Position Seat() const;
	double Stack() const;
	bool IsHero() const;
	bool IsPlaying() const;
	Action LastAction() const;
	void SetLastAction(Action action);
	void SetStack(double wager);
	void UpdateRanges(State& state);
	Holding GetHolding() const;
	double GetShowdownValue(omp::Hand board) const;
public:
	bool operator<(const Player& other);
private:
	static const omp::HandEvaluator evaluator;
private:
	Position::Position seat;
	double stack;
	bool isHero;
	Holding holding;
	Action lastAction;
	PlayerStrategy* strategy;
};