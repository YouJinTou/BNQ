#include <ctime>
#include <float.h>

#include "MCTS.h"

MCTS::MCTS(Node root, double allowance) :
	root(root),
	current(&root),
	allowance(allowance)
{
}

Action MCTS::Go()
{
	clock_t beginTime = clock();
	double elapsedSecs = 0.0;

	while (true)
	{
		current = &root;

		Select();

		if (current->Visited() && !current->IsFinal())
		{
			Expand();
		}

		Simulate();

		Backpropagate();

		//system("cls");

		clock_t endTime = clock();
		elapsedSecs = double(endTime - beginTime) / CLOCKS_PER_SEC;
	}

	return Action();
}

void MCTS::Select()
{
	if (current->IsLeaf())
	{
		return;
	}

	double currentBest = -DBL_MAX;
	std::shared_ptr<Node>bestNode;

	for (auto child : current->Children())
	{
		auto childUCB = child->UCB();

		if (childUCB > currentBest)
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
