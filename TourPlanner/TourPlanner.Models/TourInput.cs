﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourInput : INotifyPropertyChanged
    {
        private string _tourName;
        private string _tourDescription;
        private string _start;
        private string _destination;
        private string _transportType;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}