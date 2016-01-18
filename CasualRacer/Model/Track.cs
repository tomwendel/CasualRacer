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

        /// <summary>
        /// Ruft die Zellen ab.
        /// </summary>
        public TrackTile[,] Tiles { get; }

        /// <summary>
        /// Erzeugt eine neue Instanz der <see cref="Track"/> Klasse.
        /// </summary>
        /// <param name="width">Die Breite des Tracks.</param>
        /// <param name="height">Die Höhe des Tracks.</param>
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
        public TrackTile GetTileByPosition(Vector position)
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

        public Vector GetGoalPosition()
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    if (Tiles[x, y] == TrackTile.GoalDown ||
                        Tiles[x, y] == TrackTile.GoalLeft ||
                        Tiles[x, y] == TrackTile.GoalRight ||
                        Tiles[x, y] == TrackTile.GoalUp)
                        return new Vector(x, y);
                }
            }

            throw new Exception("No Goal found");
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

            using (Stream stream = File.OpenRead(path))
            {
                return LoadFromTxt(stream);
            }
        }

        /// <summary>
        /// Lädt einen Track aus einer .txt-Datei, in der jedes Zeichen den Track-Typ angibt.
        /// </summary>
        /// <param name="stream">Der <see cref="Stream" />, der die Dateiinhalte liefert.</param>
        /// <returns>
        /// Den geladenen Track.
        /// </returns>
        /// <exception cref="ArgumentNullException">Der Stream darf nicht null sein.</exception>
        /// <exception cref="FormatException">Die Datei ist leer.</exception>
        public static Track LoadFromTxt(Stream stream)
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

                do
                {
                    allTiles.Add(GetTilesFromLine(line, tilesPerLine, y++));
                }
                while ((line = streamReader.ReadLine()) != null);

                // TODO: Prüfen, ob es ein Goal gibt

                return BuildTrack(tilesPerLine, allTiles);
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
        /// <returns>Ein <see cref="Track"/> Objekt zusammengesetzt aus den Tiles.</returns>
        private static Track BuildTrack(int tilesPerLine, IEnumerable<TrackTile[]> allTiles)
        {
            int y = 0;

            var track = new Track(tilesPerLine, allTiles.Count());

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
