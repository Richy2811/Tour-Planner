using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;
using TourPlanner.Models;

namespace TourPlanner.BL
{
    public static class DeleteData
    {
        public static async Task<bool> DeleteTour(int tourID)
        {
            Dictionary<string, object> restrictions = new()
            {
                { "id", tourID }
            };
            bool success = await Database.Base.Delete("tours", restrictions);
            Dictionary<string, object> restrictions_logs = new()
            {
                { "id", tourID }
            };
            success = await Database.Base.Delete("logs", restrictions_logs);

            return success;
        }

        public static async Task<bool> DeleteLog(int LogID)
        {
            Dictionary<string, object> restrictions = new()
            {
                { "id", LogID }
            };
            bool success = await Database.Base.Delete("logs", restrictions);

            return success;
        }
    }
}
