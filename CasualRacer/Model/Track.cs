using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Track
    {
        public const int CELLSIZE = 40;

        public TrackTile[,] Tiles { get; private set; }

        public Track(int width, int height)
        {
            Tiles = new TrackTile[width, height];
        }

        /// <summary>
        /// Liefert den Geschwindigkeitsmultiplikator auf Basis einer Position. 
        /// Liegt die Position außerhalb des Spielfeldes, wird der nächste Punkt 
        /// innerhalb des gültigen Spielfeldes gesucht.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Geschwindigkeitsmultiplikator</returns>
        public float GetSpeedByPosition(Vector position)
        {
            TrackTile tile = GetTileByPosition(position);

            float retVal = 0.0f;
            switch (tile)
            {
                case TrackTile.Dirt: retVal = 0.2f; break;
                case TrackTile.Gras: retVal = 0.8f; break;
                case TrackTile.Road: retVal = 1f; break;
                case TrackTile.Sand: retVal = 0.4f; break;
            }
            return retVal;
        }

        /// <summary>
        /// Liefert den Zelleninhalt auf Basis einer Position. 
        /// Liegt die Position außerhalb des Spielfeldes, wird der nächste 
        /// Punkt innerhalb des gültigen Spielfeldes gesucht.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Zelleninhalt</returns>
        public TrackTile GetTileByPosition(Vector position)
        {
            int cellX = (int)(position.X / CELLSIZE);
            int cellY = (int)(position.Y / CELLSIZE);
            cellX = Math.Min(Tiles.GetLength(0) - 1, Math.Max(0, cellX));
            cellY = Math.Min(Tiles.GetLength(1) - 1, Math.Max(0, cellY));
            return Tiles[cellX, cellY];
        }

        public static Track LoadFromTxt(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            using (Stream stream = File.OpenRead(path))
            {
                return LoadFromTxt(stream);
            }
        }

        public static Track LoadFromTxt(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (StreamReader sr = new StreamReader(stream, Encoding.ASCII))
            {
                if (sr.EndOfStream)
                {
                    throw new FormatException("The file must not be empty.");
                }

                // reading the first line is outside the loop to calculate the size for the list
                string line = sr.ReadLine();

                int tilesPerLine = line.Length;

                List<TrackTile[]> tiles = new List<TrackTile[]>((int)Math.Ceiling((float)stream.Length / line.Length));

                for (int y = 0; ; y++, line = sr.ReadLine())
                {
                    if (line.Length == 0)
                    {
                        throw new FormatException("The file must not contain empty lines.");
                    }

                    if (line.Length != tilesPerLine)
                    {
                        throw new FormatException(string.Format("Line {0} contains a deviating amount of tiles.", y + 1));
                    }

                    TrackTile[] tilesForThisLine = new TrackTile[tilesPerLine];

                    for (int x = 0; x < line.Length; x++)
                    {
                        string tileTypeAsString = line.Substring(x, 1);
                        int tileTypeAsInt;

                        if (!int.TryParse(tileTypeAsString, out tileTypeAsInt) || !Enum.IsDefined(typeof(TrackTile), tileTypeAsInt))
                        {
                            throw new FormatException(string.Format("Line {0} contains a not supported tile identifier {1}.", y, tileTypeAsString));
                        }

                        TrackTile tileType = (TrackTile)tileTypeAsInt;

                        if  (tileType != default(TrackTile))
                        {
                            tilesForThisLine[x] = tileType;
                        }
                    }

                    tiles.Add(tilesForThisLine);

                    if (sr.EndOfStream)
                    {
                        break;
                    }
                }

                Track result = new Track(tilesPerLine, tiles.Count);

                {
                    int y = 0;

                    foreach (TrackTile[] curTiles in tiles)
                    {
                        for (int x = 0; x < tilesPerLine; x++)
                        {
                            result.Tiles[x, y] = curTiles[x];
                        }

                        y++;
                    }

                }

                return result;
            }
        }
    }
}
