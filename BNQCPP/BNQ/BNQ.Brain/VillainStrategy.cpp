#include "VillainStrategy.h"

Action VillainStrategy::ExecuteChoice(State& state)
{
	Action action;
	Player& player = state.ToAct();
	double pot = state.Pot();
	double wagerToCall = state.WagerToCall();

	if (state.FacingCheck())
	{
		Action actions[2] = { Action::Bet50, Action::Check };
		action = actions[rand() % 2];

		AdjustStack(action, player, pot, wagerToCall);

		return action;
	}

	bool canRaise = player.Stack() > wagerToCall;

	if (canRaise)
	{
		Action actions[3] = { Action::Call, Action::Fold, Action::Raise50 };
		action = actions[rand() % 3];
	}
	else
	{
		Action actions[2] = { Action::Call, Action::Fold };
		action = actions[rand() % 2];
	}

	AdjustStack(action, player, pot, wagerToCall);

	return action;
}

void VillainStrategy::AdjustStack(Action action, Player& player, double pot, double wagerToCall)
{
	switch (action)
	{
	case Bet50:
	{
		double betSize = 0.5 * pot;

		player.SetStack(betSize);

		break;
	}
	case Call:
		player.SetStack(wagerToCall);

		break;
	case Check:
		break;
	case Fold:
		break;
	case Raise50:
	{
		double raiseSize = 0.5 * (2 * wagerToCall + pot);

		player.SetStack(raiseSize);

		break;
	}
	}
}
