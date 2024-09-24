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
    public class ViewModelMedicalGrDiagnoz : BaseViewModel
    {

        WinMedicalGrDiagnoz WindowMedGrDiag = MainWindow.LinkMainWindow("WinMedicalGrDiagnoz");
        public static string controlerGrDiagnoz = "/api/MedGrupDiagnozController/";
        private ModelMedGrupDiagnoz selectedMedGrupDiagnoz;

        public static ObservableCollection<ModelMedGrupDiagnoz> MedGrupDiagnozs { get; set; }

        public ModelMedGrupDiagnoz SelectedMedGrupDiagnoz
        { get { return selectedMedGrupDiagnoz; } set { selectedMedGrupDiagnoz = value; OnPropertyChanged("SelectedMedGrupDiagnoz"); } }

        public static void ObservableViewGrDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelMedGrupDiagnoz>(CmdStroka);
            List<ModelMedGrupDiagnoz> res = result.ModelMedGrupDiagnoz.ToList();
            MedGrupDiagnozs = new ObservableCollection<ModelMedGrupDiagnoz>((IEnumerable<ModelMedGrupDiagnoz>)res);


        }
        // конструктор класса
        public ViewModelMedicalGrDiagnoz()
        {
            string json = controlerGrDiagnoz + MapOpisViewModel.EdrpouMedZaklad+"/0";
            CallServer.PostServer(controlerGrDiagnoz, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableViewGrDiagnoz(CmdStroka);
 

        }

        // команда закрытия окна
        RelayCommand? closeMedGrDiagnoz;
        public RelayCommand CloseMedGrDiagnoz
        {
            get
            {
                return closeMedGrDiagnoz ??
                  (closeMedGrDiagnoz = new RelayCommand(obj =>
                  {
                      WindowMedGrDiag.Close();
                  }));
            }
        }

        RelayCommand? selectMedGrDiagnoz;
        public RelayCommand SelectMedGrDiagnoz
        {
            get
            {
                return selectMedGrDiagnoz ??
                  (selectMedGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedMedGrupDiagnoz != null)
                      {
                          Windowmain.Diagnozt1.Text = selectedMedGrupDiagnoz.icdGrDiagnoz;
                      }
                      WindowMedGrDiag.Close();
                  }));
            }
        }


       
        RelayCommand? removeMedGrDiagnoz;
        public RelayCommand RemoveMedGrDiagnoz
        {
            get
            {
                return removeMedGrDiagnoz ??
                  (removeMedGrDiagnoz = new RelayCommand(obj =>
                  {
                      if (WindowMedGrDiag.TablMedGrupDiagnoz.SelectedIndex >= 0)
                      {
                          string json = controlerGrDiagnoz + MedGrupDiagnozs[WindowMedGrDiag.TablMedGrupDiagnoz.SelectedIndex].id.ToString();
                          CallServer.PostServer(controlerGrDiagnoz, json, "DELETE");
                          MedGrupDiagnozs.Remove(MedGrupDiagnozs[WindowMedGrDiag.TablMedGrupDiagnoz.SelectedIndex]);
                      }

                  }));
            }
        }

        

        RelayCommand? addMedGrDiagnoz;
        public RelayCommand AddMedGrDiagnoz
        {
            get
            {
                return addMedGrDiagnoz ??
                  (addMedGrDiagnoz = new RelayCommand(obj =>
                  {
                        MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                        Windowmain.Diagnozt1.Text = "";
                        MapOpisViewModel.ActCompletedInterview = "NameGrDiagnoz";
                        WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
                        NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
                        NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                        NewOrder.ShowDialog();
                      MapOpisViewModel.ActCompletedInterview = "";
                        if (Windowmain.Diagnozt1.Text != null)
                        { 
                                
                            selectedMedGrupDiagnoz = new  ModelMedGrupDiagnoz();
                            selectedMedGrupDiagnoz.edrpou = MapOpisViewModel.EdrpouMedZaklad;
                            selectedMedGrupDiagnoz.icdGrDiagnoz = Windowmain.Diagnozt1.Text;
                            string json = JsonConvert.SerializeObject(selectedMedGrupDiagnoz);
                            CallServer.PostServer(controlerGrDiagnoz, json, "POST");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelMedGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelMedGrupDiagnoz>(CallServer.ResponseFromServer);
                            if (Idinsert != null)
                            {
                                MedGrupDiagnozs.Add(Idinsert);
                            }
                        }
                          
                  }));
            }
        }
    }
}
