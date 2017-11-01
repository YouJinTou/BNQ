#include "PlayerState.h"
#include "ChanceState.h"

PlayerState::PlayerState(
	PlayerStrategy* strategy,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall),
	strategy(strategy)
{
}

PlayerState::PlayerState(
	std::shared_ptr<State> prevState,
	PlayerStrategy* strategy,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState),
	strategy(strategy)
{
}

StateType PlayerState::NextState()
{
	return IsClosingAction(ToAct()) ? StateType::Chance : StateType::PlayerAction;
}

StateType PlayerState::Type() const
{
	return StateType::PlayerAction;
}

Action PlayerState::GetAction()
{
	return strategy->ExecuteChoice(*this);
}
