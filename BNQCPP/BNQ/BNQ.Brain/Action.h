#pragma once

#include <iostream>

enum class Action
{
	Bet50,
	Call,
	Check,
	Fold,
	Raise50,
	Waiting
};

inline std::ostream& operator<<(std::ostream& os, const Action& action)
{
	switch (action)
	{
	case Action::Bet50: 
		os << "Bet50";

		break;
	case Action::Call:
		os << "Call";

		break;
	case Action::Check:
		os << "Check";

		break;
	case Action::Fold:
		os << "Fold";

		break;
	case Action::Raise50:
		os << "Raise50";

		break;
	case Action::Waiting:
		os << "Waiting";

		break;
	default:
		break;
	}

	return os;
}
