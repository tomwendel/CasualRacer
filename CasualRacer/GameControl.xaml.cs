using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CasualRacer.Model;
using System.Collections.Generic;
using System.IO;

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

        private Dictionary<TileType, ImageBrush> grasTiles = new Dictionary<TileType, ImageBrush>();
        private Dictionary<TileType, ImageBrush> sandTiles = new Dictionary<TileType, ImageBrush>();
        private Dictionary<TileType, ImageBrush> roadTiles = new Dictionary<TileType, ImageBrush>();

        public GameControl()
        {
            InitializeComponent();
            DataContext = game;

            CompositionTarget.Rendering += OnRendering;

            elapsedWatch.Start();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.KeyUp += MainWindow_KeyUp;

            var path = Path.Combine(Environment.CurrentDirectory, "Assets");

            BitmapImage image = new BitmapImage(new Uri(path + "\\tiles.png"));

            // Definition des zentralen Dirt-Brushes
            dirtBrush = new ImageBrush(image);
            dirtBrush.TileMode = TileMode.Tile;
            dirtBrush.Viewbox = new Rect(1820, 0, 128, 128);
            dirtBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            dirtBrush.ViewboxUnits = BrushMappingMode.Absolute;

            // Definition der Gras-Brushes
            grasTiles.Add(TileType.Center, new ImageBrush(image) { Viewbox = new Rect(910, 260, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.Left, new ImageBrush(image) { Viewbox = new Rect(910, 1300, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.Right, new ImageBrush(image) { Viewbox = new Rect(910, 1040, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.Upper, new ImageBrush(image) { Viewbox = new Rect(910, 0, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.Lower, new ImageBrush(image) { Viewbox = new Rect(910, 520, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.UpperLeftConcave, new ImageBrush(image) { Viewbox = new Rect(910, 780, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.UpperRightConcave, new ImageBrush(image) { Viewbox = new Rect(910, 910, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.LowerLeftConcave, new ImageBrush(image) { Viewbox = new Rect(910, 1430, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.LowerRightConcave, new ImageBrush(image) { Viewbox = new Rect(910, 1560, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.UpperLeftConvex, new ImageBrush(image) { Viewbox = new Rect(910, 130, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.UpperRightConvex, new ImageBrush(image) { Viewbox = new Rect(780, 1820, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.LowerLeftConvex, new ImageBrush(image) { Viewbox = new Rect(910, 650, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            grasTiles.Add(TileType.LowerRightConvex, new ImageBrush(image) { Viewbox = new Rect(910, 390, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });

            // Definition der Sand-Brushes
            sandTiles.Add(TileType.Center, new ImageBrush(image) { Viewbox = new Rect(780, 260, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.Left, new ImageBrush(image) { Viewbox = new Rect(780, 1300, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.Right, new ImageBrush(image) { Viewbox = new Rect(780, 1040, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.Upper, new ImageBrush(image) { Viewbox = new Rect(780, 0, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.Lower, new ImageBrush(image) { Viewbox = new Rect(780, 520, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.UpperLeftConcave, new ImageBrush(image) { Viewbox = new Rect(780, 780, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.UpperRightConcave, new ImageBrush(image) { Viewbox = new Rect(780, 910, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.LowerLeftConcave, new ImageBrush(image) { Viewbox = new Rect(780, 1430, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.LowerRightConcave, new ImageBrush(image) { Viewbox = new Rect(780, 1560, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.UpperLeftConvex, new ImageBrush(image) { Viewbox = new Rect(780, 130, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.UpperRightConvex, new ImageBrush(image) { Viewbox = new Rect(780, 1690, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.LowerLeftConvex, new ImageBrush(image) { Viewbox = new Rect(780, 650, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            sandTiles.Add(TileType.LowerRightConvex, new ImageBrush(image) { Viewbox = new Rect(780, 390, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });

            // Definition der Road-Brushes
            roadTiles.Add(TileType.Center, new ImageBrush(image) { Viewbox = new Rect(2470, 650, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.Left, new ImageBrush(image) { Viewbox = new Rect(2470, 780, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.Right, new ImageBrush(image) { Viewbox = new Rect(2470, 520, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.Upper, new ImageBrush(image) { Viewbox = new Rect(2600, 1040, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.Lower, new ImageBrush(image) { Viewbox = new Rect(2340, 260, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.UpperLeftConcave, new ImageBrush(image) { Viewbox = new Rect(2210, 1430, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.UpperRightConcave, new ImageBrush(image) { Viewbox = new Rect(2210, 1560, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.LowerLeftConcave, new ImageBrush(image) { Viewbox = new Rect(2340, 1820, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.LowerRightConcave, new ImageBrush(image) { Viewbox = new Rect(2470, 0, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.UpperLeftConvex, new ImageBrush(image) { Viewbox = new Rect(2600, 780, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.UpperRightConvex, new ImageBrush(image) { Viewbox = new Rect(2600, 650, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.LowerLeftConvex, new ImageBrush(image) { Viewbox = new Rect(2470, 390, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
            roadTiles.Add(TileType.LowerRightConvex, new ImageBrush(image) { Viewbox = new Rect(2470, 260, 128, 128), ViewboxUnits = BrushMappingMode.Absolute });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var track = game.Track;
            dirtBrush.Viewport = new Rect(0, 0, 0.5f / track.Tiles.GetLength(0), 0.5f / track.Tiles.GetLength(1));
            drawingContext.DrawRectangle(dirtBrush, null, new Rect(0, 0, Track.CELLSIZE * track.Tiles.GetLength(0), Track.CELLSIZE * track.Tiles.GetLength(1)));

            for (var x = 0; x < track.Tiles.GetLength(0); x++)
            {
                for (var y = 0; y < track.Tiles.GetLength(1); y++)
                {
                    switch (track.Tiles[x, y])
                    {
                        case TrackTile.Gras: DrawTile(drawingContext, TrackTile.Gras, x, y, grasTiles); break;
                        case TrackTile.Sand: DrawTile(drawingContext, TrackTile.Sand, x, y, sandTiles); break;
                    }

                    // Road handling
                    if (((int)track.Tiles[x,y] & 8) > 0)
                    {
                        DrawTile(drawingContext, TrackTile.Road, x, y, roadTiles);
                    }
                }
            }
        }

        private void DrawTile(DrawingContext context, TrackTile type, int x, int y, Dictionary<TileType, ImageBrush> mapping)
        {
            bool left = game.Track.GetTileByIndex(x - 1, y) == type;
            bool upper = game.Track.GetTileByIndex(x, y - 1) == type;
            bool lower = game.Track.GetTileByIndex(x, y + 1) == type;
            bool right = game.Track.GetTileByIndex(x + 1, y) == type;
            bool upperLeft = game.Track.GetTileByIndex(x - 1, y - 1) == type;
            bool upperRight = game.Track.GetTileByIndex(x + 1, y - 1) == type;
            bool lowerLeft = game.Track.GetTileByIndex(x - 1, y + 1) == type;
            bool lowerRight = game.Track.GetTileByIndex(x + 1, y + 1) == type;

            if (type == TrackTile.Road)
            {
                left = ((int)game.Track.GetTileByIndex(x - 1, y) & 8) == 8;
                upper = ((int)game.Track.GetTileByIndex(x, y - 1) & 8) == 8;
                lower = ((int)game.Track.GetTileByIndex(x, y + 1) & 8) == 8;
                right = ((int)game.Track.GetTileByIndex(x + 1, y) & 8) == 8;
                upperLeft = ((int)game.Track.GetTileByIndex(x - 1, y - 1) & 8) == 8;
                upperRight = ((int)game.Track.GetTileByIndex(x + 1, y - 1) & 8) == 8;
                lowerLeft = ((int)game.Track.GetTileByIndex(x - 1, y + 1) & 8) == 8;
                lowerRight = ((int)game.Track.GetTileByIndex(x + 1, y + 1) & 8) == 8;
            }

            #region Upper Left

            ImageBrush cell00 = mapping[TileType.Center];
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

            context.DrawRectangle(cell00, null, new Rect(x * Track.CELLSIZE, y * Track.CELLSIZE, Track.CELLSIZE / 2, Track.CELLSIZE / 2));

            #endregion

            #region Upper Right

            ImageBrush cell10 = mapping[TileType.Center];
            if (!right)
            {
                if (!upper)
                {
                    // konvexe linke ecke
                    cell10 = mapping[TileType.UpperRightConvex];
                }
                else
                {
                    // linke kante
                    cell10 = mapping[TileType.Right];
                }
            }
            else
            {
                if (!upper)
                {
                    // obere Kante
                    cell10 = mapping[TileType.Upper];
                }
                else if (!upperRight)
                {
                    // linke konkave Ecke
                    cell10 = mapping[TileType.UpperRightConcave];
                }
            }

            context.DrawRectangle(cell10, null, new Rect((x * Track.CELLSIZE) + (Track.CELLSIZE / 2), y * Track.CELLSIZE, Track.CELLSIZE / 2, Track.CELLSIZE / 2));

            #endregion

            #region Lower Right

            ImageBrush cell11 = mapping[TileType.Center];
            if (!right)
            {
                if (!lower)
                {
                    // konvexe linke ecke
                    cell11 = mapping[TileType.LowerRightConvex];
                }
                else
                {
                    // linke kante
                    cell11 = mapping[TileType.Right];
                }
            }
            else
            {
                if (!lower)
                {
                    // obere Kante
                    cell11 = mapping[TileType.Lower];
                }
                else if (!lowerRight)
                {
                    // linke konkave Ecke
                    cell11 = mapping[TileType.LowerRightConcave];
                }
            }

            context.DrawRectangle(cell11, null, new Rect((x * Track.CELLSIZE) + (Track.CELLSIZE / 2), (y * Track.CELLSIZE) + (Track.CELLSIZE / 2), Track.CELLSIZE / 2, Track.CELLSIZE / 2));

            #endregion

            #region Lower Left

            ImageBrush cell01 = mapping[TileType.Center];
            if (!left)
            {
                if (!lower)
                {
                    // konvexe linke ecke
                    cell01 = mapping[TileType.LowerLeftConvex];
                }
                else
                {
                    // linke kante
                    cell01 = mapping[TileType.Left];
                }
            }
            else
            {
                if (!lower)
                {
                    // obere Kante
                    cell01 = mapping[TileType.Lower];
                }
                else if (!lowerLeft)
                {
                    // linke konkave Ecke
                    cell01 = mapping[TileType.LowerLeftConcave];
                }
            }

            context.DrawRectangle(cell01, null, new Rect(x * Track.CELLSIZE, (y * Track.CELLSIZE) + (Track.CELLSIZE / 2), Track.CELLSIZE / 2, Track.CELLSIZE / 2));

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
