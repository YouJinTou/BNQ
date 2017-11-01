#include <ctime>
#include <float.h>

#include "MCTS.h"

MCTS::MCTS(const Node& root) :
	root(root),
	current(root)
{
	std::srand(std::time(NULL));
}

Action MCTS::Go()
{
	while (true)
	{
		current = root;

		Select();

		if (current.Visited())
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
	if (current.IsLeaf())
	{
		return;
	}

	double currentBest = DBL_MIN;
	Node bestNode;

	for (const Node& child : current.Children())
	{
		if (currentBest < child.Value())
		{
			currentBest = child.Value();
			bestNode = child;
		}
	}

	current = bestNode;
}

void MCTS::Expand()
{
	current = current.Expand();
}

void MCTS::Simulate()
{
	current.Simulate();
}

void MCTS::Backpropagate()
{
	current.Backpropagate();
}
