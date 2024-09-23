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
    class ViewModelNsiIntreview : BaseViewModel
    {
        private MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
        private WinNsiIntreview WindowMen = MainWindow.LinkMainWindow("WinNsiIntreview");
        private string pathcontroller =  "/api/InterviewController/";
        public static ModelInterview selectedInterview;
        public static ObservableCollection<ModelInterview> NsiModelInterviews { get; set; }
        public ModelInterview SelectedModelInterview
        { get { return selectedInterview; } set { selectedInterview = value; OnPropertyChanged("SelectedModelInterview"); } }
        // конструктор класса
        public ViewModelNsiIntreview()
        {
            MainWindow.UrlServer = pathcontroller;
            CallServer.PostServer(MainWindow.UrlServer, pathcontroller , "GET");
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelFeatures(CmdStroka);
            selectedInterview = new ModelInterview();
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
            List<ModelInterview> res = result.ModelInterview.ToList();
            NsiModelInterviews = new ObservableCollection<ModelInterview>((IEnumerable<ModelInterview>)res);
        }

        // команда закрытия окна
        RelayCommand? closeModelInterview;
        public RelayCommand CloseModelInterview
        {
            get
            {
                return closeModelInterview ??
                  (closeModelInterview = new RelayCommand(obj =>
                  {
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelIntreview;
        public RelayCommand SelectModelIntreviewg
        {
            get
            {
                return selectModelIntreview ??
                  (selectModelIntreview = new RelayCommand(obj =>
                  {
                      if (selectedInterview.id != 0)
                      {
                          WindowMain.Dependencyt4.Text = selectedInterview.nametInterview.ToString();
                          MapOpisViewModel.selectedDependency.kodProtokola = selectedInterview.kodProtokola;
                          WindowMen.Close();
                      }
                      
                  }));
            }
        }

        

        // Выбор названия опитування
        private RelayCommand? searchDeliting;
        public RelayCommand SearchDeliting
        {
            get
            {
                return searchDeliting ??
                  (searchDeliting = new RelayCommand(obj =>
                  {
                      if (WindowMen.PoiskDeliting.Text.Trim() != "")
                      {
                          string jason = pathcontroller + "0/0/0/" + WindowMen.PoiskDeliting.Text;
                          CallServer.PostServer(pathcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableNsiModelFeatures(CmdStroka);
                          WindowMen.TablDeliting.ItemsSource = NsiModelInterviews;
                      }

                  }));
            }
        }

    }
}
