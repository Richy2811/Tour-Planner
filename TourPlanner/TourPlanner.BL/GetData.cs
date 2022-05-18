﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;

namespace TourPlanner.BL
{
    public static class GetData
    {
        public static async Task<Dictionary<string, object>> GetAllTourLogData(int tourID)
        {
            Dictionary<string, object> data = null;
            Dictionary<string, object> restrictions = new()
            {
                { "tour_id", tourID },
            };
            //Dictionary<string, object> restrictions = 
            data = await Database.Base.Read("*", "logs", restrictions);
            return data;
        }

        public static async Task<Dictionary<string, object>> GetAllTours()
        {
            Dictionary<string, object> data = null;
            //Dictionary<string, object> restrictions = 
            data = await Database.Base.Read("*", "tours");
            return data;
        }
    }
}
