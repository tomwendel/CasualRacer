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
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedTrack"));
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedHighscores"));
                    }
                }
            }
        }

        public Highscores Highscores { get; private set; }

        public IEnumerable<Highscore> SelectedHighscores
        {
            get
            {
                if (selectedTrack != null)
                    return Highscores.Entries.Where(e => e.TrackName.Equals(SelectedTrack.Name)).OrderBy(e => e.Time);
                return null;
            }
        }

        public Main()
        {
            Settings = new Settings();
            Highscores = new Highscores() { Entries = new List<Highscore>() };
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

        public void SaveHighscores()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "CasualRacer");
            Directory.CreateDirectory(path);

            using (Stream stream = File.Open(path + "\\highscores.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                serializer.Serialize(stream, Highscores);
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

        public void LoadHighscores()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "CasualRacer");

            if (!Directory.Exists(path))
                return;

            if (!File.Exists(path + "\\highscores.xml"))
                return;

            try
            {
                using (Stream stream = File.Open(path + "\\highscores.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Highscores));
                    Highscores = (Highscores)serializer.Deserialize(stream);
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
