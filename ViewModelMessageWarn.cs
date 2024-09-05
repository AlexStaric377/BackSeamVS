using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace BackSeam
{
    public class ViewModelMessageWarn : BaseViewModel
    {

        // команда закрытия окна
        RelayCommand? closeWarning;
        public RelayCommand CloseWarning
        {
            get
            {
                return closeWarning ??
                  (closeWarning = new RelayCommand(obj =>
                  {
                      MessageWarn WindowWarn = MainWindow.LinkMainWindow("MessageWarn");
                      WindowWarn.Close();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? exitInterview;
        public RelayCommand ExitInterview
        {
            get
            {
                return exitInterview ??
                  (exitInterview = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.StopDialog = true;
                      MessageWarn WindowWarn = MainWindow.LinkMainWindow("MessageWarn");
                      WindowWarn.Close();
                  }));
            }
        }
    }
}
