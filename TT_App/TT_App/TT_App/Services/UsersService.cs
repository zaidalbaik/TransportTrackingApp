using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class UsersService : BaseService<User>
    {

        //Get
        //http://host/TTSApi/api/Users/GetUsers
        public async Task<List<User>> GetAllAccountsAsync()
        {
            string uri = "Users/GetUsers";
            return await GetRequestAsync(uri);
        }

        //Get
        //http://host/TTSApi/api/Users/GetUser/{id}
        public async Task<User> GetAccountAsync(int id)
        {
            string uri = $"Users/GetUser/{id}";
            return await GetRequestByIDAsync(uri);
        }

        //Post
        //http://host/TTSApi/api/Users/SaveUser
        public async Task<bool> SaveAccountAsync(User user)
        {
            string uri = "Users/SaveUser";
            return await PostRequestAsync(uri, user);
        }

        //Put
        //http://host/TTSApi/api/Users/EditUser/{id}
        public async Task<bool> EditAccountAsync(int id, User user)
        {
            string uri = $"Users/EditUser/{id}";
            return await PutRequestAsync(uri, user);
        }

        //Delete
        //http://host/TTSApi/api/Users/{id}
        public async Task<bool> DeleteAccountAsync(int id)
        {
            string uri = $"Users/{id}";
            return await DeleteRequestAsync(uri);
        }
        public async Task<bool> IsStoredAccount(string Email, string Password)
        { 
            string Url = $"http://host/TT_API/api/Users/IsStoredAccount?email={Email}&password={Password}";  


            Url = Url.Replace("#", "%23");
            HttpResponseMessage response = await httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> IsStoredEmail(string Email)
        {
            string Url = $"http://host/TT_API/api/Users/IsStoredEmail?email={Email}";
            //string Url = $"http://host/TT_API/api/Users/IsStoredEmail?email={Email}";


            HttpResponseMessage response = await httpClient.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<User> GetAccountByEmailAndPasswordAsync(string Email, string Password)
        { 
            string url = $"http://host/TT_API/api/Users/IsStoredAccount?email={Email}&password={Password}";
            //  string url = $"http://host/TT_API/api/Users/IsStoredAccount?email={Email}&password={Password}";


            url = url.Replace("#", "%23");


            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string user = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<User>(user);
            }
            return null;
        }

    }

}
