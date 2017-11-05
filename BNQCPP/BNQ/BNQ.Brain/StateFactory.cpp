#include <assert.h>
#include <stdint.h>

#include "Action.h"
#include "ChanceState.h"
#include "Constants.h"
#include "Holding.h"
#include "PlayerState.h"
#include "StateFactory.h"
#include "StateType.h"
#include "Street.h"

std::vector<std::shared_ptr<State> > StateFactory::CreateStates(std::shared_ptr<State> statePtr)
{
	switch (statePtr->NextStateType())
	{
	case StateType::Chance:
		return CreateChanceStates(statePtr);
	case StateType::PlayerAction:
		return CreatePlayerStates(statePtr);
	default:
		break;
	}

	return std::vector<std::shared_ptr<State> >();
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
	const Holding heroHolding = state->Hero().GetHolding();
	auto heroHand = heroHolding.Card1 | heroHolding.Card2;
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
			if (player.IsPlaying())
			{
				player.SetLastAction(Action::Waiting);
			}
		}

		chanceState->SetWagerToCall(0.0);
		chanceState->SetPlayerWager(0.0);
		chanceState->SetValue();
		chanceState->SetLastBettor(Position::Position::None);
		chanceState->SetLastActed(Position::None);
		chanceState->SetSeatToAct(chanceState->FirstToAct().Seat());

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
	double stack = player.Stack();
	double pot = bet50State->Pot();
	double betSize = 0.5 * pot;
	betSize = (betSize > stack) ? stack : betSize;

	player.SetStack(betSize);
	bet50State->SetWagerToCall(betSize);
	bet50State->SetPlayerWager(betSize);
	bet50State->SetPot(betSize);
	bet50State->SetValue();
	bet50State->SetSeatToAct();
	bet50State->SetLastBettor(player.Seat());
	bet50State->SetLastActed(player.Seat());
	player.SetLastAction(Action::Bet50);

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
	double stack = player.Stack();
	double callSize = state->WagerToCall();
	callSize = (callSize > stack) ? stack : callSize;
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
	callState->SetSeatToAct();
	callState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	callState->SetLastActed(player.Seat());
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
	checkState->SetSeatToAct();
	checkState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	checkState->SetLastActed(player.Seat());

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
	auto lastAction = player.LastAction();

	player.SetLastAction(Action::Fold);

	bool isClosingAction = foldState->IsClosingAction(player);
	bool isFinal = player.IsHero() ? true : isClosingAction && foldState->IsFinal();

	player.SetLastAction(lastAction);
	foldState->SetPlayerWager(0.0);
	foldState->SetValue();
	foldState->SetSeatToAct();
	foldState->SetLastBettor(isClosingAction ? Position::Position::None : state->LastBettor());
	foldState->SetLastActed(player.Seat());
	foldState->SetWagerToCall(isClosingAction ? 0.0 : state->WagerToCall());

	auto nextStateType = isFinal ? StateType::Final :
		isClosingAction ? StateType::Chance : StateType::PlayerAction;

	foldState->SetNextStateType(nextStateType);
	player.SetLastAction(Action::Fold);

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
	double stack = player.Stack();
	double pot = raise50State->Pot();
	double wagerToCall = state->WagerToCall();
	double raiseSize = 0.5 * (3 * wagerToCall + pot);
	raiseSize = (raiseSize > stack) ? stack : raiseSize;

	player.SetLastAction(Action::Raise50);
	player.SetStack(raiseSize);
	raise50State->SetPot(raiseSize);
	raise50State->SetPlayerWager(raiseSize);
	raise50State->SetValue();
	raise50State->SetLastBettor(player.Seat());
	raise50State->SetSeatToAct();
	raise50State->SetLastActed(player.Seat());
	raise50State->SetWagerToCall(raiseSize);

	return raise50State;
}