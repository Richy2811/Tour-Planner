using NUnit.Framework;
namespace TourPlanner.Models.Test
{
    public class LogEntryTest
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
            string expectedDate = "testDate";
            string expectedComment = "testComment";
            string expectedDifficulty = "testDifficulty";
            string expectedDuration = "testDuration";
            string expectedRating = "testRating";

            // Act
            LogEntry TestEntry = new(expectedId, expectedDate, expectedComment, expectedDifficulty, expectedDuration, expectedRating);

            int actualId = TestEntry.ID;
            string actualDate = TestEntry.Date;
            string actualComment = TestEntry.Comment;
            string actualDifficulty = TestEntry.Difficulty;
            string actualDuration = TestEntry.Duration;
            string actualRating = TestEntry.Rating;

            // Assert
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedDate, actualDate);
            Assert.AreEqual(expectedComment, actualComment);
            Assert.AreEqual(expectedDifficulty, actualDifficulty);
            Assert.AreEqual(expectedDuration, actualDuration);
            Assert.AreEqual(expectedRating, actualRating);
        }
    }
}
