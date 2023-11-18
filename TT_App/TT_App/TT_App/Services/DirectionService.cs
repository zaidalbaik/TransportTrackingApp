using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TT_App.Models;
using Xamarin.Forms.GoogleMaps;

namespace TT_App.Services
{
    public class DirectionService
    {
        private readonly HttpClient client;
        public DirectionService()
        {
            client = new HttpClient();
        }
        public async Task<Direction> GetDirectionAsync(Position origin, Position destination)
        {
            try
            {
                //google map directions api
                var url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin.Latitude},{origin.Longitude}&destination={destination.Latitude},{destination.Longitude}&key=Enter Key";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var direction = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<Direction>(direction);
                }

                return null;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine($"Error in GetDirectionAsync Method the Error Message : {e.Message}");
                return null;
            }

        }
    }
}
