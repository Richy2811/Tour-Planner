using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using TourPlanner.DAL;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;

namespace TourPlanner.ViewModels
{
    public class TitleAndDescriptionViewModel : BaseViewModel
    {
        private TourData _tourInfo = new TourData(null, null);
        private string _routeDisplay;
        private string _descriptionDisplay;
        private BitmapImage _staticMapImage;

        public TourData TourInfo
        {
            get => _tourInfo;
        }
        public string RouteDisplay
        {
            get => _routeDisplay;
            set
            {
                _routeDisplay = value;
                OnPropertyChanged(nameof(RouteDisplay));
            }
        }
        public string DescriptionDisplay
        {
            get => _descriptionDisplay;
            set
            {
                _descriptionDisplay = value;
                OnPropertyChanged(nameof(DescriptionDisplay));
            }
        }
        public BitmapImage StaticMapImage
        {
            get => _staticMapImage;
            set
            {
                _staticMapImage = value;
                OnPropertyChanged(nameof(StaticMapImage));
            }
        }

        public RelayCommand ShowRoute { get; }
        public RelayCommand ShowDescription { get; }
        public RelayCommand SaveTour { get; }

        private string GetUriPath(string imageName)
        {
            return Path.GetFullPath($"{Environment.CurrentDirectory}../../../../TourImages/{imageName}");
        }

        public void UpdateTourImage()
        {
            StaticMapImage = new BitmapImage(new Uri(GetUriPath(TourInfo.ImageName ?? "Placeholder.png"), UriKind.Absolute));
        }

        public event EventHandler<TourData> OnSaveUpdate;

        public TitleAndDescriptionViewModel()
        {
            //bind "Route" and "Description" buttons to set visibility of components
            ShowRoute = new RelayCommand((_) =>
            {
                RouteDisplay = "Visible";
                DescriptionDisplay = "Collapsed";
            });

            ShowDescription = new RelayCommand((_) =>
            {
                RouteDisplay = "Collapsed";
                DescriptionDisplay = "Visible";
            });

            SaveTour = new RelayCommand((_) =>
            {
                //retrieve mapquest return value as json object
                Task<JObject> mapQuestJsonReturn = Task.Run(() => MapQuestDirection.GetRouteInfoAsync(TourInfo));

                //check if route was successfully generated
                if ((int)mapQuestJsonReturn.Result["info"]["statuscode"] != 0)
                {
                    //show error message to user
                    MessageBox.Show("Unable to create tour using given parameters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //store generated values of interest
                _tourInfo.TourDistance = Math.Round((decimal)mapQuestJsonReturn.Result["route"]["distance"], 2);
                _tourInfo.Time = (string)mapQuestJsonReturn.Result["route"]["formattedTime"];

                //store image locally and save filename as reference
                string sessionId = (string)mapQuestJsonReturn.Result["route"]["sessionId"];
                _tourInfo.ImageName = MapQuestDirection.GetRouteImageName(sessionId);

                //save TourInfo in DB
                SaveTourInfo(_tourInfo);

                
            });

            //set initial visibility of components
            RouteDisplay = "Visible";
            DescriptionDisplay = "Collapsed";

            //set placeholder tour image
            TourInfo.ImageName = "Placeholder.png";
            UpdateTourImage();


        }

        private async void SaveTourInfo(TourData Info)
        {
            if (Info.TourDescription == null)
                Info.TourDescription = "";
            bool success = await SaveData.SaveTourInfo(Info);

            if (success)
            {
                //update image
                UpdateTourImage();

                //synchronize input values with tour list
                OnSaveUpdate?.Invoke(this, TourInfo);
                MessageBox.Show("Successfully saved Tour!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Tour Name already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
