using System;

namespace BNQ.Core.Models
{
    [Flags]
    public enum Card : ulong
    {
        None = (ulong)1 << 53,

        s2 = (ulong)1 << 0,
        h2 = (ulong)1 << 1,
        d2 = (ulong)1 << 2,
        c2 = (ulong)1 << 3,

        s3 = (ulong)1 << 4,
        h3 = (ulong)1 << 5,
        d3 = (ulong)1 << 6,
        c3 = (ulong)1 << 7,

        s4 = (ulong)1 << 8,
        h4 = (ulong)1 << 9,
        d4 = (ulong)1 << 10,
        c4 = (ulong)1 << 11,

        s5 = (ulong)1 << 12,
        h5 = (ulong)1 << 13,
        d5 = (ulong)1 << 14,
        c5 = (ulong)1 << 15,

        s6 = (ulong)1 << 16,
        h6 = (ulong)1 << 17,
        d6 = (ulong)1 << 18,
        c6 = (ulong)1 << 19,

        s7 = (ulong)1 << 20,
        h7 = (ulong)1 << 21,
        d7 = (ulong)1 << 22,
        c7 = (ulong)1 << 23,

        s8 = (ulong)1 << 24,
        h8 = (ulong)1 << 25,
        d8 = (ulong)1 << 26,
        c8 = (ulong)1 << 27,

        s9 = (ulong)1 << 28,
        h9 = (ulong)1 << 29,
        d9 = (ulong)1 << 30,
        c9 = (ulong)1 << 31,

        sT = (ulong)1 << 32,
        hT = (ulong)1 << 33,
        dT = (ulong)1 << 34,
        cT = (ulong)1 << 35,

        sJ = (ulong)1 << 36,
        hJ = (ulong)1 << 37,
        dJ = (ulong)1 << 38,
        cJ = (ulong)1 << 39,

        sQ = (ulong)1 << 40,
        hQ = (ulong)1 << 41,
        dQ = (ulong)1 << 42,
        cQ = (ulong)1 << 43,

        sK = (ulong)1 << 44,
        hK = (ulong)1 << 45,
        dK = (ulong)1 << 46,
        cK = (ulong)1 << 47,

        sA = (ulong)1 << 48,
        hA = (ulong)1 << 49,
        dA = (ulong)1 << 50,
        cA = (ulong)1 << 51
    }
}
