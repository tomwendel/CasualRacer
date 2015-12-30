namespace CasualRacer
{
    internal enum TileType
    {
        /// <summary>
        /// Zentrale Kachel
        /// </summary>
        Center,

        /// <summary>
        /// Linker Rand
        /// </summary>
        Left,

        /// <summary>
        /// Rechter Rand
        /// </summary>
        Right,

        /// <summary>
        /// Oberer Rand
        /// </summary>
        Upper,

        /// <summary>
        /// Unterer Rand
        /// </summary>
        Lower,

        /// <summary>
        /// Linke, untere Ecke
        /// </summary>
        LowerLeftConvex,

        /// <summary>
        /// Rechte, untere Ecke
        /// </summary>
        LowerRightConvex,

        /// <summary>
        /// Linke, obere Ecke
        /// </summary>
        UpperLeftConvex,

        /// <summary>
        /// Rechte, obere Ecke
        /// </summary>
        UpperRightConvex,

        /// <summary>
        /// Linke, untere Einbuchtung
        /// </summary>
        LowerLeftConcave,

        /// <summary>
        /// Rechte, untere Einbuchtung
        /// </summary>
        LowerRightConcave,

        /// <summary>
        /// Linke, obere Einbuchtung
        /// </summary>
        UpperLeftConcave,

        /// <summary>
        /// Rechte, obere Einbuchtung
        /// </summary>
        UpperRightConcave,
    }
}
