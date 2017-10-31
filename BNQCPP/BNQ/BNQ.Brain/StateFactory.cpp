#include "Action.h"
#include "ChanceState.h"
#include "ChoiceState.h"
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
		states.emplace_back(CreateState(statePtr));

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

		states.emplace_back(CreateState(statePtr));

		break;
	}
	case None:
		break;
	default:
		break;
	}

	return states;
}

std::shared_ptr<State> StateFactory::CreateState(std::shared_ptr<State> statePtr)
{
	StateType nextStateType = statePtr.get()->NextState();

	switch (nextStateType)
	{
	case Chance:
		throw std::logic_error("Should not be creating a chance state.");
	case Choice:
		return CreateChoiceState(statePtr);
	case Final:
		break;
	case HeroAction:
		break;
	case Opponent:
		break;
	case None:
		throw std::logic_error("Should not be creating a no-state.");
	default:
		throw std::logic_error("There should always be something to create.");
	}

	return std::shared_ptr<State>();
}

std::vector<std::shared_ptr<State>> StateFactory::CreateActionStates(std::shared_ptr<State> statePtr)
{
	std::vector<std::shared_ptr<State> > actionStates;
	State* state = statePtr.get();
	
	if (state->FacingCheck())
	{
		actionStates.emplace_back(CreateBet50State(statePtr));
		actionStates.emplace_back(CreateCheckState(statePtr));

		return actionStates;
	}

	bool canRaise = state->ToAct().Stack() > state->WagerToCall();

	if (canRaise)
	{
		actionStates.emplace_back(CreateRaise50State(statePtr));
	}

	actionStates.emplace_back(CreateCallState(statePtr));
	actionStates.emplace_back(CreateFoldState(statePtr));

	return actionStates;
}

std::shared_ptr<State> StateFactory::CreateChoiceState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	std::vector<Action> actions;

	auto choiceState = std::make_shared<ChoiceState>(
		statePtr,
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		actions);

	return choiceState;
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

std::shared_ptr<State> StateFactory::CreateBet50State(std::shared_ptr<State> statePtr)
{
	return std::shared_ptr<State>();
}

std::shared_ptr<State> StateFactory::CreateCallState(std::shared_ptr<State> statePtr)
{
	return std::shared_ptr<State>();
}

std::shared_ptr<State> StateFactory::CreateCheckState(std::shared_ptr<State> statePtr)
{
	return std::shared_ptr<State>();
}

std::shared_ptr<State> StateFactory::CreateFoldState(std::shared_ptr<State> statePtr)
{
	return std::shared_ptr<State>();
}

std::shared_ptr<State> StateFactory::CreateRaise50State(std::shared_ptr<State> statePtr)
{
	return std::shared_ptr<State>();
}
