#include "Player.h"

Player::Player(int seat, int stack) :
	Player(seat, stack, false, Card::None)
{
}

Player::Player(int seat, int stack, bool isHero, Card hand)
{
}

bool Player::IsHero() const
{
	return isHero;
}
