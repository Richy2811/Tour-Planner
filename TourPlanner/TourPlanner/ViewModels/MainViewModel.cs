using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TourPlanner.DAL;
using TourPlanner.ViewModels.Abstract;



namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MenuBarViewModel menuBarViewModel;
        private readonly SingleLineSearchBarViewModel singleLineSearchBarViewModel;
        private readonly TitleAndDescriptionViewModel titleAndDescriptionViewModel;
        private readonly TourListViewModel tourListViewModel;
        private readonly TourLogViewModel tourLogViewModel;

        public string CurrentTitle { get; set; }
        
        public MainViewModel(MenuBarViewModel menuBarViewModel, SingleLineSearchBarViewModel singleLineSearchBarViewModel, TitleAndDescriptionViewModel titleAndDescriptionViewModel, TourListViewModel tourListViewModel, TourLogViewModel tourLogViewModel)
        {
            this.menuBarViewModel = menuBarViewModel;
            this.singleLineSearchBarViewModel = singleLineSearchBarViewModel;
            this.titleAndDescriptionViewModel = titleAndDescriptionViewModel;
            this.tourListViewModel = tourListViewModel;
            this.tourLogViewModel = tourLogViewModel;
        }
    }
}
