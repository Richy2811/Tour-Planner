using NUnit.Framework;
namespace TourPlanner.Models.Test
{
    public class  TourDataTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLogEntry()
        {
            // Arrange
            int expectedId = 1;
            string expectedTourName = "testName";
            string expectedDescription = "testDescription";
            string expectedStart = "testStart";
            string expectedDestination = "testDestination";
            string expectedTransportType = "testTransportType";
            decimal expectedTourDistance = 100;
            string expectedTime = "testTime";
            string expectedImageName = "testImageName";
            string expectedpopularity = "testPopularity";
            string expectedfriendliness = "testFriendliness";
            string expectedfavourite = "testFavourite";

            // Act
            TourData testTour = new(expectedId, expectedTourName, expectedDescription, expectedStart, expectedDestination, expectedTransportType, expectedTourDistance, expectedTime, expectedImageName, expectedpopularity, expectedfriendliness, expectedfavourite);

            int actualId = testTour.ID;
            string actualTourName = testTour.TourName;
            string actualDescription = testTour.TourDescription;
            string actualStart = testTour.Start;
            string actualDestination = testTour.Destination;
            string actualTransportType = testTour.TransportType;
            decimal actualTourDistance = testTour.TourDistance;
            string actualTime = testTour.Time;
            string actualImageName = testTour.ImageName;

            // Assert
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedTourName, actualTourName);
            Assert.AreEqual(expectedDescription, actualDescription);
            Assert.AreEqual(expectedStart, actualStart);
            Assert.AreEqual(expectedDestination, actualDestination);
            Assert.AreEqual(expectedTransportType, actualTransportType);
            Assert.AreEqual(expectedTourDistance, actualTourDistance);
            Assert.AreEqual(expectedTime, actualTime);
            Assert.AreEqual(expectedImageName, actualImageName);
        }
    }
}

