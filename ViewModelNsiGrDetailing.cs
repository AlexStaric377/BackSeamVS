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
    public partial class ViewModelNsiGrDetailing : BaseViewModel
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //}

        private static string pathcontroller =  "/api/GrDetalingController/";
        public static ModelGrDetailing selectedGrDetailing;
        public static ObservableCollection<ModelGrDetailing> NsiModelGrDetailings { get; set; }
        public ModelGrDetailing SelectedModelGrDetailing
        { get { return selectedGrDetailing; } set { selectedGrDetailing = value; OnPropertyChanged("SelectedModelGrDetailing"); } }
        // конструктор класса
        public ViewModelNsiGrDetailing()
        {
            string jason = "";
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "SelectedGrDetailing":
                    jason = pathcontroller + "0/" + MapOpisViewModel.selectedGroupDetailing.keyGrDetailing + "/0";
                    break;
                    
                case "ViewGrDetailing":
                    jason = pathcontroller + "0/" + MapOpisViewModel.selectedViewDetailingFeature.keyGrDetailing + "/0";
                    break;

                case "GrDetailing":
                    jason = pathcontroller + "0/" + MapOpisViewModel.selectedListGroupDeliting.keyGrDetailing + "/0";
                    break;
                default:
                    jason = pathcontroller + "0/" + ViewModelNsiDetailing.selectedDetailing.keyGrDetailing + "/0";
                    break;
            }
            CallServer.PostServer(pathcontroller, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelGrDetailings(CmdStroka);
        }
        public static void ObservableNsiModelGrDetailings(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGrDetailing>(CmdStroka);
            List<ModelGrDetailing> res = result.ViewGrDetailing.ToList();
            NsiModelGrDetailings = new ObservableCollection<ModelGrDetailing>((IEnumerable<ModelGrDetailing>)res);
        }


  
        public void CloseWin()
        {
            WinNsiGrDetailing WindowMen = MainWindow.LinkMainWindow("WinNsiGrDetailing");
            WindowMen.Close();
        }

        // команда закрытия окна
        RelayCommand? closeModelGrDetailing;
        public RelayCommand CloseModelGrDetailing
        {
            get
            {
                return closeModelGrDetailing ??
                  (closeModelGrDetailing = new RelayCommand(obj =>
                  {
                      CloseWin();
                      
                  }));
            }
        }


        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelGrDetailing;
        public RelayCommand SelectModelGrDetailing
        {
            get
            {
                return selectModelGrDetailing ??
                  (selectModelGrDetailing = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      if (selectedGrDetailing != null)
                      {
                          if (MapOpisViewModel.ActCreatInterview != "CreatInterview") return;
                          { 
 
                                  MapOpisViewModel.selectQualification = selectedGrDetailing.nameGrDetailing;
                                  MapOpisViewModel.nameFeature3 = selectedGrDetailing.kodDetailing.ToString() + ":        " + selectedGrDetailing.nameGrDetailing.ToString();
                                  WindowMain.Detailingt3.Text = selectedGrDetailing.kodDetailing + ": " + selectedGrDetailing.nameGrDetailing.ToString();
                                  WindowMain.Featuret3.Text = selectedGrDetailing.kodDetailing.ToString() + ":        " + selectedGrDetailing.nameGrDetailing.ToString();                          


                              switch (MapOpisViewModel.ActCompletedInterview)
                              {
                              
                                  case null:
                                    ViewModelCreatInterview.SelectContentCompl();
                                    break;
                                 default:
                                    MapOpisViewModel.SelectContentCompleted();
                                    break;
                                 
                              }
                          }
                          

                          if (selectedGrDetailing.kodGroupQualification == null) return;
                          if(selectedGrDetailing.kodGroupQualification.Length>0) OpenQualification(); 

                      }
                  }));
            }
        }
        private void OpenQualification()
        {
            WinNsiQualification NewOrder = new WinNsiQualification();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) + 80;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 400; //350;
            NewOrder.ShowDialog();
        }

        RelayCommand? viewkodGroupQualification;
        public RelayCommand ViewkodGroupQualification
        {
            get
            {
                return viewkodGroupQualification ??
                  (viewkodGroupQualification = new RelayCommand(obj =>
                  {

                      if (selectedGrDetailing != null && MapOpisViewModel.ActCreatInterview != "CreatInterview" && MapOpisViewModel.ActCompletedInterview != "ViewGrDetailing")
                      {
                          if (selectedGrDetailing.kodGroupQualification != null && selectedGrDetailing.kodGroupQualification !="" )
                          { 
                              string pathcontroller = "/api/GrDetalingController/";
                              string jason = pathcontroller + "0/" + selectedGrDetailing.keyGrDetailing + "/0";
                              CallServer.PostServer(pathcontroller, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  
                                  MapOpisViewModel.selectedComplaintname = selectedGrDetailing.nameGrDetailing;
                                  WinNsiQualification NewOrder = new WinNsiQualification();
                                  NewOrder.Left = (MainWindow.ScreenWidth / 2) + 80;
                                  NewOrder.Top = (MainWindow.ScreenHeight / 2) - 400; //350;
                                  NewOrder.ShowDialog();
                                  

                              }                         
                          }

                      }

                  },
                 (obj) => NsiModelGrDetailings != null));
            }
        }

        // команда поиска наименования характера проявления болей
        RelayCommand? searchNameGrDeliting;
        public RelayCommand SearchNameGrDeliting
        {
            get
            {
                return searchNameGrDeliting ??
                  (searchNameGrDeliting = new RelayCommand(obj =>
                  {
                      WinNsiGrDetailing WindowWinNsiGrDetailing = MainWindow.LinkMainWindow("WinNsiGrDetailing");
                      if (WindowWinNsiGrDetailing.PoiskGrDeliting.Text.Trim() != "")
                      {
                          string jason = pathcontroller + "0/0/" + WindowWinNsiGrDetailing.PoiskGrDeliting.Text;
                          CallServer.PostServer(pathcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableNsiModelGrDetailings(CmdStroka);
                          WindowWinNsiGrDetailing.TablDeliting.ItemsSource = NsiModelGrDetailings;
                      }
                  }));
            }
        }

    }
}
