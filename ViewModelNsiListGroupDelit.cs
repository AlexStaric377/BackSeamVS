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
    public partial class ViewModelNsiListGroupDelit : BaseViewModel
    {
        private WinNsiListGroupDelit WindowMen = MainWindow.LinkMainWindow("WinNsiListGroupDelit");
        string controlerListGrDetailing =  "/api/ControllerListGroupDetail/";
        private ModelListGrDetailing selectedListGrDetailing;
        public  static ObservableCollection<ModelListGrDetailing> ViewListGrDetailings { get; set; }

        public ModelListGrDetailing SelectedListGrDetailing
        { get { return selectedListGrDetailing; } set { selectedListGrDetailing = value; OnPropertyChanged("SelectedListGrDetailing"); } }
        // конструктор класса
        public ViewModelNsiListGroupDelit()
        {
            MainWindow.UrlServer = controlerListGrDetailing;
            CallServer.PostServer(MainWindow.UrlServer, controlerListGrDetailing, "GET");
            string CmdStroka = CallServer.ServerReturn();
            ObservableListGrDetailing(CmdStroka);
        }

        public static void ObservableListGrDetailing(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelListGrDetailing>(CmdStroka);
            List<ModelListGrDetailing> res = result.ViewListGrDetailing.ToList();
            ViewListGrDetailings = new ObservableCollection<ModelListGrDetailing>((IEnumerable<ModelListGrDetailing>)res);
        }

        // команда закрытия окна
        RelayCommand? closeListGrDetailing;
        public RelayCommand CloseListGrDetailing
        {
            get
            {
                return closeListGrDetailing ??
                  (closeListGrDetailing = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WinNsiListGroupDelit WindowMen = MainWindow.LinkMainWindow("WinNsiListGroupDelit");
                      if (SelectedListGrDetailing != null)
                      {
                          WindowMain.GrDetailingst2.Text = SelectedListGrDetailing.keyGrDetailing.ToString()+": "+ SelectedListGrDetailing.nameGrup.ToString();
                          WindowMain.Detailingt4.Text = SelectedListGrDetailing.keyGrDetailing.ToString();
                          WindowMain.Detailingt2.Text = SelectedListGrDetailing.nameGrup.ToString();
                          WindowMain.Featuret2.Text = SelectedListGrDetailing.keyGrDetailing.ToString() + ": " + SelectedListGrDetailing.nameGrup.ToString();

                      }
                      WindowMen.Close();
                  }));
            }
        }

        
        // команда закрытия окна
        RelayCommand? selectedNameGrDetaling;
        public RelayCommand SelectedNameGrDetaling
        {
            get
            {
                return selectedNameGrDetaling ??
                  (selectedNameGrDetaling = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.ActCompletedInterview == "NameDeteling")
                      { 
                            if (SelectedListGrDetailing != null)
                            {
                           
                                MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                                
                                WindowMain.GrDetailingst2.Text = SelectedListGrDetailing.keyGrDetailing.ToString() + ": " + SelectedListGrDetailing.nameGrup.ToString();
                                WindowMain.Detailingt4.Text = SelectedListGrDetailing.keyGrDetailing.ToString();
                                WindowMain.Detailingt2.Text = SelectedListGrDetailing.nameGrup.ToString();
                                WindowMain.Featuret2.Text = SelectedListGrDetailing.keyGrDetailing.ToString() + ": " + SelectedListGrDetailing.nameGrup.ToString();
                                WindowMen.Close();   
                            }
                                            
                      }

                  }));
            }
        }

        

        // команда поиска наименования характера проявления болей
        RelayCommand? searchGroupDelit;
        public RelayCommand SearchGroupDelit
        {
            get
            {
                return searchGroupDelit ??
                  (searchGroupDelit = new RelayCommand(obj =>
                  {
                      MetodKeyEnterGroupDelit();
                  }));
            }
        }

        

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyEnterGroupDelit;
        public RelayCommand CheckKeyEnterGroupDelit
        {
            get
            {
                return checkKeyEnterGroupDelit ??
                  (checkKeyEnterGroupDelit = new RelayCommand(obj =>
                  {
                      MetodKeyEnterGroupDelit();
                  }));
            }
        }

        private void MetodKeyEnterGroupDelit()
        {
            if (WindowMen.PoiskGroupDelit.Text.Trim() != "")
            {
                string jason = controlerListGrDetailing + "0/" + WindowMen.PoiskGroupDelit.Text;
                CallServer.PostServer(controlerListGrDetailing, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableListGrDetailing(CmdStroka);
                WindowMen.TablGrQualifications.ItemsSource = ViewListGrDetailings;
            }
        }

    }
}
