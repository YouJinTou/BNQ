#include "HandEvaluator\Hand.h"

#include "ChanceState.h"
#include "Constants.h"
#include "PlayerState.h"

PlayerState::PlayerState(
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

StateType::StateType PlayerState::Type() const
{
	return StateType::PlayerAction;
}

void PlayerState::SetValue(bool isFinal)
{
	const Player& lastActed = LastActed();
	bool wasHero = lastActed.IsHero();
	Action action = lastActed.LastAction();

	switch (action)
	{
	case Action::Waiting:
		this->value = 0.0;

		break;
	case Action::Bet50:
		this->value = isFinal && wasHero ? pot :
			wasHero ? -playerWager : 0.0;

		break;
	case Action::Call:
		this->value = isFinal && wasHero ? ShowdownValue() : 
			wasHero ? -wagerToCall : 0.0;

		break;
	case Action::Check:
		this->value = isFinal && wasHero ? ShowdownValue() : 0.0;

		break;
	case Action::Fold:
		this->value = wasHero ? 0.0 : HeroRemains() ? pot : 0.0;

		break;
	case Action::Raise50:
		this->value = isFinal && wasHero ? pot : 
			wasHero ? -playerWager : 0.0;

		break;
	default:
		break;
	}	
}

double PlayerState::ShowdownValue() const
{
	auto boardHand = board->GetBoardAsHand();
	double bestHandValue = 0;
	double heroHandValue = 0;
	double tieHandValue = 0;
	int tieCounter = 1;
	bool tieWithHeroExists = false;

	for (auto& player : players)
	{
		if (player.LastAction() == Action::Fold)
		{
			continue;
		}

		double currentHandValue = player.GetShowdownValue(boardHand);
		bool isHero = player.IsHero();

		if (isHero)
		{
			heroHandValue = currentHandValue;
		}

		if (currentHandValue == bestHandValue)
		{
			tieHandValue = bestHandValue;
			tieCounter++;
		}
		else if (currentHandValue > bestHandValue)
		{
			bestHandValue = currentHandValue;
		}
	}

	if (heroHandValue == bestHandValue)
	{
		return (tieHandValue == bestHandValue) ? pot / tieCounter : pot;
	}

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
