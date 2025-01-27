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
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        // ViewDetailing  Совмещение двух справочников Feature и Complaint 
        // клавиша в окне:  Детализация характеристики
        //  Детализация характеристики жалобы(локальная или груповая)
        // Содержит конкретную детализацию или групповую , которая содержит в себе перечень однотипных детелизаций)
        #region Обработка событий и команд вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowDetailing = MainWindow.LinkNameWindow("BackMain");
        public static bool editboolDetailing = false, addboolDetailing = false, loadboolDetailing = false;
        private static string GrFeatureDetailing = "", NameComplate ="";
        public static string pathcontrolerDetailing =  "/api/DetailingController/";
        public ModelDetailing selectedDetailing;
        public static ModelDetailing selectedGroupDetailing;
        public static ViewDetailingFeature selectedViewDetailingFeature;
        public static string CallViewDetailing = "";
        public static ObservableCollection<ModelDetailing> ViewDetailings { get; set; }
        public static ObservableCollection<ViewDetailingFeature> ViewDetailingFeatures { get; set; }
        public ModelDetailing SelectedDetailing
        {
            get { return selectedDetailing; }
            set { selectedDetailing = value; OnPropertyChanged("SelectedDetailing"); }
        }
        public ViewDetailingFeature SelectedViewDetailingFeature
        {
            get { return selectedViewDetailingFeature; }
            set { selectedViewDetailingFeature = value; OnPropertyChanged("SelectedViewDetailingFeature"); }
        }
 
        public static void ObservableViewDetailings(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDetailing>(CmdStroka);
            List<ModelDetailing> res = result.ViewDetailing.ToList();
            ViewDetailings = new ObservableCollection<ModelDetailing>((IEnumerable<ModelDetailing>)res);
            ObservableCollectionViewDetailingFeature();
            loadboolDetailing = true;
        }

        public static void ObservableCollectionViewDetailingFeature()
        {
            string SelectednameFeature = "", keyFeature="";
            ViewDetailingFeatures = new ObservableCollection<ViewDetailingFeature>();
            foreach (ModelDetailing  modelDetailing in ViewDetailings)
            {
                ViewDetailingFeature selectedDetailingFeature = new ViewDetailingFeature();
                selectedDetailingFeature.id = modelDetailing.id;
                selectedDetailingFeature.kodDetailing = modelDetailing.kodDetailing;
                selectedDetailingFeature.keyFeature = modelDetailing.keyFeature;
                selectedDetailingFeature.keyGrDetailing = modelDetailing.keyGrDetailing;
                if (modelDetailing.keyGrDetailing == "" || modelDetailing.keyGrDetailing == null)
                { selectedDetailingFeature.nameDetailing = modelDetailing.nameDetailing; }
                else
                { selectedDetailingFeature.nameGrDetailing = modelDetailing.nameDetailing; }
                if (ViewDetailingFeatures.Count == 0 || keyFeature != modelDetailing.keyFeature)
                {
                    keyFeature = modelDetailing.keyFeature;
                    string json = featurecontroller + modelDetailing.keyFeature+"/0/0";  
                    CallServer.PostServer(featurecontroller, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelFeature Idinsert = JsonConvert.DeserializeObject<ModelFeature>(CallServer.ResponseFromServer);
                    if (Idinsert != null) SelectednameFeature = Idinsert.name;
                }

                selectedDetailingFeature.nameFeature = SelectednameFeature;
                ViewDetailingFeatures.Add(selectedDetailingFeature);
            }
            WindowDetailing.DetailingTablGrid.ItemsSource = ViewDetailingFeatures;
            selectedViewDetailingFeature = new ViewDetailingFeature();
        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadDetailing;
        public RelayCommand LoadDetailing
        {
            get
            {
                return loadDetailing ??
                  (loadDetailing = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadDetailing();
                  }));
            }
        }
       
        // команда добавления нового объекта

        private RelayCommand? addDetailing;
        public RelayCommand AddDetailing
        {
            get
            {
                return addDetailing ??
                (addDetailing = new RelayCommand(obj =>
                { if (CheckStatusUser() == false) return;  AddComandDetailing(); }));
            }
        }

        private void AddComandDetailing()
        {
            if (loadboolDetailing == false) { MethodLoadDetailing(); }
            MethodaddcomDetailing();
        }

        private void MethodaddcomDetailing()
        {
            IndexAddEdit = "addCommand" ;
            if (addboolDetailing == false)
            {
                BoolTrueDetailing();
                if(selectedViewDetailingFeature !=null) TrueNameComplaint();
                if (MapOpisViewModel.nameFeature3 != "") SelectNewDetailing();            
            } 
            else BoolFalseDetailing();
        }

        private void TrueNameComplaint()
        {
            if (MapOpisViewModel.nameFeature3 != "")
            {
                selectedViewDetailingFeature.nameFeature = MapOpisViewModel.nameFeature3;
                selectedViewDetailingFeature.keyFeature = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                WindowDetailing.DetailingCopl.Text = MapOpisViewModel.selectedComplaintname;
                WindowDetailing.Detailingt3.Text = MapOpisViewModel.nameFeature3;
                WindowDetailing.FolderComplaint.IsEnabled= false;
                WindowDetailing.FolderFut.IsEnabled = false;
                string tmp = selectedViewDetailingFeature.nameGrDetailing;
                WindowDetailing.Detailingt4.Text = "";
                selectedViewDetailingFeature.nameGrDetailing = tmp;
            }
        }

        private void MethodLoadDetailing()
        {
            NewEkzemplyarDetailing();
            WindowDetailing.Loaddel.Visibility = Visibility.Hidden;
            GrFeatureDetailing = "";
            MapOpisViewModel.nameFeature3 = "";
            CallServer.PostServer(pathcontrolerDetailing, pathcontrolerDetailing, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewDetailings(CmdStroka);
        }

        private void NewEkzemplyarDetailing()
        {
            selectedViewDetailingFeature = new ViewDetailingFeature();
            SelectedViewDetailingFeature = new ViewDetailingFeature();
        }
        private void BoolTrueDetailing()
        {
            addboolDetailing = true;
            editboolDetailing = true;
            WindowDetailing .Detailingt2.IsEnabled = true;
            WindowDetailing .Detailingt2.Background = Brushes.AntiqueWhite;
            if (MapOpisViewModel.nameFeature3 == "" && IndexAddEdit == "addCommand") WindowDetailing.FolderFut.Visibility = Visibility.Visible;
            WindowDetailing.FolderDet.Visibility = Visibility.Visible;
            if (MapOpisViewModel.nameFeature3 == "" && IndexAddEdit == "addCommand") WindowDetailing.FolderComplaint.Visibility = Visibility.Visible;
            WindowDetailing.DetailingTablGrid.IsEnabled = false;
            if (IndexAddEdit == "addCommand")
            {
                WindowMen.BorderLoadDetailing.IsEnabled = false;
                WindowMen.BorderGhangeDetailing.IsEnabled = false;
                WindowMen.BorderDeleteDetailing.IsEnabled = false;
                WindowMen.BorderPrintDetailing.IsEnabled = false;
            }
            if (IndexAddEdit == "editCommand")
            {
                WindowMen.BorderLoadDetailing.IsEnabled = false;
                WindowMen.BorderAddDetailing.IsEnabled = false;
                WindowMen.BorderDeleteDetailing.IsEnabled = false;
                WindowMen.BorderPrintDetailing.IsEnabled = false;
            }
        }

        private void BoolFalseDetailing()
        {
            addboolDetailing = false;
            editboolDetailing = false;
            IndexAddEdit = "";
            WindowDetailing .Detailingt2.IsEnabled = false;
            WindowDetailing .Detailingt2.Background =  Brushes.White ;
            WindowDetailing .FolderFut.Visibility = Visibility.Hidden;
            WindowDetailing .FolderDet.Visibility = Visibility.Hidden;
            WindowDetailing.FolderDetailing.Visibility = Visibility.Hidden;
            WindowDetailing.FolderComplaint.Visibility = Visibility.Hidden;
            WindowDetailing.FolderComplaint.IsEnabled = true;
            WindowDetailing.FolderFut.IsEnabled = true;
            WindowDetailing.DetailingTablGrid.IsEnabled = true;
            WindowMen.FeatureTablGrid.IsEnabled = true;
            WindowMen.BorderLoadDetailing.IsEnabled = true;
            WindowMen.BorderGhangeDetailing.IsEnabled = true;
            WindowMen.BorderDeleteDetailing.IsEnabled = true;
            WindowMen.BorderPrintDetailing.IsEnabled = true;
            WindowMen.BorderAddDetailing.IsEnabled = true;
            SelectedViewDetailingFeature = new ViewDetailingFeature();
        }

        // команда удаления
        private RelayCommand? removeDetailing;
        public RelayCommand RemoveDetailing
        {
            get
            {
                return removeDetailing ??
                  (removeDetailing = new RelayCommand(obj =>
                  {
                      if (selectedViewDetailingFeature != null)
                      {
                          if (selectedViewDetailingFeature.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              IndexAddEdit = "remove";
                              string json = pathcontrolerDetailing + selectedViewDetailingFeature.id.ToString();
                              CallServer.PostServer(pathcontrolerDetailing, json, "DELETE");
                              SelectNewDetailing();
                              ViewDetailings.Remove(selectedDetailing);
                              ViewDetailingFeatures.Remove(selectedViewDetailingFeature);
                              WindowDetailing.DetailingTablGrid.ItemsSource = ViewDetailingFeatures;
                              BoolFalseDetailing();
                              WindowDetailing.DetailingTablGrid.SelectedItem = null;
                          }
                      }
                  },
                 (obj) => ViewDetailingFeatures != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editDetailing;
        public RelayCommand? EditDetailing
        {
            get
            {
                return editDetailing ??
                  (editDetailing = new RelayCommand(obj =>
                  {
                      if (selectedViewDetailingFeature != null)
                      {
                          if (selectedViewDetailingFeature.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis(); 
                              return;
                          }
                          IndexAddEdit = "editCommand";
                          if (editboolDetailing == false) 
                          {
                              BoolTrueDetailing();
                          }
                          else
                          {
                              BoolFalseDetailing();
                              WindowDetailing .DetailingTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveDetailing;
        public RelayCommand SaveDetailing
        {
            get
            {
                return saveDetailing ??
                  (saveDetailing = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if (WindowDetailing .Detailingt2.Text.Length != 0 || WindowDetailing.Detailingt4.Text.Length != 0)
                      {
                          SelectNewDetailing();
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              json = JsonConvert.SerializeObject(selectedDetailing);
                              CallServer.PostServer(pathcontrolerDetailing, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              ModelDetailing Idinsert = JsonConvert.DeserializeObject<ModelDetailing>(CallServer.ResponseFromServer);
                              if (Idinsert != null)
                              {
                                  selectedViewDetailingFeature.id = Idinsert.id;

                                  ViewDetailingFeatures.Add(selectedViewDetailingFeature);
                                  WindowDetailing.DetailingTablGrid.ItemsSource = ViewDetailingFeatures;
                                  int Countins = ViewDetailings != null ? ViewDetailings.Count : 0;
                                  ViewDetailings.Insert(Countins, Idinsert);
                              }
                          }
                          else
                          {
                              json = JsonConvert.SerializeObject(selectedDetailing);
                              CallServer.PostServer(pathcontrolerDetailing, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              if (GrFeatureDetailing != "")
                              {
                                  string grjason = pathcontrolerDetailing + "0/" + GrFeatureDetailing + "/0";
                                  CallServer.PostServer(pathcontrolerDetailing, grjason, "GETID");
                                  string CmdStroka = CallServer.ServerReturn();
                                  ObservableViewDetailings(CmdStroka);                             
                              }
                          }
                          UnloadCmdStroka("Detailing/", json);
                      }
                      BoolFalseDetailing();
                  }));

            }
        }

        public void SelectNewDetailing()
        {

            //KodDetailing= "A.000.001.001", KeyFeature = "A.000.001", KeyComplaint = "A.000"            
            int _keyDetailingindex = 0;
            selectedDetailing = new ModelDetailing();
            
            if (ViewDetailings == null) ViewDetailings = new ObservableCollection<ModelDetailing>();
            if (IndexAddEdit != "addCommand")selectedDetailing.id =selectedViewDetailingFeature.id;
            else
            {
                //if (selectedViewDetailingFeature == null) selectedViewDetailingFeature = new ViewDetailingFeature();
                if (selectedViewDetailingFeature.kodDetailing == "")
                { 
 
                    selectedViewDetailingFeature.keyFeature = selectedDetailing.keyFeature = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                    string _keyDetailing = selectedDetailing.keyFeature;
                    foreach (ModelDetailing modelDetailing in ViewDetailings)
                    {
                        if (_keyDetailing == modelDetailing.keyFeature)
                        {
                            int shag = modelDetailing.kodDetailing.Length - (modelDetailing.kodDetailing.LastIndexOf(".")+1);
                            string key = modelDetailing.kodDetailing.Substring(modelDetailing.kodDetailing.LastIndexOf(".") + 1, shag);
                            if (_keyDetailingindex < Convert.ToInt32(key))
                            {
                                _keyDetailingindex = Convert.ToInt32(key);
                            }
                        }
                    }
                    _keyDetailingindex++;
                    string _repl = "000";
                    _repl = _repl.Length - _keyDetailingindex.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - _keyDetailingindex.ToString().Length) : "";
                    selectedViewDetailingFeature.kodDetailing = selectedDetailing.kodDetailing = selectedDetailing.keyFeature + "." + _repl + _keyDetailingindex.ToString();
                }
                selectedViewDetailingFeature.nameDetailing = selectedDetailing.nameDetailing = WindowDetailing.Detailingt2.Text.ToString();
                selectedViewDetailingFeature.nameGrDetailing = WindowDetailing.Detailingt4.Text.ToString();
                selectedViewDetailingFeature.idUser = selectedDetailing.idUser = RegIdUser;
                if (selectedViewDetailingFeature.keyGrDetailing != "")
                {
                    selectedDetailing.keyGrDetailing = selectedViewDetailingFeature.keyGrDetailing;
                    selectedDetailing.nameDetailing = selectedViewDetailingFeature.nameGrDetailing;
                }
                else
                { 
                        selectedViewDetailingFeature.keyGrDetailing = selectedDetailing.keyGrDetailing = WindowDetailing.Detailingt4.Text.ToString();            
                }
                selectedViewDetailingFeature.nameFeature = WindowDetailing.Detailingt3.Text.ToString().Substring(WindowDetailing.Detailingt3.Text.ToString().IndexOf(":")+1, WindowDetailing.Detailingt3.Text.Length-  (WindowDetailing.Detailingt3.Text.ToString().IndexOf(":")+1)).TrimStart();                
            }
            selectedDetailing.kodDetailing = selectedViewDetailingFeature.kodDetailing;
            selectedDetailing.keyGrDetailing = selectedViewDetailingFeature.keyGrDetailing;
            selectedDetailing.keyFeature = selectedViewDetailingFeature.keyFeature;
            selectedDetailing.nameDetailing = selectedDetailing.nameDetailing == "" ? selectedViewDetailingFeature.nameDetailing : selectedDetailing.nameDetailing;
            selectedDetailing.idUser = selectedViewDetailingFeature.idUser;

        }

        // команда печати
        RelayCommand? printDetailing;
        public RelayCommand PrintDetailing
        {
            get
            {
                return printDetailing ??
                  (printDetailing = new RelayCommand(obj =>
                  {
                      WindowDetailing .DetailingTablGrid.SelectedItem = null;
                      MessageBox.Show("Деталізація характеру :" + ViewDetailingFeatures[0].nameDetailing.ToString());
                    
                  },
                 (obj) => ViewDetailingFeatures != null));
            }
        }

 
        // команда выбора новой детализации для записи новой строки 
        private RelayCommand? addFeatureDeliting;
        public RelayCommand AddFeatureDeliting
        {
            get
            {
                return addFeatureDeliting ??
                  (addFeatureDeliting = new RelayCommand(obj =>
                  {
                      if (selectedViewDetailingFeature.kodComplaint != "")
                      {
                          CallViewDetailing = "ModelDetailing";
                          ActCompletedInterview = "ModelDetailing";
                          AddComandFeatureDeliting();
                          CallViewDetailing = "";
                          MapOpisViewModel.ActCompletedInterview = "";
                          if (MapOpisViewModel.nameFeature3 == "") return;
                          GrFeatureDetailing = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                          string jason = pathcontrolerDetailing + "0/" + GrFeatureDetailing + "/0";
                          CallServer.PostServer(pathcontrolerDetailing, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          ObservableViewDetailings(CmdStroka);   //if (CmdStroka.Contains("[]") == false)
                          if (IndexAddEdit == "addCommand") SelectNewDetailing();
                      }
                      else WarningMessageSelectComplaint();
                    
 
                  }));
            }
        }

        private void AddComandFeatureDeliting()
        {
            MapOpisViewModel.ActCompletedInterview = "Feature";
            ViewModelNsiFeature.jasonstoka = ViewModelNsiFeature.pathFeatureController + "0/" + selectedViewDetailingFeature.kodComplaint + "/0";
            ViewModelNsiFeature.Method = "GETID";
            WinNsiFeature NewOrder = new WinNsiFeature();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) -150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            if (MapOpisViewModel.nameFeature3.Length > 0)
            {
                selectedViewDetailingFeature.keyFeature = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                selectedViewDetailingFeature.nameFeature = MapOpisViewModel.selectFeature;

            }
        }

        
        // команда выбора новой детализации для записи новой строки 
        private RelayCommand? selectComplaint;
        public RelayCommand SelectComplaint
        {
            get
            {
                return selectComplaint ??
                  (selectComplaint = new RelayCommand(obj =>
                  {
                      SelectedNsiComplaint();
                  }));
            }
        }

        public void SelectedNsiComplaint()
        {
            MapOpisViewModel.ActCompletedInterview = "NameCompl";
            NsiComplaint NewOrder = new NsiComplaint();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-50;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();

            if (MapOpisViewModel.nameFeature3 == "") return;
            if (selectedViewDetailingFeature == null) selectedViewDetailingFeature = new ViewDetailingFeature();
            selectedViewDetailingFeature.kodComplaint = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
            selectedViewDetailingFeature.nameComplaint = MapOpisViewModel.selectedComplaintname;
            MapOpisViewModel.ActCompletedInterview = "";

        }




        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? addSetGrDeliting;
        public RelayCommand AddSetGrDeliting
        {
            get
            {
                return addSetGrDeliting ??
                  (addSetGrDeliting = new RelayCommand(obj =>
                  { AddComandAddSetGrDeliting(); }));
            }
        }

        private void AddComandAddSetGrDeliting()
        {
            MapOpisViewModel.ActCompletedInterview = "NameDeteling";
            WinNsiListGroupDelit NewOrder = new WinNsiListGroupDelit();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (WindowDetailing.Detailingt4.Text.Length !=0)
            {
                if (selectedViewDetailingFeature == null) selectedViewDetailingFeature = new  ViewDetailingFeature();
                selectedViewDetailingFeature.keyGrDetailing = WindowDetailing.Detailingt4.Text.ToString();
                WindowDetailing.Detailingt4.Text = WindowDetailing.Detailingt2.Text;
                WindowDetailing.Detailingt2.Text = "";
                WindowDetailing.Detailingt2.IsEnabled = false;
                WindowDetailing.Detailingt2.Background = Brushes.White;
                WindowDetailing.FolderDetailing.Visibility = Visibility.Visible;
            }
            MapOpisViewModel.ActCompletedInterview = "";
        }

        // AddnameFeature

        private RelayCommand? addnameFeature;
        public RelayCommand AddnameFeature
        {
            get
            {
                return addnameFeature ??
                  (addnameFeature = new RelayCommand(obj =>
                  { AddComandAddnameFeature(); }));
            }
        }
        private void AddComandAddnameFeature()
        {

            if (WindowDetailing.DetailingTablGrid.SelectedIndex >= 0)
            {
                selectedViewDetailingFeature= ViewDetailingFeatures[WindowDetailing.DetailingTablGrid.SelectedIndex];
                
                if (selectedViewDetailingFeature.keyFeature != "")
                { 
                    string json = featurecontroller + selectedViewDetailingFeature.keyFeature+"/0/0";
                    CallServer.PostServer(featurecontroller, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    if (CallServer.ResponseFromServer.Length != 0)
                    {
                        ModelFeature Idinsert = JsonConvert.DeserializeObject<ModelFeature>(CallServer.ResponseFromServer);
                        WindowDetailing.Detailingt3.Text = Idinsert.keyFeature + ": " + Idinsert.name;
                        selectedViewDetailingFeature.kodComplaint = Idinsert.keyComplaint;


                    }
                    if (selectedViewDetailingFeature.keyGrDetailing != "" && selectedViewDetailingFeature.keyGrDetailing !=null )
                    { 
                        json = pathcontrolerListGrDet + selectedViewDetailingFeature.keyGrDetailing + "/0";
                        CallServer.PostServer(pathcontrolerListGrDet, json, "GETID");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        if (CallServer.ResponseFromServer.Length != 0)
                        {
                            ModelListGrDetailing Idinsert = JsonConvert.DeserializeObject<ModelListGrDetailing>(CallServer.ResponseFromServer);
                            WindowDetailing.Detailingt4.Text = Idinsert.keyGrDetailing+ ": " + Idinsert.nameGrup;
                        
                        }                   
                    }
                }
                if (selectedViewDetailingFeature.kodComplaint != null)
                {

                    string jason = pathComplaint + selectedViewDetailingFeature.kodComplaint + "/0";
                    CallServer.PostServer(pathComplaint, jason, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    if (CallServer.ResponseFromServer.Length != 0)
                    {
                        ModelComplaint Idinsert = JsonConvert.DeserializeObject<ModelComplaint>(CallServer.ResponseFromServer);
                        WindowDetailing.DetailingCopl.Text = Idinsert.name;

                    }
                }
                WindowDetailing.FolderDetailing.Visibility = Visibility.Hidden;
                if (selectedViewDetailingFeature.keyGrDetailing !="") WindowDetailing.FolderDetailing.Visibility = Visibility.Visible;

            }
 

        }

        // команда открытия окна содержащего группу детализации характера жалобы  
        private RelayCommand? viewGroupDetailing;
        public RelayCommand ViewGroupDetailing
        {
            get
            {
                return viewGroupDetailing ??
                  (viewGroupDetailing = new RelayCommand(obj =>
                  {
                      if (selectedViewDetailingFeature != null)
                      {
                          if (selectedViewDetailingFeature.keyGrDetailing != null)
                          {
                              string jason = controlerGrDetailing + "0/" + selectedViewDetailingFeature.keyGrDetailing + "/0";
                              CallServer.PostServer(controlerGrDetailing, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  MapOpisViewModel.ActCompletedInterview = "ViewGrDetailing";
                                  WinNsiGrDetailing NewNsi = new WinNsiGrDetailing();
                                  NewNsi.Left = (MainWindow.ScreenWidth / 2) -50;
                                  NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                                  NewNsi.ShowDialog();
                                  MapOpisViewModel.ActCompletedInterview = null;

                              }

                          }
                      }

                  }));
            }
        }

        

        // команда открытия окна содержащего группу детализации характера жалобы  
        private RelayCommand? selectedNewFeature;
        public RelayCommand SelectedNewFeature
        {
            get
            {
                return selectedNewFeature ??
                  (selectedNewFeature = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      NewEkzemplyarDetailing();
                      SelectedNsiComplaint();
                      if (MapOpisViewModel.nameFeature3 == ""  || selectedViewDetailingFeature == null) return;

                      if (selectedViewDetailingFeature == null) selectedViewDetailingFeature = new ViewDetailingFeature();
                      MapOpisViewModel.ActCompletedInterview = "Feature";
                      MapOpisViewModel.ActCreatInterview = "Feature";
                      ViewModelNsiFeature.jasonstoka = ViewModelNsiFeature.pathFeatureController + "0/"+(selectedViewDetailingFeature.kodComplaint == "" ? "0" : selectedViewDetailingFeature.kodComplaint ) + "/0"; 
                      ViewModelNsiFeature.Method = selectedViewDetailingFeature.kodComplaint == "" ? "GET" : "GETID";
                      WinNsiFeature NewOrder = new WinNsiFeature();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      CallViewDetailing = "";
                      MapOpisViewModel.ActCompletedInterview = "";
                      MapOpisViewModel.ActCreatInterview = "";
                        if (MapOpisViewModel.nameFeature3 == "") return;
                        GrFeatureDetailing = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                        string jason = pathcontrolerDetailing + "0/" + GrFeatureDetailing + "/0";
                        CallServer.PostServer(pathcontrolerDetailing, jason, "GETID");
                        string CmdStroka = CallServer.ServerReturn();
                      if (CmdStroka.Contains("[]") == false)ObservableViewDetailings(CmdStroka);
                      else
                      { 
                        ViewDetailingFeatures = new  ObservableCollection<ViewDetailingFeature>();
                        WindowDetailing.DetailingTablGrid.ItemsSource = ViewDetailingFeatures;
                        WindowDetailing.FolderDetailing.Visibility = Visibility.Hidden;
                      }
                      loadboolDetailing = true;
                      TrueNameComplaint();
                  }));
            }
        }
        #endregion
        #endregion
    }
}
