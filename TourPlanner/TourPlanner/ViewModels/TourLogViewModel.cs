using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;

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
                LogData.Add(new LogEntry("empty", "empty", "empty", "empty", "10"));

            });

            DeleteLog = new RelayCommand((_) =>
            {
                int last = LogData.Count;
                LogData.RemoveAt(last - 1);
                OnPropertyChanged();

            });

            LoadLogs();
        }

        private async void LoadLogs()
        {
            //Dictionary<string, object> test = {("id", "1"), }

            Dictionary<string, object> data = await GetData.GetAllTourLogData();

            int index = 0;
            if(data != null)
            {
                bool exists = data.ContainsKey("date_time" + index);
                while (exists)
                {
                    LogData.Add(new LogEntry((string)data["date_time" + index], (string)data["comment" + index], (string)data["difficulty" + index], (string)data["total_time" + index], (string)data["rating" + index]));
                    index++;
                    exists = data.ContainsKey("date_time" + index);

                }

            }
        }
    }
}
