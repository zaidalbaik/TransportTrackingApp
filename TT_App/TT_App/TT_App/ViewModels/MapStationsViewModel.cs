using Newtonsoft.Json;
using System;
using TT_App.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace TT_App.ViewModels
{
    public class MapStationsViewModel : BaseViewModel
    {
        public Xamarin.Forms.GoogleMaps.Map MapStations { get; private set; }

        public Command ShowSelectedStationCommand { get; }
        public MapStationsViewModel()
        {
            MapStations = new Xamarin.Forms.GoogleMaps.Map();
            MapStations.MyLocationEnabled = true;
            MapStations.UiSettings.MyLocationButtonEnabled = true;

            ShowSelectedStationCommand = new Command(() => ShowSelectedStation());
            ShowSelectedStationCommand.Execute(null);

        }
        public double FullSecreen => DeviceDisplay.MainDisplayInfo.Height;


        public void ShowSelectedStation()
        {
            try
            {
                var jsonSelectedStation = JsonConvert.DeserializeObject<Station>(Preferences.Get("StationChosenByUser", ""));
                MapStations.Pins.Add(new Pin()
                {
                    Label = $"{jsonSelectedStation.StationName}",
                    Position = new Position(jsonSelectedStation.Lat, jsonSelectedStation.Lon),
                    Type = PinType.Place
                });
                MapStations.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(jsonSelectedStation.Lat, jsonSelectedStation.Lon), Xamarin.Forms.GoogleMaps.Distance.FromMiles(1)), true);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public override void CommandStateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
