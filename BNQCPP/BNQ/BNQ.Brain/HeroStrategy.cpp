#include <math.h>

#include "HeroStrategy.h"
#include "State.h"

void HeroStrategy::UpdateRange(const State& state)
{
}

omp::Hand HeroStrategy::GetShowdownHand() const
{
	return omp::Hand::empty();
}
