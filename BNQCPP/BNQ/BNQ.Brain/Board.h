#pragma once

#include <random>

#include "Card.h"

using FlopCards = unsigned long long;
using BoardCards = unsigned long long;

struct Board
{
	Board(FlopCards flop, Card turn, Card river);
	BoardCards BoardCards() const;
	FlopCards Flop() const;
	Card Turn() const;
	Card River() const;
	void SetFlop(FlopCards flop);
	void SetTurn(Card turn);
	void SetRiver(Card river);
	Card NextRandomCard() const;
private:
	FlopCards flop;
	Card turn;
	Card river;
};