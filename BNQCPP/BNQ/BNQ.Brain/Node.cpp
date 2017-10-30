#include "HeroStrategy.h"
#include "Node.h"
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

	StateType stateType = state->Type();

	switch (stateType)
	{
	case StateType::Chance:

		break;
	case StateType::Choice:
		break;
	case StateType::Final:
		break;
	case StateType::Opponent:
	case StateType::HeroAction:
	{
		ChoiceState* choiceState = (ChoiceState*)NextState();
		auto strategy = stateType == StateType::HeroAction ? 
			std::unique_ptr<PlayerStrategy>{ std::make_unique<HeroStrategy>() } :
			std::unique_ptr<PlayerStrategy>{ std::make_unique<VillainStrategy>() };

		for (Action action : choiceState->Actions())
		{
			auto actionState = std::make_shared<ActionState>(state, strategy.get());
			Node child{ actionState };

			children.emplace_back(child);
		}

		return children[0];
	}
	}
}

void Node::Simulate()
{
	SimulateRecursive(*CurrentState());
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

State* Node::CurrentState() const
{
	return dynamic_cast<State*>(state.get());
}

State* Node::NextState() const
{
	return CurrentState()->NextState().get();
}

void Node::SimulateRecursive(State& state)
{
	if (state.IsFinal())
	{
		value = state.Value();

		return;
	}

	SimulateRecursive(*state.NextState());
}
