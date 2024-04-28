using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    // Проверка подключения сервера. Если не запущен то прекратить работу

    public class PingControl : DependencyObject
    {
        public PingControl()
        {

            MainWindow.UrlServer =  "/api/PingController";
            CallServer.PostServer(MainWindow.UrlServer, MainWindow.UrlServer, "PING");
            if (CallServer.ResponseFromServer.Length == 0) { Environment.Exit(0); }


        }
    }

}
