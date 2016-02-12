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

            var entries = App.MainModel.Highscores.Entries.Where(e => e.TrackName.Equals(App.MainModel.SelectedTrack.Name)).OrderBy(e => e.Time);
            // TODO: Rang ermitteln
            // TODO: Highscores updaten
            // TODO: wegspeichern
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }
    }
}
