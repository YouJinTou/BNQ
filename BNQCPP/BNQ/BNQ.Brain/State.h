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
	State();
	State(
		std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall);
	State(std::vector<Player>& players,
		Board& board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall,
		std::shared_ptr<State> prevState);
	virtual StateType Type() const = 0;
	virtual StateType NextState() = 0;
public:
	Board& GetBoard();
	double Pot() const;
	Position::Position SeatToAct() const;
	Position::Position LastBettor() const;
	Player& ToAct();
	std::vector<Player>& Players();
	Street CurrentStreet() const;
	double WagerToCall() const;
	bool FacingCheck() const;
	bool IsFinal() const;
	bool IsClosingAction(const Player& player) const;
	double Value() const;
public:
	State& operator=(const State& rhs);
protected:
	static constexpr Position::Position NoLastBettor = Position::None;
protected:
	std::shared_ptr<State> prevState;
	std::vector<Player> players;
	Board& board;
	double pot;
	Position::Position seatToAct;
	Position::Position lastBettor;
	Street street;
	double wagerToCall;
	double value;
protected:
	void SetValue();
	const Player& NextToAct() const;
private:
	int IndexOf(Position::Position pos) const;
	int IndexOf(const Player& player) const;
	const Player& PlayerAt(Position::Position pos) const;
};