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
    public partial class MapOpisViewModel : INotifyPropertyChanged
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
        private bool loadbooltablDependency = false;
        public static string pathcontrolerDependency =  "/api/DependencyDiagnozController/";
        public static ModelDependencyDiagnoz selectedDependency;
        private static ObservableCollection<ModelDependency> modelDependencies { get; set; }
        public static ObservableCollection<ModelDependencyDiagnoz> ViewModelDependencys { get; set; }
        private static ModelDependencyDiagnoz modelDependencyDiagnoz = new ModelDependencyDiagnoz();
        private static ModelDependency modelDependency = new ModelDependency();
        public ModelDependencyDiagnoz SelectedModelDependency
        {
            get { return selectedDependency; }
            set { selectedDependency = value; OnPropertyChanged("SelectedModelDependency"); }
        }

        public static void ObservableViewModelDependency(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDependency>(CmdStroka);
            List<ModelDependency> res = result.ModelDependency.ToList();
            modelDependencies = new ObservableCollection<ModelDependency>((IEnumerable<ModelDependency>)res);
            ViewModelDependencys = new ObservableCollection<ModelDependencyDiagnoz>();

            foreach (ModelDependency dependency in modelDependencies)
            {
                selectedDependency = new ModelDependencyDiagnoz();
                SelectDiagnozRecom(dependency);
                ViewModelDependencys.Add(selectedDependency);
            }
            WindowMen.DependencyTablGrid.ItemsSource = ViewModelDependencys;

        }

        private  static void SelectDiagnozRecom(ModelDependency modelDependency)
        {

            selectedDependency.id = modelDependency.id;
            selectedDependency.kodDiagnoz = modelDependency.kodDiagnoz;
            selectedDependency.kodRecommend = modelDependency.kodRecommend;
            selectedDependency.kodProtokola = modelDependency.kodProtokola;           
 
            if(modelDependency.kodDiagnoz != null)
            {
                   MainWindow.UrlServer = controlerViewDiagnoz;
                    string json = controlerViewDiagnoz + modelDependency.kodDiagnoz.ToString()+"/0";
                    CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                    if(Idinsert !=null) selectedDependency.nameDiagnoza = Idinsert.nameDiagnoza;   

            }
          
            if(modelDependency.kodRecommend !=null)
            {

                    MainWindow.UrlServer = controlerModelRecommendation;
                    var json = controlerModelRecommendation + modelDependency.kodRecommend.ToString();
                    CallServer.PostServer(controlerModelRecommendation, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelRecommendation Insert = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                    if (Insert != null) selectedDependency.contentRecommendation = Insert.contentRecommendation;    
            }

            if (modelDependency.kodProtokola != null)
            {

                if (modelDependency.kodProtokola != "")
                { 
                    var json = Interviewcontroller + modelDependency.kodProtokola.ToString() + "/0/0";
                    CallServer.PostServer(Interviewcontroller, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelInterview Insert = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
                    if (Insert != null) selectedDependency.nameInterview =  Insert.nametInterview;                
                }

            }


        }

        #region Команды вставки, удаления и редектирования справочника "детализация Insert.kodProtokola
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadDependency;
        public RelayCommand LoadDependency
        {
            get
            {
                return loadDependency ??
                  (loadDependency = new RelayCommand(obj =>
                  {
                      if (loadbooltablDependency == false) MethodloadtablDependency();
                  }));
            }
        }

        // команда добавления нового объекта
        private bool activAddDependency = false;
        private RelayCommand? addDependency;
        public RelayCommand AddDependency
        {
            get
            {
                return addDependency ??
                  (addDependency = new RelayCommand(obj =>
                  { AddComandDependency(); }));
            }
        }

        private void AddComandDependency()
        {
            if (loadbooltablDependency == false) MethodloadtablDependency();
            MethodaddcomDependency();
            
        }
        private void MethodaddcomDependency()
        {
            WindowMen.DependencyTablGrid.SelectedItem = null;
            selectedDependency = new ModelDependencyDiagnoz();
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activAddDependency == false)ModelDependencytrue();
            else ModelDependencyfalse();
        }
        private void MethodloadtablDependency()
        {
            WindowMen.LoadDepen.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerDependency, pathcontrolerDependency, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewModelDependency(CmdStroka);
        }

        private void ModelDependencytrue()
        {
            activAddDependency = true;
            editboolDependency = true;
            WindowMen.FolderDiag.Visibility = Visibility.Visible;
            WindowMen.FolderRecom.Visibility = Visibility.Visible;
            WindowMen.FolderInterv.Visibility = Visibility.Visible;
            
        }

        private void ModelDependencyfalse()
        {
            activAddDependency = false;
            editboolDependency = false;
            WindowMen.FolderDiag.Visibility = Visibility.Hidden;
            WindowMen.FolderRecom.Visibility = Visibility.Hidden;
            WindowMen.FolderInterv.Visibility = Visibility.Hidden;
        }

        // команда удаления
        private RelayCommand? removeDependency;
        public RelayCommand RemoveDependency
        {
            get
            {
                return removeDependency ??
                  (removeDependency = new RelayCommand(obj =>
                  {
                      if (selectedDependency !=null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathcontrolerDependency + selectedDependency.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ViewModelDependencys.Remove(selectedDependency);
                              WindowMen.DependencyTablGrid.SelectedItem = null;
                              selectedDependency = new ModelDependencyDiagnoz();
                              ModelDependencyfalse();
                          }
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewModelDependencys != null));
            }
        }


        // команда  редактировать
        private bool editboolDependency = false;
        private RelayCommand? editDependency;
        public RelayCommand? EditDependency
        {
            get
            {
                return editDependency ??
                  (editDependency = new RelayCommand(obj =>
                  {
                      if (selectedDependency != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolDependency == false)
                          {
                              ModelDependencytrue();
                          }
                          else
                          {
                              ModelDependencyfalse();
                              WindowMen.DependencyTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveDependency;
        public RelayCommand SaveDependency
        {
            get
            {
                return saveDependency ??
                  (saveDependency = new RelayCommand(obj =>
                  {
                      modelDependency = new ModelDependency();
                      SelectNewDependency();
                      string json = "";
                      if (WindowMen.Dependencyt2.Text.Trim().Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              json = JsonConvert.SerializeObject(modelDependency);
                              CallServer.PostServer(pathcontrolerDependency, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                              int Countins = modelDependencies !=null ? modelDependencies.Count : 0;
                              modelDependencies.Insert(Countins, Insert);
                              SelectDiagnozRecom(Insert);
                              ViewModelDependencys.Add(selectedDependency);
                              WindowMen.DependencyTablGrid.ItemsSource = ViewModelDependencys;

                          }
                          else
                          {
                              json = JsonConvert.SerializeObject(modelDependency);
                              CallServer.PostServer(pathcontrolerDependency, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("DependencyDiagnoz/", json);
                      }
                      WindowMen.DependencyTablGrid.SelectedItem = null;
                      WindowMen.Dependencyt2.Text = "";
                      WindowMen.Dependencyt3.Text = "";
                      WindowMen.Dependencyt4.Text = "";
                      IndexAddEdit = "";
                      ModelDependencyfalse();

                  }));
            }
        }

        private void SelectNewDependency()
        {
            modelDependency.id = selectedDependency.id;
            modelDependency.kodProtokola = selectedDependency.kodProtokola;
            modelDependency.kodDiagnoz = selectedDependency.kodDiagnoz;
            modelDependency.kodRecommend = selectedDependency.kodRecommend;
            
        }

        // команда печати
        RelayCommand? printDependency;
        public RelayCommand PrintDependency
        {
            get
            {
                return printDependency ??
                  (printDependency = new RelayCommand(obj =>
                  {
                      ModelDependencyfalse();
                      WindowMen.DependencyTablGrid.SelectedItem = null;
                      if (ViewModelDependencys.Count != 0)
                      {
                          MessageBox.Show("Відповідність між діагнозом та рекомендацією :" + ViewModelDependencys[0].nameDiagnoza.ToString());
                      }
                  },
                 (obj) => ViewModelDependencys != null));
            }
        }



        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? setDependencyDiagnoz;
        public RelayCommand AddDependencyDiagnoz
        {
            get
            {
                return setDependencyDiagnoz ??
                  (setDependencyDiagnoz = new RelayCommand(obj =>
                  { AddComandDependencyDiagnoz(); }));
            }
        }

        private void AddComandDependencyDiagnoz()
        {
            ModelCall = "Dependency";
            WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
            NewOrder.Left = 600;
            NewOrder.Top = 200;
            NewOrder.ShowDialog();

        }

        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? setDependencyRecomen;
        public RelayCommand AddDependencyRecomen
        {
            get
            {
                return setDependencyRecomen ??
                  (setDependencyRecomen = new RelayCommand(obj =>
                  { AddComandDependencyRecomen(); }));
            }
        }

        private void AddComandDependencyRecomen()
        {
            WinNsiListRecommen NewOrder = new WinNsiListRecommen();
            NewOrder.Left = 600;
            NewOrder.Top = 200;
            NewOrder.ShowDialog();

        }

        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? setAddnsiInterview;
        public RelayCommand AddNsiInterview
        {
            get
            {
                return setAddnsiInterview ??
                  (setAddnsiInterview = new RelayCommand(obj =>
                  { AddComandAddInterview(); }));
            }
        }

        private void AddComandAddInterview()
        {
            WinNsiIntreview NewOrder = new WinNsiIntreview();
            NewOrder.Left = 600;
            NewOrder.Top = 200;
            NewOrder.ShowDialog();

        }
        
        #endregion
        #endregion
    }
}
