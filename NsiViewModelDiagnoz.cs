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
    public class NsiViewModelDiagnoz : BaseViewModel
    {

        private WinNsiListDiagnoz WindowNsiListUri = MainWindow.LinkMainWindow("WinNsiListDiagnoz");
        public static string controlerNsiDiagnoz =  "/api/DiagnozController/";
        private ModelDiagnoz selectedVeiwDiagnoz;
        public static ObservableCollection<ModelDiagnoz> VeiwDiagnozs { get; set; }

        public ModelDiagnoz SelectedVeiwDiagnoz
        { get { return selectedVeiwDiagnoz; } set { selectedVeiwDiagnoz = value; OnPropertyChanged("SelectedVeiwDiagnoz"); } }
        // конструктор класса
        public NsiViewModelDiagnoz()
        {
 
            if (MapOpisViewModel.SelectActivGrupDiagnoz == "")
            {
                CallServer.PostServer(controlerNsiDiagnoz, controlerNsiDiagnoz, "GET");
                string CmdStroka = CallServer.ServerReturn();
                ObservableViewNsiDiagnoz(CmdStroka);
            }
            else
            {
                if (MapOpisViewModel.AllWorkDiagnozs.Count > 0)
                {
                    VeiwDiagnozs = new ObservableCollection<ModelDiagnoz>();
                    foreach (ModelDiagnoz modelDiagnoz in MapOpisViewModel.AllWorkDiagnozs)
                    {
                        if (modelDiagnoz.icdGrDiagnoz == MapOpisViewModel.SelectActivGrupDiagnoz) VeiwDiagnozs.Add(modelDiagnoz);
 ;                  }

                }
                else
                { 
                    string json = controlerNsiDiagnoz +"0/"+ MapOpisViewModel.SelectActivGrupDiagnoz + "/0";
                    CallServer.PostServer(controlerNsiDiagnoz, json, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    ObservableViewNsiDiagnoz(CmdStroka);
                }
            }
                  
 

        }

        public static void ObservableViewNsiDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            VeiwDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);

        }

        // команда закрытия окна
        RelayCommand? closeVeiwDiagnoz;
        public RelayCommand CloseVeiwDiagnoz
        {
            get
            {
                return closeVeiwDiagnoz ??
                  (closeVeiwDiagnoz = new RelayCommand(obj =>
                  {
                      WindowNsiListUri.Close();
                  }));
            }
        }

        RelayCommand? selectVeiwDiagnoz;
        public RelayCommand SelectVeiwDiagnoz
        {
            get
            {
                return selectVeiwDiagnoz ??
                  (selectVeiwDiagnoz = new RelayCommand(obj =>
                  {
                      
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (SelectedVeiwDiagnoz != null)
                      {
                          Windowmain.Dependencyt2.Text =  selectedVeiwDiagnoz.nameDiagnoza;
                          Windowmain.InterviewOpis.Text = selectedVeiwDiagnoz.opisDiagnoza;
                          Windowmain.InterviewTextUri.Text = selectedVeiwDiagnoz.uriDiagnoza;
                          if (MapOpisViewModel.selectedDependency == null) MapOpisViewModel.selectedDependency = new ModelDependencyDiagnoz();
                          MapOpisViewModel.selectedDependency.kodDiagnoz = selectedVeiwDiagnoz.kodDiagnoza;
                          Windowmain.Interviewt6.Text = selectedVeiwDiagnoz.kodDiagnoza.ToString() + ": " + SelectedVeiwDiagnoz.nameDiagnoza;
                          Windowmain.LikarInterviewt6.Text = selectedVeiwDiagnoz.kodDiagnoza.ToString() + ": " + SelectedVeiwDiagnoz.nameDiagnoza;
                          Windowmain.InterviewDependencyt2.Text = selectedVeiwDiagnoz.kodDiagnoza.ToString() + ": " + SelectedVeiwDiagnoz.nameDiagnoza;
                          WindowNsiListUri.Close();
                      }
                      
                  }));
            }
        }

        

        // Выбор названия опитування
        private RelayCommand? searchDiagnoz;
        public RelayCommand SearchDiagnoz
        {
            get
            {
                return searchDiagnoz ??
                  (searchDiagnoz = new RelayCommand(obj =>
                  {
                      MetodEnter();
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
                        MetodEnter();
                  }));
            }
        }

        public void MetodEnter()
        {

            if (WindowNsiListUri.PoiskDiagnoz.Text.Trim() != "")
            {
                string jason = controlerNsiDiagnoz + "0/0/" + WindowNsiListUri.PoiskDiagnoz.Text;
                CallServer.PostServer(controlerNsiDiagnoz, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewNsiDiagnoz(CmdStroka);
                WindowNsiListUri.TablFeature.ItemsSource = VeiwDiagnozs;
            }
        }

    }
}
