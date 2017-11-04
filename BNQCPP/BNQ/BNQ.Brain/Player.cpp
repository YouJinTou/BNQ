#include "Constants.h"
#include "Player.h"

const omp::HandEvaluator Player::evaluator = omp::HandEvaluator();

Player::Player(Position::Position seat, double stack, PlayerStrategy* strategy) :
	Player(seat, stack, false, { Card::None, Card::None }, strategy)
{
}

Player::Player(Position::Position seat, double stack, bool isHero, Holding holding, PlayerStrategy* strategy) :
	seat(seat),
	stack(stack),
	isHero(isHero),
	holding(holding),
	strategy(strategy)
{
}

Position::Position Player::Seat() const
{
	return seat;
}

double Player::Stack() const
{
	return stack;
}

bool Player::IsHero() const
{
	return isHero;
}

Action Player::LastAction() const
{
	return lastAction;
}

void Player::SetLastAction(Action action)
{
	lastAction = action;
}

void Player::SetStack(double wager)
{
	stack = (stack - wager) < 0.0 ? 0.0 : stack - wager;
}

void Player::UpdateRanges(State& state)
{
	strategy->UpdateRange(state);
}

Holding Player::GetHolding() const
{
	return holding;
}

double Player::GetShowdownValue(omp::Hand board) const
{
	return strategy->GetShowdownValue();
}

bool Player::operator<(const Player& other)
{
	return seat < other.seat;
}
