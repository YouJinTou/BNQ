using BNQ.Core.Models;
using System.Collections.Generic;

namespace BNQ.Core.Abstractions
{
    public interface IBoardGenerator
    {
        Card GenerateCard(Card deadCards = Card.None);

        Card GenerateHand(Card deadCards = Card.None);

        Card GenerateFlop(Card deadCards = Card.None);

        IEnumerable<Card> GenerateNextBoards(Card currentBoard, Card deadCards);

        Card FillFromTo(Card board, Street from, Street to, Card deadCards = Card.None);

        Card FillToRiver(Card board, Street street, Card deadCards = Card.None);
    }
}
