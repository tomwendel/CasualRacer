namespace CasualRacer.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Abbildung eines Spielers.
    /// </summary>
    internal class Player : INotifyPropertyChanged
    {
        private readonly ObservableCollection<TimeSpan> roundTimes = new ObservableCollection<TimeSpan>();

        private float direction = 90f;

        private float velocity = 0f;

        private int round = 0;

        private int measuredRound = 1;

        private Vector position = default(Vector);

        private PlayerPositionRelativeToGoal goalFlag = PlayerPositionRelativeToGoal.AwayFromGoal;

        private TimeSpan roundTime = TimeSpan.Zero;

        private TimeSpan totalTime = TimeSpan.Zero;

        /// <summary>
        /// Tritt ein, wenn sich ein Eigenschaftswert ändert.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gibt die aktuelle Runde an.
        /// </summary>
        public int Round
        {
            get { return round; }
            set
            {
                if (round != value)
                {
                    round = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Round)));
                }
            }
        }

        public PlayerPositionRelativeToGoal PositionRelativeToGoal
        {
            get { return goalFlag; }
            set
            {
                if (goalFlag != value)
                {
                    if (goalFlag == PlayerPositionRelativeToGoal.BeforeGoal && value == PlayerPositionRelativeToGoal.AfterGoal)
                    {
                        // Nächste Runde
                        if (Round++ == measuredRound)
                        {
                            roundTimes.Add(RoundTime);
                            RoundTime = TimeSpan.Zero;
                            measuredRound++;
                        }
                    }
                    else if (goalFlag == PlayerPositionRelativeToGoal.AfterGoal && value == PlayerPositionRelativeToGoal.BeforeGoal)
                    {
                        // Runde zurück
                        Round--;
                    }

                    goalFlag = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PositionRelativeToGoal)));
                }
            }
        }

        /// <summary>
        /// Gibt die Gesamtzeit der Rennes zurueck.
        /// </summary>
        public TimeSpan TotalTime
        {
            get { return totalTime; }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalTime)));
                }
            }
        }

        /// <summary>
        /// Gibt die Zeit der aktuellen Runde zurueck.
        /// </summary>
        public TimeSpan RoundTime
        {
            get { return roundTime; }
            set
            {
                if (roundTime != value)
                {
                    roundTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RoundTime)));
                }
            }
        }

        public ObservableCollection<TimeSpan> RoundTimes
        {
            get { return roundTimes; }
        }

        /// <summary>
        /// Ruft ab, ob der Spieler nach links lenkt.
        /// </summary>
        public bool WheelLeft { get; set; }

        /// <summary>
        /// Ruft ab, ob der Spieler nach rechts lenkt.
        /// </summary>
        public bool WheelRight { get; set; }
    }
}
