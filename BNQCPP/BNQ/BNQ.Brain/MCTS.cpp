#include <float.h>

#include "MCTS.h"

MCTS::MCTS(const Node& root) :
	root(root),
	current(root)
{
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

	double currentBest = -DBL_MAX;
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
}
