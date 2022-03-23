using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel : BaseViewModel
    {
        public ObservableCollection<LogEntry> LogData { get; }
           = new ObservableCollection<LogEntry>();

        public RelayCommand AddLog { get; }
        public RelayCommand DeleteLog { get; }

        public TourLogViewModel()
        {
            AddLog = new RelayCommand((_) =>
            {
                LogData.Add(new LogEntry("empty", "empty", "empty"));

            });

            DeleteLog = new RelayCommand((_) =>
            {
                int last = LogData.Count;
                LogData.RemoveAt(last - 1);
                OnPropertyChanged();

            });

            LoadLogs();
        }

        private void LoadLogs()
        {
            LogData.Add(new LogEntry("10.02.2022", "01:20:34", "100km"));
            LogData.Add(new LogEntry("12.03.2022", "02:30:22", "230km"));
        }
    }
}
