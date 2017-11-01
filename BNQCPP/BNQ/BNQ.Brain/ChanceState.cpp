#include "PlayerState.h"
#include "ChanceState.h"
#include "HeroStrategy.h"
#include "PlayerState.h"
#include "VillainStrategy.h"

ChanceState::ChanceState(
	std::shared_ptr<State> prevState,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState)
{
}

StateType ChanceState::Type() const
{
	return StateType::Chance;
}

StateType ChanceState::NextState()
{
	return NextToAct().IsHero() ? StateType::PlayerAction : StateType::PlayerAction;
}

//std::shared_ptr<State> ChanceState::NextState()
//{
//	if (NextToAct().IsHero())
//	{
//		return std::make_shared<PlayerState>(
//			std::make_shared<ChanceState>(*this), &HeroStrategy());
//	}
//
//	return std::make_shared<PlayerState>(
//		players, std::make_shared<ChanceState>(*this), &VillainStrategy());
//}
