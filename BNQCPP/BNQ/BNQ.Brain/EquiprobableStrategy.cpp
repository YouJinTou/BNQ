#include <time.h>
#include <vector>

#include "Card.h"
#include "Constants.h"
#include "EquiprobableStrategy.h"
#include "State.h"

double EquiprobableStrategy::GetShowdownValue(State* statePtr) const
{
	int minCardPow = Constants::PowerTwoIndices[Card::s2];
	int maxCardPow = Constants::PowerTwoIndices[Card::dA];
	auto heroHolding = statePtr->Hero().GetHolding();
	auto board = statePtr->GetBoard()->BoardCards();
	std::vector<int> deadCards;
	uint64_t mask = 1;

	for (int c = minCardPow; c <= maxCardPow; c++)
	{
		auto card = (Card::Card)(mask << c);

		if (((card & heroHolding.Card1) != 0) || ((card & heroHolding.Card2) != 0) || ((card & board) != 0))
		{
			deadCards.push_back(c);
		}
	}

	std::vector<int> possibleVillainHands;

	for (size_t c = 0; c < 52; c++)
	{
		if (std::find(deadCards.begin(), deadCards.end(), c) != deadCards.end())
		{
			continue;
		}

		possibleVillainHands.push_back(c);
	}

	srand((unsigned int)time(NULL));
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<int> hands(0, possibleVillainHands.size() - 1);
	int firstChoice = possibleVillainHands[hands(gen)];
	int secondChoice = 0;

	do 
	{
		secondChoice = possibleVillainHands[hands(gen)];
	} while (secondChoice == firstChoice);

	auto hand = omp::Hand(firstChoice) + omp::Hand(secondChoice);
	auto boardAsHand = statePtr->GetBoard()->GetBoardAsHand();
	boardAsHand += hand;

	return evaluator.evaluate(boardAsHand);
}
