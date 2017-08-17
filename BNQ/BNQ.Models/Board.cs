using System;
using System.Collections.Generic;

namespace BNQ.Models
{
    public class Board : IBoard
    {
        ulong cards;
        ulong flop;
        ulong turn;
        ulong river;

        public Board(Card[] flop)
        {
            HashSet<Card> cardCounter = new HashSet<Card>(flop);
            int flopCardCount = 3;

            if (cardCounter.Count != flopCardCount)
            {
                throw new ArgumentException(
                    "Either invalid number of cards or there are repeating cards.");
            }

            if (cardCounter.Contains(Card.None))
            {
                throw new ArgumentException("Cannot pass a None type card.");
            }

            for (int i = 0; i < flop.Length; i++)
            {
                if (!Enum.IsDefined(typeof(Card), flop[i]))
                {
                    throw new ArgumentException("A non-existing card has been passed.");
                }

                this.flop |= (ulong)flop[i];
            }

            this.cards = this.flop;
        }

        public ulong Cards
        {
            get
            {
                return this.cards;
            }
            private set
            {
                this.cards |= value;
            }
        }

        public ulong Flop
        {
            get
            {
                return this.flop;
            }
        }

        public ulong Turn
        {
            get
            {
                return this.turn;
            }
        }

        public ulong River
        {
            get
            {
                return this.river;
            }
        }

        public void AddTurn(Card turn)
        {
            this.turn |= this.ValidateCard(turn);
            this.cards |= this.turn;
        }

        public void AddRiver(Card river)
        {
            this.river |= this.ValidateCard(river);
            this.cards |= this.river;
        }

        private ulong ValidateCard(Card card)
        {
            ulong binaryCard = (ulong)card;
            ulong lastCard = 36028797018963968;
            bool cardExists = (binaryCard & (binaryCard - 1)) == 0;

            if (binaryCard == 0 || binaryCard > lastCard || !cardExists)
            {
                throw new ArgumentException("Invalid card.");
            }

            bool isRepeat = (this.Flop & binaryCard) != 0;

            if (isRepeat)
            {
                throw new ArgumentException("Repeats are not allowed.");
            }

            return binaryCard;
        }
    }
}
