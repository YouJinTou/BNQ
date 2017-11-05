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
	omp::Hand card1 = Constants::PowerTwoIndices[holding.Card1];
	omp::Hand card2 = Constants::PowerTwoIndices[holding.Card2];
	double eval = evaluator.evaluate(boardAsHand + card1 + card2);

	return eval;
}
