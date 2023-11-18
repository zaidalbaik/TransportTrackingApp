using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TT_App.Services;
 

namespace TT_App.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {

        public TripService tripService;

        public BusService busService;

        public StationServices stationServices;

        public DirectionService directionService;

        public DriverService driverService;

        public LineService lineService;

        public UsersService usersService;

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                CommandStateChanged();
                OnPropertyChanged();
            }
        }

        public abstract void CommandStateChanged();


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }


        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnCollectionChanged([CallerMemberName] string propname = null)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, propname));
        }
    }
}
