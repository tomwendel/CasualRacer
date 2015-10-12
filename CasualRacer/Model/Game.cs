using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    internal class Game
    {
        public Player Player1 { get; set; }

        public Game()
        {
            Player1 = new Player();
        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            Player1.Update(totalTime, elapsedTime);
        }
    }
}
