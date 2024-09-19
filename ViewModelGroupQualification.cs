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
        // GroupQualificationViewModel модель ViewGroupQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        //public static MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
        public static string[] dictiontyQu = { "YAAA", "YBBB", "YCCC", "YDDD", "YEEE", "YFFF", "YGGG", "YHHH", "YIII", "YJJJ", "YKKK", "YLLL", "YMMM", "YNNN", "YOOO", "YPPP", "YQQQ", "YRRR", "YSSS", "YTTT", "YUUU", "YVVV", "YWWW", "YXXX", "YYYY", "YZZZ", "YABC", "YBCD", "YCDE", "YDEF" };
        bool activgrqualification = false, loadboolGroupQua = false;
        public static string controlerGroupQualification =  "/api/GroupQualificationController/";
        public static ModelGroupQualification selectedGroupQualification;

        public static ObservableCollection<ModelGroupQualification> ViewGroupQualifications { get; set; }
        private string edittextGroupQualification = "";

        public ModelGroupQualification SelectedViewGroupQualification
        { get { return selectedGroupQualification; } set { selectedGroupQualification = value; OnPropertyChanged("SelectedViewGroupQualification"); } }

        public static void ObservableViewGroupQualification(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGroupQualification>(CmdStroka);
            List<ModelGroupQualification> res = result.ViewGroupQualification.ToList();
            ViewGroupQualifications = new ObservableCollection<ModelGroupQualification>((IEnumerable<ModelGroupQualification>)res);
            WindowMen.GrQualificationTablGrid.ItemsSource = ViewGroupQualifications;
            WindowMen.PoiskGrQualification.IsEnabled = true;
            WindowMen.PoiskGrQualification.Background = Brushes.AntiqueWhite;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadGrQualification;
        public RelayCommand LoadGrQualification
        {
            get
            {
                return loadGrQualification ??
                  (loadGrQualification = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadGroupQua();
                  }));
            }
        }

        // команда добавления нового объекта
        private RelayCommand addGrQualification;
        public RelayCommand AddGrQualification
        {
            get
            {
                return addGrQualification ??
                  (addGrQualification = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return; AddComQualification();
                  }));
            }
        }

        private void AddComQualification()
        {
            if (loadboolGroupQua == false) MethodloadGroupQua();
            MethodaddcomGroupQua();
        }

        private void MethodaddcomGroupQua()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activgrqualification == false) BoolTrueGrQualification();
            else BoolFalseGrQualification();
            WindowMen.GrQualificationTablGrid.SelectedItem = null;

        }
        private void MethodloadGroupQua()
        {
            loadboolGroupQua = true;
            WindowMen.QualificationGroup.Visibility = Visibility.Hidden;
            
            CallServer.PostServer(controlerGroupQualification, controlerGroupQualification, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewGroupQualification(CmdStroka);
 
        }

        private void BoolTrueGrQualification()
        {
            activeditQualification = true;
            activgrqualification = true;
            WindowMen.GrQualificationt2.IsEnabled = true;
            WindowMen.GrQualificationt2.Background = Brushes.AntiqueWhite;
            WindowMen.GrQualificationTablGrid.IsEnabled = false;
        }

        private void BoolFalseGrQualification()
        {
            WindowMen.GrQualificationt2.IsEnabled = false;
            WindowMen.GrQualificationt2.Background = Brushes.White;
            activgrqualification = false;
            activeditQualification = false;
            WindowMen.GrQualificationTablGrid.IsEnabled = true;
        }
        // команда удаления
        private RelayCommand? removeGrQualification;
        public RelayCommand RemoveGrQualification
        {
            get
            {
                return removeGrQualification ??
                  (removeGrQualification = new RelayCommand(obj =>
                  {
                      if (selectedGroupQualification != null)
                      {
                          if (selectedGroupQualification.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerGroupQualification + selectedGroupQualification.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ViewGroupQualifications.Remove(selectedGroupQualification);
                              selectedGroupQualification = new ModelGroupQualification();
                              BoolFalseGrQualification();
                              IndexAddEdit = "";
                          }
                      }
                      
                  },
                 (obj) => ViewGroupQualifications !=null));
            }
        }


        // команда  редактировать
        private bool activeditQualification = false;
        private RelayCommand? editGrQualification;
        public RelayCommand? EditGrQualification
        {
            get
            {
                return editGrQualification ??
                  (editGrQualification = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (selectedGroupQualification != null)
                      {
                          if (selectedGroupQualification.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          if (activeditQualification == false)
                          {
                              BoolTrueGrQualification();
                              edittextGroupQualification = WindowMen.GrQualificationt2.Text.ToString();
                          }
                          else
                          { 
                              BoolFalseGrQualification();
                              WindowMen.GrQualificationTablGrid.SelectedItem = null;
                          } 
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveGrQualification;
        public RelayCommand SaveGrQualification
        {
            get
            {
                return saveGrQualification ??
                  (saveGrQualification = new RelayCommand(obj =>
                  {
                      string json = "";
                      BoolFalseGrQualification();
                        if (WindowMen.GrQualificationt2.Text.Trim().Length != 0)
                        {
                            if (IndexAddEdit == "addCommand")
                            {
                                SelectNewGrQualification();
                                json = JsonConvert.SerializeObject(selectedGroupQualification);
                                CallServer.PostServer(controlerGroupQualification, json, "POST");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                json = CallServer.ResponseFromServer;
                                ModelGroupQualification Idinsert = JsonConvert.DeserializeObject<ModelGroupQualification>(CallServer.ResponseFromServer);
                                int CountInsert = ViewGroupQualifications !=null ? ViewGroupQualifications.Count : 0;
                                ViewGroupQualifications.Insert(CountInsert, Idinsert);
                                SelectedViewGroupQualification = Idinsert;
                            }
                            else 
                            {
                                json = JsonConvert.SerializeObject(selectedGroupQualification);
                                CallServer.PostServer(controlerGroupQualification, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("ListGroupQualification/", json);
                      }
                        else { WindowMen.GrQualificationt2.Text = edittextGroupQualification; }
                      WindowMen.GrQualificationTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));
            }
        }

        public void SelectNewGrQualification()
        {

            //KodDetailing= "A.000.001.001", KeyFeature = "A.000.001", KeyComplaint = "A.000"
            // KodDetailing = "AA.000.001", KeyGrDetailing = "AA.000" 
            // KodGroupQualification = "AAA.000", NameGroupQualification = "Кваліфікація погоди"
            string _kodGroupQualification = "AAA.000";
            if (selectedGroupQualification == null) selectedGroupQualification = new ModelGroupQualification();
            if (ViewGroupQualifications != null) _kodGroupQualification = dictiontyQu[ViewGroupQualifications.Count] + ".000";
            selectedGroupQualification.kodGroupQualification = _kodGroupQualification;
            selectedGroupQualification.nameGroupQualification = WindowMen.GrQualificationt2.Text;
            selectedGroupQualification.idUser = RegIdUser;
        }

        // команда печати
        RelayCommand? printGrQualification;
        public RelayCommand PrintGrQualification
        {
            get
            {
                return printGrQualification ??
                  (printGrQualification = new RelayCommand(obj =>
                  {

                      if (ViewGroupQualifications != null)
                      {
                          MessageBox.Show("Групи квалифікації :" + ViewGroupQualifications[0].nameGroupQualification.ToString());
                      }
                  },
                 (obj) => ViewGroupQualifications !=null));
            }
        }

        

        RelayCommand? visibleGrQualification;
        public RelayCommand VisibleGrQualification
        {
            get
            {
                return visibleGrQualification ??
                  (visibleGrQualification = new RelayCommand(obj =>
                  {
                       WindowMen.FolderGrQua.Visibility = Visibility.Visible;
                  },
                 (obj) => ViewGroupQualifications != null));
            }
        }

        // команда открытия окна содержащего группу детализации характера жалобы  
        private RelayCommand? viewGrQualification;
        public RelayCommand ViewGrQualification
        {
            get
            {
                return viewGrQualification ??
                  (viewGrQualification = new RelayCommand(obj =>
                  {
                      if (selectedGroupQualification != null)
                      {
                          string pathcontroller =  "/api/QualificationController/"; 
                          string jason = pathcontroller + "0/" + selectedGroupQualification.kodGroupQualification;
                          CallServer.PostServer(pathcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]") == false)
                          {
                              MapOpisViewModel.ActCompletedInterview = "Qualification";
                              WinNsiQualification NewNsi = new WinNsiQualification();
                              NewNsi.Left = (MainWindow.ScreenWidth / 2);
                              NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                              NewNsi.ShowDialog();
                              MapOpisViewModel.ActCompletedInterview = null;

                          }

                      }

                  }));
            }
        }

        
        // команда поиска групы квалификации
        RelayCommand? searchGrQualification;
        public RelayCommand SearchGrQualification
        {
            get
            {
                return searchGrQualification ??
                  (searchGrQualification = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      if (WindowMen.PoiskGrQualification.Text.Trim() != "")
                        {
                            string jason = controlerGroupQualification + "0/" + WindowMen.PoiskGrQualification.Text;
                            CallServer.PostServer(controlerGroupQualification, jason, "GETID");
                            string CmdStroka = CallServer.ServerReturn();
                            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                            else ObservableViewGroupQualification(CmdStroka);
                        }
                  }));
            }
        }

        #endregion
        #endregion


    }
}
