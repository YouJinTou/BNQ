#include "Board.h"

Board::Board(FlopCards flop, Card turn, Card river) :
	flop(flop),
	turn(turn),
	river(river)
{
}

BoardCards Board::BoardCards() const
{
	return flop | turn | river;
}

FlopCards Board::Flop() const
{
	return flop;
}


Card Board::Turn() const
{
	return turn;
}

Card Board::River() const
{
	return river;
}

void Board::SetFlop(FlopCards flop)
{
	this->flop = flop;
}

void Board::SetTurn(Card turn)
{
	this->turn = turn;
}

void Board::SetRiver(Card river)
{
	this->river = river;
}

Card Board::NextRandomCard() const
{
	int minCardPow = 4; // Card::c2;
	int maxCardPow = 55; // Card::sA;
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<unsigned long long> dis(minCardPow, maxCardPow + 1);
	Card card = Card::None;

	do
	{
		int cardPower = dis(gen);
		card = (Card)(1 << cardPower);
	} while (card & BoardCards() != 0);

	return card;
}
