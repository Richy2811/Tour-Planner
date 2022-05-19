using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.BL;
using System.Windows;
using System.IO;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class TourLogViewModel : BaseViewModel
    {
        public ObservableCollection<LogEntry> LogData { get; }
           = new ObservableCollection<LogEntry>();

        public RelayCommand AddLog { get; }
        public RelayCommand DeleteLog { get; }
        public RelayCommand SaveLog { get; }

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

            SaveLog = new RelayCommand((_) =>
            {
                SaveLogs();
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

            
        }

        public async void LoadLogs(int id)
        {
            LogData.Clear();
            Dictionary<string, object> data = await GetData.GetAllTourLogData(id);

            int index = 0;
            if(data != null)
            {
                bool exists = data.ContainsKey("date_time" + index);
                while (exists)
                {
                    LogData.Add(new LogEntry((int)data["id" + index], (string)data["date_time" + index], (string)data["comment" + index], (string)data["difficulty" + index], (string)data["total_time" + index], (string)data["rating" + index]));
                    index++;
                    exists = data.ContainsKey("date_time" + index);

                }

            }
        }

        private async void SaveLogs()
        {
            bool success = await SaveData.SaveLogInfo(LogData, TourID);
            MessageBox.Show("Successfully saved Logs!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void DeleteLogFromDB(int logID)
        {
            bool success = await DeleteData.DeleteTour(logID);
        }
    }
}
