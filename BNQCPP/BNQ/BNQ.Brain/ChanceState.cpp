#include "ActionState.h"
#include "ChanceState.h"
#include "HeroStrategy.h"
#include "OpponentState.h"
#include "VillainStrategy.h"

ChanceState::ChanceState(std::shared_ptr<State> prevState) :
	State(players, board, pot, seatToAct, lastBettor, street, wagerToCall, prevState)
{
	Card nextCard = board.NextRandomCard();

	if (board.Turn() == Card::None)
	{
		board.SetTurn(nextCard);
	}
	else if (board.River() == Card::None)
	{
		board.SetRiver(nextCard);
	}
}

StateType ChanceState::Type() const
{
	return StateType::Chance;
}

std::shared_ptr<State> ChanceState::NextState()
{
	if (NextToAct().IsHero())
	{
		return std::make_shared<ActionState>(
			std::make_shared<ChanceState>(*this), &HeroStrategy());
	}

	return std::make_shared<OpponentState>(
		players, std::make_shared<ChanceState>(*this), &VillainStrategy());
}
