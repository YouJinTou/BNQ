#pragma once

#include <memory>
#include <vector>

#include "Action.h"
#include "Player.h"
#include "StateType.h"
#include "Street.h"

class State
{
public:
	State() = default;
	State(
		std::vector<Player>& players,
		int pot,
		int seatToAct,
		Street street);
	State(std::vector<Player>& players,
		int pot,
		int seatToAct,
		Street street,
		std::shared_ptr<State> prevState);
	virtual StateType Type() const = 0;
	virtual std::shared_ptr<State> NextState() = 0;
	std::vector<Player>& Players();
	int Pot() const;
	int SeatToAct() const;
	Street CurrentStreet() const;
	bool IsShowdown() const;
	double Value() const;
public:
	State& operator=(const State& rhs);
protected:
	std::shared_ptr<State> prevState;
	std::vector<Player> players;
	int pot;
	int seatToAct;
	Street street;
};