#include "ChoiceState.h"

ChoiceState::ChoiceState(std::shared_ptr<State> prevState, const std::vector<Action>& actions) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState),
	actions(actions)
{
}

std::vector<Action>& ChoiceState::Actions()
{
	return actions;
}

std::shared_ptr<State> ChoiceState::NextState()
{
	//auto nextState = std::make_shared<ActionState>(std::make_shared<ChoiceState>(this), Action::Bet50);

	return std::shared_ptr<State>();
}

StateType ChoiceState::Type() const
{
	return StateType::Choice;
}
