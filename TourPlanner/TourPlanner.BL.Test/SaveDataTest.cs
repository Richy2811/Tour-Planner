using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TourPlanner.BL;
using TourPlanner.DAL;
using TourPlanner.Models;

namespace TourPlanner.BL.Test
{
    public class SaveDataTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestSaveTourInfo()
        {
            // Arrange
            TourData TourInfo = new(1000, "name", "des", "start", "desti", "trans", 100, "time", "image", "popu", "child", "fav");


            Dictionary<string, object> expectedData = new()
            {
                { "id0", TourInfo.ID },
                { "name0", TourInfo.TourName },
                { "description0", TourInfo.TourDescription },
                { "start0", TourInfo.Start },
                { "destination0", TourInfo.Destination },
                { "transport_type0", TourInfo.TransportType },
                { "distance0", TourInfo.TourDistance },
                { "estimated_time0", TourInfo.Time },
                { "image0", TourInfo.ImageName },
                { "popularity0", TourInfo.Popularity },
                { "childfriendlyness0", TourInfo.ChildFriendliness },
                { "favourite0", TourInfo.Favourite }
            };

            // Act
            bool sussess = await SaveData.SaveTourInfo(TourInfo);

            Dictionary<string, object> restrictions = new()
            {
                { "id", TourInfo.ID }
            };

            Dictionary<string, object>  actualData = await Database.Base.Read("*", "tours", restrictions);

            // Assert
            Assert.AreEqual(expectedData, actualData, "Tours should be the same!");
        }

        [Test]
        public async Task TestSaveLogInfo()
        {
            // Arrange
            LogEntry LogInfo = new(1000, "date", "com", "diff", "dur", "rating");


            Dictionary<string, object> expectedData = new()
            {
                { "id0", LogInfo.ID },
                { "tour_id0", 1000 },
                { "date_time0", LogInfo.Date },
                { "comment0", LogInfo.Comment },
                { "difficulty0", LogInfo.Difficulty },
                { "total_time0", LogInfo.Duration },
                { "rating0", LogInfo.Rating }
            };

            ObservableCollection<LogEntry> LogData = new ObservableCollection<LogEntry>();
            LogData.Add(LogInfo);

            // Act
            bool sussess = await SaveData.SaveLogInfo(LogData,1000);

            Dictionary<string, object> restrictions = new()
            {
                { "id", LogInfo.ID }
            };

            Dictionary<string, object> actualData = await Database.Base.Read("*", "logs", restrictions);

            // Assert
            Assert.AreEqual(expectedData, actualData, "Logs should be the same!");
        }
    }
}
