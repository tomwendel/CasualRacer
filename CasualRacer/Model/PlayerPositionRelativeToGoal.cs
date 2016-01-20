namespace CasualRacer.Model
{
    /// <summary>
    /// Beschreibt die Position eines <see cref="Player"/> object relativ zur Position des Ziels einer Stecke.
    /// </summary>
    public enum PlayerPositionRelativeToGoal
    {
        /// <summary>
        /// Der Player befindet sich nicht in der unmittelbaren Naehe zum Ziel.
        /// </summary>
        AwayFromGoal,

        /// <summary>
        /// Der Spieler befindet sich direckt vor dem Ziel.
        /// </summary>
        BeforeGoal,

        /// <summary>
        /// Der Spieler befindet sich direckt hinter dem Ziel.
        /// </summary>
        AfterGoal,
    }
}
