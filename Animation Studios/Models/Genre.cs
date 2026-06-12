using System;

namespace Animation_Studios.Models
{
    [Flags]
    public enum Genre
    {
        None = 0,
        Action = 1,
        Comedy = 2,
        Horror = 4,
        Sports = 8,
        Adventure = 16,
        Drama = 32,
        Mystery = 64,
        Fantasy = 128,
        Romance = 256,
        SciFi = 512
    }
}
