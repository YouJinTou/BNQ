#include "Board.h"

Board::Board() :
	flop(Card::Card::None),
	turn(Card::Card::None),
	river(Card::Card::None)
{
}

Board::Board(FlopCards flop) :
	flop(flop),
	turn(Card::None),
	river(Card::None)
{
}

Board::Board(FlopCards flop, Card::Card turn) :
	flop(flop),
	turn(turn),
	river(Card::None)
{
}

Board::Board(FlopCards flop, Card::Card turn, Card::Card river) :
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


Card::Card Board::Turn() const
{
	return turn;
}

Card::Card Board::River() const
{
	return river;
}

void Board::SetFlop(FlopCards flop)
{
	this->flop = flop;
}

void Board::SetTurn(Card::Card turn)
{
	this->turn = turn;
}

void Board::SetRiver(Card::Card river)
{
	this->river = river;
}

Card::Card Board::NextRandomCard() const
{
	int minCardPow = 4; // Card::c2;
	int maxCardPow = 55; // Card::sA;
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<int> dis(minCardPow, maxCardPow + 1);
	Card::Card card = Card::None;

	do
	{
		int cardPower = dis(gen);
		card = (Card::Card)(1 << cardPower);
	} while ((card & BoardCards()) != 0);

	return card;
}

bool Board::AddNextCard(Card::Card card)
{
	if ((card & BoardCards()) != 0)
	{
		return false;
	}

	if (turn == Card::None)
	{
		SetTurn(card);
	}
	else if (river == Card::None)
	{
		SetRiver(card);
	}

	return true;
}
