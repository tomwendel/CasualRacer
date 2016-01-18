using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace CasualRacer.Model
{
    internal class Player : INotifyPropertyChanged
    {
        private float direction = 90f;

        private float velocity = 0f;

        private int round = 0;

        private int messuredRound = 1;

        private Vector position = new Vector();

        private GoalFlags goalFlag = GoalFlags.None;

        private TimeSpan roundTime;

        private TimeSpan totalTime;

        private ObservableCollection<TimeSpan> roundTimes = new ObservableCollection<TimeSpan>();

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

        public GoalFlags GoalFlag
        {
            get { return goalFlag; }
            set
            {
                if (goalFlag != value)
                {
                    // Nächste Runde
                    if (goalFlag == GoalFlags.BeforeGoal && value == GoalFlags.AfterGoal)
                    {
                        if (Round == MessuredRound)
                        {
                            roundTimes.Add(RoundTime);
                            RoundTime = new TimeSpan();
                            MessuredRound++;
                        }
                        Round++;
                    }

                    // Runde zurück
                    if (goalFlag == GoalFlags.AfterGoal && value == GoalFlags.BeforeGoal)
                        Round--;

                    goalFlag = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GoalFlag)));
                }
            }
        }

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

        public int MessuredRound
        {
            get { return messuredRound; }
            set
            {
                if (messuredRound != value)
                {
                    messuredRound = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessuredRound)));
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

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum GoalFlags
    {
        None,
        BeforeGoal,
        AfterGoal,
    }
}
