using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using CasualRacer.Pages;
using System.Threading.Tasks;

namespace CasualRacer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool inNavigation = false;
        private NavigatingCancelEventArgs navArgs = null;

        public MainWindow()
        {
            InitializeComponent();

            NavigationFrame.Navigate(new StartPage(true));

            DataContext = App.MainModel;
        }

        private void NavigationFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (inNavigation)
            {
                return;
            }

            e.Cancel = true;
            navArgs = e;

            var animation = new DoubleAnimation
            {
                From = 1f,
                To = 0f,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            animation.Completed += Animation_Completed;
            NavigationFrame.BeginAnimation(OpacityProperty, animation);

            inNavigation = true;
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            switch (navArgs.NavigationMode)
            {
                case NavigationMode.New:
                    if (navArgs.Uri == null)
                        NavigationFrame.Navigate(navArgs.Content);
                    else
                        NavigationFrame.Navigate(navArgs.Uri);
                    break;
                case NavigationMode.Back:
                    NavigationFrame.GoBack();
                    break;
                case NavigationMode.Forward:
                    NavigationFrame.GoForward();
                    break;
                case NavigationMode.Refresh:
                    NavigationFrame.Refresh();
                    break;
            }
            inNavigation = false;

            var animation = new DoubleAnimation
            {
                From = 0f,
                To = 1f,
                Duration = new Duration(TimeSpan.FromMilliseconds(200))
            };
            NavigationFrame.BeginAnimation(OpacityProperty, animation);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Task.Run(() =>
            {
                App.MainModel.LoadSettings();
                App.MainModel.LoadTracks();
                App.MainModel.LoadHighscores();
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            App.MainModel.SaveSettings();
            base.OnClosing(e);
        }
    }
}
