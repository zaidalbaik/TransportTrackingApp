using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class BusService : BaseService<Bus>
    {

        //Get
        //http://host/api/Buses/GetBuses
        public async Task<List<Bus>> GetAllBusesAsync()
        {
            string uri = "Buses/GetBuses";

            return await GetRequestAsync(uri);
        }

        //Get
        public async Task<List<Bus>> GetSetOfActiveBuses(int lineId)
        {
            string uri = $"Buses/GetSetOfActiveBuses/{lineId}";

            return await GetRequestAsync(uri);
        }

        //Get
        //http://host/api/Buses/GetBus/{id}
        public async Task<Bus> GetBusAsync(int id)
        {
            string uri = $"Buses/GetBus/{id}";

            return await GetRequestByIDAsync(uri);
        }


        public async Task<List<Bus>> GetSetOfBuses(List<int> ids)
        {
            // ids => identifiers
            try
            {
                //api url
                string url = "http://host/TT_API/api/Buses/GetSetOfBuses";
                 
                
                var JsonContent = JsonConvert.SerializeObject(ids, Formatting.Indented);
                StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Bus>>(result);
                }
                return null;
            }
            catch (System.Exception)
            {
                Debug.WriteLine("Error in GetSetOfBuses Methods");
                return null;
            }

        }


        //Post
        //http://host/api/Buses/SaveBus
        public async Task<bool> SaveBusAsync(Bus bus)
        {
            string uri = "Buses/SaveBus";

            return await PostRequestAsync(uri, bus);
        }

        //Put
        //http://host/api/Buses/EditBus/{id}
        public async Task<bool> EditBusAsync(int id, Bus bus)
        {
            string uri = $"Buses/EditBus/{id}";

            return await PutRequestAsync(uri, bus);
        }

        //Put
        //http://host/api/Buses/EditLatLonForBus/{id}
        public async Task<bool> EditLatLonForBus(int id, Bus bus)
        {
            string uri = $"Buses/EditLatLonForBus/{id}";

            return await PutRequestAsync(uri, bus);
        }

        //Put
        public async Task<bool> EditActivityForBus(int id, Bus bus)
        {
            string uri = $"Buses/EditActivityForBus/{id}";

            return await PutRequestAsync(uri, bus);
        }

        //Delete
        //http://host/api/Buses/{id}
        public async Task<bool> DeleteBusAsync(int id)
        {
            string uri = $"Buses/{id}";

            return await DeleteRequestAsync(uri);
        }
    }
}
