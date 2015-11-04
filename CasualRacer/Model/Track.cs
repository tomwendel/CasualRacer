using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    internal class Track
    {
        public TrackTile[,] Tiles { get; private set; }

        public Track(int width, int height)
        {
            Tiles = new TrackTile[width, height];
        }
    }

    internal enum TrackTile
    {
        Dirt,
        Sand,
        Gras,
        Road,
    }
}
