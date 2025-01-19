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
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        // GrDetailing модель ViewGrDetailing
        // клавиша в окне: "Групови детализации"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы детализации"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>
        private bool activedit = false, loadboolGrDeliting = false;
        bool activGrDeliting = false;
        int GrDetailingSelectedIndex = 0;
        public static string controlerGrDetailing =  "/api/GrDetalingController/", _nameGrDetailing = "";
        private ModelGrDetailing selectedViewGrDeliting;

        public static ObservableCollection<ModelGrDetailing> ViewGrDetailings { get; set; }
        private string edittext = "";
        public ModelGrDetailing SelectedViewGrDeliting
        { get { return selectedViewGrDeliting; } set { selectedViewGrDeliting = value; OnPropertyChanged("SelectedViewGrDeliting"); } }

        public static void ObservableViewGrDeliting(string CmdStroka)
        {
            int indexgr = 0;
            string _keyGrDetailing = "";
            var result = JsonConvert.DeserializeObject<ListModelGrDetailing>(CmdStroka);
            List<ModelGrDetailing> res = result.ViewGrDetailing.ToList();
            ViewGrDetailings = new ObservableCollection<ModelGrDetailing>((IEnumerable<ModelGrDetailing>)res);
            foreach (ModelGrDetailing modelGrDetailing in ViewGrDetailings)
            {
                if (_keyGrDetailing != modelGrDetailing.keyGrDetailing)
                {
                    
                    _keyGrDetailing = modelGrDetailing.keyGrDetailing;
                    string jason = ViewModelNsiListGroupDelit.controlerListGrDetailing + _keyGrDetailing+ "/0" ;
                    CallServer.PostServer(ViewModelNsiListGroupDelit.controlerListGrDetailing, jason, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    json = CallServer.ResponseFromServer;
                    ModelListGrDetailing Idinsert = JsonConvert.DeserializeObject<ModelListGrDetailing>(json);
                    nameGrDetailing = Idinsert.nameGrup;
                }
                ViewGrDetailings[indexgr].keyGrDetailing += " "+nameGrDetailing;
                indexgr++;
            }
            WindowMen.GrDetailingsTablGrid.ItemsSource = ViewGrDetailings;
        }

        #region Команды вставки, удаления и редектирования справочника "Жалобы"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadGrDetailing;
        public RelayCommand LoadGrDetailing
        {
            get
            {
                return loadGrDetailing ??
                  (loadGrDetailing = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MehodloadtablGrDeliting();

                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand addGrDeliting;
        public RelayCommand AddGrDeliting
        {
            get
            {
                return addGrDeliting ??
                  (addGrDeliting = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComGrDeliting(); }));
            }
        }

        private void AddComGrDeliting()
        {
           
            if (loadboolGrDeliting == false) MehodloadtablGrDeliting();
            MethodaddcomGrDeliting();
        }

        private void MethodaddcomGrDeliting()
        {
            IndexAddEdit =  "addCommand";
            if (activGrDeliting == false) BoolTrueGrDetailing(); 
            else BoolFalseGrDetailing();
            TrueNameGrDetailing();
            SelectNewGrDetailing();
        }
        private void MehodloadtablGrDeliting()
        {
            NewEkzemplyarGrDetailing();
            WindowMen.LoadGrdetalings.Visibility = Visibility.Hidden;
            CallServer.PostServer(controlerGrDetailing, controlerGrDetailing, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewGrDeliting(CmdStroka);
        }

        private void TrueNameGrDetailing()
        {
            if (_nameGrDetailing != "")
            {
                WindowMen.GrDetailingst2.Text = _nameGrDetailing;
                selectedViewGrDeliting.keyGrDetailing = _nameGrDetailing;
            }
        }

        private void NewEkzemplyarGrDetailing()
        {
            SelectedViewGrDeliting = selectedViewGrDeliting = new ModelGrDetailing();
        }
        private void BoolTrueGrDetailing()
        {
            activedit = true;
            activGrDeliting = true;
            if(IndexAddEdit == "addCommand") WindowMen.FolderGR.Visibility = Visibility.Visible;
            WindowMen.FolderQu.Visibility = Visibility.Visible;
            WindowMen.GrDetailingst3.IsEnabled = true;
            WindowMen.GrDetailingst3.Background = Brushes.AntiqueWhite;
            WindowMen.GrDetailingsTablGrid.IsEnabled = false;
        }

        private void BoolFalseGrDetailing()
        {
            activGrDeliting = false;
            activedit = false;
            IndexAddEdit = "";
            WindowMen.FolderGR.Visibility = Visibility.Hidden;
            WindowMen.FolderQu.Visibility = Visibility.Hidden;
            WindowMen.GrDetailingst3.IsEnabled = false;
            WindowMen.GrDetailingst3.Background = Brushes.White;
            WindowMen.GrDetailingsTablGrid.IsEnabled = true;
        }
        // команда удаления
        private RelayCommand? removeGrDetailing;
        public RelayCommand RemoveViewGrDetailing
        {
            get
            {
                return removeGrDetailing ??
                  (removeGrDetailing = new RelayCommand(obj =>
                  {
                      if (selectedViewGrDeliting != null)
                      {
                          if (selectedViewGrDeliting.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerGrDetailing + selectedViewGrDeliting.id.ToString();
                              CallServer.PostServer(controlerGrDetailing, json, "DELETE");
                              ViewGrDetailings.Remove(selectedViewGrDeliting);
                              selectedViewGrDeliting = new ModelGrDetailing();
                              BoolFalseGrDetailing();
                          }
                      }
                      
                  },
                 (obj) => ViewGrDetailings != null)); 
            }
        }

        // команда  редактировать

       
        private RelayCommand? editGrDetailing;
        public RelayCommand? EditGrDetailing
        {
            get
            {
                return editGrDetailing ??
                  (editGrDetailing = new RelayCommand(obj =>
                  {
                      if (selectedViewGrDeliting != null)
                      {
                          if (selectedViewGrDeliting.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          IndexAddEdit = "editCommand";    
                          if (activedit == false)
                          {
                                BoolTrueGrDetailing();
                          }
                          else
                          {
                              BoolFalseGrDetailing();
                              WindowMen.GrDetailingsTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveGrDetailing;
        public RelayCommand SaveGrDetailing
        {
            get
            {
                return saveGrDetailing ??
                  (saveGrDetailing = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if (WindowMen.GrDetailingst3.Text.Trim().Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {

                              //selectedViewGrDeliting.nameGrDetailing = WindowMen.GrDetailingst3.Text.ToString();
                              json = JsonConvert.SerializeObject(selectedViewGrDeliting);
                              CallServer.PostServer(controlerGrDetailing, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelGrDetailing Idinsert = JsonConvert.DeserializeObject<ModelGrDetailing>(CallServer.ResponseFromServer);
                              int Countins = ViewGrDetailings != null ? ViewGrDetailings.Count : 0;
                              Idinsert.keyGrDetailing += " "+nameGrDetailing;
                              ViewGrDetailings.Insert(Countins, Idinsert);
                              SelectedViewGrDeliting = Idinsert;
                              GrDetailingSelectedIndex = ViewGrDetailings.Count - 1;
                          }
                          else
                          {
                              if (selectedViewGrDeliting.keyGrDetailing != "" && selectedViewGrDeliting.keyGrDetailing != null && selectedViewGrDeliting.keyGrDetailing.Contains(" ") == true) ;
                              {
                                  selectedViewGrDeliting.keyGrDetailing = selectedViewGrDeliting.keyGrDetailing.Substring(0, selectedViewGrDeliting.keyGrDetailing.IndexOf(" "));
                              }
                              json = JsonConvert.SerializeObject(selectedViewGrDeliting);
                              CallServer.PostServer(controlerGrDetailing, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              selectedViewGrDeliting = JsonConvert.DeserializeObject<ModelGrDetailing>(CallServer.ResponseFromServer);
                              selectedViewGrDeliting.keyGrDetailing = nameGrDetailing;
                              ViewGrDetailings[GrDetailingSelectedIndex].keyGrDetailing = nameGrDetailing;

                          }
                          UnloadCmdStroka("GrDetailing/", json);

                      }
                      
                      BoolFalseGrDetailing();
                      WindowMen.GrDetailingsTablGrid.SelectedItem = null;
                  }));
            }
        }

        public void SelectNewGrDetailing()
        {

            //KodDetailing= "A.000.001.001", KeyFeature = "A.000.001", KeyComplaint = "A.000"
            // KodDetailing = "AA.000.001", KeyGrDetailing = "AA.000" 
            int _keyDetailingindex = 0, lenghkod =0;
            string stringkod = "";
            if (selectedViewGrDeliting == null) selectedViewGrDeliting = new ModelGrDetailing();
            if (WindowMen.GrDetailingst2.Text != "")
            { 
                selectedViewGrDeliting.keyGrDetailing = WindowMen.GrDetailingst2.Text.ToString().Substring(0, WindowMen.GrDetailingst2.Text.ToString().IndexOf(":"));
                string _keyGrDetailing = selectedViewGrDeliting.keyGrDetailing;
                foreach (ModelGrDetailing modelDetailing in ViewGrDetailings)
                {
                    if (_keyGrDetailing == modelDetailing.keyGrDetailing.Substring(0, modelDetailing.keyGrDetailing.IndexOf(" "))) //WindowMen.GrDetailingst2.Text.ToString()
                    {
                        lenghkod = modelDetailing.kodDetailing.Length - modelDetailing.kodDetailing.LastIndexOf(".")-1;
                        stringkod = modelDetailing.kodDetailing.Substring(modelDetailing.kodDetailing.LastIndexOf(".") + 1, lenghkod);
                        if (_keyDetailingindex < Convert.ToInt32(stringkod))
                        {
                            _keyDetailingindex = Convert.ToInt32(stringkod);
                        }
                    }
                }
                _keyDetailingindex++;
                string _repl = "000";
                _repl = _repl.Length - _keyDetailingindex.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - _keyDetailingindex.ToString().Length) : "";
                selectedViewGrDeliting.kodDetailing = selectedViewGrDeliting.keyGrDetailing + "." + _repl + _keyDetailingindex.ToString();
                selectedViewGrDeliting.nameGrDetailing = WindowMen.GrDetailingst3.Text.ToString();
                selectedViewGrDeliting.kodGroupQualification = WindowMen.GrDetailingst4.Text.ToString().Length>0 ? WindowMen.GrDetailingst4.Text.ToString().Substring(0, WindowMen.GrDetailingst4.Text.ToString().IndexOf(":")): "";
                selectedViewGrDeliting.idUser = RegIdUser;            
            }

        }


        // команда печати
        RelayCommand? printGrDetailing;
        public RelayCommand PrintDetailingGr
        {
            get
            {
                return printGrDetailing ??
                  (printGrDetailing = new RelayCommand(obj =>
                  {

                      if (ViewGrDetailings != null) 
                      {
                          MessageBox.Show("Деталізація :" + ViewGrDetailings[0].nameGrDetailing.ToString());
                      }
                  })); 
            }
        }

        //  команда открытия окна груп детализации и выбора новой группы для  добавления новой строки
        private RelayCommand? addListGroupDelit;
        public RelayCommand AddListGroupDelit
        {
            get
            {
                return addListGroupDelit ??
                  (addListGroupDelit = new RelayCommand(obj =>
                  { AddComandListGroupDelit(); }));
            }
        }

        private void AddComandListGroupDelit()
        {
            SelectGroupDelit();
        }

        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? addGroupQualification;
        public RelayCommand AddGroupQualification
        {
            get
            {
                return addGroupQualification ??
                  (addGroupQualification = new RelayCommand(obj =>
                  { AddComandGroupQualification(); }));
            }
        }

        private void AddComandGroupQualification()
        {
            WinNsiGrQualification NewOrder = new WinNsiGrQualification();
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();

        }

        // команда выбора групы детализации
        RelayCommand? selectedListGrDetailing;
        public RelayCommand SelectedListGrDetailing
        {
            get
            {
                return selectedListGrDetailing ??
                  (selectedListGrDetailing = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      SelectGroupDelit();
                  }));
            }
        }

        public void SelectGroupDelit()
        {

            NewEkzemplyarGrDetailing();
            IndexAddEdit = "";
            MapOpisViewModel.ActCompletedInterview = "NameDeteling";
            WinNsiListGroupDelit NewOrder = new WinNsiListGroupDelit();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            if (WindowMen.Detailingt4.Text.Length != 0)
            {
                string jason = controlerGrDetailing + "0/" + WindowMen.Detailingt4.Text + "/0";
                CallServer.PostServer(controlerGrDetailing, jason, "GETID");

                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]") == false)
                { 
                    ObservableViewGrDeliting(CmdStroka);
                    _nameGrDetailing = WindowMen.GrDetailingst2.Text;
                }
                else
                {
                    ViewGrDetailings = new ObservableCollection<ModelGrDetailing>();
                    WindowMen.GrDetailingsTablGrid.ItemsSource = ViewGrDetailings;
                }
            }
            loadboolGrDeliting = true;
            TrueNameGrDetailing();
            if (IndexAddEdit == "addCommand") SelectNewGrDetailing();
        }


        
        // команда выбора групы детализации
        RelayCommand? selectNameGrDeteling;
        public RelayCommand SelectNameGrDeteling
        {
            get
            {
                return selectNameGrDeteling ??
                  (selectNameGrDeteling = new RelayCommand(obj =>
                  {
                      if (WindowMen.GrDetailingsTablGrid.SelectedIndex >=0)
                      {
                          selectedViewGrDeliting = ViewGrDetailings[WindowMen.GrDetailingsTablGrid.SelectedIndex];
                          GrDetailingSelectedIndex = WindowMen.GrDetailingsTablGrid.SelectedIndex;
                          nameGrDetailing = ViewGrDetailings[WindowMen.GrDetailingsTablGrid.SelectedIndex].keyGrDetailing;
 
                      }
                  }));
            }
        }

        #endregion
        #endregion

    }
}
