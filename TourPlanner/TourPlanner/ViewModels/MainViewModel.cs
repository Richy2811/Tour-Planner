using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TourPlanner.DAL;
using TourPlanner.Logging;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;


namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger("MainViewModel");


        private readonly MenuBarViewModel menuBarViewModel;
        private readonly SingleLineSearchBarViewModel singleLineSearchBarViewModel;
        private readonly TitleAndDescriptionViewModel titleAndDescriptionViewModel;
        private readonly TourListViewModel tourListViewModel;
        private readonly TourLogViewModel tourLogViewModel;

        public MainViewModel(MenuBarViewModel menuBarViewModel, SingleLineSearchBarViewModel singleLineSearchBarViewModel, TitleAndDescriptionViewModel titleAndDescriptionViewModel, TourListViewModel tourListViewModel, TourLogViewModel tourLogViewModel)
        {
            
            logger.Debug("created()");

            this.menuBarViewModel = menuBarViewModel;
            this.singleLineSearchBarViewModel = singleLineSearchBarViewModel;
            this.titleAndDescriptionViewModel = titleAndDescriptionViewModel;
            this.tourListViewModel = tourListViewModel;
            this.tourLogViewModel = tourLogViewModel;


            titleAndDescriptionViewModel.OnchangeUpdate += (_, tourInput) =>
            {
                SynchronizeViewModelData.SynchronizeTitleDescriptionTourList(tourInput, tourListViewModel.SelectedItem);
            };

            tourListViewModel.OnSelectUpdate += (_, tourSelection) =>
            {
                SynchronizeViewModelData.SynchronizeTourListTitleDescription(tourSelection, titleAndDescriptionViewModel.TourInfo);
                //update image
                titleAndDescriptionViewModel.UpdateTourImage();
                
            };

            tourListViewModel.OnchangeUpdateID += (_, id) =>
            {
                tourLogViewModel.TourID = id;
                tourLogViewModel.LoadLogs(id);
            };

        }
    }
}
