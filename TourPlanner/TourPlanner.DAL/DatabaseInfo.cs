using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.DAL
{
    public class DatabaseInfo
    {
        public string Host { get;  private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string Database { get; private set; }

        public DatabaseInfo()
        {
            ReadInfo();
        }


        private void ReadInfo()
        {
            string[] info;
            string FileToRead = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\database.txt";
            if (!File.Exists(FileToRead))
            {
                throw new ArgumentException("Does not exist.", nameof(FileToRead));
            }

            IEnumerable<string> line = File.ReadLines(FileToRead);
            Console.WriteLine(string.Join(Environment.NewLine, line));
            info = line.ToArray();
            Host = info[0];
            User = info[1];
            Password = info[2];
            Database = info[3];
        }
    }
}
