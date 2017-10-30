#pragma once

#include <memory>
#include <vector>

#include "Action.h"
#include "Board.h"
#include "Player.h"
#include "Position.h"
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
		Position seatToAct,
		Position lastBettor,
		Street street,
		double wagerToCall);
	State(std::vector<Player>& players,
		Board& board,
		double pot,
		Position seatToAct,
		Position lastBettor,
		Street street,
		double wagerToCall,
		std::shared_ptr<State> prevState);
	virtual StateType Type() const = 0;
	virtual std::shared_ptr<State> NextState() = 0;
public:
	double Pot() const;
	Player& PlayerToAct();
	Street CurrentStreet() const;
	double WagerToCall() const;
	bool FacingCheck() const;
	bool IsFinal() const;
	bool IsClosingAction(const Player& player) const;
	double Value() const;
public:
	State& operator=(const State& rhs);
protected:
	static constexpr Position NoLastBettor = Position::None;
protected:
	std::shared_ptr<State> prevState;
	std::vector<Player> players;
	Board& board;
	double pot;
	Position seatToAct;
	Position lastBettor;
	Street street;
	double wagerToCall;
	double value;
protected:
	std::vector<Player>& Players();
	void SetValue();
	const Player& NextToAct() const;
};