#pragma once

#include <random>

#include "Card.h"

using FlopCards = unsigned long long;
using BoardCards = unsigned long long;

struct Board
{
	Board();
	Board(FlopCards flop);
	Board(FlopCards flop, Card::Card turn);
	Board(FlopCards flop, Card::Card turn, Card::Card river);
	BoardCards BoardCards() const;
	FlopCards Flop() const;
	Card::Card Turn() const;
	Card::Card River() const;
	void SetFlop(FlopCards flop);
	void SetTurn(Card::Card turn);
	void SetRiver(Card::Card river);
	Card::Card NextRandomCard() const;
	bool AddNextCard(Card::Card card);
public:
	friend std::ostream& operator<<(std::ostream& os, const Board& board);
private:
	FlopCards flop;
	Card::Card turn;
	Card::Card river;
};