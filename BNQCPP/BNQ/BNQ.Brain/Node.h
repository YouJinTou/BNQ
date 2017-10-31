#pragma once

#include <math.h>
#include <memory>
#include <vector>

#include "ActionState.h"
#include "ChoiceState.h"
#include "OpponentState.h"
#include "State.h"

class Node
{
public:
	Node() = default;
	Node(std::shared_ptr<State> state);
	bool IsLeaf() const;
	bool Visited() const;
	double UCB() const;
	double Value() const;
	std::vector<Node>& Children();
	void UpdateVisits();
	void UpdateValue(double value);
	Node Expand();
	void Simulate();
public:
	Node& operator=(const Node& rhs);
private:
	static constexpr double ExplorationConstant = 2.0;
	static int TotalVisits;
private:
	StateType CurrentState() const;
	StateType NextState() const;
	void SimulateRecursive(State& state);
private:
	std::shared_ptr<State> state;
	int visits = 0;
	double value = 0.0;
	std::vector<Node> children;
};