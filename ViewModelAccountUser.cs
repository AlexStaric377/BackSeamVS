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
        public  static MainWindow WindowAccountUser = MainWindow.LinkNameWindow("BackMain");
        private bool editboolAccountUser = false, addboolAccountUser = false, loadboolAccountUser = false;
        private string edittextAccountUser = "", SetIdStatus = "";
        public static string pathcontrolerAccountUser =  "/api/AccountUserController/";
        public static string pathcontrolerStatusUser =  "/api/NsiStatusUserController/";
        public static string pathcontrolernsiPacient =  "/api/PacientController/";
        public static string pathcontrolernsiLikar =  "/api/ApiControllerDoctor/";
        public static ModelAccountUser selectedModelAccountUser;
        public static AccountUser selectedAccountUser;
        
        public static ObservableCollection<ModelAccountUser> ViewModelAccountUsers { get; set; }
        public static ObservableCollection<AccountUser> ViewAccountUsers { get; set; }
        public ModelAccountUser SelectedModelAccountUser
        {
            get { return selectedModelAccountUser; }
            set { selectedModelAccountUser = value; OnPropertyChanged("SelectedModelAccountUser"); }
        }

        public AccountUser SelectedAccountUser
        {
            get { return selectedAccountUser; }
            set { selectedAccountUser = value; OnPropertyChanged("SelectedAccountUser"); }
        }
        public static void ObservableViewAccountUsers(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListAccountUser>(CmdStroka);
            List<AccountUser> res = result.AccountUser.ToList();
            ViewAccountUsers = new ObservableCollection<AccountUser>((IEnumerable<AccountUser>)res);
            MethodLoadModelAccountUser();

        }

        private static void MethodLoadModelAccountUser()
        {
            ViewModelAccountUsers = new ObservableCollection<ModelAccountUser>();
            foreach (AccountUser accountUser in ViewAccountUsers)
            {
                selectedModelAccountUser = new ModelAccountUser();
                selectedModelAccountUser.id = accountUser.id;
                selectedModelAccountUser.idStatus = accountUser.idStatus;
                selectedModelAccountUser.idUser = accountUser.idUser;
                selectedModelAccountUser.login = accountUser.login;
                selectedModelAccountUser.password = accountUser.password;
                string json = "";
                if (accountUser.idStatus == null)
                {
                    json = pathcontrolerAccountUser + accountUser.id;
                    CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");

                }
                else
                {
                    json = pathcontrolerStatusUser + accountUser.idStatus.ToString();
                    CallServer.PostServer(pathcontrolerStatusUser, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    NsiStatusUser Idinsert = JsonConvert.DeserializeObject<NsiStatusUser>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectedModelAccountUser.kodDostupa = Idinsert.kodDostupa;
                        selectedModelAccountUser.nameStatus = Idinsert.nameStatus;
                    }
                    ViewModelAccountUsers.Add(selectedModelAccountUser);
                }
                if (accountUser.accountCreatDate == null)
                {
                    accountUser.accountCreatDate = DateTime.Now.ToShortDateString();
                    json = JsonConvert.SerializeObject(accountUser);
                    CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                }
                selectedModelAccountUser.accountCreatDate = accountUser.accountCreatDate;
                if (accountUser.subscription == null)
                {
                    accountUser.subscription = "false";
                    json = JsonConvert.SerializeObject(accountUser);
                    CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                }
                selectedModelAccountUser.subscription = accountUser.subscription;
            }
            WindowAccountUser.AccountUserTablGrid.ItemsSource = ViewModelAccountUsers;
        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadAccountUser;
        public RelayCommand LoadAccountUser
        {
            get
            {
                return loadAccountUser ??
                  (loadAccountUser = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadAccountUser();  
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addAccountUser;
        public RelayCommand AddAccountUser
        {
            get
            {
                return addAccountUser ??
                  (addAccountUser = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandAccountUser(); }));
            }
        }

        private void AddComandAccountUser()
        {
            selectedAccountUser = new AccountUser();
            SelectedAccountUser = selectedAccountUser;
            if (loadboolAccountUser == false) MethodLoadAccountUser();
            MethodaddcomAccountUser();
        }

        private void MethodaddcomAccountUser()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectedAccountUser = new AccountUser();
            if (addboolAccountUser == false) BoolTrueAccountUser();
            else BoolFalseAccountUser();
            WindowAccountUser.AccountUserTablGrid.SelectedItem = null;

        }

        private void MethodLoadAccountUser()
        {
            IndexAddEdit = "";
            loadboolAccountUser = true;
            RegStatusUser = "Адміністратор";

            WindowAccountUser.LoadAccountUser.Visibility = Visibility.Hidden;
            if (boolSetAccountUser == false)
            { if (RegSetAccountUser() == false) return;}
            
            
            CallServer.PostServer(pathcontrolerAccountUser, pathcontrolerAccountUser, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))CallServer.BoolFalseTabl();
            else ObservableViewAccountUsers(CmdStroka);
        }

        public static  bool RegSetAccountUser()
        {

            bool _return = false;
            WinRegisterAccountUser NewAccountUser = new WinRegisterAccountUser();
            NewAccountUser.ShowDialog();
            return _return =  boolSetAccountUser;

        }
        private void BoolTrueAccountUser()
        {
            addboolAccountUser = true;
            editboolAccountUser = true;
            WindowAccountUser.AccountUsert2.IsEnabled = true;
            WindowAccountUser.AccountUsert2.Background = Brushes.AntiqueWhite;
            WindowAccountUser.AccountUsert4.IsEnabled = true;
            WindowAccountUser.AccountUsert4.Background = Brushes.AntiqueWhite;
            if (IndexAddEdit == "addCommand" || IndexAddEdit == "editCommand" )
            { 
                WindowAccountUser.FoldAccountUser0.Visibility = Visibility.Visible;
                WindowAccountUser.FoldAccountUser1.Visibility = Visibility.Visible;
            } 
        }

        private void BoolFalseAccountUser()
        {
            addboolAccountUser = false;
            editboolAccountUser = false;
            WindowAccountUser.AccountUsert2.IsEnabled = false;
            WindowAccountUser.AccountUsert2.Background = Brushes.White;
            WindowAccountUser.AccountUsert4.IsEnabled = false;
            WindowAccountUser.AccountUsert4.Background = Brushes.White;
            WindowAccountUser.FoldAccountUser0.Visibility = Visibility.Hidden;
            WindowAccountUser.FoldAccountUser1.Visibility = Visibility.Hidden;

        }

        // команда удаления
        private RelayCommand? removeAccountUser;
        public RelayCommand RemoveAccountUser
        {
            get
            {
                return removeAccountUser ??
                  (removeAccountUser = new RelayCommand(obj =>
                  {

                      if (selectedModelAccountUser != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {

                              string json = pathcontrolerAccountUser + selectedModelAccountUser.id.ToString() + "/0";
                              CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");
                              selectedAccountUser = new AccountUser();
                              selectedAccountUser.id = selectedModelAccountUser.id;
                              ViewAccountUsers.Remove(selectedAccountUser);
                              ViewModelAccountUsers.Remove(selectedModelAccountUser);
                              selectedAccountUser = new AccountUser();
                              selectedModelAccountUser = new ModelAccountUser();

                          }

                      }
                      else 
                      {
                          WindowAccountUser.AccountUsert1.Text = "";
                          WindowAccountUser.AccountUsert2.Text = "";
                          WindowAccountUser.AccountUsert3.Text = "";
                          WindowAccountUser.AccountUsert4.Text = "";
                          WindowAccountUser.AccountUsert5.Text = "";

                      }
                      BoolFalseAccountUser();
                      IndexAddEdit = "";
                  },
                 (obj) => ViewAccountUsers != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editAccountUser;
        public RelayCommand? EditAccountUser
        {
            get
            {
                return editAccountUser ??
                  (editAccountUser = new RelayCommand(obj =>
                  {
                      if (selectedModelAccountUser != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolAccountUser == false)
                          {
                              BoolTrueAccountUser();
                              edittextAccountUser = WindowAccountUser.Detailingt2.Text.ToString();
                          }
                          else
                          {
                              BoolFalseAccountUser();
                              WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveAccountUser;
        public RelayCommand SaveAccountUser
        {
            get
            {
                return saveAccountUser ??
                  (saveAccountUser = new RelayCommand(obj =>
                  {
                      
                      if (WindowAccountUser.AccountUsert2.Text.Length != 0)
                      {
                          //selectedAccountUser.idStatus = WindowAccountUser.AccountUsert3.Text.ToString().Substring(0, WindowDetailing.AccountUsert3.Text.ToString().IndexOf(":"));
                          selectedAccountUser.login = WindowAccountUser.AccountUsert2.Text.ToString();
                          selectedAccountUser.password = WindowAccountUser.AccountUsert4.Text.ToString();
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewAccountUser();
                              string json = JsonConvert.SerializeObject(selectedAccountUser);
                              CallServer.PostServer(pathcontrolerAccountUser, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              AccountUser Idinsert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
                              if (ViewAccountUsers == null)
                              {
                                  ViewAccountUsers = new ObservableCollection<AccountUser>();
                                  ViewAccountUsers.Add(Idinsert);
                              } 
                              else
                              { ViewAccountUsers.Insert(ViewAccountUsers.Count, Idinsert);  }
                              SelectedAccountUser = Idinsert;
                              MethodLoadModelAccountUser();
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedAccountUser);
                              CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                          }
                      }
                      else WindowAccountUser.AccountUsert2.Text = edittextAccountUser;
                      WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseAccountUser();

                  }));

            }
        }

        public void SelectNewAccountUser()
        {
           if (selectedAccountUser == null) selectedAccountUser = new AccountUser();
           if (ViewAccountUsers != null)
           {
                if (WindowAccountUser.AccountUsert5.Text.Trim().Length == 0)
                {

                    int _keyAccountUserindex = 0, setindex = 0;
                    _keyAccountUserindex = Convert.ToInt32(ViewAccountUsers[0].idUser.Substring(ViewAccountUsers[0].idUser.LastIndexOf(".") + 1, ViewAccountUsers[0].idUser.Length - (ViewAccountUsers[0].idUser.LastIndexOf(".") + 1)));
                    for (int i = 0; i < ViewAccountUsers.Count; i++)
                    {
                        setindex = Convert.ToInt32(ViewAccountUsers[i].idUser.Substring(ViewAccountUsers[i].idUser.LastIndexOf(".") + 1, ViewAccountUsers[i].idUser.Length - (ViewAccountUsers[i].idUser.LastIndexOf(".") + 1)));
                        if (_keyAccountUserindex < setindex) _keyAccountUserindex = setindex;
                    }
                    _keyAccountUserindex++;
                    string _repl = "0000000000";
                    selectedAccountUser.idUser = "CNT." + _repl.Substring(0, _repl.Length - _keyAccountUserindex.ToString().Length) + _keyAccountUserindex.ToString();
 
                }
                else { selectedAccountUser.idUser = WindowAccountUser.AccountUsert5.Text.ToString().Substring(0, WindowAccountUser.AccountUsert5.Text.ToString().IndexOf(":")); }               
           }
           else { selectedAccountUser.idUser = "CNT.0000000001"; }

        }

        // команда печати
        RelayCommand? printAccountUser;
        public RelayCommand PrintAccountUser
        {
            get
            {
                return printAccountUser ??
                  (printAccountUser = new RelayCommand(obj =>
                  {
                      WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
                      if (ViewAccountUsers != null)
                      {
                          MessageBox.Show("Обліковий запис :" + ViewAccountUsers[0].idStatus.ToString());
                      }
                  },
                 (obj) => ViewAccountUsers != null));
            }
        }

        

        // команда открытия окна справочника статуса пользователя пациент врач
        private RelayCommand? addStatusUser;
        public RelayCommand AddStatusUser
        {
            get
            {
                return addStatusUser ??
                  (addStatusUser = new RelayCommand(obj =>
                  { AddComandAddStatusUser(); }));
            }
        }

        private void AddComandAddStatusUser()
        {
            MapOpisViewModel.CallViewProfilLikar = "WinNsiStatusUser";
            WinNsiStatusUser NewOrder = new WinNsiStatusUser();
            //NewOrder.Left = 600;
            //NewOrder.Top = 200;
            NewOrder.ShowDialog();
            MapOpisViewModel.CallViewProfilLikar = "";
            if (WindowAccountUser.AccountUsert3.Text.Trim().Length > 0)
            { 
                SetIdStatus = WindowAccountUser.AccountUsert3.Text.ToString().Substring(0, WindowDetailing.AccountUsert3.Text.ToString().IndexOf(":"));
                selectedAccountUser.idStatus = WindowAccountUser.AccountUsert3.Text.ToString().Substring(0, WindowDetailing.AccountUsert3.Text.ToString().IndexOf(":"));
                WindowDetailing.AccountUsert3.Text = WindowDetailing.AccountUsert3.Text.ToString().Substring(WindowDetailing.AccountUsert3.Text.ToString().IndexOf(":")+1, WindowDetailing.AccountUsert3.Text.Length- (WindowDetailing.AccountUsert3.Text.ToString().IndexOf(":")+1)).TrimStart();
            }

        }


        

        private RelayCommand? addIdUser;
        public RelayCommand AddIdUser
        {
            get
            {
                return addIdUser ??
                  (addIdUser = new RelayCommand(obj =>
                  {
                      switch (SetIdStatus)
                      {
                          case "2":
                              MapOpisViewModel.CallViewProfilLikar = "WinNsiPacient";
                              AddComandAddWinNsiPacient();
                              break;
                          case "3":
                              MapOpisViewModel.CallViewProfilLikar = "WinNsiLikar";
                              AddComandAddWinNsiLikar();
                            break;
                      }
                      MapOpisViewModel.CallViewProfilLikar = "";
                  }));
            }
        }

        private void AddComandAddWinNsiPacient()
        {
            WinNsiPacient NewOrder = new WinNsiPacient();
            NewOrder.ShowDialog();
            
        }

        private void AddComandAddWinNsiLikar()
        {
            WinNsiLikar NewOrder = new WinNsiLikar();
            NewOrder.ShowDialog();
        }

        
        private RelayCommand? addnameUser;
        public RelayCommand AddnameUser
        {
            get
            {
                return addnameUser ??
                  (addnameUser = new RelayCommand(obj =>
                  {
                      if (WindowAccountUser.AccountUserTablGrid.SelectedIndex >= 0)
                      { 
                          selectedModelAccountUser = ViewModelAccountUsers[WindowAccountUser.AccountUserTablGrid.SelectedIndex];
                          selectedAccountUser = ViewAccountUsers[WindowAccountUser.AccountUserTablGrid.SelectedIndex];
                          if (selectedModelAccountUser != null)
                          {
                              string json = "";
                              string Iduser = selectedModelAccountUser.idUser.ToString().Contains(":") ? selectedModelAccountUser.idUser.ToString().Substring(0, selectedModelAccountUser.idUser.ToString().IndexOf(":")) : selectedModelAccountUser.idUser.ToString();

                              switch (selectedModelAccountUser.idStatus)
                              {
                                  case "2":
                                      json = pathcontrolernsiPacient + Iduser + "/0/0/0/0";
                                      CallServer.PostServer(pathcontrolernsiPacient, json, "GETID");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                                      if (Idinsert != null) WindowAccountUser.AccountUsert5.Text = Idinsert.kodPacient + ": " + Idinsert.name + " " + Idinsert.surname;
                                      break;
                                  case "3":
                                      json = pathcontrolernsiLikar + Iduser + "/0/0";
                                      CallServer.PostServer(pathcontrolernsiLikar, json, "GETID");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                                      if (Insert != null) WindowAccountUser.AccountUsert5.Text = Insert.kodDoctor + ": " + Insert.name + " " + Insert.surname;
                                      break;
                              }

                          }
                      }
                      


                  }));
            }
        }

 
        // Выбор назви області
        private RelayCommand? searchAccountUser;
        public RelayCommand SearchAccountUser
        {
            get
            {
                return searchAccountUser ??
                  (searchAccountUser = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      if (WindowAccountUser.PoiskAccountUser.Text.Trim() != "")
                      {
                          string jason = pathcontrolerAccountUser + "0/0/0/" + WindowAccountUser.PoiskAccountUser.Text;
                          CallServer.PostServer(pathcontrolerAccountUser, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableViewAccountUsers(CmdStroka);
                      }

                  }));
            }
        }
        #endregion
        #endregion
    }
}
