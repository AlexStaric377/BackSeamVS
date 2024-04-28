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
using System.Windows.Controls;

namespace BackSeam
{
    public  partial class VeiwModelNsiIcd : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

        public static string controlerNsiIcd =  "/api/IcdController/";
        private ModelIcd selectedVeiwIcd;
        public static ObservableCollection<ModelIcd> VeiwIcds { get; set; }

        public ModelIcd SelectedVeiwIcd
        { get { return selectedVeiwIcd; } set { selectedVeiwIcd = value; OnPropertyChanged("SelectedVeiwIcd"); } }
        // конструктор класса
        public VeiwModelNsiIcd()
        {

            if (MapOpisViewModel.GrupDiagnoz == "")
            {
                CallServer.PostServer(controlerNsiIcd, controlerNsiIcd, "GET");
            }
            else
            {
                CallServer.PostServer(controlerNsiIcd, controlerNsiIcd + "0/"+ MapOpisViewModel.GrupDiagnoz, "GETID");
            }
               
                
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewNsiIcd(CmdStroka);


        }

        public static void ObservableViewNsiIcd(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
            List<ModelIcd> res = result.ModelIcd.ToList();
            VeiwIcds = new ObservableCollection<ModelIcd>((IEnumerable<ModelIcd>)res);

        }

        // команда закрытия окна
        RelayCommand? closeVeiwIcd;
        public RelayCommand CloseVeiwIcd
        {
            get
            {
                return closeVeiwIcd ??
                  (closeVeiwIcd = new RelayCommand(obj =>
                  {
                      WinNsiIcd WindowNsiIcdUri = MainWindow.LinkMainWindow("WinNsiIcd");
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedVeiwIcd != null)
                      {
                          Windowmain.Diagnozt4.Text = SelectedVeiwIcd.keyIcd.ToString();
                          Windowmain.Diagnozt3.Text = SelectedVeiwIcd.name.ToString();
                          Windowmain.LibDiagnozt3.Text = SelectedVeiwIcd.name.ToString();

                      }
                      WindowNsiIcdUri.Close();
                  }));
            }
        }

    }

}
