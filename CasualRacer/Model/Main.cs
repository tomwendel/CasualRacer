using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CasualRacer.Model
{
    internal class Main : INotifyPropertyChanged
    {
        private Track selectedTrack;

        public Game Game { get; private set; }

        public Settings Settings { get; private set; }

        public List<Track> Tracks { get; private set; }



        public Track SelectedTrack {
            get { return selectedTrack; }
            set
            {
                if (selectedTrack != value)
                {
                    selectedTrack = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedTrack"));
                }
            }
        }

        public Highscores Highscores { get; private set; }

        public Main()
        {
            Settings = new Settings();
            Tracks = new List<Track>();
        }

        public void NewGame(Track track)
        {
            Game = new Game(track);
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

        public void LoadTracks()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Tracks");
            string[] files = Directory.GetFiles(path, "*.txt");
            foreach (var file in files)
            {
                Tracks.Add(Track.LoadFromTxt(file));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
