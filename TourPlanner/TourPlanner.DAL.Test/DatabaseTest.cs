using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TourPlanner.DAL.Test
{
    public class DatabaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestWrite()
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

            Dictionary<string, object> expected = new()
            {
                { "id0", 1000 },
                { "tour_id0", 1000 },
                { "date_time0", "TestDate" },
                { "comment0", "TestComment" },
                { "difficulty0", "TestDiff" },
                { "total_time0", "TestTime" },
                { "rating0", "TestRating" }
            };

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }
            };

            // Act
            await Database.Base.Write("logs", Log);

            Dictionary<string, object> actual = await Database.Base.Read("*", "logs", restrictions);

            await Database.Base.Delete("logs", restrictions);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task TestRead()
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

            Dictionary<string, object> expected = new()
            {
                { "id0", 1000 },
                { "tour_id0", 1000 },
                { "date_time0", "TestDate" },
                { "comment0", "TestComment" },
                { "difficulty0", "TestDiff" },
                { "total_time0", "TestTime" },
                { "rating0", "TestRating" }
            };

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }
            };

            // Act
            await Database.Base.Write("logs", Log);

            Dictionary<string, object> actual = await Database.Base.Read("*", "logs", restrictions);

            await Database.Base.Delete("logs", restrictions);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task TestDelete()
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

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }
            };

            // Act
            await Database.Base.Write("logs", Log);

            await Database.Base.Delete("logs", restrictions);

            Dictionary<string, object> actual = await Database.Base.Read("*", "logs", restrictions);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [Test]
        public async Task TestUpdate()
        {
            // Arrange
            Dictionary<string, object> Log = new()
            {
                { "id", 1000 },
                { "tour_id", 1000 },
                { "date_time", "Test" },
                { "comment", "Test" },
                { "difficulty", "Test" },
                { "total_time", "Test" },
                { "rating", "Test" }
            };

            Dictionary<string, object> Log2 = new()
            {
                { "id", 1000 },
                { "tour_id", 1000 },
                { "date_time", "TestDate" },
                { "comment", "TestComment" },
                { "difficulty", "TestDiff" },
                { "total_time", "TestTime" },
                { "rating", "TestRating" }
            };

            Dictionary<string, object> expected = new()
            {
                { "id0", 1000 },
                { "tour_id0", 1000 },
                { "date_time0", "TestDate" },
                { "comment0", "TestComment" },
                { "difficulty0", "TestDiff" },
                { "total_time0", "TestTime" },
                { "rating0", "TestRating" }
            };

            Dictionary<string, object> restrictions = new()
            {
                { "id", 1000 }
            };

            await Database.Base.Write("logs", Log);

            // Act
            await Database.Base.Update("logs", Log2, restrictions);

            Dictionary<string, object> actual = await Database.Base.Read("*", "logs", restrictions);

            await Database.Base.Delete("logs", restrictions);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
