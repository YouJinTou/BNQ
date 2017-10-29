#include "State.h"

State::State(std::vector<Player>& players) :
	players(players)
{
}

State::State(std::vector<Player>& players, std::shared_ptr<State> prevState) :
	players(players),
	prevState(prevState)
{
}

std::vector<Player>& State::Players()
{
	return players;
}

State& State::operator=(const State& rhs)
{
	pot = rhs.pot;

	return *this;
}
