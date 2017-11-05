#include <time.h>

#include "HandEvaluator\Hand.h"

#include "State.h"
#include "BalancedStrategy.h"

void BalancedStrategy::UpdateRange(State& state)
{
	lastState = &state;
}

double BalancedStrategy::GetShowdownValue() const
{
	auto board = lastState->GetBoard().get();
	auto boardAsHand = board->GetBoardAsHand();
	bool quadsPossible = evaluator.quadsPossible(boardAsHand);
	bool flushPossible = evaluator.flushPossible(boardAsHand, *board);
	bool straightPossible = evaluator.straightPossible(boardAsHand, *board);
	const int multiplier = 10000;
	int quads = (int)(quadsPossible ? multiplier * 0.0024 : 0.0);
	int boat = (int)(quadsPossible ? multiplier * 0.0144 : 0.0);
	int flush = (int)(flushPossible ? multiplier * 0.0196 : 0.0);
	int straight = (int)(straightPossible ? multiplier * 0.0392 : 0.0);
	int trips = (int)(multiplier * 0.0211);
	int twoPair = (int)(multiplier * 0.0475);
	int bluffs = (int)(multiplier * 0.3);
	int onePair = (int)(multiplier * 0.4225);
	int onePairThresh = quads + boat + flush + straight + trips + twoPair + bluffs;
	int bluffsThresh = quads + boat + flush + straight + trips + twoPair;
	int twoPairThresh = quads + boat + flush + straight + trips;
	int tripsThresh = quads + boat + flush + straight;
	int straightThresh = quads + boat + flush;
	int flushThresh = quads + boat;
	int totalHands = quads + boat + flush + straight + trips + twoPair + onePair + bluffs;
	int lowestRank = omp::lowestCardRank(board->BoardCards());
	int highestRank = omp::highestCardRank(board->BoardCards());

	srand((unsigned int)time(NULL));
	std::random_device rd;
	std::mt19937 gen(rd());
	std::uniform_int_distribution<int> totalDis(0, totalHands);
	std::uniform_int_distribution<int> rankDis(lowestRank, highestRank);

	int handNumber = totalDis(gen);
	int rank = rankDis(gen);
	double bluff = evaluator.evaluate(boardAsHand);

	if (handNumber >= onePairThresh)
		return omp::PAIR + rank;
	else if (handNumber >= bluffsThresh)
		return bluff;
	else if (handNumber >= twoPairThresh) 
		return omp::TWO_PAIR + rank;
	else if (handNumber >= tripsThresh) 
		return omp::THREE_OF_A_KIND + rank;
	else if (handNumber >= straightThresh) 
		return straightPossible ? omp::STRAIGHT : bluff;
	else if (handNumber >= flushThresh) 
		return flushPossible ? omp::FLUSH : bluff;
	else if (handNumber >= boat) 
		return quadsPossible ? omp::FULL_HOUSE : bluff;
	else 
		return omp::FOUR_OF_A_KIND;
}
