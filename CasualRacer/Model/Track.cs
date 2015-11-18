using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
