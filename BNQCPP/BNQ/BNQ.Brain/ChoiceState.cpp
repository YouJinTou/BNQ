#include "PlayerState.h"
#include "ChoiceState.h"
#include "HeroStrategy.h"
#include "PlayerState.h"
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

StateType::StateType ChoiceState::NextState()
{
	return NextToAct().IsHero() ? StateType::PlayerAction : StateType::PlayerAction;
}

void ChoiceState::SetValue()
{
	this->value = 0.0;
}

StateType::StateType ChoiceState::Type() const
{
	return StateType::Choice;
}
