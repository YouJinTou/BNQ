#include "PlayerState.h"
#include "ChanceState.h"

PlayerState::PlayerState(
	std::shared_ptr<State> prevState,
	PlayerStrategy* strategy,
	std::vector<Player>& players,
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall,
	double playerWager) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, playerWager, prevState),
	strategy(strategy)
{
}

StateType::StateType PlayerState::NextState()
{
	return IsClosingAction(ToAct()) ? StateType::Chance : StateType::PlayerAction;
}

StateType::StateType PlayerState::Type() const
{
	return StateType::PlayerAction;
}

void PlayerState::SetValue()
{
	bool isFinal = IsFinal();
	const Player& toAct = ToAct();
	bool isHero = toAct.IsHero();
	Action action = toAct.LastAction();

	switch (action)
	{
	case Action::Waiting:
		this->value = 0.0;
		break;
	case Action::Bet50:
		this->value = isFinal && isHero ? pot :
			isHero ? -playerWager : 0.0;

		break;
	case Action::Call:
		this->value = isFinal && isHero ? ShowdownValue() : 
			isHero ? -wagerToCall : 0.0;

		break;
	case Action::Check:
		this->value = isFinal ? ShowdownValue() : 0.0;

		break;
	case Action::Fold:
		this->value = isHero ? 0.0 : HeroRemains() ? pot : 0.0;

		break;
	case Action::Raise50:
		this->value = isHero ? -playerWager : 0.0;

		break;
	default:
		break;
	}	
}

Action PlayerState::GetAction()
{
	return strategy->ExecuteChoice(*this);
}

double PlayerState::ShowdownValue() const
{
	return 0.0;
}

bool PlayerState::HeroRemains() const
{
	bool heroRemains = true;

	for (auto& player : players)
	{
		if (player.IsHero())
		{
			continue;
		}

		heroRemains = heroRemains && player.LastAction() == Action::Fold;
	}

	return heroRemains;
}
