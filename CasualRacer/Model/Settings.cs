using System;

namespace CasualRacer.Model
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Benutzername des Spielers
        /// </summary>
        public string RacerName { get; set; }

        public Settings()
        {
            RacerName = "Racer 1";
        }
    }
}
