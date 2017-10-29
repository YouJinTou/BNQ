#include <assert.h>

#include "State.h"

State::State(
	std::vector<Player>& players, 
	int pot,
	int seatToAct,
	Street street) :
	players(players),
	pot(pot),
	seatToAct(seatToAct),
	street(street)
{
}

State::State(std::vector<Player>& players, int pot,
	int seatToAct,
	Street street, 
	std::shared_ptr<State> prevState) :
	players(players),
	pot(pot),
	seatToAct(seatToAct),
	street(street),
	prevState(prevState)
{
}

std::vector<Player>& State::Players()
{
	return players;
}

int State::Pot() const
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

bool State::IsShowdown() const
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
