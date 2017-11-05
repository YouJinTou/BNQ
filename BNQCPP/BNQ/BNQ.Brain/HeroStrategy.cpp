#include <math.h>

#include "ChanceState.h"
#include "Constants.h"
#include "HeroStrategy.h"
#include "PlayerState.h"
#include "State.h"

double HeroStrategy::GetShowdownValue(State* statePtr) const
{
	auto board = statePtr->GetBoard();
	auto boardAsHand = board->GetBoardAsHand();
	auto holding = statePtr->ToAct().GetHolding();
	omp::Hand card1 = Constants::PowerTwoIndices[holding.Card1];
	omp::Hand card2 = Constants::PowerTwoIndices[holding.Card2];
	double eval = evaluator.evaluate(boardAsHand + card1 + card2);

	return eval;
}
