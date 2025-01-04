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
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {

        // ViewAccountUsers  Справочник учетных записей пользователей
        // клавиша в окне:  Облікові записи


        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowStatusUser = MainWindow.LinkNameWindow("BackMain");
        private bool editboolStatusUser = false, addboolStatusUser = false, loadboolStatusUser = false;
        private string edittextStatusUser = "";
        public static string pathcontStatusUser =  "/api/NsiStatusUserController/";
        private static NsiStatusUser selectedStatusUser;
 


        public static ObservableCollection<NsiStatusUser> ViewStatustUsers { get; set; }
 
        public NsiStatusUser SelectedStatusUser
        {
            get { return selectedStatusUser; }
            set { selectedStatusUser = value; OnPropertyChanged("SelectedStatusUser"); }
        }
        public static void ObservableViewStatusUsers(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListNsiStatusUser>(CmdStroka);
            List<NsiStatusUser> res = result.NsiStatusUser.ToList();
            ViewStatustUsers = new ObservableCollection<NsiStatusUser>((IEnumerable<NsiStatusUser>)res);
            WindowStatusUser.StatusUserTablGrid.ItemsSource = ViewStatustUsers;

        }



        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadStatustUser;
        public RelayCommand LoadStatustUser
        {
            get
            {
                return loadStatustUser ??
                  (loadStatustUser = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadNsiStatusUser();
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addNsiStatusUser;
        public RelayCommand AddNsiStatusUser
        {
            get
            {
                return addNsiStatusUser ??
                  (addNsiStatusUser = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandNsiStatusUser(); }));
            }
        }

        private void AddComandNsiStatusUser()
        {
            selectedStatusUser = new NsiStatusUser();
            SelectedStatusUser = selectedStatusUser;
            if (loadboolAccountUser == false)
            { 
                MethodLoadNsiStatusUser();
                if (boolSetAccountUser == false) return;
            } 
            MethodaddcomNsiStatusUser();
        }

        private void MethodaddcomNsiStatusUser()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolStatusUser == false) BoolTrueNsiStatusUser();
            else BoolFalseNsiStatusUser();
            WindowStatusUser.StatusUserTablGrid.SelectedItem = null;

        }

        private void MethodLoadNsiStatusUser()
        {
            RegStatusUser = "Адміністратор";
            WindowStatusUser.LoadStatusUser.Visibility = Visibility.Hidden;
            //if (boolSetAccountUser == false)
            //{ if (RegSetAccountUser() == false) return; }
            
            CallServer.PostServer(pathcontStatusUser, pathcontStatusUser, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))CallServer.BoolFalseTabl();
            else ObservableViewStatusUsers(CmdStroka);
            boolSetAccountUser = true;
        }


        private void BoolTrueNsiStatusUser()
        {
            addboolStatusUser = true;
            editboolStatusUser = true;
            WindowStatusUser.StatusUsert1.IsEnabled = true;
            WindowStatusUser.StatusUsert1.Background = Brushes.AntiqueWhite;
            WindowStatusUser.StatusUsert4.IsEnabled = true;
            WindowStatusUser.StatusUsert4.Background = Brushes.AntiqueWhite;
            if (IndexAddEdit == "addCommand")
            {
                WindowStatusUser.StatusUsert3.IsEnabled = true;
                WindowStatusUser.StatusUsert3.Background = Brushes.AntiqueWhite;
            }
        }

        private void BoolFalseNsiStatusUser()
        {
            addboolStatusUser = false;
            editboolStatusUser = false;
            WindowStatusUser.StatusUsert1.IsEnabled = false;
            WindowStatusUser.StatusUsert1.Background = Brushes.White;
            WindowStatusUser.StatusUsert4.IsEnabled = false;
            WindowStatusUser.StatusUsert4.Background = Brushes.White;
            WindowStatusUser.StatusUsert3.IsEnabled = false;
            WindowStatusUser.StatusUsert3.Background = Brushes.White;

        }

        // команда удаления
        private RelayCommand? removeNsiStatusUser;
        public RelayCommand RemoveNsiStatusUser
        {
            get
            {
                return removeNsiStatusUser ??
                  (removeNsiStatusUser = new RelayCommand(obj =>
                  {
                      if (selectedStatusUser != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathcontStatusUser + selectedStatusUser.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ViewStatustUsers.Remove(selectedStatusUser);
                          }
                      }
                      BoolFalseNsiStatusUser();
                  },
                 (obj) => ViewStatustUsers != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editNsiStatusUser;
        public RelayCommand? EditNsiStatusUser
        {
            get
            {
                return editNsiStatusUser ??
                  (editNsiStatusUser = new RelayCommand(obj =>
                  {
                      if (selectedStatusUser != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolStatusUser == false)
                          {
                              BoolTrueNsiStatusUser();
                              edittextStatusUser = WindowStatusUser.StatusUsert1.Text.ToString();
                          }
                          else
                          {
                              BoolFalseNsiStatusUser();
                              WindowStatusUser.StatusUserTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveNsiStatusUser;
        public RelayCommand SaveNsiStatusUser
        {
            get
            {
                return saveNsiStatusUser ??
                  (saveNsiStatusUser = new RelayCommand(obj =>
                  {
                      BoolFalseNsiStatusUser();
                      if (WindowStatusUser.StatusUsert1.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewNsiStatusUser();
                              string json = JsonConvert.SerializeObject(selectedStatusUser);
                              CallServer.PostServer(pathcontStatusUser, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              NsiStatusUser Idinsert = JsonConvert.DeserializeObject<NsiStatusUser>(CallServer.ResponseFromServer);
                              if (ViewStatustUsers == null)
                              {
                                  ViewStatustUsers = new ObservableCollection<NsiStatusUser>();
                                  ViewStatustUsers.Add(Idinsert);
                              }
                              else
                              { ViewStatustUsers.Insert(ViewStatustUsers.Count, Idinsert); }
                              SelectedStatusUser = Idinsert;
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedStatusUser);
                              CallServer.PostServer(pathcontStatusUser, json, "PUT");
                          }
                      }
                      else WindowStatusUser.StatusUsert2.Text = edittextStatusUser;
                      WindowStatusUser.StatusUserTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));

            }
        }

        public void SelectNewNsiStatusUser()
        {

            if (selectedStatusUser == null) selectedStatusUser = new NsiStatusUser();
            if (ViewStatustUsers != null)
            {
                int _keyStatusUserindex = 0, setindex = 0;
                _keyStatusUserindex = Convert.ToInt32(ViewStatustUsers[0].idStatus.Substring(ViewStatustUsers[0].idStatus.LastIndexOf(".") + 1, ViewStatustUsers[0].idStatus.Length - (ViewStatustUsers[0].idStatus.LastIndexOf(".") + 1)));
                for (int i = 0; i < ViewStatustUsers.Count; i++)
                {
                    setindex = Convert.ToInt32(ViewStatustUsers[i].idStatus.Substring(ViewStatustUsers[i].idStatus.LastIndexOf(".") + 1, ViewStatustUsers[i].idStatus.Length - (ViewStatustUsers[i].idStatus.LastIndexOf(".") + 1)));
                    if (_keyStatusUserindex < setindex) _keyStatusUserindex = setindex;
                }
                _keyStatusUserindex++;
                selectedStatusUser.idStatus = _keyStatusUserindex.ToString();
            }
            else { selectedStatusUser.idStatus = "1"; }

            selectedStatusUser.kodDostupa = WindowDetailing.StatusUsert1.Text.ToString();
            selectedStatusUser.nameStatus = WindowDetailing.StatusUsert3.Text.ToString();
            selectedStatusUser.statusUser = WindowDetailing.StatusUsert4.Text.ToString();

        }

        // команда печати
        RelayCommand? printNsiStatusUser;
        public RelayCommand PrintNsiStatusUser
        {
            get
            {
                return printNsiStatusUser ??
                  (printNsiStatusUser = new RelayCommand(obj =>
                  {
                      WindowStatusUser.StatusUserTablGrid.SelectedItem = null;
                      if (ViewStatustUsers != null)
                      {
                          MessageBox.Show("Обліковий запис :" + ViewStatustUsers[0].nameStatus.ToString());
                      }
                  },
                 (obj) => ViewStatustUsers != null));
            }
        }


        #endregion
        #endregion
    }
}
