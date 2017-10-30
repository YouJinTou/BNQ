#include "Board.h"

Board::Board(FlopCards flop, Card turn, Card river) :
	flop(flop),
	turn(turn),
	river(river)
{
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