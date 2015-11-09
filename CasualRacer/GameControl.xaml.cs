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
using System.Threading;

namespace CasualRacer
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        private readonly Game game = new Game();

        private readonly Stopwatch totalWatch = new Stopwatch();
        private readonly Stopwatch elapsedWatch = new Stopwatch();

        public GameControl()
        {
            InitializeComponent();
            DataContext = game;

            CompositionTarget.Rendering += OnRendering;

            elapsedWatch.Start();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Track track = game.Track;

            Brush dirtBrush = new SolidColorBrush(Color.FromArgb(255, 127, 51, 0));
            Brush sandBrush = new SolidColorBrush(Color.FromArgb(255, 255, 226, 147));
            Brush grasBrush = new SolidColorBrush(Color.FromArgb(255, 76, 255, 0));
            Brush roadBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));

            for (int x = 0; x < track.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < track.Tiles.GetLength(1); y++)
                {
                    Brush brush = dirtBrush;
                    switch (track.Tiles[x, y])
                    {
                    case TrackTile.Gras:
                        brush = grasBrush;
                        break;
                    case TrackTile.Road:
                        brush = roadBrush;
                        break;
                    case TrackTile.Sand:
                        brush = sandBrush;
                        break;
                    }

                    drawingContext.DrawRectangle(brush, null, new Rect(x * 40, y * 40, 40, 40));
                }
            }
        }

        private void OnRendering(object sender, EventArgs e)
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;

            Application.Current.MainWindow.KeyDown -= MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp -= MainWindow_KeyUp;
        }
    }
}
