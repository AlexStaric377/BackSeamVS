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
    class ViewModelNsiQualification : BaseViewModel
    {
  
        private string pathcontroller =  "/api/QualificationController/";
        public static ModelQualification selectedlQualification;
        public static ObservableCollection<ModelQualification> NsiModelQualifications { get; set; }
        public ModelQualification SelectedQualification
        { get { return selectedlQualification; } set { selectedlQualification = value; OnPropertyChanged("SelectedQualification"); } }
        // конструктор класса
        public ViewModelNsiQualification()
        {
            MainWindow.UrlServer = pathcontroller;
            string jason = "";
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Qualification":
                    jason = pathcontroller + "0/" + MapOpisViewModel.selectedGroupQualification.kodGroupQualification + "/0";
                    break;
       
                default:
                    jason = pathcontroller + "0/" + ViewModelNsiGrDetailing.selectedGrDetailing.kodGroupQualification + "/0"; ;
                    break;
            }


            CallServer.PostServer(MainWindow.UrlServer, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelFeatures(CmdStroka);
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelQualification>(CmdStroka);
            List<ModelQualification> res = result.ViewQualification.ToList();
            NsiModelQualifications = new ObservableCollection<ModelQualification>((IEnumerable<ModelQualification>)res);
        }

        // команда закрытия окна
        RelayCommand? closeNsiModelQualification;
        public RelayCommand CloseNsiModelQualification
        {
            get
            {
                return closeNsiModelQualification ??
                  (closeNsiModelQualification = new RelayCommand(obj =>
                  {
                      WinNsiQualification WindowMen = MainWindow.LinkMainWindow("WinNsiQualification");
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectNsiModelQualification;
        public RelayCommand SelectNsiModelQualification
        {
            get
            {
                return selectNsiModelQualification ??
                  (selectNsiModelQualification = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WinNsiQualification WindowMen = MainWindow.LinkMainWindow("WinNsiQualification");
                      if (selectedlQualification != null)
                      {
                          if (MapOpisViewModel.ActCreatInterview == "CreatInterview")
                          {
                              MapOpisViewModel.nameFeature3 = selectedlQualification.kodQualification.ToString() + ":            " + selectedlQualification.nameQualification.ToString();
                              WindowMain.Featuret3.Text = selectedlQualification.kodQualification.ToString() + ":            " + selectedlQualification.nameQualification.ToString();
                              switch (MapOpisViewModel.ActCompletedInterview)
                              {
                                  case "Admin":
                                      break;
                                  case "Guest":
                                      MapOpisViewModel.SelectContentCompleted();
                                      break;
                                  case "Likar":
                                      MapOpisViewModel.SelectContentCompleted();
                                      break;
                                  case "Pacient":
                                      MapOpisViewModel.SelectContentCompleted();
                                      break;
                                  default:
                                      if (ViewModelCreatInterview.ContentIntervs != null) ViewModelCreatInterview.SelectContentCompl();
                                      break;
                              }
                          }
    
                      }
                      WindowMen.TablQualification.SelectedItem = null;
                  }));
            }
        }

        

        RelayCommand? selectQualification;
        public RelayCommand SelectQualification
        {
            get
            {
                return selectQualification ??
                  (selectQualification = new RelayCommand(obj =>
                  {
                      
                  },
                 (obj) => NsiModelQualifications != null));
            }
        }

    }
}
