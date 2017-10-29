#pragma once

#include "Action.h"
#include "Node.h"

class MCTS
{
public:
	MCTS(const Node& root);
	Action Go();
private:
	Node root;
	Node current;
private:
	void Select();
	void Expand();
	void Simulate();
	void Backpropagate();
};