#pragma once

#include <iostream>
#include <memory>
#include <vector>

#include "HandEvaluator\HandEvaluator.h"

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
	State(std::vector<Player>& players,
		std::shared_ptr<Board> board,
		double pot,
		Position::Position seatToAct,
		Position::Position lastBettor,
		Street street,
		double wagerToCall,
		double playerWager,
		std::shared_ptr<State> prevState,
		StateType::StateType nextStateType);
	virtual StateType::StateType Type() const = 0;
	virtual void SetValue(bool isFinal = false) = 0;
public:
	std::shared_ptr<State> PrevState();
	StateType::StateType NextStateType() const;
	void SetNextStateType(StateType::StateType nextStateType);
	const Board& GetBoard() const;
	std::shared_ptr<Board> GetBoard();
	double Pot() const;
	void SetPot(double wager);
	Position::Position SeatToAct() const;
	void SetSeatToAct();
	Position::Position LastBettor() const;
	void SetLastBettor(Position::Position position);
	Player& ToAct();
	std::vector<Player>& Players();
	const Player& Hero() const;
	Street CurrentStreet() const;
	void SetStreet(Street street);
	double WagerToCall() const;
	void SetWagerToCall(double wager);
	double PlayerWager() const;
	void SetPlayerWager(double wager);
	bool FacingCheck() const;
	bool IsFinal() const;
	bool IsClosingAction(const Player& player) const;
	double Value() const;
	void UpdateToActRange();
public:
	State& operator=(const State& rhs);
	friend std::ostream& operator<<(std::ostream& os, const State& state);
protected:
	static constexpr Position::Position NoLastBettor = Position::Position::None;
	static const omp::HandEvaluator evaluator;
protected:
	std::shared_ptr<State> prevState;
	StateType::StateType nextStateType;
	std::vector<Player> players;
	std::shared_ptr<Board> board;
	double pot;
	Position::Position seatToAct;
	Position::Position lastBettor;
	Street street;
	double wagerToCall;
	double playerWager;
	double value;
protected:
	const Player& NextToAct() const;
private:
	int IndexOf(Position::Position pos) const;
	int IndexOf(const Player& player) const;
	const Player& PlayerAt(Position::Position pos) const;
};