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
    public class ViewModelNsiDetailing : BaseViewModel
    {
        private MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
        private NsiDetailing WindowMen = MainWindow.LinkMainWindow("NsiDetailing");
        private static string pathcontroller =  "/api/DetailingController/";
        public static ModelDetailing selectedDetailing;
        public static ObservableCollection<ModelDetailing> NsiModelDetailings { get; set; }
        
        public ModelDetailing SelectedModelDetailing
        { get { return selectedDetailing; } set { selectedDetailing = value; OnPropertyChanged("SelectedModelDetailing"); } }
        // конструктор класса
        public ViewModelNsiDetailing()
        {
            string jason = "";
            if (ViewModelNsiDetailing.NsiModelDetailings == null)
            {
                switch (MapOpisViewModel.ActCompletedInterview)
                {
                    case "FeatureGET":
                        jason = pathcontroller + "0/0/0";
                        break;
                    case "Feature":
                        jason = pathcontroller + "0/" + MapOpisViewModel.selectedViewFeature.keyFeature + "/0";
                        break;
                    case "Detailing":
                        jason = pathcontroller + "0/" + ViewModelNsiFeature.selectedFeature.keyFeature + "/0";
                        break;
                    case "ViewDetailing":
                        jason = pathcontroller + "0/" + MapOpisViewModel.selectedViewFeature.keyFeature + "/0";
                        break;
                    case null:
                        jason = pathcontroller + "0/" + ViewModelCreatInterview.selectedContentInterv.kodDetailing + "/0";
                        break;
                    default:
                        jason = pathcontroller + "0/" + MapOpisViewModel.selectedGuestInterv.kodDetailing + "/0";
                        break;
                }
                CallServer.PostServer(pathcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                ObservableNsiModelFeatures(CmdStroka);
            }
 
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDetailing>(CmdStroka); 
            List<ModelDetailing> res = result.ViewDetailing.ToList();
            NsiModelDetailings = new ObservableCollection<ModelDetailing>((IEnumerable<ModelDetailing>)res);
        }


        // команда закрытия окна
        RelayCommand? closeModelDetailing;
        public RelayCommand CloseModelDetailing
        {
            get
            {
                return closeModelDetailing ??
                  (closeModelDetailing = new RelayCommand(obj =>
                  {
                      WindowMen.Close();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? backfeature;
        public RelayCommand Backfeature
        {
            get
            {
                return backfeature ??
                  (backfeature = new RelayCommand(obj =>
                  {
                      
                      WindowMen.Close();
                      MapOpisViewModel.BackComplaint();

                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelDetailing;
        public RelayCommand SelectModelDetailing
        {
            get
            {
                return selectModelDetailing ??
                  (selectModelDetailing = new RelayCommand(obj =>
                  {
                      
                      if (selectedDetailing != null)
                      {
                          bool keyGr = false;

                          if (selectedDetailing.keyGrDetailing != null)
                          {
                              if (selectedDetailing.keyGrDetailing.Length != 0)keyGr = true;
                          }
                          if(keyGr == true && MapOpisViewModel.ActCompletedInterview != "FeatureGET"
                          && MapOpisViewModel.ActCompletedInterview != "Feature" )  //&& MapOpisViewModel.ActCompletedInterview != null&& MapOpisViewModel.ActCompletedInterview != "ActCreatInterview"
                          {
                              MapOpisViewModel.selectGrDetailing = selectedDetailing.nameDetailing.ToString().ToUpper();
                              WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                              NewOrder.Left = (MainWindow.ScreenWidth / 2) -50;
                              NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
                              NewOrder.ShowDialog();
                          }
                          else
                          {
                              if (MapOpisViewModel.ActCompletedInterview != "Detailing")
                              {
                                  if (selectedDetailing.keyGrDetailing != null && selectedDetailing.keyGrDetailing != "")
                                  {
                                      MapOpisViewModel.nameFeature3 = selectedDetailing.keyGrDetailing.ToString() + ":        " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Detailingt3.Text = selectedDetailing.keyGrDetailing + ": " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Featuret2.Text = selectedDetailing.keyGrDetailing + ": " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Featuret3.Text = selectedDetailing.keyGrDetailing.ToString() + ":        " + selectedDetailing.nameDetailing.ToString();
                                  }
                                  else
                                  { 
                                       MapOpisViewModel.nameFeature3 = selectedDetailing.kodDetailing.ToString() + ":        " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Detailingt3.Text = selectedDetailing.kodDetailing + ": " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Featuret2.Text = selectedDetailing.kodDetailing + ": " + selectedDetailing.nameDetailing.ToString();
                                      WindowMain.Featuret3.Text = selectedDetailing.kodDetailing.ToString() + ":        " + selectedDetailing.nameDetailing.ToString();                                 
                                  }


                                  switch (MapOpisViewModel.ActCompletedInterview)
                                  {
                                      
                                      case null:
                                          if (ViewModelCreatInterview.ContentIntervs != null) ViewModelCreatInterview.SelectContentCompl();
                                          break;
                                      
                                      default:
                                      if (keyGr == false) MapOpisViewModel.SelectContentCompleted();
                                      break;
                                  }
                              }

                          }
                          }
                      WindowMen.TablDeliting.SelectedItem = null;
                  }));
            }
        }

        
        RelayCommand? viewGrDetaling;
        public RelayCommand ViewGrDetaling
        {
            get
            {
                return viewGrDetaling ??
                  (viewGrDetaling = new RelayCommand(obj =>
                  {
                      if (WindowMen.TablDeliting.SelectedIndex != -1 && MapOpisViewModel.ActCreatInterview != "ActCreatInterview")
                      { 
                          selectedDetailing = NsiModelDetailings[WindowMen.TablDeliting.SelectedIndex];
                          string tmpActCompletedInterview = MapOpisViewModel.ActCreatInterview;
                          
                          if (selectedDetailing.keyGrDetailing != null && selectedDetailing.keyGrDetailing !="" )
                          {
                              string pathcontroller = "/api/GrDetalingController/";
                              string jason = pathcontroller + "0/" + selectedDetailing.keyGrDetailing + "/0";
                              CallServer.PostServer(pathcontroller, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  MapOpisViewModel.selectedComplaintname = selectedDetailing.nameDetailing;
                                  WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                                  NewOrder.Left = (MainWindow.ScreenWidth / 2)-50;
                                  NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                                  NewOrder.ShowDialog();
                              }
                              MapOpisViewModel.ActCreatInterview = tmpActCompletedInterview;
                          }                     
                      }
                      
                    

                  },
                 (obj) => NsiModelDetailings != null));
            }
        }

       
        // команда поиска наименования характера проявления болей
        RelayCommand?  searchNameDeliting;
        public RelayCommand SearchNameDeliting
        {
            get
            {
                return searchNameDeliting ??
                  (searchNameDeliting = new RelayCommand(obj =>
                  {
                      MetodDetailingEnter();
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
                      MetodDetailingEnter();
                  }));
            }
        }
        public void MetodDetailingEnter()
        {

            //NsiDetailing WindowWinNsiDetailing = MainWindow.LinkMainWindow("NsiDetailing");
            if (WindowMen.PoiskDeliting.Text.Trim() != "")
            {
                string jason = pathcontroller + "0/0/" + WindowMen.PoiskDeliting.Text;
                CallServer.PostServer(pathcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelFeatures(CmdStroka);
                WindowMen.TablDeliting.ItemsSource = NsiModelDetailings;
            }
        }

       
        RelayCommand? addAllModelDetailing;
        public RelayCommand AddAllModelDetailing
        {
            get
            {
                return addAllModelDetailing ??
                  (addAllModelDetailing = new RelayCommand(obj =>
                  {
                      foreach (ModelDetailing modelDetailing in NsiModelDetailings)
                      {
                          WindowMain.Featuret3.Text = MapOpisViewModel.nameFeature3 = modelDetailing.kodDetailing.ToString() + ":        " + modelDetailing.nameDetailing.ToString();
                          ViewModelCreatInterview.SelectContentCompl();
                      }
                      WindowMen.Close();
                  }));
            }
        }
    }
}
