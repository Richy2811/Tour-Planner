using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Logging;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuBarViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger("MenuBarViewModel");

        private bool _exportSingle;

        public RelayCommand PdfExportSingle { get; }
        public RelayCommand PdfExportAll { get; }
        public event EventHandler<bool> OnClickGenerate;

        public MenuBarViewModel()
        {
            logger.Debug("MenuBarViewModelCreated()");
            PdfExportSingle = new RelayCommand((_) =>
            {
                _exportSingle = true;
                logger.Debug("PdfExportSingle()");
                //notify business layer
                OnClickGenerate.Invoke(this, _exportSingle);
            });

            PdfExportAll = new RelayCommand((_) =>
            {
                _exportSingle = false;
                logger.Debug("PdfExportAll()");
                //notify business layer
                OnClickGenerate.Invoke(this, _exportSingle);
            });
        }
    }
}
