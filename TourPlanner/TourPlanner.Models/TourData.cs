using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Models
{
    public class TourData
    {
        public string TourName { get; set; }
        public string TourDescription { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }
        public string TransportType { get; set; }
        public int TourDistance { get; set; }
    }
}
