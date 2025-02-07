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
using System.Windows.Shapes;

namespace BackSeam
{
    /// <summary>
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    /// <summary>
    /// Логика взаимодействия для NsiDetailing.xaml
    /// </summary>
    public partial class NsiDetailing : Window
    {
        public NsiDetailing()
        {
            InitializeComponent();
            MapOpisViewModel.ViewNsiDetaling();
            NsiDetailing WindowMen = MainWindow.LinkMainWindow("NsiDetailing");
            //if (MapOpisViewModel.ActCreatInterview == "CreatInterview") WindowMen.BorderAddAll.Visibility = Visibility.Hidden;
            if (ViewModelNsiDetailing.NsiModelDetailings.Count == 0) WindowMen.Close();
        }
    }
}
