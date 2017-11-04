#pragma once

#include <iostream>

namespace Position
{
	enum Position
	{
		None = 0,
		SB,
		BB,
		UTG1,
		UTG2,
		UTG3,
		MP1,
		MP2,
		MP3,
		CO,
		BUT
	};
}

inline std::ostream& operator<<(std::ostream& os, const Position::Position& position)
{
	switch (position)
	{
	case Position::Position::None:
		os << "None";

		break;
	case Position::Position::SB:
		os << "SB";

		break;
	case Position::Position::BB:
		os << "BB";

		break;
	case Position::Position::UTG1:
		os << "UTG1";

		break;
	case Position::Position::UTG2:
		os << "UTG2";

		break;
	case Position::Position::UTG3:
		os << "UTG3";

		break;
	case Position::Position::MP1:
		os << "MP1";

		break;
	case Position::Position::MP2:
		os << "MP2";

		break;
	case Position::Position::MP3:
		os << "MP3";

		break;
	case Position::Position::CO:
		os << "CO";

		break;
	case Position::Position::BUT:
		os << "BUT";

		break;
	default:
		break;
	}

	return os;
}
