#pragma once

#include "Card.h"

struct Player
{
public:
	Player(int seat, int stack);
	Player(int seat, int stack, bool isHero, Card hand);
	bool IsHero() const;
private:
	int seat;
	int stack;
	bool isHero;
	Card hand;
};