#pragma once

#include "Card.h"

typedef unsigned long long Hand;

struct Player
{
public:
	Player(int seat, int stack);
	Player(int seat, int stack, bool isHero, Hand hand);
	bool IsHero() const;
private:
	int seat;
	int stack;
	bool isHero;
	Hand hand;
};