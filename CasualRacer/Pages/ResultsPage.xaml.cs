using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace CasualRacer.Pages
{
    /// <summary>
    /// Interaction logic for ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : Page
    {
        public ResultsPage()
        {
            InitializeComponent();
            DataContext = App.MainModel;

            TimeSpan time = App.MainModel.Game.Player1.LapTimes.OrderBy(t => t).FirstOrDefault();
            if (time != TimeSpan.Zero)
            {
                // Neuer Eintrag
                App.MainModel.Highscores.Entries.Add(new Model.Highscore()
                {
                    CreateDate = DateTime.Now,
                    TrackName = App.MainModel.SelectedTrack.Key,
                    Time = time,
                    RacerName = App.MainModel.Settings.RacerName
                });

                // Entferne alle Einträge größer 10
                var entries = App.MainModel.Highscores.Entries.Where(e => e.TrackName.Equals(App.MainModel.SelectedTrack.Key)).OrderBy(e => e.Time).Skip(10);
                foreach (var item in entries.ToArray())
                    App.MainModel.Highscores.Entries.Remove(item);

                // Speichern
                App.MainModel.SaveHighscores();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }
    }
}
