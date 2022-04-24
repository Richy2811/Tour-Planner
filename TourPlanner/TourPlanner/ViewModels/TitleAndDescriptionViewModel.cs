using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TitleAndDescriptionViewModel : BaseViewModel
    {
        private TourInput _tourInfo;

        public TourInput TourInfo
        {
            get => _tourInfo;
        }

        public TitleAndDescriptionViewModel()
        {
            _tourInfo = new TourInput();
            //subscribe to property-changed-handler
            _tourInfo.PropertyChanged += PropertyChangedHandler;
        }

        public event EventHandler<TourInput> OnchangeUpdate;
        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            //if property of TourInfo changes -> update view
            OnchangeUpdate?.Invoke(this, TourInfo);
        }
    }
}
