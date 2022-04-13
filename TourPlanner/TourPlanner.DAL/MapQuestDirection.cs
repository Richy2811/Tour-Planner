using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TourPlanner.DAL
{
    class MapQuestDirection
    {
        public async Task<JObject> GetRouteInfoAsync(string path)
        {
            JObject routeInfo = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            routeInfo = JObject.Parse(await response.Content.ReadAsStringAsync());

            return routeInfo;
        }
    }
}
