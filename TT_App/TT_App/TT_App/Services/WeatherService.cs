using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TT_App.Models.WeatherInfoModel;

namespace TT_App.Services
{
    public class WeatherService
    {
        HttpClient Client = new HttpClient();

        public async Task<WeatherInfo> GetWeatherAsync(double lat, double lon)
        {
            WeatherInfo WeatherInfoObj;
            if (!(lat.ToString() == "" || lat.ToString() == "" || lon.ToString() == "0" || lon.ToString() == "0"))
            {
                try
                {
                    //weather api
                    var Jsonresult = await Client.GetStringAsync("https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&appid=enter key");

                    WeatherInfoObj = JsonConvert.DeserializeObject<WeatherInfo>(Jsonresult);


                    if (WeatherInfoObj.Name == string.Empty)
                    {
                        throw new Exception();
                    }
                    return WeatherInfoObj;
                }
                catch (Exception)
                {
                    WeatherInfoObj = new WeatherInfo()
                    {
                        Cod = 404,
                    };

                    return WeatherInfoObj;
                }
            }

            else
            {
                WeatherInfoObj = new WeatherInfo()
                {
                    Cod = 0,
                };

                return WeatherInfoObj;
            }
        }
    }
}
