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
    class ViewModelNsiRecommen : BaseViewModel
    {
        private WinNsiListRecommen WindowMen = MainWindow.LinkMainWindow("WinNsiListRecommen");
        private string pathcontrapirecom = "/api/RecommendationController/";
        private ModelRecommendation selectedRecommendation;
        public static ObservableCollection<ModelRecommendation> NsiModelRecommendations { get; set; }


        public ModelRecommendation SelectedModelRecommendation
        { get { return selectedRecommendation; } set { selectedRecommendation = value; OnPropertyChanged("SelectedModelRecommendation"); } }
        // конструктор класса
        public ViewModelNsiRecommen()
        {
            if (MapOpisViewModel.ModelRecommendations == null)
            {
                MainWindow.UrlServer =  pathcontrapirecom;
                CallServer.PostServer(MainWindow.UrlServer,  pathcontrapirecom, "GET");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelRecommendation(CmdStroka);

            }
            else { NsiModelRecommendations = MapOpisViewModel.ModelRecommendations; }

        }

        public static void ObservableNsiModelRecommendation(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelRecommendation>(CmdStroka);
            List<ModelRecommendation> res = result.ViewRecommendation.ToList();
            NsiModelRecommendations = new ObservableCollection<ModelRecommendation>((IEnumerable<ModelRecommendation>)res);

        }

        // команда закрытия окна
        RelayCommand? closeRecommendation;
        public RelayCommand CloseRecommendation
        {
            get
            {
                return closeRecommendation ??
                  (closeRecommendation = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WindowMen.Close();
                      
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? selectModelRecommendation;
        public RelayCommand SelectModelRecommendation
        {
            get
            {
                return selectModelRecommendation ??
                  (selectModelRecommendation = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");

                      if (SelectedModelRecommendation != null)
                      {
                          WindowMain.Dependencyt3.Text = SelectedModelRecommendation.contentRecommendation;
                          MapOpisViewModel.selectedDependency.kodRecommend = SelectedModelRecommendation.kodRecommendation;
                          WindowMain.Interviewt5.Text = SelectedModelRecommendation.kodRecommendation.ToString() + ": " + SelectedModelRecommendation.contentRecommendation;
                          WindowMain.LikarInterviewt5.Text = SelectedModelRecommendation.kodRecommendation.ToString() + ": " + SelectedModelRecommendation.contentRecommendation;
                          WindowMain.InterviewDependencyt3.Text = SelectedModelRecommendation.kodRecommendation.ToString() + ": " + SelectedModelRecommendation.contentRecommendation;
                          WindowMen.Close();
                      }
                      
                  }));
            }
        }

        

        // Выбор названия опитування
        private RelayCommand? searchRecomen;
        public RelayCommand SearchRecomen
        {
            get
            {
                return searchRecomen ??
                  (searchRecomen = new RelayCommand(obj =>
                  {
                      MetodKeyEnterRecomen();
                  }));
            }
        }
        
        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyEnterRecomen;
        public RelayCommand CheckKeyEnterRecomen
        {
            get
            {
                return checkKeyEnterRecomen ??
                  (checkKeyEnterRecomen = new RelayCommand(obj =>
                  {
                      MetodKeyEnterRecomen();
                  }));
            }
        }

        private void MetodKeyEnterRecomen()
        {
            if (WindowMen.PoiskRecomen.Text.Trim() != "")
            {
                string jason = pathcontrapirecom + "0/" + WindowMen.PoiskRecomen.Text;
                CallServer.PostServer(pathcontrapirecom, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelRecommendation(CmdStroka);
                WindowMen.TablFeature.ItemsSource = NsiModelRecommendations;
            }
        }
    }
}
