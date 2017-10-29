#pragma once

#include "Card.h"

class Player
{
public:
	Player(int seat, int stack);
	Player(int seat, int stack, Card hand);
private:
	int seat;
	int stack;
	Card hand;
};