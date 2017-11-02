#include "PlayerState.h"
#include "ChanceState.h"
#include "HeroStrategy.h"
#include "PlayerState.h"
#include "VillainStrategy.h"

ChanceState::ChanceState(
	std::shared_ptr<State> prevState,
	StateType::StateType nextStateType,
	std::vector<Player>& players,
	std::shared_ptr<Board> board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall,
	double playerWager) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, playerWager, prevState, nextStateType)
{
}

StateType::StateType ChanceState::Type() const
{
	return StateType::Chance;
}

void ChanceState::SetValue()
{
	this->value = 0.0;
}
