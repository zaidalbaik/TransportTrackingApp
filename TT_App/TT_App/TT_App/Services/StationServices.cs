using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class StationServices :BaseService<Station>
    {
        //Get
        //http://host/api/Stations/GetStations
        public async Task<List<Station>> GetAllStationsAsync()
        {
            string uri = "Stations/GetStations";

            return await GetRequestAsync(uri);
        }

        //Get
        //http://host/api/Stations/GetStation/{id}
        public async Task<Station> GetStationAsync(int id)
        {
            string uri = $"Stations/GetStation/{id}";

            return await GetRequestByIDAsync(uri);
        }

        //Post
        //http://host/api/Stations/SaveStation
        public async Task<bool> SaveStationAsync(Station station)
        {
            string uri = "Stations/SaveStation";

            return await PostRequestAsync(uri, station);

        }

        //Put
        //http://host/api/Stations/EditStation/{id}
        public async Task<bool> EditStationAsync(int id, Station station)
        {
            string uri = $"Stations/EditStation/{id}";
            
            return await PutRequestAsync(uri, station);
        }

        //http://host/api/Stations/{id}
        public async Task<bool> DeleteStationAsync(int id)
        {
            string uri = $"Stations/{id}";

            return await DeleteRequestAsync(uri);
        }
    }
}
