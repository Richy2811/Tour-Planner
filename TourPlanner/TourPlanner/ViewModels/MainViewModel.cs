using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TourPlanner.DAL;
using TourPlanner.Models;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;


namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MenuBarViewModel menuBarViewModel;
        private readonly SingleLineSearchBarViewModel singleLineSearchBarViewModel;
        private readonly TitleAndDescriptionViewModel titleAndDescriptionViewModel;
        private readonly TourListViewModel tourListViewModel;
        private readonly TourLogViewModel tourLogViewModel;

        public MainViewModel(MenuBarViewModel menuBarViewModel, SingleLineSearchBarViewModel singleLineSearchBarViewModel, TitleAndDescriptionViewModel titleAndDescriptionViewModel, TourListViewModel tourListViewModel, TourLogViewModel tourLogViewModel)
        {
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
            };
        }
    }
}
