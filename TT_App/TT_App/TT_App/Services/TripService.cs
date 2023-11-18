using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class TripService:BaseService<Trip>
    {
        //Get
        public async Task<List<Trip>> GetAllTripsAsync()
        {
            string uri = "Trips/GetTrips";

            return await GetRequestAsync(uri);
        }

        //Get
        public async Task<Trip> GetTripAsync(int id)
        {
            string uri = $"Trips/GetTrip/{id}";

            return await GetRequestByIDAsync(uri);
        }

        //Post
        public async Task<bool> SaveTripAsync(Trip Trip)
        {
            string uri = "Trips/SaveTrip";

            return await PostRequestAsync(uri, Trip);
        }

        //Put
        public async Task<bool> EditTripAsync(int id, Trip Trip)
        {
            string uri = $"Trips/EditTrips/{id}";

            return await PutRequestAsync(uri, Trip);
        }

        //delete
        public async Task<bool> DeleteTripAsync(int id)
        {
            string uri = $"Trips/{id}";

            return await DeleteRequestAsync(uri);
        }
    }
}
