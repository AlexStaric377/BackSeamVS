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
    class ViewModelNsiFeature : BaseViewModel
    {
        private WinNsiFeature WindowFeature = MainWindow.LinkMainWindow("WinNsiFeature");
        private MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
        public static string pathFeatureController = "/api/FeatureController/";
        public static string jasonstoka="", Method = "";
        public static ModelFeature selectedFeature;
        public static ObservableCollection<ModelFeature> NsiModelFeatures { get; set; }
        public ModelFeature SelectedModelFeature
        { get { return selectedFeature; } set { selectedFeature = value; OnPropertyChanged("SelectedModelFeature"); } }
        // конструктор класса
        public ViewModelNsiFeature()
        {
            
                CallServer.PostServer(pathFeatureController, jasonstoka, Method);
                string CmdStroka = CallServer.ServerReturn();
                ObservableNsiModelFeatures(CmdStroka);
            
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelFeature>(CmdStroka);
            List<ModelFeature> res = result.ModelFeature.ToList();
            NsiModelFeatures = new ObservableCollection<ModelFeature>((IEnumerable<ModelFeature>)res);
        }

        // команда закрытия окна
        RelayCommand? closeModelFeature;
        public RelayCommand CloseModelFeature
        {
            get
            {
                return closeModelFeature ??
                  (closeModelFeature = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.nameFeature3 = "";
                      WindowMain.Detailingt3.Text = "";
                      WindowMain.Featuret3.Text = "";
                      WindowFeature.Close();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? backComplaint;
        public RelayCommand BackComplaint
        {
            get
            {
                return backComplaint ??
                  (backComplaint = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.nameFeature3 = "";
                      WindowMain.Detailingt3.Text = "";
                      WindowMain.Featuret3.Text = "";
                      WindowFeature.Close();

                      MapOpisViewModel.ActCreatInterview = "SelectInterview";
                      NsiComplaint NewOrder = new NsiComplaint();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 50;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.IndikatorSelected = "NsiComplaint";
                      MapOpisViewModel.Selectedswitch();
                  }));
            }
        }
        

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelFeature;
        public RelayCommand SelectModelFeature
        {
            get
            {
                return selectModelFeature ??
                  (selectModelFeature = new RelayCommand(obj =>
                  {
                      if (selectedFeature != null)
                      {
                          MapOpisViewModel.selectFeature = selectedFeature.name.ToString();
                          MapOpisViewModel.nameFeature3 = selectedFeature.keyFeature.ToString() + ":    " + selectedFeature.name.ToString();
                          WindowMain.Detailingt3.Text = selectedFeature.keyFeature+": "+ selectedFeature.name.ToString();
                          WindowMain.Featuret3.Text = selectedFeature.keyFeature.ToString() + ":    " + selectedFeature.name.ToString();
                          switch (MapOpisViewModel.ActCompletedInterview)
                          {
                              case "Feature":
                                  break;
                              case null:
                                  if (ViewModelCreatInterview.ContentIntervs != null) ViewModelCreatInterview.SelectContentCompl();
                                  break;
                              default:
                                  MapOpisViewModel.SelectContentCompleted();
                                  break;
                          }
                          if(MapOpisViewModel.CallViewDetailing == "ModelDetailing") WindowFeature.Close();
                      }
                  }));
            }
        }


        RelayCommand? viewDetaling;
        public RelayCommand ViewDetaling
        {
            get
            {
                return viewDetaling ??
                  (viewDetaling = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.ActCompletedInterview == "Complaint" || MapOpisViewModel.ActCompletedInterview == "" || MapOpisViewModel.ActCompletedInterview == "Feature") // (MapOpisViewModel.ActCompletedInterview != "ModelDetailing" && MapOpisViewModel.ActCreatInterview != "CreatInterview") || 
                      {
                         if (WindowFeature.TablFeature.SelectedIndex != -1)
                          {
                              selectedFeature = NsiModelFeatures[WindowFeature.TablFeature.SelectedIndex];
                              if (selectedFeature != null)
                              {

                                  if (MapOpisViewModel.ActCompletedInterview == "Feature")
                                  {
                                      MapOpisViewModel.selectFeature = selectedFeature.name.ToString();
                                      MapOpisViewModel.nameFeature3 = selectedFeature.keyFeature.ToString() + ":    " + selectedFeature.name.ToString();
                                      WindowFeature.Close();
                                  }
                                  else
                                  { 
                                      string pathcontroller = "/api/DetailingController/";
                                      string jason = pathcontroller + "0/" + selectedFeature.keyFeature + "/0";
                                      CallServer.PostServer(pathcontroller, jason, "GETID");
                                      string CmdStroka = CallServer.ServerReturn();
                                      if (CmdStroka.Contains("[]") == false)
                                      {
                                          ViewModelNsiDetailing.NsiModelDetailings = null;
                                          MapOpisViewModel.ActCompletedInterview = "Detailing";
                                          NsiDetailing NewNsi = new NsiDetailing();
                                          NewNsi.Left = (MainWindow.ScreenWidth / 2) - 90;
                                          NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                                          NewNsi.ShowDialog();
                                          MapOpisViewModel.ActCompletedInterview = "";

                                      }                                  
                                  }
                                  
                              }
                          }                     
                      }
                      
  

                  },
                 (obj) => NsiModelFeatures != null));
            }
        }

        
        // команда поиска наименования характера проявления болей
        RelayCommand? searchNameFeature;
        public RelayCommand SearchNameFeature
        {
            get
            {
                return searchNameFeature ??
                  (searchNameFeature = new RelayCommand(obj =>
                  {
                      MetodKeyEnterFeature();
                  }));
            }
        }

        
        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyEnterFeature;
        public RelayCommand CheckKeyEnterFeature
        {
            get
            {
                return checkKeyEnterFeature ??
                  (checkKeyEnterFeature = new RelayCommand(obj =>
                  {
                      MetodKeyEnterFeature();
                  }));
            }
        }

        private void MetodKeyEnterFeature()
        {

            if (WindowFeature.PoiskFeature.Text.Trim() != "")
            {
                string jason = pathFeatureController + "0/0/" + WindowFeature.PoiskFeature.Text;
                CallServer.PostServer(pathFeatureController, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelFeatures(CmdStroka);
                WindowFeature.TablFeature.ItemsSource = NsiModelFeatures;
            }
        }

       

        // команда выбора строки харакутера жалобы
        RelayCommand? addAllModelFeature;
        public RelayCommand AddAllModelFeature
        {
            get
            {
                return addAllModelFeature ??
                  (addAllModelFeature = new RelayCommand(obj =>
                  {
                      foreach (ModelFeature modelFeature in NsiModelFeatures)
                      {
                          WindowMain.Featuret3.Text = MapOpisViewModel.nameFeature3 = modelFeature.keyFeature + ":        " + modelFeature.name;
                          ViewModelCreatInterview.SelectContentCompl();
                      }
                      WindowFeature.Close();
                     
                  }));
            }
        }
    }
}
