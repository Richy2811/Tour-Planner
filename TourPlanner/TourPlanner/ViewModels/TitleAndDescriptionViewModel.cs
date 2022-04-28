using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using TourPlanner.DAL;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TitleAndDescriptionViewModel : BaseViewModel
    {
        private TourData _tourInfo = new TourData(null, null);
        private string _routeDisplay;
        private string _descriptionDisplay;
        private string _mapQuestUrl;
        private string _mapQuestParameters;
        private Task<JObject> _mapQuestJsonReturn;
        
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

        public RelayCommand ShowRoute { get; }
        public RelayCommand ShowDescription { get; }
        public RelayCommand SaveTour { get; }

        public TitleAndDescriptionViewModel()
        {
            //subscribe to property-changed-handler
            _tourInfo.PropertyChanged += PropertyChangedHandler;

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
                _mapQuestUrl = "http://www.mapquestapi.com/directions/v2/route";
                string transportType = _tourInfo.TransportType;
                switch (transportType)
                {
                    case "Bicycle":
                        transportType = "bicycle";
                        break;

                    case "Walk":
                        transportType = "pedestrian";
                        break;

                    default:
                        transportType = "bicycle";
                        break;
                }
                _mapQuestParameters = $"?key=8Ls9YtZjQESK1vRFuUIEgCyRjAwWP3SI&from={_tourInfo.Start}&to={_tourInfo.Destination}&unit=k&routeType={transportType}&locale=de_DE";
                _mapQuestJsonReturn = Task.Run(() => MapQuestDirection.GetRouteInfoAsync(_mapQuestUrl + _mapQuestParameters));

                _tourInfo.TourDistance = (decimal)_mapQuestJsonReturn.Result["route"]["distance"];
                _tourInfo.Time = (string)_mapQuestJsonReturn.Result["route"]["formattedTime"];
                string sessionId = (string)_mapQuestJsonReturn.Result["route"]["sessionId"];

                //image generation
            });

            //set initial visibility of components
            RouteDisplay = "Visible";
            DescriptionDisplay = "Collapsed";
        }

        public event EventHandler<TourData> OnchangeUpdate;
        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            //if property of TourInfo changes -> update view
            OnchangeUpdate?.Invoke(this, TourInfo);
        }
    }
}
