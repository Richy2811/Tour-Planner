using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using Moq;

namespace TourPlanner.Test
{
    public class TourListViewModelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTourListCollection_ShouldContainInitialList()
        {
            // Arrange
            TourListViewModel tourListViewModel = new();
            tourListViewModel.TourListCollection.Clear();

            // Act

            int expected = 0;
            int actual = tourListViewModel.TourListCollection.Count;

            // Assert
            Assert.AreEqual(expected, actual, "TourListCollection should contain 0 Tours!");
        }

        [Test]
        public void TestTourListCollectionAdd_ShouldAddToList()
        {
            // Arrange
            TourListViewModel tourListViewModel = new();

            // Act
            tourListViewModel.TourListCollection.Add(new TourData("TestName1", "TestTransportType1"));
            tourListViewModel.TourListCollection.Add(new TourData("TestName2", "TestTransportType2"));

            int expected = 2;
            int actual = tourListViewModel.TourListCollection.Count;

            // Assert
            Assert.AreEqual(expected, actual, "TourListCollection should contain two Tours!");
        }

        [Test]
        public void TestAddTour_ShouldAddToList()
        {
            // Arrange
            TourListViewModel tourListViewModel = new();
            

            string[] info;
            string IdFile = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\id.txt";
            if (!File.Exists(IdFile))
            {
                throw new ArgumentException("Does not exist.", nameof(IdFile));
            }

            IEnumerable<string> line = File.ReadLines(IdFile);
            Console.WriteLine(string.Join(Environment.NewLine, line));
            info = line.ToArray();
            int expectedID = Int32.Parse(info[0]);

            int id = expectedID-1;

            using (StreamWriter writeNewID = new StreamWriter(IdFile))
            {
                writeNewID.WriteLine(id.ToString());
            }

            // Act
            tourListViewModel.TourListCollection.Clear();
            System.Threading.Thread.Sleep(5000);
            var lastDataCount = tourListViewModel.TourListCollection.Count;
            tourListViewModel.AddTour.Execute(null);
            var expectedDataCount = lastDataCount + 1;
            var currentDataCount = tourListViewModel.TourListCollection.Count;
            
            string currentTourName = tourListViewModel.TourListCollection[expectedDataCount - 1].TourName;
            string expectedTourName = $"SampleTour{expectedID}";
            int currentID = tourListViewModel.TourListCollection[expectedDataCount - 1].ID;
            string currentTransportType = tourListViewModel.TourListCollection[expectedDataCount - 1].TransportType;
            string expectedTransportType = "Bicycle";

            // Assert
            Assert.AreEqual(expectedDataCount, currentDataCount, "A new Tour should be added!");
            Assert.AreEqual(expectedTourName, currentTourName, "The new Tour Name should be SampleTour + id");
            Assert.AreEqual(expectedID, currentID);
            Assert.AreEqual(expectedTransportType, currentTransportType, "The new Tour Transport Type should be Bicycle");

        }

        /*[Test]
        public void TestSearchTour()
        {
            // Arrange
            TourListViewModel tourListViewModel = new();
            tourListViewModel.TourListCollection.Add(new TourData("Name1", "TestTransportType1"));
            tourListViewModel.TourListCollection.Add(new TourData("Test", "TestTransportType2"));

            // Act
            tourListViewModel.TourSearchText = "TestTransportType1";
            tourListViewModel.SearchTour.Execute(null);
            int expected = 1;
            int actual = tourListViewModel.TourListCollection.Count;

            // Assert
            Assert.AreEqual(expected, actual, "TourListCollection should contain one Tours!");
        }*/




    }
}
