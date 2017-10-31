#include "ActionState.h"
#include "ChoiceState.h"
#include "HeroStrategy.h"
#include "OpponentState.h"
#include "VillainStrategy.h"

ChoiceState::ChoiceState(std::shared_ptr<State> prevState,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall,
	const std::vector<Action>& actions) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState),
	actions(actions)
{
}

std::vector<Action>& ChoiceState::Actions()
{
	return actions;
}

StateType ChoiceState::NextState()
{
	return NextToAct().IsHero() ? StateType::HeroAction : StateType::Opponent;
}

//std::shared_ptr<State> ChoiceState::NextState()
//{
//	if (NextToAct().IsHero())
//	{
//		return std::make_shared<ActionState>(
//			std::make_shared<ChoiceState>(*this), &HeroStrategy());
//	}
//
//	return std::make_shared<OpponentState>(
//		players, std::make_shared<ChoiceState>(*this), &VillainStrategy());
//}

StateType ChoiceState::Type() const
{
	return StateType::Choice;
}
