using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DAL;
using TourPlanner.Models;


namespace TourPlanner.BL
{
    public class ComputeTourLogInfo
    {
        public static async Task<string> ComputeChildFriendliness(int tourID)
        {
            int index = 0;
            int lowCount = 0, medCount = 0, highCount = 0, counter = 0;
            string friendliness = string.Empty;
            Dictionary<string, object> data = null;

            data =  await GetData.GetAllTourLogData(tourID);

            if (data != null)
            {
                bool exists = data.ContainsKey("id" + index);
                while (exists)
                {
                    if ((string)data["difficulty" + index] == "high")
                    {
                        lowCount++;
                        counter++;
                    }
                    else if ((string)data["difficulty" + index] == "medium")
                    {
                        medCount++;
                        counter++;
                    }
                    else if ((string)data["difficulty" + index] == "low")
                    {
                        highCount++;
                        counter++;
                    }

                    index++;
                    exists = data.ContainsKey("id" + index);
                }

                if(counter != 0)
                {
                    if (lowCount / counter >= 0.2f)
                    {
                        friendliness = "low";
                    }
                    else if (medCount / counter >= 0.2f)
                    {
                        friendliness = "medium";
                    }
                    else
                    {
                        friendliness = "high";
                    }

                }

                return friendliness;
            }

            return "low";

            
        }

        public static async Task<string> ComputePopularity(int tourID)
        {
            int index = 0;
            string popularity = string.Empty;
            Dictionary<string, object> data = null;

            data = await GetData.GetAllTourLogData(tourID);


            if (data != null)
            {
                bool exists = data.ContainsKey("id" + index);
                while (exists)
                {
                    index++;
                    exists = data.ContainsKey("id" + index);
                }

                if(index > 9)
                {
                    popularity = "high";
                }
                else if(index > 4)
                {
                    popularity = "medium";
                }
                else
                {
                    popularity = "low";
                }

                return popularity;

            }
            return "low";

        }
    }
}
