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
using System.ComponentModel;
using System.Threading;


namespace BackSeam
{
    /// <summary>
    /// Логика взаимодействия для StrokaProgressBar.xaml
    /// </summary>
    public partial class WinStrokaProgressBar : Window
    {
		public static int AutoCloseTick = 0, SetTimeClose = 0;
		System.Windows.Threading.DispatcherTimer CloseAuto = new System.Windows.Threading.DispatcherTimer();

		public WinStrokaProgressBar()
        {
            InitializeComponent();

			SetTimeClose = 1;
			CloseAuto.Tick += CloseAutoTick;
			CloseAuto.Interval = TimeSpan.FromSeconds(1);
			CloseAuto.Start();
		}


		private void CloseAutoTick(object sender, EventArgs e)
		{
			--SetTimeClose;
			if (SetTimeClose < 0)
			{
				CloseAuto.Stop();
				this.Close();
			}
		}

	}


}
