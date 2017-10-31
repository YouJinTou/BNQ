#include "ChanceState.h"
#include "HeroStrategy.h"
#include "Node.h"
#include "OpponentState.h"
#include "StateFactory.h"
#include "VillainStrategy.h"

int Node::TotalVisits = 0;

Node::Node(std::shared_ptr<State> state) :
	state(state)
{
}

bool Node::IsLeaf() const
{
	return children.size() == 0;
}

bool Node::Visited() const
{
	return visits > 0;
}

double Node::UCB() const
{
	double exploitationTerm = value / visits;
	double explorationTerm = std::sqrt(std::log(TotalVisits) / visits);
	double UCB = exploitationTerm + ExplorationConstant * explorationTerm;

	return UCB;
}

double Node::Value() const
{
	return value;
}

void Node::UpdateVisits()
{
	++visits;
}

void Node::UpdateValue(double value)
{
	this->value += value;
}

Node Node::Expand()
{
	if (!IsLeaf() || children.size() > 0)
	{
		throw std::logic_error("Cannot expand an already expanded node.");
	}

	auto states = StateFactory::CreateStates(state);

	for (auto& state : states)
	{
		children.emplace_back(Node(state));
	}

	return children[0];
}

void Node::Simulate()
{
	SimulateRecursive(*state.get());
}

std::vector<Node>& Node::Children()
{
	return children;
}

Node& Node::operator=(const Node& rhs)
{
	state = rhs.state;
	visits = rhs.visits;
	value = rhs.value;
	children = rhs.children;

	return *this;
}

StateType Node::CurrentState() const
{
	return state.get()->Type();
}

StateType Node::NextState() const
{
	return state.get()->NextState();
}

void Node::SimulateRecursive(State& state)
{
	if (state.IsFinal())
	{
		value = state.Value();

		return;
	}

	auto nextState = StateFactory::CreateStates(this->state)[0].get();

	SimulateRecursive(*nextState);
}
