using System.Windows;
using System.Windows.Controls;

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

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
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
