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
        private WinNsiIcd WindowNsiIcdUri = MainWindow.LinkMainWindow("WinNsiIcd");
        public static string controlerNsiIcd =  "/api/IcdController/";
        private ModelIcd selectedVeiwIcd;
        public static ObservableCollection<ModelIcd> VeiwIcds { get; set; }

        public ModelIcd SelectedVeiwIcd
        { get { return selectedVeiwIcd; } set { selectedVeiwIcd = value; OnPropertyChanged("SelectedVeiwIcd"); } }
        // конструктор класса
        public VeiwModelNsiIcd()
        {
            if (MapOpisViewModel.GrupDiagnoz == "") CallServer.PostServer(controlerNsiIcd, controlerNsiIcd, "GET");
            else
            { 
                if(MapOpisViewModel.ActCompletedInterview == "KeiIcdGrup") CallServer.PostServer(controlerNsiIcd, controlerNsiIcd + "0/" + MapOpisViewModel.GrupDiagnoz , "GETID");
                else CallServer.PostServer(controlerNsiIcd, controlerNsiIcd + "0/"+ MapOpisViewModel.GrupDiagnoz, "GETID");
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

        // Выбор названия интервью диагностики 
        private RelayCommand? searchIcd;
        public RelayCommand SearchIcd
        {
            get
            {
                return searchIcd ??
                  (searchIcd = new RelayCommand(obj =>
                  {
                      MetodIcdEnter();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? selectIcdDiagnoz;
        public RelayCommand SelectIcdDiagnoz
        {
            get
            {
                return selectIcdDiagnoz ??
                  (selectIcdDiagnoz = new RelayCommand(obj =>
                  {
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (selectedVeiwIcd != null)
                      {
                          Windowmain.Diagnozt4.Text = SelectedVeiwIcd.keyIcd.ToString();
                          Windowmain.Diagnozt3.Text = SelectedVeiwIcd.name.ToString();
                          Windowmain.LibDiagnozt3.Text = SelectedVeiwIcd.name.ToString();
                          WindowNsiIcdUri.Close();
                      }
                      
                  }));
            }
        }

        RelayCommand? checkKeyText;
        public RelayCommand CheckKeyText
        {
            get
            {
                return checkKeyText ??
                  (checkKeyText = new RelayCommand(obj =>
                  {
                      MetodIcdEnter();
                  }));
            }
        }
        public void MetodIcdEnter()
        {

            if (WindowNsiIcdUri.PoiskIcd.Text.Trim() != "")
            {
                string jason = controlerNsiIcd + "0/" + WindowNsiIcdUri.PoiskIcd.Text;
                CallServer.PostServer(controlerNsiIcd, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewNsiIcd(CmdStroka);
                WindowNsiIcdUri.TablFeature.ItemsSource = VeiwIcds;
            }
        }
    }

}
