#pragma once

#include <math.h>
#include <memory>
#include <vector>

#include "PlayerState.h"
#include "State.h"

class Node
{
public:
	Node() = default;
	Node(Node* prev, std::shared_ptr<State> state);
	bool IsLeaf() const;
	bool IsFinal() const;
	bool Visited() const;
	double UCB() const;
	double Value() const;
	std::vector<std::shared_ptr<Node> >& Children();
	void UpdateVisits();
	void UpdateValue(double value);
	std::shared_ptr<Node> Expand();
	void Simulate();
	void Backpropagate();
public:
	Node& operator=(const Node& rhs);
private:
	static constexpr double ExplorationConstant = 2.0;
	static int IterationsCount;
private:
	StateType::StateType CurrentState() const;
	StateType::StateType NextState() const;
	void SimulateRecursive(std::shared_ptr<State> statePtr);
private:
	std::shared_ptr<Node> prevPtr;
	std::shared_ptr<State> statePtr;
	int visits = 0;
	double value = 0.0;
	std::vector<std::shared_ptr<Node> > children;
};