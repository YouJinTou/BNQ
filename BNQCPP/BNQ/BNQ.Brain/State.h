#pragma once

#include <memory>
#include <vector>

#include "Action.h"
#include "Board.h"
#include "Player.h"
#include "StateType.h"
#include "Street.h"

class State
{
public:
	State() = default;
	State(
		std::vector<Player>& players,
		Board& board,
		double pot,
		int seatToAct,
		Street street,
		double facingBet);
	State(std::vector<Player>& players,
		Board& board,
		double pot,
		int seatToAct,
		Street street,
		double facingBet,
		std::shared_ptr<State> prevState);
	virtual StateType Type() const = 0;
	virtual std::shared_ptr<State> NextState() = 0;
public:
	std::vector<Player>& Players();
	double Pot() const;
	int SeatToAct() const;
	Street CurrentStreet() const;
	double FacingBet() const;
	bool IsFinal() const;
	double Value() const;
public:
	State& operator=(const State& rhs);
protected:
	std::shared_ptr<State> prevState;
	std::vector<Player> players;
	Board& board;
	double pot;
	int seatToAct;
	Street street;
	double facingBet;
};