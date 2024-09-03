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
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        //  ModelFeature модель Feature
        // клавиша в окне: "Характеристики"
        #region Обработка событий и команд вставки, удаления и редектирования справочника "характер,Особеность жалобы"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>      
        private bool editboolFeature = false, addtboolFeature = false, loadboolFeature = false;
        private string edittextFeature = "";
        public static string featurecontroller =  "/api/FeatureController/", nameComplaint = "";
        public static ModelFeature selectedFeature;
        public static ViewFeatureComplaint selectedViewFeature;
        public static ObservableCollection<ModelFeature> ModelFeatures { get; set; }
        public static ObservableCollection<ViewFeatureComplaint> ViewFeatures { get; set; }
        public ModelFeature SelectedFeature
        {
            get { return selectedFeature; }
            set { selectedFeature = value; OnPropertyChanged("SelectedFeature"); }
        }

        public ViewFeatureComplaint SelectedViewFeature
        {
            get { return selectedViewFeature; }
            set { selectedViewFeature = value; OnPropertyChanged("SelectedViewFeature"); }
        }

        public static void ObservableModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelFeature>(CmdStroka);
            List<ModelFeature> res = result.ModelFeature.ToList();
            ModelFeatures = new ObservableCollection<ModelFeature>((IEnumerable<ModelFeature>)res);
            if (ModelFeatures.Count > 0) LoadCollectionViewFeatures();


           
        }

        public static void LoadCollectionViewFeatures()
        {
            string  keyComplaint="";
            ViewFeatures = new ObservableCollection<ViewFeatureComplaint>();
            foreach (ModelFeature modelFeature in ModelFeatures)
            {
                selectedViewFeature = new ViewFeatureComplaint();
                selectedViewFeature.id = modelFeature.id;
                selectedViewFeature.keyComplaint = modelFeature.keyComplaint;
                selectedViewFeature.keyFeature = modelFeature.keyFeature;
                selectedViewFeature.nameFeature = modelFeature.name;
                selectedViewFeature.nameComplaint = "";
                if (ViewFeatures.Count == 0 || keyComplaint != modelFeature.keyComplaint)
                {
                    selectedViewFeature.nameComplaint= NameCompl(modelFeature.keyComplaint).ToUpper();
                }
                ViewFeatures.Add(selectedViewFeature);
            }
            WindowMen.FeatureTablGrid.ItemsSource = ViewFeatures;
        }

        private static string NameCompl(string keyComplaint ="")
        {

            string json = pathComplaint + selectedViewFeature.keyComplaint+"/0";  
            CallServer.PostServer(pathComplaint, json, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelComplaint Idinsert = JsonConvert.DeserializeObject<ModelComplaint>(CallServer.ResponseFromServer);
            json = Idinsert == null ? "" : Idinsert.name;
            return json;
        }

        #region Команды вставки, удаления и редектирования справочника "Жалобы"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadFeature;
            public RelayCommand LoadFeature
            {
                get
                {
                    return loadFeature ??
                      (loadFeature = new RelayCommand(obj =>
                      {
                          if (loadboolFeature == false) MethodLoadtableFeature();
                      }));
                }
            }

        // команда добавления нового объекта
        private RelayCommand? addFeature;
        public RelayCommand AddFeature
        {
            get
            {
                return addFeature ??
                  (addFeature = new RelayCommand(obj =>
                  { AddComandFeature(); }));
            }
        }

        private void AddComandFeature()
        {
            if (loadboolFeature == false) MethodLoadtableFeature();
            MethodaddcomFeature();
        }

        private void MethodLoadtableFeature()
        {
            WindowMen.Loadnsi.Visibility = Visibility.Hidden;
            MapOpisViewModel.ActCompletedInterview = "null";
            CallServer.PostServer(featurecontroller, featurecontroller, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelFeatures(CmdStroka);
        }

        private void MethodaddcomFeature()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addtboolFeature == false) BoolTrueFeature();
            else BoolFalseFeature();
            WindowMen.FeatureTablGrid.SelectedItem = null;
            SelectedViewFeature = new ViewFeatureComplaint();

        }


        private void BoolTrueFeature()
        {
            addtboolFeature = true;
            editboolFeature = true;
            WindowMen.Featuret2.IsEnabled = true;
            WindowMen.Featuret2.Background = Brushes.AntiqueWhite;
            WindowMen.FeatureTablGrid.IsEnabled = false;
            //if (IndexAddEdit == "addCommand")
            //{ 
            //WindowMen.Folder.Visibility = Visibility.Visible;
            //}
            //if (IndexAddEdit == "editCommand")
            //{
            WindowMen.Folder.Visibility = Visibility.Visible;
                WindowMen.FolderDet5.Visibility = Visibility.Visible;

            //}
        }

        private void BoolFalseFeature()
        {
            addtboolFeature = false;
            editboolFeature = false;
            WindowMen.Featuret2.IsEnabled =  false;
            WindowMen.Featuret2.Background =  Brushes.White;
            WindowMen.Folder.Visibility = Visibility.Hidden;
            WindowMen.FolderDet5.Visibility = Visibility.Hidden;
            WindowMen.FeatureTablGrid.IsEnabled = true;
        }
        // команда удаления
        private RelayCommand? removeFeature;
        public RelayCommand RemoveFeature
        {
            get
            {
                return removeFeature ??
                  (removeFeature = new RelayCommand(obj =>
                  {
                      WindowMen.Folder.Visibility = Visibility.Hidden;
                      if (selectedViewFeature != null)
                      {
                          if (selectedViewFeature.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = featurecontroller + selectedViewFeature.id.ToString();
                              CallServer.PostServer(featurecontroller, json, "DELETE");
                              IndexAddEdit = "remove";
                              SelectNewFeature();
                              ModelFeatures.Remove(selectedFeature);
                              ViewFeatures.Remove(selectedViewFeature);
                              WindowMen.FeatureTablGrid.ItemsSource = ViewFeatures;
                              BoolFalseFeature();
                              IndexAddEdit = "";
                              WindowMen.FeatureTablGrid.SelectedItem = null;
                          }
                      }
 
                  },
                 (obj) => ModelFeatures !=null));
            }
        }

        // команда  редактировать
        private RelayCommand? editFeature;
        public RelayCommand? EditFeature
        {
            get
            {
                return editFeature ??
                  (editFeature = new RelayCommand(obj =>
                  {
                      
                      IndexAddEdit = "editCommand";
                      if (editboolFeature == false )
                      {
                          if (WindowMen.FeatureTablGrid.SelectedIndex>=0)
                          { 
                              selectedViewFeature = ViewFeatures[WindowMen.FeatureTablGrid.SelectedIndex];
                              if (selectedViewFeature.idUser != RegIdUser && RegUserStatus != "1")
                              {
                                  InfoEditZapis();
                                  return;
                              }
                              BoolTrueFeature();
                          }
                          
                      }
                      else 
                      {
                          BoolFalseFeature();
                          IndexAddEdit = "";
                          WindowMen.FeatureTablGrid.SelectedItem = null;
                      }
 
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveFeature;
        public RelayCommand SaveFeature
        {
            get
            {
                return saveFeature ??
                  (saveFeature = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if ( WindowMen.Featuret2.Text.Length !=0)
                      {
                          //  формирование кода Feature по значениею группы выранной жалобы
                          SelectNewFeature();
                          if (IndexAddEdit == "addCommand")
                          {
                              
                              
                              // ОБращение к серверу добавляем запись
                              json = JsonConvert.SerializeObject(selectedFeature);
                              CallServer.PostServer(featurecontroller, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelFeature Idinsert = JsonConvert.DeserializeObject<ModelFeature>(CallServer.ResponseFromServer);
                              if (Idinsert != null)
                              { 
                                  selectedViewFeature = new ViewFeatureComplaint();
                                  selectedViewFeature.id = Idinsert.id;
                                  selectedViewFeature.keyComplaint = Idinsert.keyComplaint;
                                  selectedViewFeature.keyFeature = Idinsert.keyFeature;
                                  selectedViewFeature.nameComplaint = NameCompl(Idinsert.keyComplaint).ToUpper(); ;
                                  selectedViewFeature.nameFeature = Idinsert.name;
                                  selectedViewFeature.idUser = Idinsert.idUser;
                              }

                              if (ViewFeatures == null)ViewFeatures = new ObservableCollection<ViewFeatureComplaint>();
                              ViewFeatures.Add(selectedViewFeature);
                              WindowMen.FeatureTablGrid.ItemsSource = ViewFeatures;
                          }
                          else
                          {
                               // ОБращение к серверу измнить корректируемую запись в БД
                              json = JsonConvert.SerializeObject(selectedFeature);
                              CallServer.PostServer(featurecontroller, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("Feature/", json);
                      }
                      BoolFalseFeature();
                      IndexAddEdit = "";
                      SelectedViewFeature = new ViewFeatureComplaint();
                      WindowMen.FeatureTablGrid.SelectedItem = null;

                  }));
            }
        }

     
        public void SelectNewFeature()
        {
            
           //KeyFeature = "A.000.001", KeyComplaint = "A.000"            
            int _keyFeatureindex = 0;
            if(selectedFeature == null) selectedFeature = new ModelFeature();
            if (IndexAddEdit == "addCommand" )
            {
                selectedFeature.keyComplaint = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                selectedFeature.name = WindowMen.Featuret2.Text.ToString();
                selectedFeature.idUser = RegIdUser;
                string _keyComplaint = selectedFeature.keyComplaint;
                foreach (ModelFeature modelFeature in ModelFeatures)
                {
                    if (_keyComplaint == modelFeature.keyComplaint)
                    {
                        int lengthstr = modelFeature.keyFeature.Length - (modelFeature.keyFeature.LastIndexOf(".") + 1);
                        if (_keyFeatureindex < Convert.ToInt32(modelFeature.keyFeature.Substring(modelFeature.keyFeature.LastIndexOf(".") + 1, lengthstr)))
                        {
                            _keyFeatureindex = Convert.ToInt32(modelFeature.keyFeature.Substring(modelFeature.keyFeature.LastIndexOf(".") + 1, lengthstr));
                        }
                    }
                }
                _keyFeatureindex++;
                string _repl = "000";
                _repl = _repl.Length - _keyFeatureindex.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - _keyFeatureindex.ToString().Length) : "";
                selectedFeature.keyFeature = selectedFeature.keyComplaint + "." + _repl + _keyFeatureindex.ToString();
            }
            else
            {

                selectedFeature.id = selectedViewFeature.id;
                selectedFeature.keyComplaint = selectedViewFeature.keyComplaint;
                selectedFeature.keyFeature = selectedViewFeature.keyFeature;
                selectedFeature.name = WindowMen.Featuret2.Text.ToString();
                selectedFeature.idUser = RegIdUser;
            }
 
        }

        // команда печати
        RelayCommand? printFeature;
        public RelayCommand PrintFeature
        {
            get
            {
                return printFeature ??
                  (printFeature = new RelayCommand(obj =>
                  {
                      if (ModelFeatures != null)
                      {
                          MessageBox.Show("Характер прояву :" + ModelFeatures[0].name.ToString());
                      }
                  },
                 (obj) => ModelFeatures != null));
            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? addFeatureComplaint;
        public RelayCommand AddFeatureComplaint
        {
            get
            {
                return addFeatureComplaint ??
                  (addFeatureComplaint = new RelayCommand(obj =>
                  { AddComandFeatureComplaint(); }));
            }
        }

        private void AddComandFeatureComplaint()
        {
            if (selectedViewFeature == null) selectedViewFeature = new ViewFeatureComplaint();
            if (selectedFeature == null) selectedFeature = new  ModelFeature();
            MapOpisViewModel.ActCompletedInterview = "Feature";
            NsiComplaint NewOrder = new NsiComplaint();
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            selectedViewFeature.keyComplaint = selectedFeature.keyComplaint = MapOpisViewModel.nameFeature3.Length > 0 ? MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":")) : selectedFeature.keyComplaint;
            
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? addnameComplaint;
        public RelayCommand AddnameComplaint
        {
            get
            {
                return addnameComplaint ??
                  (addnameComplaint = new RelayCommand(obj =>
                  { AddComandnameComplaint(); }));
            }
        }
        private void AddComandnameComplaint()
        {
            if (selectedViewFeature != null)
            {
                WindowMen.FolderDet5.Visibility = Visibility.Visible;
                selectedFeature = ModelFeatures[WindowMen.FeatureTablGrid.SelectedIndex-1];

            }

        }


        // команда открытия окна содержащего детализацию характера жалобы  
        private RelayCommand? viewDetailingFeature;
        public RelayCommand ViewDetailingFeature
        {
            get
            {
                return viewDetailingFeature ??
                  (viewDetailingFeature = new RelayCommand(obj =>
                  {
                      if (WindowMen.FeatureTablGrid.SelectedIndex != -1)
                      {
                          if (IndexAddEdit != "editCommand" && selectedViewFeature.keyFeature != null && selectedViewFeature.keyFeature != "")
                          {
                              MapOpisViewModel.ActCompletedInterview = "ViewDetailing";
                              string pathDetailingcontroller = "/api/DetailingController/";
                              string jason = pathDetailingcontroller + "0/" + selectedViewFeature.keyFeature;
                              CallServer.PostServer(pathDetailingcontroller, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == true) return;
                          }

                          MapOpisViewModel.ActCompletedInterview = "Feature";
                          NsiDetailing NewNsi = new NsiDetailing();
                          NewNsi.Left = (MainWindow.ScreenWidth / 2);
                          NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                          NewNsi.ShowDialog();
                          MapOpisViewModel.ActCompletedInterview = "";
                          //ModelFeatures
                      }


                  }));
            }
        }

        // команда открытия окна содержащего детализацию характера жалобы  
        private RelayCommand? selectedCompl;
        public RelayCommand SelectedCompl
        {
            get
            {
                return selectedCompl ??
                  (selectedCompl = new RelayCommand(obj =>
                  {
                     
 
                          MapOpisViewModel.ActCompletedInterview = "NameCompl";
                          NsiComplaint NewOrder = new NsiComplaint();
                          NewOrder.Left = (MainWindow.ScreenWidth / 2);
                          NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                          NewOrder.ShowDialog();
                          MapOpisViewModel.ActCompletedInterview = "";
                          if (MapOpisViewModel.nameFeature3 == "") return;
                          string jason = featurecontroller + "0/" + MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                          CallServer.PostServer(featurecontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableModelFeatures(CmdStroka);

  
                  }));
            }
        }
        #endregion
        #endregion
    }
}
