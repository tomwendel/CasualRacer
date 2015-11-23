using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Game
    {
        public Track Track { get; private set; }

        public Player Player1 { get; private set; }

        public Game()
        {
            Track = Track.LoadFromTxt("./Tracks/Track1.txt");

            Player1 = new Player();

        }

        public void Update(TimeSpan totalTime, TimeSpan elapsedTime)
        {
            UpdatePlayer(totalTime, elapsedTime, Player1);
        }

        private void UpdatePlayer(TimeSpan totalTime, TimeSpan elapsedTime,Player player)
        {
            // Lenkung
            if (player.WheelLeft)
                player.Direction -= (float)elapsedTime.TotalSeconds * 100;
            if (player.WheelRight)
                player.Direction += (float)elapsedTime.TotalSeconds * 100;

            // Beschleunigung & Bremse
            float targetSpeed = 0f;
            if (player.Acceleration)
                targetSpeed = 100f;
            if (player.Break)
                targetSpeed = -50f;

            //Anpassung je nach Untergrund
            targetSpeed *= Track.SpeedAdjustmentByTile(player.Position.X, player.Position.Y);

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
            float direction = (float)(player.Direction * Math.PI) / 180f;
            Vector velocity = new Vector(
                Math.Sin(direction) * player.Velocity * elapsedTime.TotalSeconds,
                -Math.Cos(direction) * player.Velocity * elapsedTime.TotalSeconds);
            player.Position += velocity;
        }
    }
}
