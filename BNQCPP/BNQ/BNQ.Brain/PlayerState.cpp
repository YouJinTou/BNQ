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
	double prevValue = prevState == nullptr ? 0.0 : prevState->Value();

	switch (action)
	{
	case Action::Waiting:
		value = prevValue;

		break;
	case Action::Bet50:
		value = isFinal && wasHero ? pot :
			wasHero ? prevValue - playerWager : prevValue;

		break;
	case Action::Call:
		value = isFinal ? ShowdownValue() :
			wasHero ? prevValue - wagerToCall : prevValue;

		break;
	case Action::Check:
		value =  isFinal ? ShowdownValue() : prevValue;

		break;
	case Action::Fold:
		value = wasHero ? prevValue :
			HeroRemains() ? pot : prevValue;

		break;
	case Action::Raise50:
		value = isFinal && wasHero ? pot :
			wasHero ? prevValue - playerWager : prevValue;

		break;
	default:
		value = 0.0;

		break;
	}	
}

double PlayerState::ShowdownValue()
{
	auto boardHand = board->GetBoardAsHand();
	double bestHandValue = 0;
	double heroHandValue = 0;
	double tieHandValue = 0;
	int tieCounter = 1;

	for (auto& player : players)
	{
		if (player.LastAction() == Action::Fold)
		{
			continue;
		}

		double currentHandValue = GetPlayerShowdownValue(player);
		heroHandValue = player.IsHero() ? currentHandValue : heroHandValue;

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

		heroRemains = heroRemains && !player.IsPlaying();
	}

	return heroRemains;
}
