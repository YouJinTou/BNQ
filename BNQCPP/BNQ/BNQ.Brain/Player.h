#pragma once

#include "HandEvaluator\Hand.h"

#include "Action.h"
#include "Card.h"
#include "Holding.h"
#include "Position.h"
#include "PlayerStrategy.h"

class Player
{
public:
	Player() = default;
	Player(Position::Position seat, double stack, PlayerStrategy* strategy);
	Player(Position::Position seat, double stack, bool isHero, Holding holding, PlayerStrategy* strategy);
	Position::Position Seat() const;
	double Stack() const;
	bool IsHero() const;
	Action LastAction() const;
	void SetLastAction(Action action);
	void SetStack(double wager);
	Holding GetHolding() const;
	omp::Hand GetShowdownHand() const;
public:
	bool operator<(const Player& other);
private:
	Position::Position seat;
	double stack;
	bool isHero;
	Holding holding;
	Action lastAction;
	PlayerStrategy* strategy;
};