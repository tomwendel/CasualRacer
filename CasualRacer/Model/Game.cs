using System;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Game
    {
        /// <summary>
        /// Ruft den <see cref="Track"/> ab.
        /// </summary>
        public Track Track { get; }

        /// <summary>
        /// Ruft den <see cref="Player"/> 1 ab.
        /// </summary>
        public Player Player1 { get; }

        /// <summary>
        /// Ruft den <see cref="Player"/> 2 ab.
        /// </summary>
        public Player Player2 { get; }

        public Game()
        {
            Track = Track.LoadFromTxt("./Tracks/Track1.txt");

            Vector goal = Track.GetGoalPosition();
            Vector startOffset1 = new Vector();
            Vector startOffset2 = new Vector();
            float startRotation = 0f;
            switch (Track.GetTileByIndex((int)goal.X, (int)goal.Y))
            {
                case TrackTile.GoalDown:
                    startOffset1 = new Vector(0.75f, 0.25f);
                    startOffset2 = new Vector(0.25f, 0.25f);
                    startRotation = 180f;
                    break;
                case TrackTile.GoalLeft:
                    startOffset1 = new Vector(0.75f, 0.75f);
                    startOffset2 = new Vector(0.75f, 0.25f);
                    startRotation = -90f;
                    break;
                case TrackTile.GoalRight:
                    startOffset1 = new Vector(0.25f, 0.25f);
                    startOffset2 = new Vector(0.25f, 0.75f);
                    startRotation = 90f;
                    break;
                case TrackTile.GoalUp:
                    startOffset1 = new Vector(0.25f, 0.75f);
                    startOffset2 = new Vector(0.75f, 0.75f);
                    startRotation = 0f;
                    break;
            }

            Player1 = new Player() { Position = (goal + startOffset1) * Track.CELLSIZE, Direction = startRotation };

        }

        /// <summary>
        /// Aktualisiert den Spielstatus.
        /// </summary>
        /// <param name="totalTime">Die total abgelaufene Zeit.</param>
        /// <param name="elapsedTime">Die abgelaufene Zeit seit dem letzten Update.</param>
        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            UpdatePlayer(totalTime, elapsedTime, Player1);
        }

        /// <summary>
        /// Aktualisiert den gegebenen Spieler.
        /// </summary>
        /// <param name="totalTime">Die total abgelaufene Zeit.</param>
        /// <param name="elapsedTime">Die abgelaufene Zeit seit dem letzten Update.</param>
        /// <param name="player">Der Spieler.</param>
        private void UpdatePlayer(TimeSpan totalTime, TimeSpan elapsedTime, Player player)
        {
            // Lenkung
            if (player.WheelLeft)
                player.Direction -= (float)elapsedTime.TotalSeconds * 100;
            if (player.WheelRight)
                player.Direction += (float)elapsedTime.TotalSeconds * 100;

            // Beschleunigung & Bremse
            var targetSpeed = 0f;
            if (player.Acceleration)
                targetSpeed = 100f;
            if (player.Break)
                targetSpeed = -50f;

            // Anpassung je nach Untergrund
            targetSpeed *= Track.GetSpeedByPosition(player.Position);

            // Beschleunigung
            if (targetSpeed > player.Velocity)
            {
                player.Velocity += 80f * (float)elapsedTime.TotalSeconds;
                player.Velocity = Math.Min(targetSpeed, player.Velocity);
            }
            else if (targetSpeed < player.Velocity)
            {
                player.Velocity -= 100f * (float)elapsedTime.TotalSeconds;
                player.Velocity = Math.Max(targetSpeed, player.Velocity);
            }

            // Positionsveränderung
            var direction = (float)(player.Direction * Math.PI) / 180f;
            var velocity = new Vector(
                Math.Sin(direction) * player.Velocity * elapsedTime.TotalSeconds, 
                -Math.Cos(direction) * player.Velocity * elapsedTime.TotalSeconds);
            player.Position += velocity;

            // Goal State ermitteln
            var playerCell = Track.GetTileByPosition(player.Position);
            if (((int)playerCell & 0xC) > 0)
            {
                // Player stelt in einer Goal-Zelle
                switch (playerCell)
                {
                    case TrackTile.GoalDown:
                        player.GoalFlag = (player.Position.Y % Track.CELLSIZE < Track.CELLSIZE / 2) ? GoalFlags.BeforeGoal : GoalFlags.AfterGoal;
                        break;
                    case TrackTile.GoalLeft:
                        player.GoalFlag = (player.Position.X % Track.CELLSIZE >= Track.CELLSIZE / 2) ? GoalFlags.BeforeGoal : GoalFlags.AfterGoal;
                        break;
                    case TrackTile.GoalRight:
                        player.GoalFlag = (player.Position.X % Track.CELLSIZE < Track.CELLSIZE / 2) ? GoalFlags.BeforeGoal : GoalFlags.AfterGoal;
                        break;
                    case TrackTile.GoalUp:
                        player.GoalFlag = (player.Position.Y % Track.CELLSIZE >= Track.CELLSIZE / 2) ? GoalFlags.BeforeGoal : GoalFlags.AfterGoal;
                        break;
                }
            }
            else
            {
                player.GoalFlag = GoalFlags.None;
            }
        }
    }
}
