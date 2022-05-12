using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TourPlanner.BL;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
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
            ReadToursFromDB();
            AddTour = new RelayCommand((_) =>
            {
                string[] info;
                string IdFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\id.txt";
                if (!File.Exists(IdFile))
                {
                    throw new ArgumentException("Does not exist.", nameof(IdFile));
                }

                IEnumerable<string> line = File.ReadLines(IdFile);
                Console.WriteLine(string.Join(Environment.NewLine, line));
                info = line.ToArray();
                int id = Int32.Parse(info[0]);

                id++;

                using (StreamWriter writeNewID = new StreamWriter(IdFile))
                {
                    writeNewID.WriteLine(id.ToString());
                }

                TourListCollection.Add(new TourData(id,$"SampleTour{id}", "Bicycle"));
            });

            DeleteTour = new RelayCommand((_) =>
            {
                if (SelectedItem == null)
                {
                    return;
                }
                foreach (TourData element in TourListCollection)
                {
                    if (SelectedItem.ID == element.ID)
                    {
                        //remove selected item and break statement to prevent an exception throw (property changed during iteration)
                        TourListCollection.Remove(element);
                        DeleteTourFromDB(element.ID);
                        break;
                    }
                }
            });
        }

        private async void ReadToursFromDB()
        {
            Dictionary<string, object> data = await GetData.GetAllTours();

            int index = 0;
            if (data != null)
            {
                bool exists = data.ContainsKey("id" + index);
                while (exists)
                {
                    TourListCollection.Add(new TourData((int)data["id" + index], (string)data["name" + index], (string)data["description" + index], (string)data["start" + index], (string)data["destination" + index], 
                        (string)data["transport_type" + index], (int)data["distance" + index], (string)data["estimated_time" + index], (string)data["image" + index]));
                    index++;
                    exists = data.ContainsKey("id" + index);

                }

            }
        }

        private async void DeleteTourFromDB(int tourID)
        {
            bool success = await DeleteData.DeleteTour(tourID);
        }
    }
}
