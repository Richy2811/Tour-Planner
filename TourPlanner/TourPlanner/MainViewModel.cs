using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace TourPlanner
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<LogEntry> Data { get; }
            = new ObservableCollection<LogEntry>();

        public string CurrentTitle { get; set; }
        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }


        public MainViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
                Data.Add(new LogEntry("empty", "empty", "empty"));

            });

            /*DeleteCommand = new RelayCommand((_) =>
            {
                int last = Data.Count;
                Data.RemoveAt(last);
                //OnPropertyChanged(nameof(CurrentTitle));

            });*/

            LoadLogs();
        }

        private void LoadLogs()
        {
            Data.Add(new LogEntry("10.02.2022", "01:20:34", "100km"));
            Data.Add(new LogEntry("12.03.2022", "02:30:22", "230km"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
