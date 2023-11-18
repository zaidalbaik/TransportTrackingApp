using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class MapPageViewModel : BaseViewModel
    {

        //Full screen of map 
        public double FullSecreen => DeviceDisplay.MainDisplayInfo.Height;

        public Command ShowPinsOnMapUserCommand { get; }

        public Command ShowRouteOnMapUserCommand { get; }
        private Command TrackingBusCommand { get; }
        public Xamarin.Forms.GoogleMaps.Map Map { get; private set; }


        public ObservableCollection<Pin> Pins { get; set; }



        public MapPageViewModel()
        {

            busService = new BusService();

            stationServices = new StationServices();

            directionService = new DirectionService();


            Map = new Xamarin.Forms.GoogleMaps.Map();

            Map.MyLocationEnabled = true;
            Map.UiSettings.MyLocationButtonEnabled = true;




            ShowPinsOnMapUserCommand = new Command(async () =>
            {
                // if change the type from List<Bus> to var  will get an error
                List<Bus> ActiveBuses = await GetActiveBuses();
                if (ActiveBuses != null)
                {
                    foreach (var item in ActiveBuses)
                    {
                        Map.Pins.Add(new Pin()
                        {
                            Label = $" Bus Id : {item.BusID}",
                            Position = new Position(item.Lat, item.Lon),
                            Icon = BitmapDescriptorFactory.FromBundle("bus")
                        });
                    }
                }
            });
            ShowPinsOnMapUserCommand.Execute(null);


            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(31.9539, 35.9106), Xamarin.Forms.GoogleMaps.Distance.FromMeters(12)), true);


            ShowRouteOnMapUserCommand = new Command(async () => await ShowRouteOnMap());
            ShowRouteOnMapUserCommand.Execute(null);

            TrackingBusCommand = new Command(async () => await TrackThebusesAsync());
            TrackingBusCommand.Execute(null);


        }





        public List<Models.Location> Locations { get; set; }

        public Line GetLineSelected()
        {
            try
            {
                return JsonConvert.DeserializeObject<Line>(Preferences.Get("LineChosenByUser", null));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Station> GetDepartureStationAsync()
        {
            try
            {
                var DepartureStation = await stationServices.GetStationAsync(GetLineSelected().Dep_StationID);
                //add departure staiton pin on the map  
                Map.Pins.Add(new Pin()
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
                Map.Pins.Add(new Pin()
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

                Map.Polylines.Add(polyline);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(polyline.Positions[polyline.Positions.ToList().Count / 2], Xamarin.Forms.GoogleMaps.Distance.FromKilometers(distance)), true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Bus>> GetActiveBuses()
        {
            try
            {
                return await busService.GetSetOfActiveBuses(GetLineSelected().LineID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task TrackThebusesAsync()
        {
            try
            {
                var ActiveBuses = await GetActiveBuses();
                if (ActiveBuses == null)
                    return;


                List<int> ids = new List<int>();
                foreach (var item in ActiveBuses)
                {
                    ids.Add(item.BusID);
                }


                while (true)
                {
                    var setOfActiveBuses = await busService.GetSetOfBuses(ids);
                    if (setOfActiveBuses == null)
                        return;

                    for (int i = 0; i < Map.Pins.Count - 2; i++)
                    {
                        foreach (var item in setOfActiveBuses)
                        {
                            Map.Pins[i].Label = $"bus id = {item.BusID}";
                            Map.Pins[i].Position = new Position(item.Lat, item.Lon);
                            Map.Pins[i].Icon = BitmapDescriptorFactory.FromBundle("bus");
                        }
                        OnPropertyChanged(nameof(Map.Pins));
                    }
                    OnCollectionChanged(nameof(Map.Pins));


                }



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

        public override void CommandStateChanged()
        {
            //
        }



    }
}
