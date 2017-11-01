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
	virtual StateType::StateType Type() const = 0;
	virtual StateType::StateType NextState() = 0;
	virtual void SetValue() = 0;
public:
	std::shared_ptr<State> PrevState();
	Board& GetBoard();
	double Pot() const;
	void SetPot(double wager);
	Position::Position SeatToAct() const;
	void SetSeatToAct();
	Position::Position LastBettor() const;
	void SetLastBettor(Position::Position position);
	Player& ToAct();
	std::vector<Player>& Players();
	Street CurrentStreet() const;
	double WagerToCall() const;
	void SetWagerToCall(double wager);
	bool FacingCheck() const;
	bool IsFinal() const;
	bool IsClosingAction(const Player& player) const;
	double Value() const;
public:
	State& operator=(const State& rhs);
protected:
	static constexpr Position::Position NoLastBettor = Position::Position::None;
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
	const Player& NextToAct() const;
private:
	int IndexOf(Position::Position pos) const;
	int IndexOf(const Player& player) const;
	const Player& PlayerAt(Position::Position pos) const;
};