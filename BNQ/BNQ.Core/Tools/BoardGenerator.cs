using BNQ.Core.Models;
using System;
using System.Collections.Generic;

namespace BNQ.Core.Tools
{
    public static class BoardGenerator
    {
        public static Card GenerateCard(Card deadCards = Card.None)
        {
            return GenerateCards(1, deadCards);
        }

        public static Card GenerateHand(Card deadCards = Card.None)
        {
            return GenerateCards(2, deadCards);
        }

        public static Card GenerateFlop(Card deadCards = Card.None)
        {
            return GenerateCards(3, deadCards);
        }

        public static IEnumerable<Card> GenerateNextBoards(Card currentBoard, Card deadCards)
        {
            var nextBoards = new List<Card>();
            var existingCards = (long)(currentBoard | deadCards);

            for (int c = Constants.CardMinBitIndex; c <= Constants.CardMaxBitIndex; c++)
            {
                var mask = (Constants.LOne << c);

                if ((mask & existingCards) == 0)
                {
                    var nextBoard = (Card)(mask | (long)currentBoard);

                    nextBoards.Add(nextBoard);
                }
            }

            return nextBoards;
        }

        public static Card FillFromTo(Card board, Street from, Street to, Card deadCards = Card.None)
        {
            switch (from)
            {
                case Street.Flop:
                    switch (to)
                    {
                        case Street.Flop:
                            return board;
                        case Street.Turn:
                            return board | GenerateCard(board | deadCards);
                        case Street.River:
                            return FillToRiver(board, from, deadCards);
                        default:
                            throw new InvalidOperationException();
                    }
                case Street.Turn:
                    switch (to)
                    {
                        case Street.Flop:
                            throw new InvalidOperationException();
                        case Street.Turn:
                            return board;
                        case Street.River:
                            return FillToRiver(board, from, deadCards);
                        default:
                            throw new InvalidOperationException();
                    }
                case Street.River:
                    switch (to)
                    {
                        case Street.Flop:
                            throw new InvalidOperationException();
                        case Street.Turn:
                            throw new InvalidOperationException();
                        case Street.River:
                            return board;
                        default:
                            throw new InvalidOperationException();
                    }
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Card FillToRiver(Card board, Street street, Card deadCards = Card.None)
        {
            switch (street)
            {
                case Street.Flop:
                    return board | GenerateCards(2, deadCards | board);
                case Street.Turn:
                    return board | GenerateCards(1, deadCards | board);
                case Street.River:
                case Street.Showdown:
                case Street.Fold:
                    return board;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Card GenerateCards(int count, Card deadCards)
        {
            var firstIndex = Constants.CardMinBitIndex;
            var lastIndex = Constants.CardMaxBitIndex + 1;
            var random = new Random();
            Card dirtyHand = Card.None;

            for (int c = 1; c <= count; c++)
            {
                var randomIndex = random.Next(firstIndex, lastIndex);
                var nextCard = (Card)(Constants.LOne << randomIndex);
                var shouldDrawAgain = ((nextCard & deadCards) > 0);

                while (shouldDrawAgain)
                {
                    randomIndex = random.Next(firstIndex, lastIndex);
                    nextCard = (Card)(Constants.LOne << randomIndex);
                    shouldDrawAgain = ((nextCard & deadCards) > 0);
                }

                deadCards |= nextCard;
                dirtyHand |= nextCard;
            }

            var cleanHand = dirtyHand & ~Card.None;

            return cleanHand;
        }
    }
}
