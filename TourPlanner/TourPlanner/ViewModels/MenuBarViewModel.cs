using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuBarViewModel : BaseViewModel
    {
        private bool _exportSingle;

        public RelayCommand PdfExportSingle { get; }
        public RelayCommand PdfExportAll { get; }
        public event EventHandler<bool> OnClickGenerate;

        public MenuBarViewModel()
        {
            PdfExportSingle = new RelayCommand((_) =>
            {
                _exportSingle = true;
                //notify business layer
                OnClickGenerate.Invoke(this, _exportSingle);
            });

            PdfExportAll = new RelayCommand((_) =>
            {
                _exportSingle = false;
                //notify business layer
                OnClickGenerate.Invoke(this, _exportSingle);
            });
        }
    }
}
