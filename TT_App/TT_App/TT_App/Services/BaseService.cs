using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TT_App.Services
{
    public class BaseService<T> where T : class
    {
        private const string BaseUrl ="Enter base Url for api";       

        public HttpClient httpClient;
        public BaseService()
        {
            httpClient = new HttpClient();
        }
        public async Task<List<T>> GetRequestAsync(string uri)
        {
            try
            {
                string URL = BaseUrl + uri;
                HttpResponseMessage response = await httpClient.GetAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    var Jsonresult = await httpClient.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<List<T>>(Jsonresult);
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.Message}");
                return null;
            }
        }

        public async Task<T> GetRequestByIDAsync(string uri)
        {
            try
            {
                string URL = BaseUrl + uri;

                HttpResponseMessage response = await httpClient.GetAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    var JsonResult = await httpClient.GetStringAsync(URL);
                    return JsonConvert.DeserializeObject<T>(JsonResult);
                }

                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.Message}");
                return null;
            }
        }

        public async Task<bool> PostRequestAsync(string uri, T t)
        {
            try
            {
                string URL = BaseUrl + uri;


                var JsonContent = JsonConvert.SerializeObject(t, Formatting.Indented);
                StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(URL, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\Saved successfully");
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> PutRequestAsync(string uri, T t)
        {
            try
            {
                string URL = BaseUrl + uri;

                var JsonContent = JsonConvert.SerializeObject(t, Formatting.Indented);
                StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync(URL, content);


                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\Updated successfully");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

        }

        public async Task<bool> DeleteRequestAsync(string uri)
        {
            try
            {
                string URL = BaseUrl + uri;

                HttpResponseMessage response = await httpClient.DeleteAsync(URL);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\Deletedsuccessfully");
                    return true;
                }

                return false;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

        }

    }
}
