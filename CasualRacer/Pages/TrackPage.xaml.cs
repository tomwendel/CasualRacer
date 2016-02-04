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

namespace CasualRacer.Pages
{
    /// <summary>
    /// Interaction logic for TrackPage.xaml
    /// </summary>
    public partial class TrackPage : Page
    {
        public TrackPage()
        {
            InitializeComponent();
            DataContext = App.MainModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {            
            if (App.MainModel.SelectedTrack != null)
            {
                App.MainModel.NewGame(App.MainModel.SelectedTrack);
                NavigationService.Navigate(new GamePage());
            }
        }
    }
}
