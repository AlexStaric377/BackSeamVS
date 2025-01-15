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
    /// Логика взаимодействия для WaitWindow.xaml
    /// </summary>
    public partial class WaitWindow : Window
    {
 

        public static int AutoCloseTick = 0, SetTimeClose = 0;
        System.Windows.Threading.DispatcherTimer CloseAuto = new System.Windows.Threading.DispatcherTimer();
    public WaitWindow(string TextWindows = null, int AutoClose = 0, int TimeClose = 0)
        {
            InitializeComponent();

            if (AutoClose == 1 || AutoClose == 2)
            {
                // Автозакрытие окна
                SetTimeClose = TimeClose;
                AutoCloseTick = AutoClose;
                CloseAuto.Tick += CloseAutoTick;
                CloseAuto.Interval = TimeSpan.FromSeconds(1);
                CloseAuto.Start();
            }
 
        }

        private void CloseAutoTick(object sender, EventArgs e)
        {
            --SetTimeClose;
            if (SetTimeClose < 0)
            {
                CloseAuto.Stop();
                if (AutoCloseTick == 2)
                {
                    this.Close();
                }
                if (AutoCloseTick == 1)
                {
                    this.Close();
                }

            }

        }
    }
}
