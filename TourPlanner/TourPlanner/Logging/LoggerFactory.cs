using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging
{
    public class LoggerFactory
    {
        public static ILoggerWrapper GetLogger(string creator)
        {
            return Log4NetWrapper.CreateLogger("./log4net.config", creator);
        }
    }
}
