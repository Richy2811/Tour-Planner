using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.BL;
using TourPlanner.DAL;

namespace TourPlanner.BL.Test
{
    public class DeleteDataTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestDeleteTour()
        {
            // Arrange
            Dictionary<string, object> Tour = new()
            {
                { "id", 1000 },
                { "name", "test" },
                { "description", "test" },
                { "start", "test" },
                { "destination", "test" },
                { "transport_type", "test" },
                { "distance", "test" },
                { "estimated_time", "test" },
                { "image", "test" },
                { "popularity", "test" },
                { "childfriendlyness", "test" },
                { "favourite", "test" }
            };
            bool success = await Database.Base.Write("tours", Tour);

            Dictionary<string, object> expectedData = null;

            // Act
            success = await DeleteData.DeleteTour(1000);

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }
                
            };

            Dictionary<string, object> actualData = await Database.Base.Read("*", "tours", restrictions);

            // Assert
            Assert.AreEqual(expectedData, actualData, "Tour should not exist");
        }

        [Test]
        public async Task TestDeleteLog()
        {
            // Arrange
            Dictionary<string, object> Log = new()
            {
                { "id", 1000 },
                { "tour_id", 1000 },
                { "date_time", "TestDate" },
                { "comment", "TestComment" },
                { "difficulty", "TestDiff" },
                { "total_time", "TestTime" },
                { "rating", "TestRating" }
            };
            bool success = await Database.Base.Write("logs", Log);

            Dictionary<string, object> expectedData = null;

            // Act
            success = await DeleteData.DeleteLog(1000);

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }

            };

            Dictionary<string, object> actualData = await Database.Base.Read("*", "logs", restrictions);

            // Assert
            Assert.AreEqual(expectedData, actualData, "Tour should not exist");
        }
    }
}
