using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static Track Load(string path)
        {
            List<string> lines = new List<string>();
            using (Stream stream = File.OpenRead(path))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        lines.Add(sr.ReadLine());
                    }
                }
            }

            // TODO: Fehlerquellen abfangen
            if (lines.Count > 0)
            {
                Track result = new Track(lines[0].Length, lines.Count);
                for (int y = 0; y < lines.Count; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        int id = int.Parse(lines[y][x].ToString());
                        result.Tiles[x, y] = (TrackTile)id;
                    }
                }
                return result;
            }

            return null;
        }
    }

    internal enum TrackTile
    {
        Dirt = 0,
        Sand = 1,
        Gras = 2,
        Road = 3,
    }
}
