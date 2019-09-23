using BNQ.Core;
using BNQ.Core.Models;
using BNQ.Core.Tools;
using NUnit.Framework;

namespace Tests.Tools
{
    public class BoardGeneratorTests
    {
        [Test]
        public void GenerateCard_GeneratesOneCard()
        {
            for (int i = 0; i < 1000; i++)
            {
                var hand = BoardGenerator.GenerateCard().ToString();

                Assert.True(hand.Split(',').Length == 1, $"{hand}");
            }
        }

        [Test]
        public void GenerateCard_NeverGeneratesADeadCard()
        {
            for (int i = Constants.CardMinBitIndex; i <= Constants.CardMaxBitIndex; i++)
            {
                var deadCard = (Card)(Constants.LOne << i);

                for (int j = 0; j < 300; j++)
                {
                    var generatedCard = BoardGenerator.GenerateCard(deadCard);

                    Assert.True((deadCard & generatedCard) == 0, $"{i}:{generatedCard}");
                }
            }
        }

        [Test]
        public void GenerateHand_GeneratesTwoCards()
        {
            for (int i = 0; i < 1000; i++)
            {
                var hand = BoardGenerator.GenerateHand().ToString();

                Assert.True(hand.Split(',').Length == 2, $"{hand}");
            }
        }

        [Test]
        public void GenerateHand_NeverGeneratesADeadCard()
        {
            for (int i = Constants.CardMinBitIndex; i <= Constants.CardMaxBitIndex; i++)
            {
                var deadCard = (Card)(Constants.LOne << i);

                for (int j = 0; j < 300; j++)
                {
                    var generatedCard = BoardGenerator.GenerateHand(deadCard);

                    Assert.True((deadCard & generatedCard) == 0, $"{i}:{generatedCard}");
                }
            }
        }

        [Test]
        public void GenerateFlop_GeneratesThreeCards()
        {
            for (int i = 0; i < 1000; i++)
            {
                var hand = BoardGenerator.GenerateFlop().ToString();

                Assert.True(hand.Split(',').Length == 3, $"{hand}");
            }
        }

        [Test]
        public void GenerateFlop_NeverGeneratesADeadCard()
        {
            for (int i = Constants.CardMinBitIndex; i <= Constants.CardMaxBitIndex; i++)
            {
                var deadCard = (Card)(Constants.LOne << i);

                for (int j = 0; j < 300; j++)
                {
                    var generatedCard = BoardGenerator.GenerateFlop(deadCard);

                    Assert.True((deadCard & generatedCard) == 0, $"{i}:{generatedCard}");
                }
            }
        }

        [Test]
        public void GenerateNextBoards_UseFlop_GenerateFourCards()
        {
            for (int i = 0; i < 100; i++)
            {
                var flop = BoardGenerator.GenerateFlop();

                for (int j = 0; j < 100; j++)
                {
                    var deadCards = BoardGenerator.GenerateHand(flop);
                    var nextBoards = BoardGenerator.GenerateNextBoards(flop, deadCards);

                    foreach (var board in nextBoards)
                    {
                        Assert.True(
                            board.ToString().Split(',').Length == 4, $"{deadCards}:{board}");
                    }
                }
            }
        }

        [Test]
        public void GenerateNextBoards_UseFlop_NeverGeneratesADeadCard()
        {
            for (int i = 0; i < 100; i++)
            {
                var flop = BoardGenerator.GenerateFlop();

                for (int j = 0; j < 100; j++)
                {
                    var deadCards = BoardGenerator.GenerateHand(flop);
                    var nextBoards = BoardGenerator.GenerateNextBoards(flop, deadCards);

                    foreach (var board in nextBoards)
                    {
                        Assert.True((deadCards & board) == 0, $"{deadCards}:{board}");
                    }
                }
            }
        }
    }
}