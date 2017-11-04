#include "State.h"
#include "BalancedStrategy.h"

void BalancedStrategy::UpdateRange(const State& state)
{
}

omp::Hand BalancedStrategy::GetShowdownHand() const
{
	return omp::Hand::empty();
}
