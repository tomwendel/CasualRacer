using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    internal class Game
    {
        public Track Track { get; private set; }

        public Player Player1 { get; private set; }

        public Game()
        {
            Track = new Track(30, 15);
            Track.Tiles[10, 10] = TrackTile.Road;
            Track.Tiles[10, 9] = TrackTile.Gras;
            Track.Tiles[10, 8] = TrackTile.Sand;

            Player1 = new Player();

        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            Player1.Update(totalTime, elapsedTime);
        }
    }
}
