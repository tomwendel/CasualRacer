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
        private readonly ObservableCollection<TimeSpan> lapTimes = new ObservableCollection<TimeSpan>();

        private float direction = 90f;

        private float velocity = 0f;

        private int lap = 0;

        private int measuredLap = 1;

        private Point position = default(Point);

        private PlayerPositionRelativeToGoal goalFlag = PlayerPositionRelativeToGoal.AwayFromGoal;

        private TimeSpan lapTime = TimeSpan.Zero;

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
        public Point Position
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
        public int Lap
        {
            get { return lap; }
            set
            {
                if (lap != value)
                {
                    lap = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lap)));
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
                        if (Lap++ == measuredLap)
                        {
                            lapTimes.Add(LapTime);
                            LapTime = TimeSpan.Zero;
                            measuredLap++;
                        }
                    }
                    else if (goalFlag == PlayerPositionRelativeToGoal.AfterGoal && value == PlayerPositionRelativeToGoal.BeforeGoal)
                    {
                        // Runde zurück
                        Lap--;
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
        public TimeSpan LapTime
        {
            get { return lapTime; }
            set
            {
                if (lapTime != value)
                {
                    lapTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LapTime)));
                }
            }
        }

        public ObservableCollection<TimeSpan> LapTimes
        {
            get { return lapTimes; }
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
