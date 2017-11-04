#include "State.h"
#include "BalancedStrategy.h"

void BalancedStrategy::UpdateRange(const State& state)
{
	lastState = &state;
}

omp::Hand BalancedStrategy::GetShowdownHand() const
{
	auto boardAsHand = lastState->GetBoard().GetBoardAsHand();
	auto boardValue = evaluator.evaluate(boardAsHand);
	//bool 
	//double bluffs = 0.3;
	//if (evaluator.)
	return omp::Hand();
}
