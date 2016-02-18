using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualRacer.Model
{
    [Serializable]
    public class Highscores
    {
        public List<Highscore> Entries { get; set; }
    }

    [Serializable]
    public class Highscore
    {
        public DateTime CreateDate { get; set; }

        public string TrackName { get; set; }

        public TimeSpan Time { get; set; }

        public string RacerName { get; set; }
    }
}
