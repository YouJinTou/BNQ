#include <assert.h>

#include "ChanceState.h"
#include "FinalState.h"
#include "PlayerState.h"
#include "State.h"

const omp::HandEvaluator State::evaluator = omp::HandEvaluator();

State::State() :
	board(std::make_shared<Board>(Board()))
{
}

State::State(
	std::vector<Player>& players,
	std::shared_ptr<Board> board,
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

const Player& State::Hero() const
{
	const Player* heroPtr = nullptr;

	for (auto& player : players)
	{
		if (player.IsHero())
		{
			heroPtr = &player;

			break;
		}
	}

	return *heroPtr;
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

const Board& State::GetBoard() const
{
	return *board;
}

std::shared_ptr<Board> State::GetBoard()
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
	seatToAct = nextStateType == StateType::Final ?
		Position::None :
		NextToAct().Seat();
}

void State::SetSeatToAct(Position::Position position)
{
	seatToAct = position;
}

void State::SetLastActed(Position::Position position)
{
	lastActed = position;
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

Player& State::LastActed()
{
	size_t playerLastActed = 0;

	for (size_t p = 0; p < players.size(); ++p)
	{
		if (players[p].Seat() == lastActed)
		{
			playerLastActed = p;

			break;
		}
	}

	return players[playerLastActed];
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
	if (nextStateType == StateType::Final)
	{
		return true;
	}

	bool allPassiveActions = true;
	bool allFolded = true;
	bool lastBettorExists = lastBettor != NoLastBettor;

	for (auto& player : players)
	{
		if (player.IsHero() && !player.IsPlaying())
		{
			return true;
		}

		bool isLastBettor = lastBettorExists && player.Seat() == lastBettor;

		if (isLastBettor)
		{
			continue;
		}

		Action lastAction = player.LastAction();
		bool isLastActionPassive = lastAction == Action::Call || lastAction == Action::Check;
		allPassiveActions = allPassiveActions && isLastActionPassive;
		allFolded = allFolded && !player.IsPlaying();
	}

	bool isRiver = board->River() != Card::None;
	bool isFinal = (allPassiveActions && isRiver) || allFolded;

	return isFinal;
}

bool State::IsFinalState() const
{
	return nextStateType == StateType::Final;
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
				if (isClosingAction && players[p].IsPlaying())
				{
					isClosingAction = playerIndex == p;

					if (isClosingAction)
					{
						break;
					}
				}
			}

			return isClosingAction;
		}

		for (auto& p : players)
		{
			if (p.Seat() > player.Seat() && p.IsPlaying())
			{
				return false;
			}
		}

		for (auto& p : players)
		{
			if (p.Seat() < lastBettor && p.IsPlaying())
			{
				return false;
			}
		}

		return true;
	}

	bool isClosingAction = true;

	for (int p = players.size() - 1; p >= 0; --p)
	{
		if (isClosingAction && players[p].IsPlaying())
		{
			isClosingAction = playerIndex == p;

			if (isClosingAction)
			{
				break;
			}
		}
	}

	return isClosingAction;
}

double State::Value() const
{
	return value;
}

void State::UpdateToActRange()
{
	ToAct().UpdateRanges(*this);
}

State& State::operator=(const State& rhs)
{
	prevState = rhs.prevState;
	nextStateType = rhs.nextStateType;
	players = rhs.players;
	board = rhs.board;
	pot = rhs.pot;
	seatToAct = rhs.seatToAct;
	lastActed = rhs.lastActed;
	lastBettor = rhs.lastBettor;
	street = rhs.street;
	wagerToCall = rhs.wagerToCall;
	playerWager = rhs.playerWager;
	value = rhs.value;

	return *this;
}

std::ostream& operator<<(std::ostream& os, const State& state)
{
	os << state.GetBoard();
	os << "Pot: " << state.pot << std::endl;
	os << "Last bettor: " << state.lastBettor << std::endl;
	os << "Last bet: " << state.wagerToCall << std::endl;
	os << "Hero value: " << state.value << std::endl;
	os << "-------------------------------------------------" << std::endl;

	for (auto& player : state.players)
	{
		os << "Player: " << player.Seat() << 
			(player.Seat() == state.seatToAct ? " <<< " : "")  << std::endl;
		os << "Stack: " << player.Stack() << std::endl;
		os << "Last action: " << player.LastAction() << std::endl;
		os << std::endl;
	}

	os << std::endl << std::endl;

	return os;
}

bool State::FacingCheck() const
{
	return wagerToCall == 0.0;
}

const Player& State::FirstToAct()
{
	Player* firstToAct = nullptr;

	for (auto& player : players)
	{
		if (player.IsPlaying())
		{
			firstToAct = &player;

			break;
		}
	}

	return *firstToAct;
}

const Player& State::NextToAct()
{
	const Player& toAct = ToAct();
	Player* nextToAct = nullptr;

	if (IsClosingAction(toAct))
	{
		return FirstToAct();
	}

	for (auto& player : players)
	{
		if (player.Seat() > seatToAct && player.IsPlaying())
		{
			nextToAct = &player;

			break;
		}
	}

	return (nextToAct == nullptr) ? FirstToAct() : *nextToAct;
}

int State::IndexOf(Position::Position pos) const
{
	int index = -1;

	for (size_t p = 0; p < players.size(); ++p)
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

	for (size_t p = 0; p < players.size(); ++p)
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
