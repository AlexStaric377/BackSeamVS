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
    /// Логика взаимодействия для MessageWarn.xaml
    /// </summary>
    public partial class MessageWarn : Window
    {
        public static int AutoCloseTick = 0, SetTimeClose = 0;
        System.Windows.Threading.DispatcherTimer CloseAuto = new System.Windows.Threading.DispatcherTimer();
        public MessageWarn(string TextWindows = null, int AutoClose = 0, int TimeClose = 0)
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
            this.MessageText.Text = TextWindows;
            if (TextWindows == null) this.MessageText.Text = "Сообщение отсутствует!";
            //{
            //    // Определить высоту окна по количеству \n в многострочном тексте
            //    var TextWindows_a = TextWindows.Split('\n');

            //    this.MessageText.Text = TextWindows != null ? TextWindows : "Сообщение отсутствует!";
            //    //}
            //}
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
