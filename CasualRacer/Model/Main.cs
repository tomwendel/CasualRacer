using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    internal class Main
    {
        public Game Game { get; private set; }

        public Settings Settings { get; private set; }

        public Highscores Highscores { get; private set; }

        public Main()
        {

        }

        public void NewGame()
        {
            Game = new Game();
        }
    }
}
