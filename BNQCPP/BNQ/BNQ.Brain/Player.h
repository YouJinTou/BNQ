#pragma once

#include "Action.h"
#include "Card.h"
#include "Position.h"
#include "PlayerStrategy.h"

typedef unsigned long long Hand;

class Player
{
public:
	Player() = default;
	Player(Position::Position seat, double stack, PlayerStrategy* strategy);
	Player(Position::Position seat, double stack, bool isHero, Hand hand, PlayerStrategy* strategy);
	Position::Position Seat() const;
	double Stack() const;
	bool IsHero() const;
	Action LastAction() const;
	void SetLastAction(Action action);
	void SetStack(double wager);
	Hand GetHand() const;
public:
	bool operator<(const Player& other);
private:
	Position::Position seat;
	double stack;
	bool isHero;
	Hand hand;
	Action lastAction;
	PlayerStrategy* strategy;
};