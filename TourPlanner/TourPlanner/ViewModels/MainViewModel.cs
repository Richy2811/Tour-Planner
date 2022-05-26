using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
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
        private readonly TitleAndDescriptionViewModel titleAndDescriptionViewModel;
        private readonly TourListViewModel tourListViewModel;
        private readonly TourLogViewModel tourLogViewModel;

        public MainViewModel(MenuBarViewModel menuBarViewModel, TitleAndDescriptionViewModel titleAndDescriptionViewModel, TourListViewModel tourListViewModel, TourLogViewModel tourLogViewModel)
        {
            
            logger.Debug("created()");

            this.menuBarViewModel = menuBarViewModel;
            this.titleAndDescriptionViewModel = titleAndDescriptionViewModel;
            this.tourListViewModel = tourListViewModel;
            this.tourLogViewModel = tourLogViewModel;

            menuBarViewModel.OnClickGenerate += (_, exportSingle) =>
            {
                switch (exportSingle ? ExportPDF.GeneratePdfSingle(tourListViewModel.SelectedItem) : ExportPDF.GeneratePdfAll())
                {
                    case 0:
                        MessageBox.Show("PDF document successfully created", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case 1:
                        MessageBox.Show("Please select a tour in the list first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case 2:
                        MessageBox.Show("Tour contains invalid values. Make sure to save the tour first", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            };

            titleAndDescriptionViewModel.OnSaveUpdate += (_, tourInput) =>
            {
                SynchronizeViewModelData.SynchronizeTitleDescriptionTourList(tourInput, tourListViewModel.SelectedItem);
            };

            tourListViewModel.OnSelectUpdate += (_, tourSelection) =>
            {
                SynchronizeViewModelData.SynchronizeTourListTitleDescription(tourSelection, titleAndDescriptionViewModel.TourInfo);
                //update image
                titleAndDescriptionViewModel.UpdateTourImage();
                
            };

            tourListViewModel.OnchangeUpdateID += async (_, id) =>
            {
                tourLogViewModel.TourID = id;
                await tourLogViewModel.LoadLogs(id);
            };

        }
    }
}
