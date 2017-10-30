#pragma once

#include "Action.h"
#include "Card.h"
#include "Position.h"

typedef unsigned long long Hand;

class Player
{
public:
	Player() = default;
	Player(Position seat, double stack);
	Player(Position seat, double stack, bool isHero, Hand hand);
	Position Seat() const;
	double Stack() const;
	bool IsHero() const;
	Action LastAction() const;
	void SetLastAction(Action action);
	void SetStack(double wager);
public:
	bool operator<(const Player& other);
private:
	Position seat;
	double stack;
	bool isHero;
	Hand hand;
	Action lastAction;
};