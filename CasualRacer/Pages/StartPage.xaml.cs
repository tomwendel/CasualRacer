using CasualRacer.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace CasualRacer.Pages
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        public StartPage(bool animateLogo)
        {
            InitializeComponent();

            if (animateLogo)
            {
                ((Storyboard)Resources["LoadLogo"]).Begin();
            }
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            Track track = App.MainModel.Tracks[0];
            App.MainModel.NewGame(track);
            NavigationService.Navigate(new GamePage());
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OptionPage());
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreditsPage());
        }
    }
}
