#include <algorithm>
#include <memory>
#include <vector>

#include "Card.h"
#include "HeroStrategy.h"
#include "MCTS.h"
#include "Node.h"
#include "Player.h"
#include "PlayerState.h"
#include "Position.h"
#include "State.h"
#include "VillainStrategy.h"

int main()
{
	VillainStrategy vStrategy;
	HeroStrategy hStrategy;
	std::vector<Player> players
	{
		Player(Position::MP1, 100, &vStrategy),
		Player(Position::BB, 100, &vStrategy),
		Player(Position::BUT, 100, &vStrategy),
		Player(Position::CO, 100, true, Card::s3 | Card::cJ, &hStrategy),
	};
	Board board = Board(Card::c2, Card::hJ, Card::dA);
	std::sort(players.begin(), players.end());

	auto rootState = std::make_shared<PlayerState>(
		nullptr,
		StateType::PlayerAction,
		players,
		std::make_shared<Board>(board),
		3.0,
		Position::Position::BB,
		Position::Position::None,
		Street::Flop,
		0.0,
		0.0);
	auto rootNode = Node(nullptr, rootState);
	MCTS mcts = MCTS(rootNode);
	Action bestAction = mcts.Go();

	return 0;
}