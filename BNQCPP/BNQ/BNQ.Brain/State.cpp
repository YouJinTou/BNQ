#include <assert.h>

#include "ChanceState.h"
#include "FinalState.h"
#include "PlayerState.h"
#include "State.h"

State::State() :
	board(Board())
{
}

State::State(
	std::vector<Player>& players, 
	Board& board,
	double pot,
	Position::Position seatToAct,
	Position::Position lastBettor,
	Street street,
	double wagerToCall,
	double playerWager,
	std::shared_ptr<State> prevState,
	StateType::StateType nextStateType) :
	players(players),
	board(board),
	pot(pot),
	seatToAct(seatToAct),
	lastBettor(lastBettor),
	street(street),
	wagerToCall(wagerToCall),
	playerWager(playerWager),
	prevState(prevState),
	nextStateType(nextStateType)
{
}

std::vector<Player>& State::Players()
{
	return players;
}

std::shared_ptr<State> State::PrevState()
{
	return prevState;
}

StateType::StateType State::NextStateType() const
{
	return nextStateType;
}

void State::SetNextStateType(StateType::StateType nextStateType)
{
	this->nextStateType = nextStateType;
}

Board& State::GetBoard()
{
	return board;
}

double State::Pot() const
{
	return pot;
}

void State::SetPot(double wager)
{
	pot += wager;
}

Position::Position State::SeatToAct() const
{
	return seatToAct;
}

void State::SetSeatToAct()
{
	seatToAct = NextToAct().Seat();
}

Position::Position State::LastBettor() const
{
	return lastBettor;
}

void State::SetLastBettor(Position::Position position)
{
	lastBettor = position;
}

Player& State::ToAct()
{
	size_t playerToAct = 0;

	for (size_t p = 0; p < players.size(); ++p)
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

void State::SetStreet(Street street)
{
	this->street = street;
}

double State::WagerToCall() const
{
	return wagerToCall;
}

void State::SetWagerToCall(double wager)
{
	wagerToCall = wager;
}

double State::PlayerWager() const
{
	return playerWager;
}

void State::SetPlayerWager(double wager)
{
	playerWager = wager;
}

bool State::IsFinal() const
{
	bool allPassiveActions = true;
	bool allFolded = true;
	bool lastBettorExists = lastBettor != NoLastBettor;

	for (auto& player : players)
	{
		bool isLastBettor = lastBettorExists && player.Seat() == lastBettor;

		if (isLastBettor)
		{
			continue;
		}

		Action lastAction = player.LastAction();
		bool isLastActionPassive = lastAction == Action::Call || lastAction == Action::Check;
		allPassiveActions = allPassiveActions && isLastActionPassive;
		allFolded = allFolded && lastAction == Action::Fold;
	}

	bool isRiver = board.River() != Card::None;
	bool isFinal = (allPassiveActions && isRiver) || allFolded;

	return isFinal;
}

bool State::IsClosingAction(const Player& player) const
{
	bool lastBettorExists = lastBettor != NoLastBettor;
	int playerIndex = IndexOf(player);

	if (lastBettorExists)
	{
		int lastBettorIndex = IndexOf(lastBettor);
		bool lastBettorInPosition = lastBettorIndex > playerIndex;

		if (lastBettorInPosition)
		{
			bool isClosingAction = true;

			for (int p = lastBettorIndex - 1; p >= 0; p--)
			{
				if (isClosingAction && players[p].LastAction() != Action::Fold)
				{
					isClosingAction = playerIndex == p;
				}
			}

			return isClosingAction;
		}

		for (auto& p : players)
		{
			if (p.Seat() > player.Seat() && p.LastAction() != Action::Fold)
			{
				return false;
			}
		}

		for (auto& p : players)
		{
			if (p.Seat() < lastBettor && p.LastAction() != Action::Fold)
			{
				return false;
			}
		}

		return true;
	}

	bool isClosingAction = true;

	for (int p = players.size() - 1; p >= 0; --p)
	{
		if (isClosingAction && players[p].LastAction() != Action::Fold)
		{
			isClosingAction = playerIndex == p;
		}
	}

	return isClosingAction;
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

const Player& State::NextToAct() const
{
	bool lastBettorExists = lastBettor != NoLastBettor;

	if (lastBettorExists)
	{
		bool lastBettorInPosition = lastBettor - seatToAct > 0;

		if (lastBettorInPosition)
		{
			for (auto& player : players)
			{
				if (player.Seat() > seatToAct && player.LastAction() != Action::Fold)
				{
					return player;
				}
			}

			assert(0);
		}
		else
		{
			for (auto& player : players)
			{
				if (player.Seat() > seatToAct && player.LastAction() != Action::Fold)
				{
					return player;
				}				
			}

			for (auto& player : players)
			{
				if (player.Seat() < seatToAct && player.LastAction() != Action::Fold)
				{
					return player;
				}
			}

			assert(0);
		}		
	}

	for (auto& player : players)
	{
		if (player.Seat() > seatToAct && player.LastAction() != Action::Fold)
		{
			return player;
		}
	}

	assert(0);
}

int State::IndexOf(Position::Position pos) const
{
	int index = -1;

	for (int p = 0; p < players.size(); ++p)
	{
		if (players[p].Seat() == pos)
		{
			index = p;

			break;
		}
	}

	return index;
}

int State::IndexOf(const Player& player) const
{
	int index = -1;

	for (int p = 0; p < players.size(); ++p)
	{
		if (players[p].Seat() == player.Seat())
		{
			index = p;

			break;
		}
	}

	return index;
}

const Player& State::PlayerAt(Position::Position pos) const
{
	const Player* p = nullptr;

	for (auto& player : players)
	{
		if (player.Seat() == pos)
		{
			p = &player;

			break;
		}
	}

	return *p;
}
