using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
        public ObservableCollection<TourData> TourListCollection { get; set; } = new ObservableCollection<TourData>();

        private string _tourSearchText;

        public string _favButton = "<3";
        public string TourSearchText
        {
            get => _tourSearchText;
            set
            {
                _tourSearchText = value;
                OnPropertyChanged(nameof(TourSearchText));
            }
        }

        public string FavButton
        {
            get => _favButton;
            set
            {
                _favButton = value;
                OnPropertyChanged(nameof(FavButton));
            }
        }

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
                if (SelectedItem == null)
                {
                    return;
                }
                OnchangeUpdateID?.Invoke(this, SelectedItem.ID);
            }
        }

        public event EventHandler<TourData> OnSelectUpdate;
        public event EventHandler<int> OnchangeUpdateID;

        public RelayCommand AddTour { get; }
        public RelayCommand DeleteTour { get; }
        public RelayCommand SearchTour { get; }
        public RelayCommand ShowFav { get; }

        public TourListViewModel()
        {
            //search text must not be null
            _tourSearchText = "";

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
                    //remove selected item and break statement to prevent an exception throw (property changed during iteration)
                    if (SelectedItem.ID == element.ID)
                    {
                        //set selected item to null to prevent event handler from asynchronously loading tours into database while deleting from database
                        SelectedItem = null;
                        TourListCollection.Remove(element);
                        DeleteTourFromDB(element.ID);
                        break;
                    }
                }
            });

            SearchTour = new RelayCommand(async (_) =>
            {
                //reload all tour entries into collection
                await ReadToursFromDB();

                for (int i = TourListCollection.Count - 1; i >= 0; i--)
                {
                    //remove tour from collection if its name matches the search string
                    if (TourListCollection[i].TourName.IndexOf(_tourSearchText, StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        TourListCollection.Remove(TourListCollection[i]);
                    }
                }
            });

            ShowFav = new RelayCommand(async (_) =>
            {
                if(FavButton == "<3")
                {
                    FavButton = "all";
                    await ReadToursFromDB();

                    for (int i = TourListCollection.Count - 1; i >= 0; i--)
                    {
                        //remove tour from collection if its name matches the search string
                        if (TourListCollection[i].Favourite == "false")
                        {
                            TourListCollection.Remove(TourListCollection[i]);
                        }
                    }
                }

                else if(FavButton == "all")
                {
                    FavButton = "<3";
                    await ReadToursFromDB();
                }
            });
        }

        private async Task ReadToursFromDB()
        {
            _selectedItem = null;
            TourListCollection.Clear();

            Dictionary<string, object> data = await GetData.GetAllTours();

            int index = 0;
            if (data != null)
            {
                bool exists = data.ContainsKey("id" + index);
                while (exists)
                {
                    string friendliness = await ComputeTourLogInfo.ComputeChildFriendliness((int)data["id" + index]);
                    string popularity = await ComputeTourLogInfo.ComputePopularity((int)data["id" + index]);
                    TourListCollection.Add(new TourData((int)data["id" + index], (string)data["name" + index], (string)data["description" + index], (string)data["start" + index], (string)data["destination" + index], 
                        (string)data["transport_type" + index], (int)data["distance" + index], (string)data["estimated_time" + index], (string)data["image" + index], popularity, friendliness, (string)data["favourite" + index]));
                    index++;
                    exists = data.ContainsKey("id" + index);
                }
            }
        }

        private async void DeleteTourFromDB(int tourID)
        {
            bool success = await DeleteData.DeleteTour(tourID);
            //after async database access is complete -> change selection in list if collection contains at least one entry
            if (TourListCollection.Count > 0)
            {
                SelectedItem = TourListCollection[0];
            }

            if (success)
            {
                MessageBox.Show("Deletion successful!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
