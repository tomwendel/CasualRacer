using CasualRacer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CasualRacer
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        private Game game = new Game();

        private DispatcherTimer timer = new DispatcherTimer();
        private Stopwatch totalWatch = new Stopwatch();
        private Stopwatch elapsedWatch = new Stopwatch();

        public GameControl()
        {
            InitializeComponent();
            DataContext = game;

            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += Timer_Tick;
            timer.IsEnabled = true;

            totalWatch.Start();
            elapsedWatch.Start();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = elapsedWatch.Elapsed;
            elapsedWatch.Restart();
            game.Update(totalWatch.Elapsed, elapsed);
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up: game.Player1.Acceleration = false; break;
                case Key.Left: game.Player1.WheelLeft = false; break;
                case Key.Right: game.Player1.WheelRight = false; break;
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up: game.Player1.Acceleration = true; break;
                case Key.Left: game.Player1.WheelLeft = true; break;
                case Key.Right: game.Player1.WheelRight = true; break;
            }
        }
    }
}
