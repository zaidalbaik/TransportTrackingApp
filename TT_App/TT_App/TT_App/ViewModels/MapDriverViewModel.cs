using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace TT_App.ViewModels
{
    public class MapDriverViewModel : BaseViewModel
    {
        public Command RefreshLocationCommand { get; }
        public Command FinishTheTripCommand { get; }

        public Command ShowRouteOnMapCommand { get; }

        public Xamarin.Forms.GoogleMaps.Map MapDriver { get; private set; }

        public Pin Pin { get; set; }

        private bool IsMoves { get; set; }


        public MapDriverViewModel()
        {
            IsMoves = true;
            MapDriver = new Xamarin.Forms.GoogleMaps.Map();

            Pin = new Pin()
            {
                Position = new Position(31.9539, 35.9106),
                Label = $"{GetDriverInfo().DriverName}",
                Icon = BitmapDescriptorFactory.FromBundle("bus")
            };
            MapDriver.Pins.Add(Pin);

            MapDriver.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(31.9539, 35.9106), Xamarin.Forms.GoogleMaps.Distance.FromMiles(1)), true);

            tripService = new TripService();
            busService = new BusService();
            stationServices = new StationServices();
            directionService = new DirectionService();

            ShowRouteOnMapCommand = new Command(async () => await ShowRouteOnMap());
            ShowRouteOnMapCommand.Execute(null);

            RefreshLocationCommand = new Command(async () => await UpdateLocaitonAsync());
            RefreshLocationCommand.Execute(null);


            FinishTheTripCommand = new Command(async () => await FinishTheTrip(), () => !IsBusy);

        }
        public double FullSecreen => DeviceDisplay.MainDisplayInfo.Height;

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged();
            }
        }
        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged();
            }
        }

        public Driver GetDriverInfo()
        {
            try
            {
                Driver driver = JsonConvert.DeserializeObject<Driver>(Preferences.Get("DriverInfo", ""));
                return driver;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Line GetLineSelected()
        {
            try
            {
                return JsonConvert.DeserializeObject<Line>(Preferences.Get("SelectedLine", ""));

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Station> GetDepartureStationAsync()
        {
            try
            {
                var DepartureStation = await stationServices.GetStationAsync(GetLineSelected().Dep_StationID);
                //add departure staiton pin on the map  
                MapDriver.Pins.Add(new Pin()
                {
                    Position = new Position(DepartureStation.Lat, DepartureStation.Lon),
                    Label = $"{DepartureStation.StationName}"
                });

                return DepartureStation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Station> GetArrivalStationAsync()
        {
            try
            {
                var ArrivalStation = await stationServices.GetStationAsync(GetLineSelected().Arr_StationID);
                //add arrival staiton pin on the map  
                MapDriver.Pins.Add(new Pin()
                {
                    Position = new Position(ArrivalStation.Lat, ArrivalStation.Lon),
                    Label = $"{ArrivalStation.StationName}"
                });

                return ArrivalStation;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Position>> GetPositionsAsync()
        {
            try
            {
                var DepStation = await GetDepartureStationAsync();
                var ArrStation = await GetArrivalStationAsync();

                var direction = await directionService.GetDirectionAsync(new Position(DepStation.Lat, DepStation.Lon),
                     new Position(ArrStation.Lat, ArrStation.Lon));
                if (direction.Routes != null && direction.Routes.Count > 0)
                {
                    var positions = Enumerable.ToList(PolylineHelper.Decode(direction.Routes.First().Overview_polyline.Points));
                    return positions;
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error in GetPositionsAsync Method Error Message : {e.Message}");
                return null;
            }
        }

        public async Task ShowRouteOnMap()
        {
            try
            {
                var polyline = new Xamarin.Forms.GoogleMaps.Polyline()
                {
                    StrokeColor = Color.Purple,
                    StrokeWidth = 6,
                };

                var ListOfPositions = await GetPositionsAsync();
                foreach (var item in ListOfPositions)
                {
                    polyline.Positions.Add(item);
                }

                double distance = await GetDistanceForLineSelected();
                MapDriver.Polylines.Add(polyline);

                MapDriver.MoveToRegion(MapSpan.FromCenterAndRadius(polyline.Positions[polyline.Positions.ToList().Count / 2], Xamarin.Forms.GoogleMaps.Distance.FromKilometers(distance)), true);


            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<double> GetDistanceForLineSelected()
        {
            var arrSt = await GetArrivalStationAsync();
            var depSt = await GetDepartureStationAsync();

            return HaversineDistance(arrSt.Lat, arrSt.Lon, depSt.Lat, depSt.Lon);
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

        public async Task UpdateLocaitonAsync()
        {
            try
            {
                while (IsMoves)
                {
                    var location = await Geolocation.GetLocationAsync(new
                          GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(15)));

                    if (location != null)
                    {
                        await busService.EditLatLonForBus(GetDriverInfo().BusId, new Bus() { Lat = location.Latitude, Lon = location.Longitude });
                        Pin.Position = new Position(location.Latitude, location.Longitude);
                        OnPropertyChanged(nameof(Pin));
                        Latitude = location.Latitude;
                        Longitude = location.Longitude;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public async Task FinishTheTrip()
        {
            try
            {
                IsBusy = true;
                IsMoves = false;
                OnPropertyChanged(nameof(IsMoves));

                var TripInfo = JsonConvert.DeserializeObject<Trip>(Preferences.Get("TripInfo", ""));
                TripInfo.Arrival_time = DateTime.Now;
                await tripService.SaveTripAsync(TripInfo);

                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Save failed");
                throw;
            }
            finally
            {
                IsBusy = false;
            }

        }

        public override void CommandStateChanged()
        {
            FinishTheTripCommand?.ChangeCanExecute();
        }
    }
}
