using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;


namespace BackSeam
{
    /// <summary>
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string CmdStrokaServer = "", UrlServer ="", UrlAdresServer = "", UrlAdresServerClient = "http://31.43.159.113:15122", IncreDecre = "", MessageError = "";
        public static string TextName = "", SourceServer="", SelectLanguageUI=""; //UrlAdresServer = "http://31.43.159.113:15122"

 
        public static int Idstr = 0;
        public static double ScreenHeight = 0.0, ScreenWidth = 0.0, SetHeigtCurent = 0.0;
        #region Вернуть ссылку на главное окно по запросу WPF C# {LinkMainWindow}
        /// <summary>
        /// Вернуть ссылку на окно по запросу WPF C# {ListWindowMain}MainWindow
        /// </summary>
        /// <param name="NameWindow"> Имя главного окна Рабочий стол или Панель</param>
        /// <returns></returns>
        public static dynamic LinkMainWindow(string NameWindow)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Title == NameWindow)
                    return (dynamic)window;
            }
            return null;
        }

        public static dynamic LinkNameWindow(string NameWindow)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == NameWindow)
                    return (dynamic)window;
            }
            return null;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            MapOpisViewModel.VersiyaBack();
            

        }


    }
}
