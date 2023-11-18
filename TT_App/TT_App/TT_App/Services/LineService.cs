using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;

namespace TT_App.Services
{
    public class LineService : BaseService<Line>
    {
        //Get
        public async Task<List<Line>> GetAllLinesAsync()
        {
            string uri = "Lines/GetLines";
            return await GetRequestAsync(uri);
        }

        //Get
        public async Task<Line> GetLineAsync(int id)
        {
            string uri = $"Lines/GetLine/{id}";
            return await GetRequestByIDAsync(uri);

        }

        //Post
        public async Task<bool> SaveLineAsync(Line Line)
        {
            string uri = "Lines/SaveLine";
            return await PostRequestAsync(uri, Line);

        }

        //Put
        public async Task<bool> EditLineAsync(int id, Line Line)
        {

            string uri = $"Lines/EditLine/{id}";
            return await PutRequestAsync(uri, Line);
        }

        //delete
        public async Task<bool> DeleteLineAsync(int id)
        {
            string uri = $"Lines/{id}";
            return await DeleteRequestAsync(uri);
        }
    }
}
