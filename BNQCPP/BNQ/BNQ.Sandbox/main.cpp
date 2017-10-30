#include <algorithm>
#include <vector>

#include "Card.h"
#include "MCTS.h"
#include "Node.h"
#include "OpponentState.h"
#include "Player.h"
#include "Position.h"
#include "State.h"

int main()
{
	std::vector<Player> players
	{
		Player(Position::MP1, 100),
		Player(Position::BB, 100),
		Player(Position::BUT, 100),
		Player(Position::CO, 100, true, Card::c2 | Card::cJ),
	};

	std::sort(players.begin(), players.end());

	auto rootState = std::make_shared<OpponentState>(players);
	Node rootNode = Node(rootState);
	MCTS mcts = MCTS(rootNode);
	Action bestAction = mcts.Go();

	return 0;
}