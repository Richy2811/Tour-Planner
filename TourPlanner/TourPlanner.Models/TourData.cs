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
        private string _tourName;
        private string _tourDescription;
        private string _start;
        private string _destination;
        private string _transportType;
        private int _tourDistance;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TourName
        {
            get
            {
                return _tourName;
            }
            set
            {
                _tourName = value;
                OnPropertyChanged(nameof(TourName));
            }
        }
        public string TourDescription {
            get
            {
                return _tourDescription;
            }
            set
            {
                _tourDescription = value;
                OnPropertyChanged(nameof(TourDescription));
            }
        }
        public string Start {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                OnPropertyChanged(nameof(Start));
            }
        }
        public string Destination {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }
        public string TransportType {
            get
            {
                return _transportType;
            }
            set
            {
                _transportType = value;
                OnPropertyChanged(nameof(TransportType));
            }
        }
        public int TourDistance {
            get
            {
                return _tourDistance;
            }
            set
            {
                _tourDistance = value;
                OnPropertyChanged(nameof(TourDistance));
            }
        }

        public TourData(string tourName, string tourDescription, string start, string destination, string transportType, int tourDistance)
        {
            TourName = tourName;
            TourDescription = tourDescription;
            Start = start;
            Destination = destination;
            TransportType = transportType;
            TourDistance = tourDistance;
        }
    }
}
