using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner
{
    public class LogEntry : INotifyPropertyChanged
    {
        private string _date;
        private string _comment;
        private string _difficulty;
        private string _duration;
        private string _rating;

        public LogEntry(string date, string comment, string difficulty, string duration, string rating)
        {
            this.Date = date;
            this.Comment = comment;
            this.Difficulty = difficulty;
            this.Duration = duration;
            this.Rating = rating;
        }

        public string Date
        {
            get => this._date;
            set
            {
                this._date = value;
                this.OnPropertyChanged(nameof(Date));
            }
        }

        public string Comment
        {
            get => this._comment;
            set
            {
                this._comment = value;
                this.OnPropertyChanged(nameof(Comment));
            }
        }

        public string Difficulty
        {
            get => this._difficulty;
            set
            {
                this._difficulty = value;
                this.OnPropertyChanged(nameof(Difficulty));
            }
        }
        public string Duration
        {
            get => this._duration;
            set
            {
                this._duration = value;
                this.OnPropertyChanged(nameof(Duration));
            }
        }

        public string Rating
        {
            get => this._rating;
            set
            {
                this._rating = value;
                this.OnPropertyChanged(nameof(Rating));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
