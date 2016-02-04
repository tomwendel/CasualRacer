using System.Windows;
using System.Windows.Controls;

namespace CasualRacer.Pages
{
    /// <summary>
    /// Interaction logic for OptionPage.xaml
    /// </summary>
    public partial class OptionPage : Page
    {
        public OptionPage()
        {
            InitializeComponent();
            DataContext = App.MainModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
