using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    /// <summary>
    /// Klasse mit Erweiterungsmethoden fuer die <see cref="TrackTile"/>-Klasse.
    /// </summary>
    internal static class TrackTileExtensions
    {
        /// <summary>
        /// Prueft ob der <see cref="TrackTile"/> ein Ziel-Type ist.
        /// </summary>
        /// <param name="tile">Der <see cref="TrackTile"/> der geprueft werden soll.</param>
        /// <returns><c>true</c>, wenn der <see cref="TrackTile"/> ein Ziel-Type ist, andernfalls <c>false</c>.</returns>
        internal static bool IsGoalTile(this TrackTile tile)
        {
            return ((int)tile & 12) == 12; // siehe TrackTile.cs
        }
    }
}
