using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TourPlanner.ViewModels;

namespace TourPlanner.Test
{
    public class TourLogViewModelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLogData_ShouldContainInitialList()
        {
            // Arrange
            TourLogViewModel tourLogViewModel = new();

            // Act

            int expected = 0;
            int actual = tourLogViewModel.LogData.Count;

            // Assert
            Assert.AreEqual(expected, actual, "LogData should contain 0 Logs!");
        }

        [Test]
        public void TestLogDataAdd_ShouldAddToList()
        {
            // Arrange
            TourLogViewModel tourLogViewModel = new();

            // Act
            tourLogViewModel.LogData.Add(new LogEntry(1, "Test1", "Test1", "Test1", "Test1", "10"));
            tourLogViewModel.LogData.Add(new LogEntry(1, "Test2", "Test2", "Test2", "Test2", "10"));

            int expected = 2;
            int actual = tourLogViewModel.LogData.Count;

            // Assert
            Assert.AreEqual(expected, actual, "LogData should contain two Logs!");
        }

        [Test]
        public void TestAddLog_ShouldAddToList()
        {
            // Arrange
            TourLogViewModel tourLogViewModel = new();
            var lastDataCount = tourLogViewModel.LogData.Count;

            string[] info;
            string IdFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\logid.txt";
            if (!File.Exists(IdFile))
            {
                throw new ArgumentException("Does not exist.", nameof(IdFile));
            }

            IEnumerable<string> line = File.ReadLines(IdFile);
            Console.WriteLine(string.Join(Environment.NewLine, line));
            info = line.ToArray();
            int expectedID = Int32.Parse(info[0]);

            int id = expectedID - 1;

            using (StreamWriter writeNewID = new StreamWriter(IdFile))
            {
                writeNewID.WriteLine(id.ToString());
            }

            // Act
            tourLogViewModel.AddLog.Execute(null);
            var expectedDataCount = lastDataCount + 1;
            var currentDataCount = tourLogViewModel.LogData.Count;

            string currentComment = tourLogViewModel.LogData[expectedDataCount - 1].Comment;
            string expectedComment = "empty";
            int currentID = tourLogViewModel.LogData[expectedDataCount - 1].ID;
            string currentDate = tourLogViewModel.LogData[expectedDataCount - 1].Date;
            string expectedDate = "empty";

            // Assert
            Assert.AreEqual(expectedDataCount, currentDataCount, "A new Log should be added!");
            Assert.AreEqual(expectedComment, currentComment, "The new Log Comment should be empty");
            Assert.AreEqual(expectedID, currentID);
            Assert.AreEqual(expectedDate, currentDate, "The new Log Date Type should be empty");

        }

        [Test]
        public void TestDeleteLog()
        {
            // Arrange
            TourLogViewModel tourLogViewModel = new();
            tourLogViewModel.LogData.Add(new LogEntry(1000, "Test1", "Test1", "Test1", "Test1", "10"));
            tourLogViewModel.LogData.Add(new LogEntry(1001, "Test2", "Test2", "Test2", "Test2", "10"));

            // Act
            tourLogViewModel.SelectedItem = tourLogViewModel.LogData[0];
            tourLogViewModel.DeleteLog.Execute(null);

            int expected = 1;
            int actual = tourLogViewModel.LogData.Count;

            // Assert
            Assert.AreEqual(expected, actual, "LogData should contain one Log!");
        }

    }
}
