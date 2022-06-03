using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TourPlanner.Models;

namespace TourPlanner.DAL
{
    public class MapQuestDirection
    {

        public static async Task<JObject> GetRouteJsonAsync(TourData tourInfo)
        {
            string mapQusestKey = GetMapQuestKey();
            string transportType = tourInfo.TransportType;
            switch (transportType)
            {
                case "Bicycle":
                    transportType = "bicycle";
                    break;

                case "Walk":
                    transportType = "pedestrian";
                    break;

                default:
                    transportType = "bicycle";
                    break;
            }

            string mapQuestRouteUrl = "http://www.mapquestapi.com/directions/v2/route";
            string mapQuestParameters = $"?key={mapQusestKey}&from={tourInfo.Start}&to={tourInfo.Destination}&unit=k&routeType={transportType}&locale=de_DE";
            JObject routeInfo = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(mapQuestRouteUrl + mapQuestParameters);
            routeInfo = JObject.Parse(await response.Content.ReadAsStringAsync());

            return routeInfo;
        }

        public static string GetRouteImageName(string sessionId)
        {
            string imagePath = $"../../../TourImages/{sessionId}.png";
            WebClient client = new WebClient();
            client.DownloadFile(new Uri($"https://www.mapquestapi.com/staticmap/v5/map?key=8Ls9YtZjQESK1vRFuUIEgCyRjAwWP3SI&session={sessionId}"), imagePath);
            client.Dispose();

            return $"{sessionId}.png";
        }

        private static string GetMapQuestKey()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("settings.json", false, true);
            IConfigurationRoot config = builder.Build();

            return config["mapquestkey"];
        }
    }
}
