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

	int handNumber = rand() % totalHands + 1;

	if (handNumber >= onePairThresh) 
		return omp::PAIR + lowestRank + rand() % (omp::PAIR + lowestRank + highestRank + 1);
	else if (handNumber >= bluffsThresh) 
		return evaluator.evaluate(boardAsHand);
	else if (handNumber >= twoPairThresh) 
		return omp::TWO_PAIR + lowestRank + rand() % (omp::TWO_PAIR + lowestRank + highestRank + 1);
	else if (handNumber >= tripsThresh) 
		return omp::THREE_OF_A_KIND + lowestRank + rand() % (omp::THREE_OF_A_KIND + lowestRank + highestRank + 1);
	else if (handNumber >= straightThresh) 
		return omp::STRAIGHT;
	else if (handNumber >= flushThresh) 
		return omp::FLUSH;
	else if (handNumber >= boat) 
		return omp::FULL_HOUSE;
	else 
		return omp::FOUR_OF_A_KIND;
}
