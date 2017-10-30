#include <math.h>

#include "HeroStrategy.h"

Action HeroStrategy::ExecuteChoice(State& state) const
{
	bool facingCheck = state.FacingBet() == 0.0;

	return Action();
}
