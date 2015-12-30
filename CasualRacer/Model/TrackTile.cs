using System;

namespace CasualRacer.Model
{
    [Flags]
    internal enum TrackTile
    {
        Dirt = 0x0,
        Sand = 0x1,
        Gras = 0x2,

        Road = 0x8,
        GoalUp = 0xC,
        GoalDown = 0xD,
        GoalLeft = 0xE,
        GoalRight = 0xF,
    }

    // 0000
    // rxxx
    // 
    // 0000 - dirt(0)
    // 0001 - sand(1)
    // 0010 - gras(2)
    // 0011 - ?
    // 0100 - ?
    // 0101 - ?
    // 0110 - ?
    // 0111 - ?
    // 
    // 1000 - road(8)
    // 10xx - ?
    // 1100 - goal up(C)
    // 1101 - goal down(D)
    // 1110 - goal left(E)
    // 1111 - goal right(F)
}
