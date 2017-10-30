#pragma once

#include "Card.h"

typedef unsigned long long FlopCards;

struct Board
{
	Board(FlopCards flop, Card turn, Card river);
	FlopCards Flop() const;
	Card Turn() const;
	Card River() const;
	void SetFlop(FlopCards flop);
	void SetTurn(Card turn);
	void SetRiver(Card river);
private:
	FlopCards flop;
	Card turn;
	Card river;
};