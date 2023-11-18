using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class StationViewModel : BaseViewModel
    {
         
        private Command GetAllStationCommand { get; }
        public List<Station> Stations { get; set; }

        public Command GoToMapPageForStationCommand { get; }


        public StationViewModel()
        {
            stationServices = new StationServices();
            GetAllStationCommand = new Command(async () => await GetNearbyStations());
            GetAllStationCommand.Execute(null);

            GoToMapPageForStationCommand = new Command(async () => await GoToMapPageForStationAsync(), () => !IsBusy);
        }

        private Station station;
        public Station SelectedStation
        {
            get { return station; }
            set
            {
                station = value;
                OnPropertyChanged();
                GoToMapPageForStationCommand.Execute(null); 
            }
        }

        private async Task GetNearbyStations()
        {
            try
            {
                List<Station> AllStations = await stationServices.GetAllStationsAsync();
                Dictionary<Station, double> NearbyStations = new Dictionary<Station, double>();

                var Mylocation = await Geolocation.GetLocationAsync(new
                             GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(15)));

                foreach (var station in AllStations)
                {
                    NearbyStations.Add(station, HaversineDistance(Mylocation.Latitude, Mylocation.Longitude, station.Lat, station.Lon));
                }
                NearbyStations = NearbyStations.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                Stations = NearbyStations.Keys.ToList();
                OnPropertyChanged(nameof(Stations));
            }
            catch (Exception)
            {

                throw;
            }
        }     
        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var r = 6371; // Earth's radius in kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = r * c;

            return d;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }


        public async Task GoToMapPageForStationAsync()
        {
            try
            {
                if (SelectedStation == null)
                    return;

                IsBusy = true;
                var StationSelected = JsonConvert.SerializeObject(SelectedStation);
                Preferences.Set("StationChosenByUser", StationSelected);

                await App.Current.MainPage.Navigation.PushAsync(new MapStationPage());
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }

        }

        public override void CommandStateChanged()
        {
            GoToMapPageForStationCommand?.ChangeCanExecute();
        }


    }
}
