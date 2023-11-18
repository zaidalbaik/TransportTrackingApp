using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class DriverService : BaseService<Driver>
    { 
        //Get
        //http://host/api/Drivers/GetDrivers
        public async Task<List<Driver>> GetAllDriversAsync()
        {
            string uri = "Drivers/GetDrivers";
            return await GetRequestAsync(uri);
        }

        //Get
        //http://host/api/Drivers/GetDriver/{id}
        public async Task<Driver> GetDriverAsync(int id)
        {
            string uri = $"Drivers/GetDriver/{id}";
            return await GetRequestByIDAsync(uri);
        }

        //Post
        //http://host/api/Drivers/SaveDriver
        public async Task<bool> SaveDriverAsync(Driver driver)
        {
            string uri = "Drivers/SaveDriver";
            return await PostRequestAsync(uri, driver);
        }

        //Put
        //http://host/api/Drivers/EditDriver/{id}
        public async Task<bool> EditDriverAsync(int id, Driver driver)
        {
            string uri = $"Drivers/EditDriver/{id}";
            return await PutRequestAsync(uri, driver);
        }

        //http://host/api/Drivers/{id}
        public async Task<bool> DeleteDriverAsync(int id)
        {
            string uri = $"Drivers/{id}";
            return await DeleteRequestAsync(uri);
        }
        public async Task<Driver> GetDriverByEmailAndPasswordAsync(string Email, string Password)
        { 
            string Url = $"http://host/TT_API/api/Drivers/GetDriver?Email=" + Email.Trim() + "&Password=" + Password.Trim(); 

            Url = Url.Replace("#", "%23");

            HttpResponseMessage response = await httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                string driver = await httpClient.GetStringAsync(Url);
                Debug.WriteLine("");
                return JsonConvert.DeserializeObject<Driver>(driver);
            }
            return null;
        }
    }
}

