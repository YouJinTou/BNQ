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
		this->value = isFinal && isHero ? ShowdownValue() : 0.0;

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

double PlayerState::ShowdownValue() const
{
	auto boardHand = board->GetBoardAsHand();
	int currentHandValue = -1;
	uint16_t bestHandValue = 0;
	uint16_t heroHandValue = 0;
	uint16_t tieHandValue = 0;
	int tieCounter = 1;
	bool tieWithHeroExists = false;

	for (auto& player : players)
	{
		auto playerHand = boardHand + player.GetShowdownHand();
		currentHandValue = evaluator.evaluate(playerHand);
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
			currentHandValue = -1;
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
