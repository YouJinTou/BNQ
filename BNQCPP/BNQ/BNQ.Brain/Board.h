#pragma once

#include <random>

#include "HandEvaluator\Hand.h"

#include "Card.h"

using FlopCards = unsigned long long;
using BoardCards = unsigned long long;

struct Board
{
	Board();
	Board(Card::Card flop1, Card::Card flop2, Card::Card flop3);
	Board(Card::Card flop1, Card::Card flop2, Card::Card flop3, Card::Card turn);
	Board(Card::Card flop1, Card::Card flop2, Card::Card flop3, Card::Card turn, Card::Card river);
	BoardCards BoardCards() const;
	FlopCards Flop() const;
	Card::Card Turn() const;
	Card::Card River() const;
	void SetFlop(Card::Card flop1, Card::Card flop2, Card::Card flop3);
	void SetTurn(Card::Card turn);
	void SetRiver(Card::Card river);
	Card::Card NextRandomCard() const;
	bool AddNextCard(Card::Card card);
	omp::Hand GetBoardAsHand() const;
public:
	friend std::ostream& operator<<(std::ostream& os, const Board& board);
private:
	Card::Card flop1;
	Card::Card flop2;
	Card::Card flop3;
	Card::Card turn;
	Card::Card river;
};