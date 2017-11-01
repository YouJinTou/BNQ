#include "ChanceState.h"
#include "HeroStrategy.h"
#include "Node.h"
#include "PlayerState.h"
#include "StateFactory.h"
#include "VillainStrategy.h"

int Node::TotalVisits = 0;

Node::Node(std::shared_ptr<Node> prev, std::shared_ptr<State> state) :
	prev(prev),
	statePtr(state)
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

	auto states = StateFactory::CreateStates(statePtr);

	for (auto& state : states)
	{
		children.emplace_back(Node(std::make_shared<Node>(*this), state));
	}

	return children[0];
}

void Node::Simulate()
{
	SimulateRecursive(statePtr);
}

void Node::Backpropagate()
{
	Node* current = this;

	while (current != nullptr)
	{
		current->UpdateVisits();
		current->UpdateValue(current->statePtr->Value());

		current = current->prev.get();
	}
}

std::vector<Node>& Node::Children()
{
	return children;
}

Node& Node::operator=(const Node& rhs)
{
	statePtr = rhs.statePtr;
	visits = rhs.visits;
	value = rhs.value;
	children = rhs.children;

	return *this;
}

StateType::StateType Node::CurrentState() const
{
	return statePtr.get()->Type();
}

StateType::StateType Node::NextState() const
{
	return statePtr.get()->NextState();
}

void Node::SimulateRecursive(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();

	if (state->IsFinal())
	{
		state->SetValue();

		this->value = state->Value();

		return;
	}

	auto nextStates = StateFactory::CreateStates(statePtr);
	auto nextState = nextStates[std::rand() % nextStates.size()];

	SimulateRecursive(nextState);
}
