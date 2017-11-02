#include <ctime>
#include <float.h>

#include "MCTS.h"

MCTS::MCTS(Node root) :
	root(root),
	current(&root)
{
	std::srand(std::time(NULL));
}

Action MCTS::Go()
{
	while (true)
	{
		current = &root;

		Select();

		if (current->Visited())
		{
			Expand();
		}

		Simulate();

		Backpropagate();
	}

	return Action();
}

void MCTS::Select()
{
	if (current->IsLeaf())
	{
		return;
	}

	double currentBest = DBL_MIN;
	std::shared_ptr<Node>bestNode;

	for (auto child : current->Children())
	{
		auto childUCB = child->UCB();

		if (currentBest < childUCB)
		{
			currentBest = childUCB;
			bestNode = child;
		}
	}

	current = bestNode.get();

	Select();
}

void MCTS::Expand()
{
	auto firstChild = current->Expand();
	current = firstChild.get();
}

void MCTS::Simulate()
{
	current->Simulate();
}

void MCTS::Backpropagate()
{
	current->Backpropagate();
}
