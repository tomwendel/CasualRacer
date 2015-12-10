using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CasualRacer.Model;
using System.Collections.Generic;
using System.Windows.Threading;

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

        private readonly ImageBrush dirtBrush;
        private readonly ImageBrush sandBrush;
        private readonly ImageBrush grassBrush;
        private readonly ImageBrush roadBrush;
        private readonly ImageBrush tilesBrush;

        private Dictionary<TileType, Rect> grasTiles = new Dictionary<TileType, Rect>();
        private Dictionary<TileType, Rect> sandTiles = new Dictionary<TileType, Rect>();

        public GameControl()
        {
            InitializeComponent();
            DataContext = game;

            CompositionTarget.Rendering += OnRendering;

            elapsedWatch.Start();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;

            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets");
            dirtBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\dirt_center.png")));
            sandBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\sand_center.png")));
            grassBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\grass_center.png")));
            roadBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\asphalt_center.png")));

            tilesBrush = new ImageBrush(new BitmapImage(new Uri(path + "\\tiles.png")));

            grasTiles.Add(TileType.Center, new Rect(910, 260, 128, 128));
            grasTiles.Add(TileType.Left, new Rect(910, 1300, 128, 128));
            grasTiles.Add(TileType.Right, new Rect(910, 1040, 128, 128));
            grasTiles.Add(TileType.Upper, new Rect(910, 0, 128, 128));
            grasTiles.Add(TileType.Lower, new Rect(910, 520, 128, 128));
            grasTiles.Add(TileType.UpperLeftConcave, new Rect(910, 780, 128, 128));
            grasTiles.Add(TileType.UpperRightConcave, new Rect(910, 910, 128, 128));
            grasTiles.Add(TileType.LowerLeftConcave, new Rect(910, 1430, 128, 128));
            grasTiles.Add(TileType.LowerRightConcave, new Rect(910, 1560, 128, 128));
            grasTiles.Add(TileType.UpperLeftConvex, new Rect(910, 130, 128, 128));
            grasTiles.Add(TileType.UpperRightConvex, new Rect(780, 1820, 128, 128));
            grasTiles.Add(TileType.LowerLeftConvex, new Rect(910, 650, 128, 128));
            grasTiles.Add(TileType.LowerRightConvex, new Rect(910, 390, 128, 128));

            sandTiles.Add(TileType.Center, new Rect(780, 260, 128, 128));
            sandTiles.Add(TileType.Left, new Rect(780, 1300, 128, 128));
            sandTiles.Add(TileType.Right, new Rect(780, 1040, 128, 128));
            sandTiles.Add(TileType.Upper, new Rect(780, 0, 128, 128));
            sandTiles.Add(TileType.Lower, new Rect(780, 520, 128, 128));
            sandTiles.Add(TileType.UpperLeftConcave, new Rect(780, 780, 128, 128));
            sandTiles.Add(TileType.UpperRightConcave, new Rect(780, 910, 128, 128));
            sandTiles.Add(TileType.LowerLeftConcave, new Rect(780, 1430, 128, 128));
            sandTiles.Add(TileType.LowerRightConcave, new Rect(780, 1560, 128, 128));
            sandTiles.Add(TileType.UpperLeftConvex, new Rect(780, 130, 128, 128));
            sandTiles.Add(TileType.UpperRightConvex, new Rect(780, 1690, 128, 128));
            sandTiles.Add(TileType.LowerLeftConvex, new Rect(780, 650, 128, 128));
            sandTiles.Add(TileType.LowerRightConvex, new Rect(780, 390, 128, 128));
        }

        private void X_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var track = game.Track;

            tilesBrush.TileMode = TileMode.Tile;
            tilesBrush.Viewport = new Rect(0, 0, 0.5f / track.Tiles.GetLength(0), 0.5f / track.Tiles.GetLength(1));
            tilesBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;

            tilesBrush.Viewbox = new Rect(1820, 0, 128, 128);
            tilesBrush.ViewboxUnits = BrushMappingMode.Absolute;
            drawingContext.DrawRectangle(tilesBrush, null, new Rect(0, 0, Track.CELLSIZE * track.Tiles.GetLength(0), Track.CELLSIZE * track.Tiles.GetLength(1)));

            tilesBrush.TileMode = TileMode.None;
            tilesBrush.Viewport = new Rect(0, 0, 1f, 1f);
            tilesBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            tilesBrush.ViewboxUnits = BrushMappingMode.Absolute;

            for (var x = 0; x < track.Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < track.Tiles.GetLength(1); y++)
                {
                    switch (track.Tiles[x, y])
                    {
                        case TrackTile.Gras:
                            DrawTile(drawingContext, TrackTile.Gras, x, y, grasTiles);
                            break;
                        case TrackTile.Road:
                            break;
                        case TrackTile.Sand:
                            DrawTile(drawingContext, TrackTile.Sand, x, y, sandTiles);
                            break;
                    }
                }
            }
        }

        private void DrawTile(DrawingContext context, TrackTile type, int x, int y, Dictionary<TileType, Rect> mapping)
        {
            bool left = game.Track.GetTileByIndex(x - 1, y) == type;
            bool upper = game.Track.GetTileByIndex(x, y - 1) == type;
            bool lower = game.Track.GetTileByIndex(x, y + 1) == type;
            bool right = game.Track.GetTileByIndex(x + 1, y) == type;
            bool upperLeft = game.Track.GetTileByIndex(x - 1, y - 1) == type;
            bool upperRight = game.Track.GetTileByIndex(x + 1, y - 1) == type;
            bool lowerLeft = game.Track.GetTileByIndex(x - 1, y + 1) == type;
            bool lowerRight = game.Track.GetTileByIndex(x + 1, y + 1) == type;

            #region Upper Left

            Rect cell00 = mapping[TileType.Center];
            if (!left)
            {
                if (!upper)
                {
                    // konvexe linke ecke
                    cell00 = mapping[TileType.UpperLeftConvex];
                }
                else
                {
                    // linke kante
                    cell00 = mapping[TileType.Left];
                }
            }
            else
            {
                if (!upper)
                {
                    // obere Kante
                    cell00 = mapping[TileType.Upper];
                }
                else if (!upperLeft)
                {
                    // linke konkave Ecke
                    cell00 = mapping[TileType.UpperLeftConcave];
                }
            }

            tilesBrush.Viewbox = cell00;

            context.DrawRectangle(tilesBrush, null, new Rect(x * Track.CELLSIZE, y * Track.CELLSIZE, Track.CELLSIZE / 2, Track.CELLSIZE / 2));

            #endregion
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
                case Key.Down: game.Player1.Break = false; break;
                case Key.Left: game.Player1.WheelLeft = false; break;
                case Key.Right: game.Player1.WheelRight = false; break;
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up: game.Player1.Acceleration = true; break;
                case Key.Down: game.Player1.Break = true; break;
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
