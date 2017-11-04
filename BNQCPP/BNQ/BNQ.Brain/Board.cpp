#include "Constants.h"
#include "Board.h"

Board::Board() :
	flop1(Card::Card::None),
	flop2(Card::Card::None),
	flop3(Card::Card::None),
	turn(Card::Card::None),
	river(Card::Card::None)
{
}

Board::Board(Card::Card flop1, Card::Card flop2, Card::Card flop3) :
	flop1(flop1),
	flop2(flop2),
	flop3(flop3),
	turn(Card::None),
	river(Card::None)
{
}

Board::Board(Card::Card flop1, Card::Card flop2, Card::Card flop3, Card::Card turn) :
	flop1(flop1),
	flop2(flop2),
	flop3(flop3),
	turn(turn),
	river(Card::None)
{
}

Board::Board(Card::Card flop1, Card::Card flop2, Card::Card flop3, Card::Card turn, Card::Card river) :
	flop1(flop1),
	flop2(flop2),
	flop3(flop3),
	turn(turn),
	river(river)
{
}

BoardCards Board::BoardCards() const
{
	return Flop() | turn | river;
}

FlopCards Board::Flop() const
{
	return flop1 | flop2 | flop3;
}


Card::Card Board::Turn() const
{
	return turn;
}

Card::Card Board::River() const
{
	return river;
}

void Board::SetFlop(Card::Card flop1, Card::Card flop2, Card::Card flop3)
{
	this->flop1 = flop1;
	this->flop2 = flop2;
	this->flop3 = flop3;
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
	int minCardPow = 1; // Card::s2;
	int maxCardPow = 51; // Card::dA;
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<int> dis(minCardPow, maxCardPow + 1);
	Card::Card card = Card::None;

	do
	{
		int cardPower = dis(gen);
		card = (Card::Card)(1i64 << cardPower);
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

omp::Hand Board::GetBoardAsHand() const
{
	omp::Hand h = omp::Hand::empty();

	if (flop1 != Card::None)
	{
		h += omp::Hand(Constants::PowerTwoIndices[flop1]);
	}

	if (flop2 != Card::None)
	{
		h += omp::Hand(Constants::PowerTwoIndices[flop2]);
	}

	if (flop3 != Card::None)
	{
		h += omp::Hand(Constants::PowerTwoIndices[flop3]);
	}

	if (turn != Card::None)
	{
		h += omp::Hand(Constants::PowerTwoIndices[turn]);
	}

	if (river != Card::None)
	{
		h += omp::Hand(Constants::PowerTwoIndices[river]);
	}

	return h;
}

std::ostream& operator<<(std::ostream& os, const Board& board)
{
	os << 
		board.flop1 <<
		" " <<
		board.flop2 <<
		" " <<
		board.flop3 <<
		" | " << 
		board.turn << 
		" | " << 
		board.river << 
		std::endl;

	return os;
}
