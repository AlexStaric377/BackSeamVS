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
    public class ViewModelWinVisitingDays : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        private string pathcontrollerVisitingDays = "/api/VisitingDaysController/";
        public static ModelVisitingDays selectVisitingDays;

        public ModelVisitingDays SelectedVisitingDays
        {
            get { return selectVisitingDays; }
            set { selectVisitingDays = value; OnPropertyChanged("SelectedVisitingDays"); }
        }
        public static ObservableCollection<ModelVisitingDays> ViewVisitingDayss { get; set; }



        public ViewModelWinVisitingDays()
        {

            CallServer.PostServer(pathcontrollerVisitingDays, pathcontrollerVisitingDays  + MapOpisViewModel._kodDoctor + "/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableVisitingDays(CmdStroka);
        }

        public static void ObservableVisitingDays(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelVisitingDays>(CmdStroka);
            List<ModelVisitingDays> res = result.ModelVisitingDays.ToList();
            ViewVisitingDayss = new ObservableCollection<ModelVisitingDays>((IEnumerable<ModelVisitingDays>)res);
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectDaysVisiting;
        public RelayCommand SelectDaysVisiting
        {
            get
            {
                return selectDaysVisiting ??
                  (selectDaysVisiting = new RelayCommand(obj =>
                  {
                      if (selectVisitingDays != null)
                      {
                          MapOpisViewModel.selectVisitingDays = new ModelVisitingDays();
                          MapOpisViewModel.selectVisitingDays = selectVisitingDays;

                      }
                      WinVisitingDays WindowMen = MainWindow.LinkMainWindow("WinVisitingDays");
                      WindowMen.Close();
                  }));
                
            }
        }

        // команда закрытия окна
        RelayCommand? closeModelWinVisitingDays;
        public RelayCommand CloseModelWinVisitingDays
        {
            get
            {
                return closeModelWinVisitingDays ??
                  (closeModelWinVisitingDays = new RelayCommand(obj =>
                  {

                      WinVisitingDays WindowMen = MainWindow.LinkMainWindow("WinVisitingDays");
                      WindowMen.Close();
                  }));
            }
        }

    }
}
