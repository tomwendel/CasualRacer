namespace CasualRacer.Model
{
    internal enum TrackTile
    {
        Dirt = 0,
        Sand = 1,
        Gras = 2,

        Road = 8,
        GoalUp = 12,
        GoalDown = 13,
        GoalLeft = 14,
        GoalRight = 15,
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
