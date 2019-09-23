using BNQ.Core.Models;
using System;

namespace BNQ.Core.Extensions
{
    public static class StreetExtensions
    {
        public static Street GetNextStreet(this Street street)
        {
            switch (street)
            {
                case Street.Flop:
                    return Street.Turn;
                case Street.Turn:
                    return Street.River;
                case Street.River:
                    return Street.Showdown;
                default:
                    throw new InvalidOperationException(street.ToString());
            }
        }

        public static bool ShouldShowdown(this Street street)
        {
            return street.GetNextStreet() == Street.Showdown;
        }

        public static bool IsFinalStreet(this Street street)
        {
            return street == Street.Fold || street == Street.Showdown;
        }
    }
}
