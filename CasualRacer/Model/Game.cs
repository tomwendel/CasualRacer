using System;
using System.Windows;

using static System.Math;

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

        public Game()
        {
            Track = Track.LoadFromTxt("./Tracks/Track1.txt");

            Player1 = new Player();

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
                player.Velocity = Min(targetSpeed, player.Velocity);
            }
            else if (targetSpeed < player.Velocity)
            {
                player.Velocity -= 100f * (float)elapsedTime.TotalSeconds;
                player.Velocity = Max(targetSpeed, player.Velocity);
            }

            // Positionsveränderung
            var direction = (float)(player.Direction * PI) / 180f;
            var velocity = new Vector(
                Sin(direction) * player.Velocity * elapsedTime.TotalSeconds, 
                -Cos(direction) * player.Velocity * elapsedTime.TotalSeconds);
            player.Position += velocity;
        }
    }
}
