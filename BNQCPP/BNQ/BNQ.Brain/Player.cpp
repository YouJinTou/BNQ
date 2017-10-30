#include "Player.h"

Player::Player(int seat, int stack) :
	Player(seat, stack, false, Card::None)
{
}

Player::Player(int seat, int stack, bool isHero, Hand hand) :
	seat(seat),
	stack(stack),
	isHero(isHero),
	hand(hand)
{
}

bool Player::IsHero() const
{
	return isHero;
}
