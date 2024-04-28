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
            //if (MapOpisViewModel.SelectActivGrupDiagnoz == "GrupDiagnoz")
            //{
            //    ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>();
            //    foreach (ModelDiagnoz modelDiagnoz in MapOpisViewModel.TmpDiagnozs)
            //    {
            //        selectedViewGrupDiagnoz = new  ModelGrupDiagnoz();
            //        selectedViewGrupDiagnoz.icdGrDiagnoz = modelDiagnoz.icdGrDiagnoz;
            //        selectedViewGrupDiagnoz.nameGrDiagnoz = modelDiagnoz.nameDiagnoza;
            //        ViewGrupDiagnozs.Add(selectedViewGrupDiagnoz);
            //    }
            //}
            //else
            //{
                CallServer.PostServer(controlerGrDiagnoz, controlerGrDiagnoz, "GET");
                string CmdStroka = CallServer.ServerReturn();
                ObservableViewGrDiagnoz(CmdStroka);

            //}


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

                      NewOrder.Left = (MainWindow.ScreenWidth / 2)+100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      if (Windowmain.Diagnozt1.Text != null)
                      {

                          //selectedViewGrupDiagnoz = new ModelGrupDiagnoz();
                          //selectedViewGrupDiagnoz. = MapOpisViewModel._kodDoctor;
                          //selectedViewGrupDiagnoz.icdGrDiagnoz = Windowmain.Diagnozt1.Text;
                          //string json = JsonConvert.SerializeObject(selectedLikarGrupDiagnoz);
                          //CallServer.PostServer(controlerLikarGrDiagnoz, json, "POST");
                          //CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          //ModelLikarGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelLikarGrupDiagnoz>(CallServer.ResponseFromServer);
                          //if (Idinsert != null)
                          //{
                          //    LikarGrupDiagnozs.Add(Idinsert);
                          //}
                      }
                  }));
            }
        }

    }
}
