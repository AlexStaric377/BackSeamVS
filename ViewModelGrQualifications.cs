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
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        /// 
        private WinNsiGrQualification WindowNsiGrQua = MainWindow.LinkMainWindow("WinNsiGrQualification");
        private MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
        private static bool loadboolQualifications = false;
        public static string controlerViewQualification =  "/api/QualificationController/";
        public ModelQualification selectedViewQualification;

        public static ObservableCollection<ModelQualification> ViewQualifications { get; set; }
        private string edittextViewQualification = "";

        public ModelQualification SelectedViewQualification
        { get { return selectedViewQualification; } set { selectedViewQualification = value; OnPropertyChanged("SelectedViewQualification"); } }

        public static void ObservableViewQualification(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelQualification>(CmdStroka);
            List<ModelQualification> res = result.ViewQualification.ToList();
            ViewQualifications = new ObservableCollection<ModelQualification>((IEnumerable<ModelQualification>)res);
            WindowMen.QualificationTablGrid.ItemsSource = ViewQualifications;
            loadboolQualifications = true;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>
 


        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadViewQualification;
        public RelayCommand LoadViewQualification
        {
            get
            {
                return loadViewQualification ??
                  (loadViewQualification = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadQualification();
                  }));
            }
        }


        // команда добавления нового объекта
        bool activViewQualification = false;
        private RelayCommand addViewQualification;
        public RelayCommand AddViewQualification
        {
            get
            {
                return addViewQualification ??
                  (addViewQualification = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComViewQualification(); }));
            }
        }

        private void AddComViewQualification()
        {
            if (loadboolQualifications == false)
            {
                SelectedViewQualification = new ModelQualification();
                selectedViewQualification = new ModelQualification();
                MethodLoadQualification();
            } 
            MethodAddQualification();

        }

        private void MethodAddQualification()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activViewQualification == false)
            { 
                BoolTrueQualification();
                if(WindowMen.Qualificationt4.Text != "" && ViewQualifications != null) TrueNameGRQualification();
                SelectNewQualification();
            } 
            else BoolFalseQualification();


        }
        private void TrueNameGRQualification()
        {
            Windowmain.Qualificationt4.Text = Windowmain.GrDetailingst4.Text;
            WindowMen.GrQuaFolder.IsEnabled = false;

        }

        

        private void MethodLoadQualification()
        {
            WindowMen.QualificationLoad.Visibility = Visibility.Hidden;
            SelectedViewQualification = new ModelQualification();
            selectedViewQualification = new ModelQualification();
            CallServer.PostServer(controlerViewQualification, controlerViewQualification, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewQualification(CmdStroka);

        }
 
        
        private void BoolTrueQualification()
        {
            WindowMen.Qualificationt2.IsEnabled = true;
            WindowMen.Qualificationt2.Background = Brushes.AntiqueWhite;
            if(IndexAddEdit == "addCommand") WindowMen.GrQuaFolder.Visibility = Visibility.Visible;
            activViewQualification = true;
            activeditViewQualification = true;
            WindowMen.QualificationTablGrid.IsEnabled = false;
        }

        private void BoolFalseQualification()
        {
            WindowMen.Qualificationt2.IsEnabled = false;
            WindowMen.Qualificationt2.Background = Brushes.White;
            activViewQualification = false;
            activeditViewQualification = false;
            WindowMen.GrQuaFolder.Visibility = Visibility.Hidden;
            WindowMen.GrQuaFolder.IsEnabled = true;
            WindowMen.QualificationTablGrid.IsEnabled = true;
            SelectedViewQualification = new ModelQualification();
            selectedViewQualification = new ModelQualification();
        }

        // команда удаления
        private RelayCommand? removeViewQualification;
        public RelayCommand RemoveViewQualification
        {
            get
            {
                return removeViewQualification ??
                  (removeViewQualification = new RelayCommand(obj =>
                  {
                      if (selectedViewQualification != null)
                      {
                          if (selectedViewQualification.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerViewQualification + selectedViewQualification.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ViewQualifications.Remove(selectedViewQualification);
                              BoolFalseQualification();
                              WindowMen.QualificationTablGrid.SelectedItem = null;
                          }
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewQualifications != null));
            }
        }


        // команда  редактировать
        private bool activeditViewQualification = false;
        ModelQualification tmpViewQualification = new ModelQualification();
        private RelayCommand? editViewQualification;
        public RelayCommand? EditViewQualification
        {
            get
            {
                return editViewQualification ??
                  (editViewQualification = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (selectedViewQualification != null)
                      {
                          if (selectedViewQualification.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          if (activeditViewQualification == false )
                          {
                              BoolTrueQualification();
                              edittextViewQualification = WindowMen.Qualificationt2.Text.ToString();
                          }
                          else
                          {
                              BoolFalseQualification();
                              WindowMen.QualificationTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveViewQualification;
        public RelayCommand SaveViewQualification
        {
            get
            {
                return saveViewQualification ??
                  (saveViewQualification = new RelayCommand(obj =>
                  {
                      string json = "";
                     
                          if (WindowMen.Qualificationt2.Text.Trim().Length != 0)
                          {
                            if(selectedViewQualification == null ) selectedViewQualification = new ModelQualification();
                            selectedViewQualification.nameQualification = WindowMen.Qualificationt2.Text;
                              if (IndexAddEdit == "addCommand")
                              {
                                    if (WindowMen.Qualificationt4.Text != "") SelectNewQualification();
                                    json = JsonConvert.SerializeObject(selectedViewQualification);
                                  CallServer.PostServer(controlerViewQualification, json, "POST");
                                  CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                  json = CallServer.ResponseFromServer;
                                  ModelQualification Idinsert = JsonConvert.DeserializeObject<ModelQualification>(CallServer.ResponseFromServer);
                                  int Countins = ViewQualifications != null ? ViewQualifications.Count : 0;
                                  ViewQualifications.Insert(Countins, Idinsert);
                                  SelectedViewQualification = Idinsert;
                              }
                              else
                              {
                                  json = JsonConvert.SerializeObject(selectedViewQualification);
                                  CallServer.PostServer(controlerViewQualification, json, "PUT");
                                  CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                  json = CallServer.ResponseFromServer;
                              }
                              UnloadCmdStroka("Qualification/", json);
                          }                      
                          else { WindowMen.Qualificationt2.Text = edittextGroupQualification; }
                       WindowMen.QualificationTablGrid.SelectedItem = null;
                      BoolFalseQualification();
                      IndexAddEdit = "";

                  }));
            }
        }

        private void SelectNewQualification()
        {

            // KodGroupQualification = "AAA.000", KodQualification = "AAA.000.001"

            int _keyQuaindex = 0 , lenghkod = 0;
            string stringkod = "";
            if (WindowMen.Qualificationt4.Text != "")
            { 

                if (selectedViewQualification == null) selectedViewQualification = new ModelQualification();
                selectedViewQualification.kodGroupQualification = WindowMen.Qualificationt4.Text.Substring(0, WindowMen.Qualificationt4.Text.IndexOf(":"));
                string _keyGroupQua = selectedViewQualification.kodGroupQualification;
                foreach (ModelQualification modelGroupQua in ViewQualifications)
                {
                    if (_keyGroupQua == modelGroupQua.kodGroupQualification)
                    {
                        lenghkod = modelGroupQua.kodQualification.Length - modelGroupQua.kodQualification.LastIndexOf(".") - 1;
                        stringkod = modelGroupQua.kodQualification.Substring(modelGroupQua.kodQualification.LastIndexOf(".") + 1, lenghkod);
                        if (_keyQuaindex < Convert.ToInt32(stringkod))
                        {
                            _keyQuaindex = Convert.ToInt32(stringkod);
                        }
                    }
                }
                _keyQuaindex++;
                string _repl = "000";
                _repl = _repl.Length - _keyQuaindex.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - _keyQuaindex.ToString().Length) : "";
                selectedViewQualification.kodQualification = selectedViewQualification.kodGroupQualification + "." + _repl + _keyQuaindex.ToString();
                WindowMen.Qualificationt1.Text = selectedViewQualification.kodQualification;
                selectedViewQualification.idUser = RegIdUser;            
            }

        }
        // команда печати
        RelayCommand? printViewQualification;
        public RelayCommand PrintViewQualification
        {
            get
            {
                return printViewQualification ??
                  (printViewQualification = new RelayCommand(obj =>
                  {

                      if (ViewQualifications != null)
                      {
                          MessageBox.Show("Групові квалифікації :" + ViewQualifications[0].nameQualification.ToString());
                      }
                  },
                 (obj) => ViewQualifications != null));
            }
        }

        //  команда открытия окна груп детализации и выбора новой группы для  добавления новой строки
        private RelayCommand? addQualificationGroup;
        public RelayCommand AddQualificationGroup
        {
            get
            {
                return addQualificationGroup ??
                  (addQualificationGroup = new RelayCommand(obj =>
                  { AddComandQualificationGroup(); }));
            }
        }

        private void AddComandQualificationGroup()
        {
            MapOpisViewModel.ActCompletedInterview = "Admin";
            WinNsiGrQualification NewOrder = new WinNsiGrQualification();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) - 150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            WindowMen.Qualificationt4.Text += ":  " + WindowMen.Featuret3.Text;
            MapOpisViewModel.ActCompletedInterview = "";

        }
        private RelayCommand? addNameGrQualification;
        public RelayCommand AddNameGrQualification
        {
            get
            {
                return addNameGrQualification ??
                  (addNameGrQualification = new RelayCommand(obj =>
                  {
                      CallServer.PostServer(controlerGroupQualification, controlerGroupQualification+ WindowMen.Qualificationt4.Text+"/0", "GETID");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      if (CallServer.ResponseFromServer.Length != 0)
                      {
                          ModelGroupQualification Idinsert = JsonConvert.DeserializeObject<ModelGroupQualification>(CallServer.ResponseFromServer);
                          WindowMen.Qualificationt4.Text += ":  "+ Idinsert.nameGroupQualification;
                      }
           
                  }));
            }
        }

        
        // команда выбора групы детализации
        RelayCommand? selectedListGrQualification;
        public RelayCommand SelectedListGrQualification
        {
            get
            {
                return selectedListGrQualification ??
                  (selectedListGrQualification = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      SelectGroupQualification();
                  }));
            }
        }

        public void SelectGroupQualification()
        {
            MapOpisViewModel.ActCompletedInterview = "Admin";
            WinNsiGrQualification NewOrder = new WinNsiGrQualification();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) - 150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            if (WindowMen.Qualificationt4.Text.Length != 0)
            {
                string jason = controlerViewQualification + "0/" + WindowMen.Qualificationt4.Text + "/0";
                CallServer.PostServer(controlerViewQualification, jason, "GETID");

                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]") == false) ObservableViewQualification(CmdStroka);
                else ViewQualifications = new  ObservableCollection<ModelQualification>();
            }
        }

        // команда выбора групы детализации
        RelayCommand? viewGrQualifications;
        public RelayCommand ViewGrQualifications
        {
            get
            {
                return viewGrQualifications ??
                  (viewGrQualifications = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.ActCompletedInterview == "Admin")
                      {
                          if (SelectedViewQualification != null)
                          {
                              Windowmain.Qualificationt4.Text = SelectedViewQualification.kodQualification.ToString();
                              Windowmain.GrDetailingst4.Text = SelectedViewQualification.kodQualification.ToString() + ":  " + SelectedViewQualification.nameQualification.ToString();
                              Windowmain.Featuret3.Text = ":           " + SelectedViewQualification.nameQualification.ToString();
                              WindowNsiGrQua.Close();
                          }
                      }


                  }));
            }
        }
        

        #endregion
        #endregion

    }
}
