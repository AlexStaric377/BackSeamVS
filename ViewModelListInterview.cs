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
    public partial class ViewModelListInterview : BaseViewModel
    {

  
        private WinListInteviewPacient WindowInteview = MainWindow.LinkMainWindow("WinListInteviewPacient");
        public static string controllerColectionInterview = "/api/ColectionInterviewController/";
        public static ColectionInterview selectedColectionInterview;
        public static ObservableCollection<ColectionInterview> ViewColectionInterviews { get; set; }

        public ColectionInterview SelectedColectionInterview
        { get { return selectedColectionInterview; } set { selectedColectionInterview = value; OnPropertyChanged("SelectedColectionInterview"); } }


        public ViewModelListInterview()
        {
            CallServer.PostServer(controllerColectionInterview, controllerColectionInterview + "0/0/" + MapOpisViewModel._pacientProfil, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionIntev(CmdStroka);

        }

        public static void ObservablelColectionIntev(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ViewColectionInterviews = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
        }


        // команда закрытия окна
        RelayCommand? closeColectionInterview;
        public RelayCommand CloseColectionInterview
        {
            get
            {
                return closeColectionInterview ??
                  (closeColectionInterview = new RelayCommand(obj =>
                  {
                      WindowInteview.Close();
                  }));
            }
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectColectionInterview;
        public RelayCommand SelectColectionCompInterview
        {
            get
            {
                return selectColectionInterview ??
                  (selectColectionInterview = new RelayCommand(obj =>
                  {

                      WinListInteviewPacient WindowInteview = MainWindow.LinkMainWindow("WinListInteviewPacient");
                      MapOpisViewModel.KodInterviewPacient = ViewColectionInterviews[WindowInteview.TablInterviews.SelectedIndex].kodComplInterv.ToString();
                      MapOpisViewModel.NameInterviewPacient = ViewColectionInterviews[WindowInteview.TablInterviews.SelectedIndex].nameInterview.ToString();
                      MapOpisViewModel.KodProtokola = ViewColectionInterviews[WindowInteview.TablInterviews.SelectedIndex].kodProtokola.ToString();
                      MapOpisViewModel.DateInterview = ViewColectionInterviews[WindowInteview.TablInterviews.SelectedIndex].dateInterview.ToString();
                      MapOpisViewModel.DetalishIterv = ViewColectionInterviews[WindowInteview.TablInterviews.SelectedIndex].detailsInterview.ToString();
                      WindowInteview.Close();
                  }));
            }
        }
    }
}
