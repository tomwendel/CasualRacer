using System;

namespace CasualRacer.Model
{
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Benutzername des Spielers
        /// </summary>
        public string Username { get; set; }

        public Settings()
        {
            Username = "Username";
        }
    }
}
