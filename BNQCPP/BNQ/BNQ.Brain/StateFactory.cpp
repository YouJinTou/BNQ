#include <assert.h>
#include <stdint.h>

#include "Action.h"
#include "ChanceState.h"
#include "Constants.h"
#include "PlayerState.h"
#include "StateFactory.h"
#include "StateType.h"
#include "Street.h"

std::vector<std::shared_ptr<State> > StateFactory::CreateStates(std::shared_ptr<State> statePtr)
{
	std::vector<std::shared_ptr<State> > states;
	State* state = statePtr.get();
	auto nextStateType = state->NextStateType();

	switch (nextStateType)
	{
	case StateType::Chance:
		return CreateChanceStates(statePtr);
	case StateType::Final:
		assert("CreateStates Final reached.");
	case StateType::PlayerAction:
		return CreatePlayerStates(statePtr);
	case StateType::None:
		assert("CreateStates None reached.");
	default:
		assert("CreateStates default reached.");
	}

	return states;
}

std::vector<std::shared_ptr<State>> StateFactory::CreatePlayerStates(std::shared_ptr<State> statePtr)
{
	std::vector<std::shared_ptr<State> > playerStates;
	State* state = statePtr.get();
	bool isAllIn = state->ToAct().Stack() == 0.0;

	if (isAllIn)
	{
		playerStates.emplace_back(CreateCheckState(statePtr));

		return playerStates;
	}

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

std::vector<std::shared_ptr<State> > StateFactory::CreateChanceStates(std::shared_ptr<State> statePtr)
{
	int minCardPow = Constants::PowerTwoIndices[Card::s2];
	int maxCardPow = Constants::PowerTwoIndices[Card::dA];
	auto board = statePtr.get()->GetBoard();
	std::vector<std::shared_ptr<State> > chanceStates;
	Street street = board->Turn() == Card::Card::None ? Street::Turn : Street::River;
	State* state = statePtr.get();
	const Hand heroHand = state->Hero().GetHand();
	uint64_t mask = 1;

	for (int c = minCardPow; c <= maxCardPow; c++)
	{
		auto newBoard = std::make_shared<Board>(*board);
		Card::Card nextCard = (Card::Card)(mask << c);
		bool cardAvailable = ((heroHand & nextCard) == 0) && newBoard->AddNextCard(nextCard);

		if (!cardAvailable)
		{
			continue;
		}

		auto chanceState = std::make_shared<ChanceState>(
			statePtr,
			StateType::PlayerAction,
			state->Players(),
			newBoard,
			state->Pot(),
			state->SeatToAct(),
			state->LastBettor(),
			street,
			state->WagerToCall(),
			state->PlayerWager());

		for (auto& player : chanceState->Players())
		{
			if (player.LastAction() != Action::Fold)
			{
				player.SetLastAction(Action::Waiting);
			}
		}

		chanceState->SetWagerToCall(0.0);
		chanceState->SetPlayerWager(0.0);
		chanceState->SetValue();
		chanceState->SetLastBettor(Position::Position::None);
		chanceState->SetSeatToAct();

		chanceStates.emplace_back(chanceState);
	}

	return chanceStates;
}

std::shared_ptr<State> StateFactory::CreateBet50State(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	auto bet50State = std::make_shared<PlayerState>(
		statePtr,
		StateType::PlayerAction,
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		state->PlayerWager());
	Player& player = bet50State->ToAct();
	double pot = bet50State->Pot();
	double betSize = 0.5 * pot;

	player.SetLastAction(Action::Bet50);
	player.SetStack(betSize);
	bet50State->SetWagerToCall(betSize);
	bet50State->SetPlayerWager(betSize);
	bet50State->SetPot(betSize);
	bet50State->SetValue();
	bet50State->SetLastBettor(player.Seat());
	bet50State->SetSeatToAct();

	return bet50State;
}

std::shared_ptr<State> StateFactory::CreateCallState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	auto callState = std::make_shared<PlayerState>(
		statePtr,
		state->NextStateType(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		state->PlayerWager());
	Player& player = callState->ToAct();
	double callSize = state->WagerToCall();
	bool isClosingAction = callState->IsClosingAction(player);
	bool isFinal = isClosingAction && callState->GetBoard()->River() != Card::None;
	auto nextStateType = isFinal ? StateType::Final : 
		isClosingAction ? StateType::Chance : StateType::PlayerAction;

	player.SetLastAction(Action::Call);
	player.SetStack(callSize);
	callState->SetNextStateType(nextStateType);
	callState->SetPot(callSize);
	callState->SetPlayerWager(0.0);
	callState->SetValue();
	callState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	callState->SetSeatToAct();
	callState->SetWagerToCall(isClosingAction ? 0.0 : callSize);

	return callState;
}

std::shared_ptr<State> StateFactory::CreateCheckState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	auto checkState = std::make_shared<PlayerState>(
		statePtr,
		state->NextStateType(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		state->PlayerWager());
	Player& player = checkState->ToAct();
	bool isClosingAction = checkState->IsClosingAction(player);
	bool isFinal = isClosingAction && checkState->GetBoard()->River() != Card::None;
	auto nextStateType = isFinal ? StateType::Final :
		isClosingAction ? StateType::Chance : StateType::PlayerAction;

	player.SetLastAction(Action::Check);
	checkState->SetNextStateType(nextStateType);
	checkState->SetPlayerWager(0.0);
	checkState->SetValue();
	checkState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	checkState->SetSeatToAct();

	return checkState;
}

std::shared_ptr<State> StateFactory::CreateFoldState(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	auto foldState = std::make_shared<PlayerState>(
		statePtr,
		state->NextStateType(),
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		state->PlayerWager());
	Player& player = foldState->ToAct();

	player.SetLastAction(Action::Fold);

	bool isClosingAction = foldState->IsClosingAction(player);
	bool isFinal = isClosingAction && foldState->IsFinal();
	auto nextStateType = isFinal ? StateType::Final :
		isClosingAction ? StateType::Chance : StateType::PlayerAction;

	foldState->SetNextStateType(nextStateType);
	foldState->SetPlayerWager(0.0);
	foldState->SetValue();
	foldState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	foldState->SetSeatToAct();
	foldState->SetWagerToCall(isClosingAction ? 0.0 : state->WagerToCall());

	return foldState;
}

std::shared_ptr<State> StateFactory::CreateRaise50State(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();
	auto raise50State = std::make_shared<PlayerState>(
		statePtr,
		StateType::PlayerAction,
		state->Players(),
		state->GetBoard(),
		state->Pot(),
		state->SeatToAct(),
		state->LastBettor(),
		state->CurrentStreet(),
		state->WagerToCall(),
		state->PlayerWager());
	Player& player = raise50State->ToAct();
	double pot = raise50State->Pot();
	double wagerToCall = state->WagerToCall();
	double raiseSize = 0.5 * (3 * wagerToCall + pot);

	player.SetLastAction(Action::Raise50);
	player.SetStack(raiseSize);
	raise50State->SetPot(raiseSize);
	raise50State->SetPlayerWager(raiseSize);
	raise50State->SetValue();
	raise50State->SetLastBettor(player.Seat());
	raise50State->SetSeatToAct();
	raise50State->SetWagerToCall(raiseSize);

	return raise50State;
}