using CasualRacer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            NavigationFrame.Navigate(new StartPage());
        }

        private void NavigationFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (!inNavigation)
            {
                e.Cancel = true;
                navArgs = e;

                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 1f;
                animation.To = 0f;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                animation.Completed += Animation_Completed;
                NavigationFrame.BeginAnimation(OpacityProperty, animation);

                inNavigation = true;
            }
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

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0f;
            animation.To = 1f;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            NavigationFrame.BeginAnimation(OpacityProperty, animation);
        }
    }
}
