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
/// Многопоточность
using System.Threading;
using System.Windows.Threading;
using System.ServiceProcess;
using System.Diagnostics;


namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ViewModelResultInterview : BaseViewModel
    {
        
        private string pathcontroller =  "/api/InterviewController/";
        public static ModelInterview selectedResultInterview;
        public static ModelResultInterview selectItogInterview;
        public static ObservableCollection<ModelResultInterview> ResultInterviews { get; set; }
        public ModelResultInterview SelectedResultInterview
        { get { return selectItogInterview; } set { selectItogInterview = value; OnPropertyChanged("SelectedResultInterview"); } }
        // конструктор класса
        public ViewModelResultInterview()
        {
            CallServer.PostServer(pathcontroller, pathcontroller + MapOpisViewModel.modelColectionInterview.kodProtokola + "/0/0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelInterview Interview = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
                selectedResultInterview = Interview;
                          
            }

            selectItogInterview = new ModelResultInterview();
            selectItogInterview.dateInterview = MapOpisViewModel.modelColectionInterview.dateInterview;
            selectItogInterview.nameDiagnoza = MapOpisViewModel.NameDiagnoz;
            selectItogInterview.nameRecommendation = MapOpisViewModel.NameRecomendaciya;
            selectItogInterview.nametInterview = MapOpisViewModel.modelColectionInterview.nameInterview;
            selectItogInterview.opistInterview = selectedResultInterview.opistInterview;
            selectItogInterview.uriInterview = selectedResultInterview.uriInterview;
            selectItogInterview.kodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
            selectItogInterview.kodComplInterv = MapOpisViewModel.modelColectionInterview.kodComplInterv;
            selectItogInterview.detailsInterview = MapOpisViewModel.modelColectionInterview.detailsInterview;
         
            SelectedResultInterview = selectItogInterview;
            MapOpisViewModel.modelColectionInterview.nameDiagnoz = MapOpisViewModel.NameDiagnoz;
            MapOpisViewModel.modelColectionInterview.nameRecomen = MapOpisViewModel.NameRecomendaciya;
            MapOpisViewModel.NameDiagnoz = selectItogInterview.nameDiagnoza;
            MapOpisViewModel.NameRecomendaciya = selectItogInterview.nameRecommendation;
            MapOpisViewModel.OpistInterview = selectItogInterview.opistInterview;
            MapOpisViewModel.UriInterview = selectItogInterview.uriInterview;

            ResultInterviews = new ObservableCollection<ModelResultInterview>();
            ResultInterviews.Add(selectItogInterview);
            MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
            ViewModelCreatInterview.LoadCreatInterview();
            
        }



        // команда закрытия окна
        RelayCommand? closeResult;
        public RelayCommand CloseResult
        {
            get
            {
                return closeResult ??
                  (closeResult = new RelayCommand(obj =>
                  {
                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      MapOpisViewModel.ViewAnalogDiagnoz = false;
                      WindowResult.Close();
                  }));
            }
        }
 

        // команда закрытия окна
        RelayCommand? nextLikarResult;
        public RelayCommand NextLikarResult
        {
            get
            {
                return nextLikarResult ??
                  (nextLikarResult = new RelayCommand(obj =>
                  {
                      
                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      WindowResult.Close();
                  }));
            }
        }

        // команда просмотра содержимого интервью
        private RelayCommand? readIntreviewProtokol;
        public RelayCommand ReadIntreviewProtokol
        {
            get
            {
                return readIntreviewProtokol ??
                  (readIntreviewProtokol = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "";
                      string t = MapOpisViewModel.KodProtokola;
                      MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola ;
                      
                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 150;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();

                  }));
            }
        }

        
        // команда просмотра содержимого интервью
        private RelayCommand? runGoogleUri;
        public RelayCommand RunGoogleUri
        {
            get
            {
                return runGoogleUri ??
                  (runGoogleUri = new RelayCommand(obj =>
                  {
                      string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                      string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                      string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                      Process Rungoogle = new Process();
                      Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                      Rungoogle.StartInfo.Arguments = ViewModelResultInterview.selectItogInterview.uriInterview;
                      Rungoogle.StartInfo.UseShellExecute = false;
                      Rungoogle.EnableRaisingEvents = true;
                      Rungoogle.Start();
                  }));
            }
        }

    }
}
