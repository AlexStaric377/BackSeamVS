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
    class ViewModelLikarGrupDiagnoz : BaseViewModel
    {

        WinLikarGrupDiagnoz WindowLikarGrDiag = MainWindow.LinkMainWindow("WinLikarGrupDiagnoz");
        public static string controlerLikarGrDiagnoz = "/api/LikarGrupDiagnozController/";
        private static ModelLikarGrupDiagnoz selectedLikarGrupDiagnoz;

        public static ObservableCollection<ModelLikarGrupDiagnoz> LikarGrupDiagnozs { get; set; }
        public static ObservableCollection<ModelLikarGrupDiagnoz> AddLikarGrupDiagnozs { get; set; }
        public ModelLikarGrupDiagnoz SelectedLikarGrupDiagnoz
        { get { return selectedLikarGrupDiagnoz; } set { selectedLikarGrupDiagnoz = value; OnPropertyChanged("SelectedLikarGrupDiagnoz"); } }

        public static void ObservableViewLikarGrDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelLikarGrupDiagnoz>(CmdStroka);
            List<ModelLikarGrupDiagnoz> res = result.ModelLikarGrupDiagnoz.ToList();
            LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>((IEnumerable<ModelLikarGrupDiagnoz>)res);


        }
        // конструктор класса
        public ViewModelLikarGrupDiagnoz()
        {
            LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            AddLikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            string json = controlerLikarGrDiagnoz + MapOpisViewModel._kodDoctor+"/0";
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableViewLikarGrDiagnoz(CmdStroka);
 

        }

        // команда закрытия окна
        RelayCommand? closeLikarGrDiagnoz;
        public RelayCommand CloseLikarGrDiagnoz
        {
            get
            {
                return closeLikarGrDiagnoz ??
                  (closeLikarGrDiagnoz = new RelayCommand(obj =>
                  {
                      WindowLikarGrDiag.Close();
                  }));
            }
        }

        RelayCommand? selectLikarGrDiagnoz;
        public RelayCommand SelectLikarGrDiagnoz
        {
            get
            {
                return selectLikarGrDiagnoz ??
                  (selectLikarGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedLikarGrupDiagnoz != null)
                      {
                          Windowmain.Diagnozt1.Text = selectedLikarGrupDiagnoz.icdGrDiagnoz;
                      }
                      WindowLikarGrDiag.Close();
                  }));
            }
        }

        RelayCommand? removeLikarGrDiagnoz;
        public RelayCommand RemoveLikarGrDiagnoz
        {
            get
            {
                return removeLikarGrDiagnoz ??
                  (removeLikarGrDiagnoz = new RelayCommand(obj =>
                  {
                      if (WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex >= 0)
                      {
                          string json = controlerLikarGrDiagnoz + LikarGrupDiagnozs[WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex].id.ToString();
                          CallServer.PostServer(controlerLikarGrDiagnoz, json, "DELETE");
                          LikarGrupDiagnozs.Remove(LikarGrupDiagnozs[WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex]);
                      }
                  }));
            }
        }



        RelayCommand? addLikarGrDiagnoz;
        public RelayCommand AddLikarGrDiagnoz
        {
            get
            {
                return addLikarGrDiagnoz ??
                  (addLikarGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      Windowmain.Diagnozt1.Text = "";
                      WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      if (Windowmain.Diagnozt1.Text != null)MetodAddLikarGrDiagnoz(Windowmain.Diagnozt1.Text);
 
                  }));
            }
        }

        public static void MetodAddLikarGrDiagnoz(string grdiagnoz = "")
        {
            selectedLikarGrupDiagnoz = new ModelLikarGrupDiagnoz();
            selectedLikarGrupDiagnoz.kodDoctor = MapOpisViewModel._kodDoctor;
            selectedLikarGrupDiagnoz.icdGrDiagnoz = grdiagnoz;
            ModelLikarGrupDiagnoz addLikarGrupDiagnoz = selectedLikarGrupDiagnoz;
            if (MapOpisViewModel._kodDoctor != "")
            {
                string json = JsonConvert.SerializeObject(selectedLikarGrupDiagnoz);
                CallServer.PostServer(controlerLikarGrDiagnoz, json, "POST");
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelLikarGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelLikarGrupDiagnoz>(CallServer.ResponseFromServer);
                if (LikarGrupDiagnozs == null) LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
                if (Idinsert != null) addLikarGrupDiagnoz = Idinsert;
            }
 
            LikarGrupDiagnozs.Add(addLikarGrupDiagnoz);
            if (MapOpisViewModel.SelectActivGrupDiagnoz == "WorkGrupDiagnoz")
            {
                AddLikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
                AddLikarGrupDiagnozs.Add(addLikarGrupDiagnoz);
            }
        }

    }
}
