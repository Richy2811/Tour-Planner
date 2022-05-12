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

            return success;
        }
    }
}
