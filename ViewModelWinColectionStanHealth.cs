using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    class ViewModelWinColectionStanHealth : BaseViewModel
    {

        private bool loadboolStanHealth = false;
        public static PacientMapAnaliz selectPacientStanHealth;

        public PacientMapAnaliz SelectedPacientStanHealth
        {
            get { return selectPacientStanHealth; }
            set { selectPacientStanHealth = value; OnPropertyChanged("SelectedPacientStanHealth"); }
        }
        public static ObservableCollection<PacientMapAnaliz> ViewPacientStanHealths { get; set; }


        public ViewModelWinColectionStanHealth()
        {
            LoadStanHealth();
            loadboolStanHealth = true;

        }

        public static void LoadStanHealth()
        {
            CallServer.PostServer(MapOpisViewModel.pathcontrollerPacientMapAnaliz, MapOpisViewModel.pathcontrollerPacientMapAnaliz + MapOpisViewModel._pacientProfil + "/0", "GETID"); // 
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionPacientStanHealth(CmdStroka);
        }

        public static void ObservablelColectionPacientStanHealth(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListPacientMapAnaliz>(CmdStroka);
            List<PacientMapAnaliz> res = result.PacientMapAnaliz.ToList();
            ViewPacientStanHealths = new ObservableCollection<PacientMapAnaliz>((IEnumerable<PacientMapAnaliz>)res);
            
        }

        private RelayCommand? onVisibleObjStanHealth;
        public RelayCommand OnVisibleObjStanHealth
        {
            get
            {
                return onVisibleObjStanHealth ??
                  (onVisibleObjStanHealth = new RelayCommand(obj =>
                  {
                      WinColectionStanHealth WinStanHealth = MainWindow.LinkMainWindow("WinColectionStanHealth");
                      if (WinStanHealth.ColectionStanHealthTablGrid.SelectedIndex == -1) return;
                      if (ViewPacientStanHealths != null)
                      {
                         
                          WinStanHealth.DateAnaliz.Text = ViewPacientStanHealths[WinStanHealth.ColectionStanHealthTablGrid.SelectedIndex].dateAnaliza;

                      }
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? closeStanHealth;
        public RelayCommand CloseStanHealth
        {
            get
            {
                return closeStanHealth ??
                  (closeStanHealth = new RelayCommand(obj =>
                  {
                      WinColectionStanHealth WinStanHealth = MainWindow.LinkMainWindow("WinColectionStanHealth");
                      WinStanHealth.Close();
                  }));
            }
        }
    }
}
