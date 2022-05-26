using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.ViewModels;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // MVVM:
            //var searchBarViewModel = new SearchBarViewModel();
            var menuBarViewModel = new MenuBarViewModel();
            var titleAndDescriptionViewModel = new TitleAndDescriptionViewModel();
            var tourListViewModel = new TourListViewModel();
            var tourLogViewModel = new TourLogViewModel();


            var wnd = new MainWindow
            {
                DataContext = new MainViewModel(menuBarViewModel, titleAndDescriptionViewModel, tourListViewModel, tourLogViewModel),
                MenuBar = { DataContext = menuBarViewModel},
                TourList = { DataContext = tourListViewModel},
                TitleAndDescription = { DataContext = titleAndDescriptionViewModel},
                TourLog = { DataContext = tourLogViewModel}
            };

            wnd.Show();
        }
    }
}
