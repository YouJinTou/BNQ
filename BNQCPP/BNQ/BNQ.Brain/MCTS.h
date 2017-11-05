#pragma once

#include <memory>

#include "Action.h"
#include "Node.h"

class MCTS
{
public:
	MCTS(Node root, double allowance);
	Action Go();
private:
	Node root;
	Node* current;
	const double allowance;
private:
	void Select();
	void Expand();
	void Simulate();
	void Backpropagate();
};