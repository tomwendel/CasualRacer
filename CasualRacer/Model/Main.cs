using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CasualRacer.Model
{
    internal class Main
    {
        public Game Game { get; private set; }

        public Settings Settings { get; private set; }

        public Highscores Highscores { get; private set; }

        public Main()
        {
            Settings = new Settings();
            Settings.Username = "Anderer Tom";
        }

        public void NewGame()
        {
            Game = new Game();
        }

        public void SaveSettings()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "CasualRacer");
            Directory.CreateDirectory(path);

            using (Stream stream = File.Open(path + "\\settings.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(stream, Settings);
            }

        }

        public void LoadSettings()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "CasualRacer");

            if (!Directory.Exists(path))
                return;

            if (!File.Exists(path + "\\settings.xml"))
                return;

            try
            {
                using (Stream stream = File.Open(path + "\\settings.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings = (Settings)serializer.Deserialize(stream);
                }
            }
            catch (Exception) { }
        }
    }
}
