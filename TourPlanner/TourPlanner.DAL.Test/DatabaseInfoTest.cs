using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TourPlanner.DAL.Test
{
    public class DatabaseInfoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDBInfo()
        {
            // Arrange
            DatabaseInfo dBInfo = new();

            // Act

            string[] info;
            string FileToRead = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\database.txt";
            if (!File.Exists(FileToRead))
            {
                throw new ArgumentException("Does not exist.", nameof(FileToRead));
            }

            IEnumerable<string> line = File.ReadLines(FileToRead);
            Console.WriteLine(string.Join(Environment.NewLine, line));
            info = line.ToArray();
            string expectedHost = info[0];
            string expectedUser = info[1];
            string expectedPassword = info[2];
            string expectedDatabase = info[3];

            string actualHost = dBInfo.Host;
            string actualUser = dBInfo.User;
            string actualPassword = dBInfo.Password;
            string actualDatabase = dBInfo.Database;


            // Assert
            Assert.AreEqual(expectedHost, actualHost);
            Assert.AreEqual(expectedUser, actualUser);
            Assert.AreEqual(expectedPassword, actualPassword);
            Assert.AreEqual(expectedDatabase, actualDatabase);
        }
    }
}
