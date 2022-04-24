using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
        private int i;
        public ObservableCollection<TourData> TourListCollection { get; set; } = new ObservableCollection<TourData>();
        private TourData _selectedItem;

        public TourData SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SelectedItem == value)
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OnSelectUpdate?.Invoke(this, SelectedItem);
            }
        }

        public event EventHandler<TourData> OnSelectUpdate;

        public RelayCommand AddTour { get; }
        public RelayCommand DeleteTour { get; }

        public TourListViewModel()
        {
            i = 0;
            AddTour = new RelayCommand((_) =>
            {
                TourListCollection.Add(new TourData($"SampleTour{i++}", "Sampledescription", "Samplestart", "Sampledestination", "Sampletransporttype"));
            });

            DeleteTour = new RelayCommand((_) =>
            {
                if (SelectedItem == null)
                {
                    return;
                }
                foreach (TourData element in TourListCollection)
                {
                    if (SelectedItem.TourName == element.TourName)
                    {
                        //remove selected item and break statement to prevent an exception throw (property changed during iteration)
                        TourListCollection.Remove(element);
                        break;
                    }
                }
            });
        }
    }
}
