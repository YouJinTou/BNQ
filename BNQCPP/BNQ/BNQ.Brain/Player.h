#pragma once

#include "Action.h"
#include "Card.h"

typedef unsigned long long Hand;

class Player
{
public:
	Player() = default;
	Player(int seat, double stack);
	Player(int seat, double stack, bool isHero, Hand hand);
	int Seat() const;
	double Stack() const;
	bool IsHero() const;
	Action LastAction() const;
	void SetLastAction(Action action);
	void SetStack(double wager);
private:
	int seat;
	double stack;
	bool isHero;
	Hand hand;
	Action lastAction;
};