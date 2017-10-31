#include "ChanceState.h"
#include "StateFactory.h"
#include "Street.h"

std::vector<std::shared_ptr<State> > StateFactory::CreateStates(std::shared_ptr<State> statePtr)
{
	std::vector<std::shared_ptr<State> > states;
	State* state = statePtr.get();
	StateType nextStateType = state->NextState();

	switch (state->Type())
	{
	case Chance:
	case Choice:
		states.emplace_back(CreateState(nextStateType));

		break;
	case Final:
		break;
	case HeroAction:
	case Opponent:
	{
		if (nextStateType == StateType::Chance)
		{
			return CreateChanceStates(statePtr);
		}

		states.emplace_back(CreateState(nextStateType));

		break;
	}
	case None:
		break;
	default:
		break;
	}

	return states;
}

std::shared_ptr<State> StateFactory::CreateState(StateType stateType)
{
	switch (stateType)
	{
	case Chance:
		break;
	case Choice:
		break;
	case Final:
		break;
	case HeroAction:
		break;
	case Opponent:
		break;
	case None:
		break;
	default:
		break;
	}

	return std::shared_ptr<State>();
}

std::vector<std::shared_ptr<State> > StateFactory::CreateChanceStates(std::shared_ptr<State> statePtr)
{
	int minCardPow = 4; // Card::c2;
	int maxCardPow = 55; // Card::sA;
	Board& board = statePtr.get()->GetBoard();
	std::vector<std::shared_ptr<State> > chanceStates;
	Street street = board.Turn() == Card::Card::None ? Street::Turn : Street::River;
	State* state = statePtr.get();

	for (int c = minCardPow; c <= maxCardPow; c++)
	{
		if (board.AddNextCard((Card::Card)(1 << c)))
		{
			auto chanceState = std::make_shared<ChanceState>(
				statePtr,
				state->Players(),
				state->GetBoard(),
				state->Pot(),
				state->SeatToAct(),
				state->LastBettor(),
				street,
				state->WagerToCall());

			chanceStates.emplace_back(chanceState);
		}
	}

	return chanceStates;
}
