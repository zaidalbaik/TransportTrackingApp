using System;
using System.Threading.Tasks;
using TT_App.Models.WeatherInfoModel;
using TT_App.Services;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class WeatherViewModel : BaseViewModel
    {
        private WeatherInfo Weather { get; set; }

        private readonly WeatherService WeatherService;

        public Command GetWeatherCommand { get; }
        public WeatherViewModel()
        {
            WeatherService = new WeatherService();
            GetWeatherCommand = new Command(async () => await GetWeatherForCityAsync(), () => !IsBusy);

        }

        private string note;
        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }

        private double lat;
        public double Lat
        {
            get { return lat; }
            set
            {
                lat = value;
                OnPropertyChanged();
            }
        }


        private double lon;
        public double Lon
        {
            get { return lon; }
            set
            {
                lon = value;
                OnPropertyChanged();

            }
        }

        private string cityName;
        public string CityName
        {
            get { return cityName; }
            set
            {
                cityName = value;
                OnPropertyChanged();
            }
        }

        private string temp;
        public string Temp
        {
            get { return temp; }
            set
            {
                temp = value;
                OnPropertyChanged();
            }
        }

        private bool flag;
        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                OnPropertyChanged();
            }
        }

        public async Task GetWeatherForCityAsync()
        {
            try
            {
                IsBusy = true;
                Weather = await WeatherService.GetWeatherAsync(lat, lon);
                OnPropertyChanged(nameof(Weather));

                if (Weather.Cod == 0)
                {
                    Note = " * please Fill in the blank fields";
                    CityName = string.Empty;
                    Temp = "";
                    Flag = false;

                }
                else if (Weather.Cod == 404)
                {
                    Note = " * This location does not exist";
                    CityName = string.Empty;
                    Temp = "";
                    Flag = false;
                }
                else
                {
                    CityName = Weather.Name;
                    var Kelvin = Weather.Main.Temp;
                    var Temprerature = Kelvin - 273.15;
                    Temp = Temprerature + "C";
                    Note = string.Empty;
                    Lat = 0; Lon = 0;
                    Flag = true;

                }
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
            GetWeatherCommand?.ChangeCanExecute();
        }
    }

}
