using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ViewModelMessageError : BaseViewModel
    {
        // команда закрытия окна
        RelayCommand? closeError;
        public RelayCommand CloseError
        {
            get
            {
                return closeError ??
                  (closeError = new RelayCommand(obj =>
                  {
                      MessageError WindowMessageError = MainWindow.LinkMainWindow("MessageError");
                      WindowMessageError.Close();
                  }));
            }
        }
    }
}

