using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public static class SaveData
    {
        public static async Task<bool> SaveTourInfo(TourData TourInfo)
        {
            Dictionary<string, object> restrictions = new()
            {
                { "id", TourInfo.ID },
            };

            Dictionary<string, object> exists = await Database.Base.Read("*", "tours", restrictions);

            Dictionary<string, object> Info = new()
            {
                { "id", TourInfo.ID },
                { "name", TourInfo.TourName },
                { "description", TourInfo.TourDescription },
                { "start", TourInfo.Start },
                { "destination", TourInfo.Destination },
                { "transport_type", TourInfo.TransportType },
                { "distance", TourInfo.TourDistance },
                { "estimated_time", TourInfo.Time },
                { "image", TourInfo.ImageName }
            };

            if (exists == null)
            {
                bool success = await Database.Base.Write("tours", Info);
                return success;
            }
            else
            {
                bool success = await Database.Base.Update("tours", Info, restrictions);
                return success;
            }

        }
    }
}
