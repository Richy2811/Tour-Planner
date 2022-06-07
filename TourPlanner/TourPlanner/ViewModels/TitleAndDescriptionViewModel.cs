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
using System.Collections.Generic;
using TourPlanner.Logging;

namespace TourPlanner.ViewModels
{
    public class TitleAndDescriptionViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger("TitleAndDescriptionModel");

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
            logger.Debug("TitleAndDescriptionViewModelCreated()");
            //bind "Route" and "Description" buttons to set visibility of components
            ShowRoute = new RelayCommand((_) =>
            {
                logger.Debug("showRoute()");
                RouteDisplay = "Visible";
                DescriptionDisplay = "Collapsed";
            });

            ShowDescription = new RelayCommand((_) =>
            {
                logger.Debug("showDes()");
                RouteDisplay = "Collapsed";
                DescriptionDisplay = "Visible";
            });

            SaveTour = new RelayCommand((_) =>
            {
                //retrieve mapquest return value as json object
                JObject mapQuestJsonReturn = GetRouteInfo.GetJson(TourInfo);

                //check if route was successfully generated
                if ((int)mapQuestJsonReturn["info"]["statuscode"] != 0)
                {
                    //show error message to user
                    MessageBox.Show("Unable to create tour using given parameters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //store generated values of interest
                _tourInfo.TourDistance = Math.Round((decimal)mapQuestJsonReturn["route"]["distance"], 2);
                _tourInfo.Time = (string)mapQuestJsonReturn["route"]["formattedTime"];

                //store image locally and save filename as reference
                string sessionId = (string)mapQuestJsonReturn["route"]["sessionId"];
                _tourInfo.ImageName = GetRouteInfo.GetImageName(sessionId);

                //save TourInfo in DB
                SaveTourInfo(_tourInfo);
                logger.Debug("saveTour()");
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

        public void ClearView()
        {
            TourInfo.ID = 0;
            TourInfo.TourName = null;
            TourInfo.TourDescription = null;
            TourInfo.Start = null;
            TourInfo.Destination = null;
            TourInfo.TransportType = null;
            TourInfo.TourDistance = 0;
            TourInfo.Time = null;
            TourInfo.Popularity = null;
            TourInfo.ChildFriendliness = null;
            TourInfo.Favourite = "false";
            TourInfo.IsUpdating = false;

            RouteDisplay = "Visible";
            DescriptionDisplay = "Collapsed";

            TourInfo.ImageName = "Placeholder.png";
            UpdateTourImage();
        }
    }
}
