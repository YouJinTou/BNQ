#include "Player.h"

Player::Player(Position::Position seat, double stack) :
	Player(seat, stack, false, Card::None)
{
}

Player::Player(Position::Position seat, double stack, bool isHero, Hand hand) :
	seat(seat),
	stack(stack),
	isHero(isHero),
	hand(hand)
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

Hand Player::GetHand() const
{
	return hand;
}

bool Player::operator<(const Player& other)
{
	return seat < other.seat;
}
