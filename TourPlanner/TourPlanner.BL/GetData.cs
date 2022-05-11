using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;

namespace TourPlanner.BL
{
    public static class GetData
    {
        public static async Task<Dictionary<string, object>> GetAllTourLogData()
        {
            Dictionary<string, object> data = null;
            //Dictionary<string, object> restrictions = 
            data = await Database.Base.Read("*", "logs");
            return data;
        }

        public static async void WriteLogData(Dictionary<string,object> data)
        {
            await Database.Base.Write("logs", data);
        }
    }
}
