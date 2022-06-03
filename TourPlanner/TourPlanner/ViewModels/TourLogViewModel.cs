using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel : BaseViewModel
    {
        public ObservableCollection<LogEntry> LogData { get; } = new ObservableCollection<LogEntry>();
        
        private string _logSearchText;
        public string LogSearchText
        {
            get => _logSearchText;
            set
            {
                _logSearchText = value;
                OnPropertyChanged(nameof(LogSearchText));
            }
        }

        public RelayCommand AddLog { get; }
        public RelayCommand DeleteLog { get; }
        public RelayCommand SaveLog { get; }
        public RelayCommand SearchLog { get; }

        private int _tourID;

        private LogEntry _selectedItem;

        public LogEntry SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SelectedItem == value)
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public int TourID
        {
            get => _tourID;

            set
            {
                _tourID = value;
            }
        }

        public TourLogViewModel()
        {
            //search text must not be null
            _logSearchText = "";

            AddLog = new RelayCommand((_) =>
            {
                string[] info;
                string IdFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\logid.txt";
                if (!File.Exists(IdFile))
                {
                    throw new ArgumentException("Does not exist.", nameof(IdFile));
                }

                IEnumerable<string> line = File.ReadLines(IdFile);
                Console.WriteLine(string.Join(Environment.NewLine, line));
                info = line.ToArray();
                int id = Int32.Parse(info[0]);

                id++;

                using (StreamWriter writeNewID = new StreamWriter(IdFile))
                {
                    writeNewID.WriteLine(id.ToString());
                }

                LogData.Add(new LogEntry( id,"empty", "empty", "empty", "empty", "10"));
            });

            DeleteLog = new RelayCommand((_) =>
            {
                if (SelectedItem == null)
                {
                    return;
                }
                foreach (LogEntry element in LogData)
                {
                    if (SelectedItem.ID == element.ID)
                    {
                        //remove selected item and break statement to prevent an exception throw (property changed during iteration)
                        LogData.Remove(element);
                        DeleteLogFromDB(element.ID);
                        break;
                    }
                }
            });

            SaveLog = new RelayCommand((_) =>
            {
                SaveLogs();
            });

            SearchLog = new RelayCommand(async (_) =>
            {
                //reload all log data into collection
                await LoadLogs(_tourID);
                
                for (int i = LogData.Count - 1; i >= 0; i--)
                {
                    //if none of the strings in a log matches the search string (case insensitive) -> remove it from view
                    if (LogData[i].Date.IndexOf(_logSearchText, StringComparison.CurrentCultureIgnoreCase) < 0  &&
                        LogData[i].Duration.IndexOf(_logSearchText, StringComparison.CurrentCultureIgnoreCase) < 0 &&
                        LogData[i].Comment.IndexOf(_logSearchText, StringComparison.CurrentCultureIgnoreCase) < 0 &&
                        LogData[i].Difficulty.IndexOf(_logSearchText, StringComparison.CurrentCultureIgnoreCase) < 0 &&
                        LogData[i].Rating.IndexOf(_logSearchText, StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        LogData.Remove(LogData[i]);
                    }
                }
            });
        }

        public async Task LoadLogs(int id)
        {
            LogData.Clear();
            Dictionary<string, object> data = await GetData.GetAllTourLogData(id);

            int index = 0;
            if(data != null)
            {
                bool exists = data.ContainsKey("date_time" + index);
                while (exists)
                {
                    //tour selection changed after starting async method
                    if (id != _tourID)
                    {
                        //end task prematurely if tour selection changes (prevents log entries from previous id being loaded in addition)
                        return;
                    }

                    this.LogData.Add(new LogEntry((int)data["id" + index], (string)data["date_time" + index], (string)data["comment" + index], (string)data["difficulty" + index], (string)data["total_time" + index], (string)data["rating" + index]));
                    index++;
                    exists = data.ContainsKey("date_time" + index);
                }
            }
        }

        public async void SaveLogs()
        {
            bool success = await SaveData.SaveLogInfo(LogData, TourID);
            MessageBox.Show("Successfully saved Logs!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public async void DeleteLogFromDB(int logID)
        {
            bool success = await DeleteData.DeleteLog(logID);
            if (LogData.Count > 0)
            {
                SelectedItem = LogData[0];
            }
            if (success)
            {
                MessageBox.Show("Deletion successful!", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
