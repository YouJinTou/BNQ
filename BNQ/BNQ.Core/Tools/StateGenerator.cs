using BNQ.Core.Extensions;
using BNQ.Core.Factories;
using BNQ.Core.Models;
using BNQ.Core.Search;
using System;

namespace BNQ.Core.Tools
{
    public static class StateGenerator
    {
        public static GameState GenerateState()
        {
            var street = GenerateStreet();
            var hero = GeneratePlayer(isHero: true, isToPlay: true);
            var villain = GeneratePlayer(
                deadCards: hero.HoleCards,
                deadPosition: hero.Position,
                isToPlay: false,
                isHero: false);
            var board = BoardGenerator.GenerateFlop(hero.HoleCards | villain.HoleCards);
            board |= BoardGenerator.FillFromTo(board, Street.Flop, street);
            var action = GenerateAction();
            var pot = action.IsPassive() ? 1.5d : 2.25d;
            var wagerToCall = action.IsPassive() ? 0.0d : 0.75d;

            return StateFactory.CreateStartState(
                board, street, hero, villain, pot, wagerToCall, action);
        }

        public static Street GenerateStreet()
        {
            var random = new Random();
            var street = random.Next((int)Street.Flop, (int)Street.Showdown);

            return (Street)street;
        }

        public static Player GeneratePlayer(
            Card deadCards = Card.None,
            Position deadPosition = Position.None,
            double stack = -1d,
            bool? isToPlay = null,
            bool? isHero = null)
        {
            var random = new Random();
            var cards = BoardGenerator.GenerateHand(deadCards);
            var position = GeneratePosition(deadPosition);
            stack = (stack == -1d) ? GenerateStack() : stack;
            isToPlay = isToPlay.HasValue ? isToPlay : Convert.ToBoolean(random.Next(0, 2));
            isHero = isHero.HasValue ? isHero : Convert.ToBoolean(random.Next(0, 2));
            var player = new Player(cards, position, stack, isToPlay.Value, isHero.Value);

            return player;
        }

        public static Position GeneratePosition(Position deadPosition = Position.None)
        {
            if (deadPosition != Position.None)
            {
                return deadPosition == Position.SB ? Position.BB : Position.SB;
            }

            var random = new Random();
            var position = random.Next((int)Position.SB, (int)Position.BB + 1);

            return (Position)position;
        }

        public static double GenerateStack()
        {
            var random = new Random();
            var stack = random.Next(1, 100);

            return stack;
        }

        public static Models.Action GenerateAction()
        {
            var random = new Random();
            var actions = new Models.Action[]
            {
                Models.Action.Bet50,
                Models.Action.Check
            };

            return actions[random.Next(0, actions.Length)];
        }
    }
}
