#include "ChanceState.h"
#include "HeroStrategy.h"
#include "Node.h"
#include "PlayerState.h"
#include "StateFactory.h"
#include "BalancedStrategy.h"

Node::Node(Node* prev, std::shared_ptr<State> state) :
	prevPtr(prev),
	statePtr(state)
{
}

bool Node::IsLeaf() const
{
	return children.size() == 0;
}

bool Node::IsFinal() const
{
	return statePtr->IsFinalState();
}

bool Node::Visited() const
{
	return visits > 0;
}

double Node::UCB() const
{
	if (visits == 0)
	{
		return DBL_MAX;
	}

	int parentVisits = this->prevPtr->visits;
	double exploitationTerm = value / visits;
	double explorationTerm = std::sqrt(std::log(parentVisits) / visits);
	double UCB = exploitationTerm + (ExplorationConstant * explorationTerm);

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

std::shared_ptr<Node> Node::Expand()
{
	if (!IsLeaf() || children.size() > 0)
	{
		throw std::logic_error("Cannot expand an already expanded node.");
	}

	auto states = StateFactory::CreateStates(statePtr);

	for (auto& state : states)
	{
		auto newNode = std::make_shared<Node>(this, state);

		children.emplace_back(newNode);
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
	double value = (current == nullptr) ? 0.0 : current->statePtr->Value();

	while (current != nullptr)
	{
		current->UpdateVisits();
		current->UpdateValue(value);

		current = current->prevPtr.get();
	}
}

std::vector<std::shared_ptr<Node> >& Node::Children()
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
	return statePtr.get()->NextStateType();
}

static int counter = 0;

int GetDebugState(int size)
{
	switch (counter)
	{
	case 0: return 0;
	case 1: return 1;
	case 2: return 20;
	case 3: return 1;
	case 4: return 0;
	case 5: return 1;
	case 6: return 20;
	case 7: return 1;
	case 8: return 0;
	case 9: return 1;
	default:
		counter = 0;

		return std::rand() % size;
	}
}

void Node::SimulateRecursive(std::shared_ptr<State> statePtr)
{
	State* state = statePtr.get();

	state->SetValue();

	//std::cout << *state << std::endl;

	if (state->IsFinal())
	{
		state->SetValue(true);

		//std::cout << *state << std::endl;

		return;
	}

	auto nextStates = StateFactory::CreateStates(statePtr);
	/*int i = GetDebugState(nextStates.size());
	auto nextState = nextStates[i]; 
	counter++;*/
	auto nextState = nextStates[std::rand() % nextStates.size()];
	
	SimulateRecursive(nextState);
}


