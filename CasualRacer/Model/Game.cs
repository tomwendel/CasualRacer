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
            Track = Track.Load("./Tracks/Track1.txt");

            Player1 = new Player();

        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            // Lenkung
            if (Player1.WheelLeft)
                Player1.Direction -= (float)elapsedTime.TotalSeconds * 100;
            if (Player1.WheelRight)
                Player1.Direction += (float)elapsedTime.TotalSeconds * 100;

            // Beschleunigung & Bremse

        }
    }
}
