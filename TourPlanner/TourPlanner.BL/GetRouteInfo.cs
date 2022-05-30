using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TourPlanner.DAL;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public class GetRouteInfo
    {
        public static JObject GetJson(TourData tourInfo)
        {
            Task<JObject> mapQuestJsonReturn = Task.Run(() => MapQuestDirection.GetRouteJsonAsync(tourInfo));
            return mapQuestJsonReturn.Result;
        }

        public static string GetImageName(string sessionId)
        {
            string imageName = MapQuestDirection.GetRouteImageName(sessionId);
            return imageName;
        }
    }
}
