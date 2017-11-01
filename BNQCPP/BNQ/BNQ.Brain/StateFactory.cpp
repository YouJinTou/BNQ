#include "Action.h"
#include "PlayerState.h"
#include "ChanceState.h"
#include "ChoiceState.h"
#include "HeroStrategy.h"
#include "StateFactory.h"
#include "StateType.h"
#include "Street.h"
#include "VillainStrategy.h"

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
	case PlayerAction:
	{
		if (nextStateType == StateType::Chance)
		{
			return CreateChanceStates(statePtr);
		}

		return CreatePlayerStates(statePtr);
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
	case PlayerAction:
		break;
	case None:
		throw std::logic_error("Should not be creating a non-state.");
	default:
		throw std::logic_error("There should always be something to create.");
	}

	return std::shared_ptr<State>();
}

std::vector<std::shared_ptr<State>> StateFactory::CreatePlayerStates(std::shared_ptr<State> statePtr)
{
	std::vector<std::shared_ptr<State> > playerStates;
	State* state = statePtr.get();
	
	if (state->FacingCheck())
	{
		playerStates.emplace_back(CreateBet50State(statePtr));
		playerStates.emplace_back(CreateCheckState(statePtr));

		return playerStates;
	}

	bool canRaise = state->ToAct().Stack() > state->WagerToCall();

	if (canRaise)
	{
		playerStates.emplace_back(CreateRaise50State(statePtr));
	}

	playerStates.emplace_back(CreateCallState(statePtr));
	playerStates.emplace_back(CreateFoldState(statePtr));

	return playerStates;
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
	State* state = statePtr.get();
	bool isHero = state->ToAct().IsHero();
	auto bet50State = std::make_shared<PlayerState>(
		statePtr,
		Strategy(isHero).get(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall());
	Player& player = bet50State->ToAct();
	double pot = bet50State->Pot();
	double betSize = 0.5 * pot;

	player.SetStack(betSize);
	bet50State->SetPot(betSize);
	bet50State->SetWagerToCall(betSize);
	bet50State->SetSeatToAct();
	bet50State->SetLastBettor(player.Seat());

	return bet50State;
}

std::shared_ptr<State> StateFactory::CreateCallState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	bool isHero = state->ToAct().IsHero();
	auto callState = std::make_shared<PlayerState>(
		statePtr,
		Strategy(isHero).get(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall());
	Player& player = callState->ToAct();
	double callSize = state->WagerToCall();
	bool isClosingAction = callState->IsClosingAction(player);

	player.SetStack(callSize);
	callState->SetPot(callSize);
	callState->SetSeatToAct();
	callState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	callState->SetWagerToCall(isClosingAction ? 0.0 : callSize);

	return callState;
}

std::shared_ptr<State> StateFactory::CreateCheckState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	bool isHero = state->ToAct().IsHero();
	auto checkState = std::make_shared<PlayerState>(
		statePtr,
		Strategy(isHero).get(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall());
	Player& player = checkState->ToAct();
	bool isClosingAction = checkState->IsClosingAction(player);

	checkState->SetSeatToAct();
	checkState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());

	return checkState;
}

std::shared_ptr<State> StateFactory::CreateFoldState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	bool isHero = state->ToAct().IsHero();
	auto foldState = std::make_shared<PlayerState>(
		statePtr,
		Strategy(isHero).get(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall());
	Player& player = foldState->ToAct();
	bool isClosingAction = foldState->IsClosingAction(player);

	player.SetLastAction(Action::Fold);
	foldState->SetSeatToAct();
	foldState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	foldState->SetWagerToCall(isClosingAction ? 0.0 : state->WagerToCall());

	return foldState;
}

std::shared_ptr<State> StateFactory::CreateRaise50State(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	bool isHero = state->ToAct().IsHero();
	auto raise50State = std::make_shared<PlayerState>(
		statePtr,
		Strategy(isHero).get(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall());
	Player& player = raise50State->ToAct();
	double pot = raise50State->Pot();
	double wagerToCall = state->WagerToCall();
	double raiseSize = 0.5 * (2 * wagerToCall + pot);
	bool isClosingAction = raise50State->IsClosingAction(player);

	player.SetLastAction(Action::Fold);
	raise50State->SetPot(raiseSize);
	raise50State->SetSeatToAct();
	raise50State->SetLastBettor(player.Seat());
	raise50State->SetWagerToCall(raiseSize);

	return raise50State;
}

std::shared_ptr<PlayerStrategy> StateFactory::Strategy(bool isHero)
{
	if (isHero)
	{
		return std::make_shared<HeroStrategy>();
	}

	return std::make_shared<VillainStrategy>();
}
