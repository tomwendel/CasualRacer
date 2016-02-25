using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Track
    {
        /// <summary>
        /// Die Seitenlänge der Zellen.
        /// </summary>
        public const int CELLSIZE = 40;

        /// <summary>
        /// Standard-Tile für Strassen und Rand.
        /// </summary>
        public const TrackTile DEFAULT_TILE = TrackTile.Dirt;

        private readonly Point goalPosition;

        /// <summary>
        /// Erzeugt eine neue Instanz der <see cref="Track"/> Klasse.
        /// </summary>
        /// <param name="width">Die Breite des Tracks.</param>
        /// <param name="height">Die Höhe des Tracks.</param>
        /// <param name="goalPosition">Die Position des Ziels.</param>
        public Track(string name, string key, int width, int height, Point goalPosition)
        {
            Name = name;
            Key = key;
            Tiles = new TrackTile[width, height];

            this.goalPosition = goalPosition;
        }

        public string Name { get; private set; }

        public string Key { get; private set; }

        /// <summary>
        /// Ruft die Zellen ab.
        /// </summary>
        public TrackTile[,] Tiles { get; }

        /// <summary>
        /// Gibt die Position des Ziels zurueck.
        /// </summary>
        public Point GoalPosition
        {
            get { return goalPosition; }
        }

        /// <summary>
        /// Liefert den Geschwindigkeitsmultiplikator auf Basis einer Position. 
        /// Liegt die Position außerhalb des Spielfeldes, wird der nächste Punkt 
        /// innerhalb des gültigen Spielfeldes gesucht.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Geschwindigkeitsmultiplikator</returns>
        public float GetSpeedByPosition(Point position)
        {
            var tile = GetTileByPosition(position);

            var speed = 0.0f;
            if (tile.HasFlag(TrackTile.Road))
            {
                speed = 1f;
            }
            else
            {
                switch (tile)
                {
                case TrackTile.Dirt: speed = 0.2f; break;
                case TrackTile.Gras: speed = 0.8f; break;
                case TrackTile.Sand: speed = 0.4f; break;
                }
            }

            return speed;
        }

        /// <summary>
        /// Liefert den Zelleninhalt auf Basis einer Position. 
        /// Liegt die Position außerhalb des Spielfeldes, wird der nächste 
        /// Punkt innerhalb des gültigen Spielfeldes gesucht.
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Zelleninhalt</returns>
        public TrackTile GetTileByPosition(Point position)
        {
            var cellX = (int)(position.X / CELLSIZE);
            var cellY = (int)(position.Y / CELLSIZE);
            cellX = Math.Min(Tiles.GetLength(0) - 1, Math.Max(0, cellX));
            cellY = Math.Min(Tiles.GetLength(1) - 1, Math.Max(0, cellY));
            return Tiles[cellX, cellY];
        }

        public TrackTile GetTileByIndex(int x, int y)
        {
            if (x < 0 ||
                y < 0 ||
                x >= Tiles.GetLength(0) ||
                y >= Tiles.GetLength(1))
                return DEFAULT_TILE;

            return Tiles[x, y];
        }

        /// <summary>
        /// Lädt einen Track aus einer .txt-Datei, in der jedes Zeichen den Track-Typ angibt.
        /// </summary>
        /// <param name="path">Der Pfad zu der zu ladenden Datei.</param>
        /// <returns>Den geladenen Track.</returns>
        /// <exception cref="ArgumentNullException">Der Pfad darf nicht null sein.</exception>
        public static Track LoadFromTxt(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            FileInfo file = new FileInfo(path);
            string name = file.Name.Substring(0, file.Name.Length - file.Extension.Length);

            using (Stream stream = File.OpenRead(path))
            {

                return LoadFromTxt(stream, name, path);
            }
        }

        /// <summary>
        /// Lädt einen Track aus einer .txt-Datei, in der jedes Zeichen den Track-Typ angibt.
        /// </summary>
        /// <param name="stream">Der <see cref="Stream" />, der die Dateiinhalte liefert.</param>
        /// <param name="name">Name des Tracks</param>
        /// <param name="key">Eindeutiger Key dieses Tracks</param>
        /// <returns>
        /// Den geladenen Track.
        /// </returns>
        /// <exception cref="ArgumentNullException">Der Stream darf nicht null sein.</exception>
        /// <exception cref="FormatException">Die Datei ist leer.</exception>
        public static Track LoadFromTxt(Stream stream, string name, string key)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var streamReader = new StreamReader(stream, Encoding.ASCII))
            {
                if (streamReader.EndOfStream)
                {
                    throw new FormatException("The file must not be empty.");
                }

                // reading the first line is outside the loop to calculate the size for the list
                var line = streamReader.ReadLine();

                int tilesPerLine = line.Length;

                var allTiles = new List<TrackTile[]>((int)Math.Ceiling((float)stream.Length / line.Length));

                var y = 1;

                Point? goalPos = null;

                do
                {
                    TrackTile[] tiles = GetTilesFromLine(line, tilesPerLine, y++);

                    for (int i = 0; i < tiles.Length; i++)
                    {
                        if (tiles[i].IsGoalTile())
                        {
                            if (goalPos != null)
                            {
                                throw new Exception("The file contains multiple goals.");
                            }

                            goalPos = new Point(i, y - 2);
                        }
                    }

                    allTiles.Add(tiles);
                }
                while ((line = streamReader.ReadLine()) != null);

                if (goalPos == null)
                {
                    throw new Exception("The file does not contains any goal.");
                }

                return BuildTrack(name, key, tilesPerLine, allTiles, goalPos.Value);
            }
        }

        /// <summary>
        /// Lädt die Tiles einer Zeile der Eingabedatei.
        /// </summary>
        /// <param name="line">Die Eingabezeile.</param>
        /// <param name="tilesPerLine">Die Anzahl der Tiles in einer Zeile.</param>
        /// <param name="lineNumber">Die dazugehörige Zeilennummer.</param>
        /// <returns>Das geladene Array von <see cref="TrackTile"/>s.</returns>
        /// <exception cref="FormatException">Die Zeile darf nicht leer sein, muss genau so lang sein wie vorgesehen und darf nur Werte aus <see cref="TrackTile"/> enthalten.</exception>
        private static TrackTile[] GetTilesFromLine(string line, int tilesPerLine, int lineNumber)
        {
            if (line.Length == 0)
            {
                throw new FormatException("The file must not contain empty lines.");
            }

            if (line.Length != tilesPerLine)
            {
                throw new FormatException($"Line {lineNumber + 1} contains a deviating amount of tiles.");
            }

            var tilesForThisLine = new TrackTile[tilesPerLine];

            for (var x = 0; x < line.Length; x++)
            {
                var tileTypeAsString = line.Substring(x, 1);
                int tileTypeAsInt;

                if (!int.TryParse(tileTypeAsString, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, out tileTypeAsInt) || !Enum.IsDefined(typeof(TrackTile), tileTypeAsInt))
                {
                    throw new FormatException($"Line {lineNumber} contains a not supported tile identifier {tileTypeAsString}.");
                }

                var tileType = (TrackTile)tileTypeAsInt;

                if (tileType != default(TrackTile))
                {
                    tilesForThisLine[x] = tileType;
                }
            }
            return tilesForThisLine;
        }

        /// <summary>
        /// Baut ein <see cref="Track"/> Objekt aus den gegebenen Tiles.
        /// </summary>
        /// <param name="tilesPerLine">Die Anzahl an Tiles pro Zeile.</param>
        /// <param name="allTiles">Die Liste aller Tiles.</param>
        /// <param name="goal">Die Position des Ziels.</param>
        /// <returns>Ein <see cref="Track"/> Objekt zusammengesetzt aus den Tiles.</returns>
        private static Track BuildTrack(string name, string key, int tilesPerLine, IEnumerable<TrackTile[]> allTiles, Point goal)
        {
            int y = 0;

            var track = new Track(name, key, tilesPerLine, allTiles.Count(), goal);

            foreach (var tileRow in allTiles)
            {
                for (var x = 0; x < tilesPerLine; x++)
                {
                    track.Tiles[x, y] = tileRow[x];
                }

                y++;
            }
            return track;
        }
    }
}
