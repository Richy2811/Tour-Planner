using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourData : INotifyPropertyChanged
    {
        private int _id;
        private string _tourName;
        private string _tourDescription;
        private string _start;
        private string _destination;
        private string _transportType;
        private decimal _tourDistance;
        private string _timeEstimation;
        private string _imageName;
        private string _popularity;
        private string _childFriendliness;
        private string _favourite;

        public bool IsUpdating { get; set; }

        public int ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        public string TourName
        {
            get => _tourName;
            set
            {
                _tourName = value;
                OnPropertyChanged(nameof(TourName));
            }
        }
        public string TourDescription
        {
            get => _tourDescription;
            set
            {
                _tourDescription = value;
                OnPropertyChanged(nameof(TourDescription));
            }
        }
        public string Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged(nameof(Start));
            }
        }
        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }
        public string TransportType
        {
            get => _transportType;
            set
            {
                _transportType = value;
                OnPropertyChanged(nameof(TransportType));
            }
        }
        public decimal TourDistance
        {
            get => _tourDistance;
            set
            {
                _tourDistance = value;
                OnPropertyChanged(nameof(TourDistance));
            }
        }
        public string Time
        {
            get => _timeEstimation;
            set
            {
                _timeEstimation = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        public string ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                OnPropertyChanged(nameof(ImageName));
            }
        }

        public string Popularity
        {
            get => _popularity;
            set
            {
                _popularity = value;
                OnPropertyChanged(nameof(Popularity));
            }
        }

        public string ChildFriendliness
        {
            get => _childFriendliness;
            set
            {
                _childFriendliness = value;
                OnPropertyChanged(nameof(ChildFriendliness));
            }
        }

        public string Favourite
        {
            get => _favourite;
            set
            {
                _favourite = value;
                OnPropertyChanged(nameof(Favourite));
            }
        }

        public TourData(string tourName, string transportType)
        {
            TourName = tourName;
            TransportType = transportType;
            Favourite = "false";
            ChildFriendliness = "low";
            Popularity = "low";
        }

        public TourData(int id, string tourName, string transportType)
        {
            ID = id;
            TourName = tourName;
            TransportType = transportType;
            Favourite = "false";
            ChildFriendliness = "low";
            Popularity = "low";
        }

        public TourData(int id, string tourName, string description, string start, string destination, string transportType, decimal distance, string estimated_time, string imageName, string popularity, string childFriendliness, string favourite)
        {
            ID = id;
            TourName = tourName;
            TourDescription = description;
            Start = start;
            Destination = destination;
            TransportType = transportType;
            TourDistance = distance;
            Time = estimated_time;
            ImageName = imageName;
            Popularity = popularity;
            ChildFriendliness = childFriendliness;
            Favourite = favourite;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
