#include <vector>

#include "MCTS.h"
#include "Node.h"
#include "OpponentState.h"
#include "Player.h"
#include "State.h"

int main()
{
	std::vector<Player> players
	{
		Player(1, 100),
		Player(2, 100)
	};
	auto rootState = std::make_shared<OpponentState>(players);
	Node rootNode = Node(rootState);
	MCTS mcts = MCTS(rootNode);
	Action bestAction = mcts.Go();

	return 0;
}