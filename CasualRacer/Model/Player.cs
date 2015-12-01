using System.ComponentModel;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Player : INotifyPropertyChanged
    {
        private float direction = 0f;

        private float velocity = 0f;

        private Vector position = new Vector();

        /// <summary>
        /// Ruft die Richtung ab oder setzt diese.
        /// </summary>
        public float Direction
        {
            get { return direction; }
            set
            {
                if (direction != value)
                {
                    direction = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Direction)));
                }
            }
        }

        /// <summary>
        /// Ruft die Position ab oder setzt diese.
        /// </summary>
        public Vector Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
                }
            }
        }

        /// <summary>
        /// Ruft die Geschwindigkeit ab oder setzt diese.
        /// </summary>
        public float Velocity
        {
            get { return velocity; }
            set
            {
                if (velocity != value)
                {
                    velocity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Velocity)));
                }
            }
        }

        /// <summary>
        /// Ruft ab, ob der Spieler beschleunigt.
        /// </summary>
        public bool Acceleration { get; set; }

        /// <summary>
        /// Ruft ab, ob der Spieler bremst.
        /// </summary>
        public bool Break { get; set; }

        /// <summary>
        /// Ruft ab, ob der Spieler nach links lenkt.
        /// </summary>
        public bool WheelLeft { get; set; }

        /// <summary>
        /// Ruft ab, ob der Spieler nach rechts lenkt.
        /// </summary>
        public bool WheelRight { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
