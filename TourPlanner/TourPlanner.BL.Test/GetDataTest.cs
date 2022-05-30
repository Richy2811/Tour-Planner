using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.BL;
using TourPlanner.DAL;

namespace TourPlanner.BL.Test
{
    public class GetDataTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetAllTours()
        {
            // Arrange
            Dictionary<string, object> expectedData = null;
            Dictionary<string, object> actualData = null;
            expectedData = await Database.Base.Read("*", "tours");

            // Act
            actualData = await GetData.GetAllTours();

            // Assert
            Assert.AreEqual(expectedData, actualData, "Tours should be the same!");
        }

        [Test]
        public async Task TestGetAllTourLogData()
        {
            // Arrange
            int testID = 1000;
            Dictionary<string, object> expectedData = new()
            {
                { "id", testID },
                { "tour_id", testID },
                { "date_time", "TestDate" },
                { "comment", "TestComment" },
                { "difficulty", "TestDiff" },
                { "total_time", "TestTime" },
                { "rating", "TestRating" }
            };
            Dictionary<string, object> restrictions = new()
            {
                { "id", testID }
            };
            Dictionary<string, object> actualData = null;

            await Database.Base.Write("logs", expectedData);
            expectedData = await Database.Base.Read("*", "logs", restrictions);

            // Act
            actualData = await GetData.GetAllTourLogData(testID);

            
            await Database.Base.Delete("logs", restrictions);

            // Assert
            Assert.AreEqual(expectedData, actualData, "Logs should be the same!");
        }
    }
}
