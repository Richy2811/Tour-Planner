using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace TourPlanner
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<LogEntry> LogData { get; }
            = new ObservableCollection<LogEntry>();

        public ObservableCollection<TourEntry> TourData { get; }
           = new ObservableCollection<TourEntry>();

        public string CurrentTitle { get; set; }
        public RelayCommand AddLog { get; }
        public RelayCommand DeleteLog { get; }

       public RelayCommand AddTour { get; }


        public MainViewModel()
        {
            AddLog = new RelayCommand((_) =>
            {
                LogData.Add(new LogEntry("empty", "empty", "empty"));
                
            });

            DeleteLog = new RelayCommand((_) =>
            {
                int last = LogData.Count;
                LogData.RemoveAt(last-1);
                OnPropertyChanged();
                CurrentTitle = string.Empty;
                OnPropertyChanged(nameof(CurrentTitle));

            });

            LoadLogs();
            LoadTours();
        }

        private void LoadLogs()
        {
            LogData.Add(new LogEntry("10.02.2022", "01:20:34", "100km"));
            LogData.Add(new LogEntry("12.03.2022", "02:30:22", "230km"));
        }

        private void LoadTours()
        {
            TourData.Add(new TourEntry("Tour1"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
