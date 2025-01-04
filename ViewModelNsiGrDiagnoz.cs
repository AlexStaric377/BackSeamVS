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
    public class ViewModelNsiGrDiagnoz : BaseViewModel
    {

        WinNsiListGrDiagnoz WindowNsiGrDiag = MainWindow.LinkMainWindow("WinNsiListGrDiagnoz");
        public static string controlerGrDiagnoz = "/api/GrupDiagnozController/";
        private ModelGrupDiagnoz selectedViewGrupDiagnoz;
        private MedGrupDiagnoz selectedMedGrupDiagnoz;

        public static ObservableCollection<ModelGrupDiagnoz> ViewGrupDiagnozs { get; set; }

        public ModelGrupDiagnoz SelectedViewGrupDiagnoz
        { get { return selectedViewGrupDiagnoz; } set { selectedViewGrupDiagnoz = value; OnPropertyChanged("SelectedViewGrupDiagnoz"); } }

        public static void ObservableViewGrDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGrupDiagnoz>(CmdStroka);
            List<ModelGrupDiagnoz> res = result.ModelGrupDiagnoz.ToList();
            ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>((IEnumerable<ModelGrupDiagnoz>)res);
 
            
        }
        // конструктор класса
        public ViewModelNsiGrDiagnoz()
        {
                CallServer.PostServer(controlerGrDiagnoz, controlerGrDiagnoz, "GET");
                string CmdStroka = CallServer.ServerReturn();
                ObservableViewGrDiagnoz(CmdStroka);

        }

       

        // команда закрытия окна
        RelayCommand? closeVeiwGrDiagnoz;
        public RelayCommand CloseVeiwGrDiagnoz
        {
            get
            {
                return closeVeiwGrDiagnoz ??
                  (closeVeiwGrDiagnoz = new RelayCommand(obj =>
                  {
                      //MapOpisViewModel.cikl = false;
                      WindowNsiGrDiag.Close();
                  }));
            }
        }

        RelayCommand? selectVeiwGrDiagnoz;
        public RelayCommand SelectVeiwGrDiagnoz
        {
            get
            {
                return selectVeiwGrDiagnoz ??
                  (selectVeiwGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedViewGrupDiagnoz != null)
                      {
                          Windowmain.Diagnozt1.Text = selectedViewGrupDiagnoz.nameGrDiagnoz;
                          WindowNsiGrDiag.Close();
                      }

                  }));
            }
        }

        RelayCommand? addVeiwGrDiagnoz;
        public RelayCommand AddVeiwGrDiagnoz
        {
            get
            {
                return addVeiwGrDiagnoz ??
                  (addVeiwGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      Windowmain.Diagnozt1.Text = "";
                      WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();

                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
 
                  }));
            }
        }

        
        // команда поиска по наименованию групового напрвления диагноза
        RelayCommand? searchGrDiagnoz;
        public RelayCommand SearchGrDiagnoz
        {
            get
            {
                return searchGrDiagnoz ??
                  (searchGrDiagnoz = new RelayCommand(obj =>
                  {
                      MetodGrupDiagnozEnter();
                  }));
            }
        }

        
        // команда выбора по наименованию групового напрвления диагноза
        RelayCommand? selectGrupDiagnoz;
        public RelayCommand SelectGrupDiagnoz
        {
            get
            {
                return selectGrupDiagnoz ??
                  (selectGrupDiagnoz = new RelayCommand(obj =>
                  {
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedViewGrupDiagnoz != null)
                      {
                          switch (MapOpisViewModel.ActCompletedInterview)
                          {
                              case "IcdGrDiagnoz":
                                  Windowmain.Diagnozt1.Text = selectedViewGrupDiagnoz.icdGrDiagnoz;
                                  break;
                              case "NameGrDiagnoz":
                                  Windowmain.Diagnozt1.Text = selectedViewGrupDiagnoz.nameGrDiagnoz;
                                  break;
                              default:
                                  
                                  break;

                          }
                          MapOpisViewModel.selectednameGrDiagnoz = selectedViewGrupDiagnoz.nameGrDiagnoz;
                          WindowNsiGrDiag.Close();
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
                      MetodGrupDiagnozEnter();
                  }));
            }
        }
        public void MetodGrupDiagnozEnter()
        {

            WinNsiListGrDiagnoz WindowNsiGrDiag = MainWindow.LinkMainWindow("WinNsiListGrDiagnoz");
            string jason = controlerGrDiagnoz + "0/" + WindowNsiGrDiag.PoiskGrDiagnoz.Text;
            CallServer.PostServer(controlerGrDiagnoz, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewGrDiagnoz(CmdStroka);
            WindowNsiGrDiag.TablGrupDiagnozs.ItemsSource = ViewGrupDiagnozs;
        }



    }
}
