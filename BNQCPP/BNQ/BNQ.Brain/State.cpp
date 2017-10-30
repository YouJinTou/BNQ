#include "State.h"

#include "ActionState.h"
#include "ChanceState.h"
#include "ChoiceState.h"
#include "FinalState.h"
#include "OpponentState.h"

State::State(
	std::vector<Player>& players, 
	Board& board,
	double pot,
	int seatToAct,
	int lastBettor,
	Street street,
	double wagerToCall) :
	players(players),
	board(board),
	pot(pot),
	seatToAct(seatToAct),
	lastBettor(lastBettor),
	street(street),
	wagerToCall(wagerToCall)
{
}

State::State(
	std::vector<Player>& players, 
	Board& board,
	double pot,
	int seatToAct,
	int lastBettor,
	Street street,
	double wagerToCall,
	std::shared_ptr<State> prevState) :
	players(players),
	board(board),
	pot(pot),
	seatToAct(seatToAct),
	lastBettor(lastBettor),
	street(street),
	wagerToCall(wagerToCall),
	prevState(prevState)
{
}

std::vector<Player>& State::Players()
{
	return players;
}

double State::Pot() const
{
	return pot;
}

Player& State::PlayerToAct()
{
	int playerToAct = 0;

	for (int p = 0; p < players.size(); ++p)
	{
		if (players[p].Seat() == seatToAct)
		{
			playerToAct = p;

			break;
		}
	}

	return players[playerToAct];
}

Street State::CurrentStreet() const
{
	return street;
}

double State::WagerToCall() const
{
	return wagerToCall;
}

bool State::IsFinal() const
{
	bool allPassiveActions = true;
	bool allFolded = true;
	bool lastBettorExists = lastBettor != NoLastBettor;

	for (int p = 0; p < players.size(); ++p)
	{
		bool isLastBettor = lastBettorExists && players[p].Seat() == lastBettor;

		if (isLastBettor)
		{
			continue;
		}

		Action lastAction = players[p].LastAction();
		bool isLastActionPassive = lastAction == Action::Call || lastAction == Action::Check;
		allPassiveActions = allPassiveActions && isLastActionPassive;
		allFolded = allFolded && lastAction == Action::Fold;
	}

	bool isRiver = board.River() != Card::None;
	bool isFinal = (allPassiveActions && isRiver) || allFolded;

	return isFinal;
}

double State::Value() const
{
	return value;
}

State& State::operator=(const State& rhs)
{
	pot = rhs.pot;

	return *this;
}

bool State::FacingCheck() const
{
	return wagerToCall == 0.0;
}

void State::SetValue()
{
	value = 0.0;
}

const Player& State::NextToAct() const
{
	int seatDifference = 0;
	int bestDifference = INT_MAX;
	int bestIndex = -1;

	for (int p = 0; p < players.size(); ++p)
	{
		Player player = players[p];

		if (player.Seat() > seatToAct && player.LastAction() != Action::Fold)
		{
			seatDifference = player.Seat() - seatToAct;
			
			if (seatDifference < bestDifference)
			{
				bestDifference = seatDifference;
				bestIndex = p;
			}
		}
	}

	return players[bestIndex];
}
