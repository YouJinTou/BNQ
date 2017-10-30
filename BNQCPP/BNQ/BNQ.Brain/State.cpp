#include <assert.h>

#include "State.h"

State::State(
	std::vector<Player>& players, 
	Board& board,
	double pot,
	int seatToAct,
	Street street,
	double facingBet) :
	players(players),
	board(board),
	pot(pot),
	seatToAct(seatToAct),
	street(street),
	facingBet(facingBet)
{
}

State::State(
	std::vector<Player>& players, 
	Board& board,
	double pot,
	int seatToAct,
	Street street, 
	double facingBet,
	std::shared_ptr<State> prevState) :
	players(players),
	board(board),
	pot(pot),
	seatToAct(seatToAct),
	street(street),
	facingBet(facingBet),
	prevState(prevState)
{
}

std::vector<Player>& State::Players()
{
	return players;
}

double State::Pot() const
{
	return pot;
}

int State::SeatToAct() const
{
	return seatToAct;
}

Street State::CurrentStreet() const
{
	return street;
}

double State::FacingBet() const
{
	return facingBet;
}

bool State::IsFinal() const
{
	return false;
}

double State::Value() const
{
	assert(street == Street::River);

	return 0.0;
}

State& State::operator=(const State& rhs)
{
	pot = rhs.pot;

	return *this;
}
