#include "ActionState.h"
#include "ChanceState.h"
#include "ChoiceState.h"
#include "HeroStrategy.h"
#include "OpponentState.h"
#include "VillainStrategy.h"

OpponentState::OpponentState(
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

OpponentState::OpponentState(
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

StateType OpponentState::NextState()
{
	if (IsClosingAction(ToAct()))
	{
		return StateType::Chance;
	}

	if (NextToAct().IsHero())
	{
		return StateType::HeroAction;
	}

	return StateType::Opponent;
}

//std::shared_ptr<State> OpponentState::NextState()
//{
//	if (IsClosingAction(ToAct()))
//	{
//		return std::make_shared<ChanceState>(std::make_shared<OpponentState>(*this));
//	}
//
//	if (NextToAct().IsHero())
//	{
//		return std::make_shared<ActionState>(
//			std::make_shared<OpponentState>(*this), &HeroStrategy());
//	}
//
//	return std::make_shared<OpponentState>(
//		players, std::make_shared<OpponentState>(*this), &VillainStrategy());
//}

StateType OpponentState::Type() const
{
	return StateType::Opponent;
}
