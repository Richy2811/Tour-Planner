using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
        public ObservableCollection<TourData> TourListCollection { get; } = new ObservableCollection<TourData>();
        public TourData SelectedItem { get; set; }

        public RelayCommand AddTour { get; }
        public RelayCommand DeleteTour { get; }

        public TourListViewModel()
        {
            AddTour = new RelayCommand((_) =>
            {
                TourListCollection.Add(new TourData("Samplename", "Sampledescription", "Samplestart", "Sampledestination", "Sampletransporttype", 10));
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
                        TourListCollection.Remove(element);
                        break;
                    }
                }
            });
        }
    }
}
