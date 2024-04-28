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
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    class ViewNsiGrQualification : BaseViewModel
    {
        public static WinNsiGrQualification WindowNsiUri = MainWindow.LinkMainWindow("WinNsiGrQualification");

        string controlerGroupQualification =  "/api/GroupQualificationController/";
        private ModelGroupQualification selectedGroupQualification;
        public static ObservableCollection<ModelGroupQualification> NsiGroupQualifications { get; set; }

        public ModelGroupQualification SelectedGroupQualification
        { get { return selectedGroupQualification; } set { selectedGroupQualification = value; OnPropertyChanged("SelectedGroupQualification"); } }
        // конструктор класса
        public ViewNsiGrQualification()
        {
            if (MapOpisViewModel.ViewGroupQualifications == null)
            {
                MainWindow.UrlServer = controlerGroupQualification;
                CallServer.PostServer(MainWindow.UrlServer, controlerGroupQualification, "GET");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableGrViewQualification(CmdStroka);
            }
            else
            {
                NsiGroupQualifications = MapOpisViewModel.ViewGroupQualifications;
            }
        }

        public static void ObservableGrViewQualification(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGroupQualification>(CmdStroka);
            List<ModelGroupQualification> res = result.ViewGroupQualification.ToList();
            NsiGroupQualifications = new ObservableCollection<ModelGroupQualification>((IEnumerable<ModelGroupQualification>)res);

        }

        // команда закрытия окна
        RelayCommand? closeGrQualification;
        public RelayCommand CloseGrQualification
        {
            get
            {
                return closeGrQualification ??
                  (closeGrQualification = new RelayCommand(obj =>
                  {
                      WinNsiGrQualification WindowMen = MainWindow.LinkMainWindow("WinNsiGrQualification");
                      WindowMen.Close();
                  }));
            }
        }
        // команда закрытия окна
        RelayCommand? saveGrQualification;
        public RelayCommand SaveGrQualification
        {
            get
            {
                return saveGrQualification ??
                  (saveGrQualification = new RelayCommand(obj =>
                  {
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      WinNsiGrQualification WindowMen = MainWindow.LinkMainWindow("WinNsiGrQualification");
                      if (SelectedGroupQualification != null)
                      {
                          Windowmain.Qualificationt4.Text = SelectedGroupQualification.kodGroupQualification.ToString();
                          Windowmain.GrDetailingst4.Text = SelectedGroupQualification.kodGroupQualification.ToString() + ":  " + SelectedGroupQualification.nameGroupQualification.ToString();
                          Windowmain.Featuret3.Text = ":           " + SelectedGroupQualification.nameGroupQualification.ToString();
                          if (MapOpisViewModel.ActCreatInterview == "CreatInterview")
                          { 
                              switch (MapOpisViewModel.ActCompletedInterview)
                              {
                                  case "Guest":
                                      MapOpisViewModel.SelectContentCompleted();
                                      break;
                                  case null:
                                      if (ViewModelCreatInterview.ContentIntervs != null) ViewModelCreatInterview.SelectContentCompl();
                                      break;
                              }                          
                          
                          }

                      }
                  }));
            }
        }

  
    }
}
