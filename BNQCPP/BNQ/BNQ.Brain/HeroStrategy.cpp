#include <math.h>

#include "Constants.h"
#include "HeroStrategy.h"
#include "State.h"

void HeroStrategy::UpdateRange(State& state)
{
	lastState = &state;
}

double HeroStrategy::GetShowdownValue() const
{
	auto board = lastState->GetBoard().get();
	auto boardAsHand = board->GetBoardAsHand();
	auto holding = lastState->ToAct().GetHolding();

	return evaluator.evaluate(
		boardAsHand +
		omp::Hand(Constants::PowerTwoIndices[holding.Card1]) +
		omp::Hand(Constants::PowerTwoIndices[holding.Card2]));
}
