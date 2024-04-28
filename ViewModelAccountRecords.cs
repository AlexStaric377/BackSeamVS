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
    class ViewModelAccountRecords : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private WinAccountRecords WindowAccount = MainWindow.LinkNameWindow("AccountRecords");
        public static bool On1OffEye = false, Reestr1OnOff = false, On2OffEye = false, Reestr2OnOff = false;
        public static string Password1Text = "", Password2Text = "", _telefon ="", _fio="";
        private static string pathcontrolerAccountUser = "/api/AccountUserController/";
        private string pathcontroller = "/api/PacientController/";
        private string pathcontrolerProfilLikar = "/api/ApiControllerDoctor/";
        private static ModelAccountUser selectedModelAccountUser;
        public static ModelDoctor selectProfilLikar;
        public ModelAccountUser SelectedModelAccountUser
        {
            get { return selectedModelAccountUser; }
            set { selectedModelAccountUser = value; OnPropertyChanged("SelectedModelAccountUser"); }
        }

        public ModelDoctor SelectProfilLikar
        {
            get { return selectProfilLikar; }
            set { selectProfilLikar = value; OnPropertyChanged("SelectProfilLikar"); }
        }

        public ViewModelAccountRecords()
        {
            switch (MapOpisViewModel.CallViewProfilLikar )
            {
                case  "ProfilLikar":
                selectProfilLikar = MapOpisViewModel.selectedProfilLikar;
                break;
                case "PacientProfil":
                    selectProfilLikar = new ModelDoctor();
                    if (ViewModelRegisterAccountUser._GhangePaswUser == false)
                    {
                        selectProfilLikar.name = MapOpisViewModel.selectedProfilPacient.name;
                        selectProfilLikar.surname = MapOpisViewModel.selectedProfilPacient.surname;
                        selectProfilLikar.telefon = MapOpisViewModel.selectedProfilPacient.tel;
                        
                    }
                    else { selectProfilLikar.telefon = ViewModelRegisterAccountUser.AccountTel; }

                    break;
            }
            
 
        }



        // команда закрытия окна
        RelayCommand? closeAccount;
        public RelayCommand CloseAccountRecords
        {
            get
            {
                return closeAccount ??
                  (closeAccount = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.boolSetAccountUser = false;
                      WindowAccount.Close();
                  }));
            }
        }


        // команда ввода пароля
        RelayCommand? password1OnOff;
        public RelayCommand Password1OnOff
        {
            get
            {
                return password1OnOff ??
                  (password1OnOff = new RelayCommand(obj =>
                  {
                      if (On1OffEye == false)
                      {
                          Password1Text = WindowAccount.Passw1.Password;
                          WindowAccount.Passw1Text.Text = Password1Text;
                      }
                      else
                      {
                          Password1Text = WindowAccount.Passw1Text.Text;
                          WindowAccount.Passw1.Password = WindowAccount.Passw1Text.Text;
                      }
                  }));
            }
        }


        // команда изменения состояния видимости текста пароля
        RelayCommand? eye1OnOff;
        public RelayCommand Eye1OnOff
        {
            get
            {
                return eye1OnOff ??
                  (eye1OnOff = new RelayCommand(obj =>
                  {
                      if (On1OffEye == false)
                      {
                          WindowAccount.Passw1Text.Visibility = Visibility.Visible;
                          WindowAccount.Passw1.Visibility = Visibility.Hidden;
                          On1OffEye = true;
                          WindowAccount.Eye1.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis1.Visibility = Visibility.Hidden;
                      }
                      else
                      {
                          WindowAccount.Passw1Text.Visibility = Visibility.Hidden;
                          WindowAccount.Passw1.Visibility = Visibility.Visible;
                          On1OffEye = false;
                          WindowAccount.Eye1.Visibility = Visibility.Hidden;
                          WindowAccount.EyeDis1.Visibility = Visibility.Visible;
                      }
                  }));
            }
        }

        // команда ввода пароля
        RelayCommand? password2OnOff;
        public RelayCommand Password2OnOff
        {
            get
            {
                return password2OnOff ??
                  (password2OnOff = new RelayCommand(obj =>
                  {
                      if (On2OffEye == false)
                      {
                          Password2Text = WindowAccount.Passw.Password;
                          WindowAccount.PasswText.Text = Password2Text;
                      }
                      else
                      {
                          Password2Text = WindowAccount.PasswText.Text;
                          WindowAccount.Passw.Password = WindowAccount.PasswText.Text;
                      }
                  }));
            }
        }


        // команда изменения состояния видимости текста пароля
        RelayCommand? eye2OnOff;
        public RelayCommand Eye2OnOff
        {
            get
            {
                return eye2OnOff ??
                  (eye2OnOff = new RelayCommand(obj =>
                  {
                      if (On2OffEye == false)
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Visible;
                          WindowAccount.Passw.Visibility = Visibility.Hidden;
                          On2OffEye = true;
                          WindowAccount.Eye.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis.Visibility = Visibility.Hidden;
                      }
                      else
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Hidden;
                          WindowAccount.Passw.Visibility = Visibility.Visible;
                          On2OffEye = false;
                          WindowAccount.Eye.Visibility = Visibility.Hidden;
                          WindowAccount.EyeDis.Visibility = Visibility.Visible;
                      }
                  }));
            }
        }

        
        // команда закрытия окна
        RelayCommand? saveAccountRecords;
        public RelayCommand SaveAccountRecords
        {
            get
            {
                return saveAccountRecords ??
                  (saveAccountRecords = new RelayCommand(obj =>
                  {

                      if (WindowAccount.Passw1Text.Text != WindowAccount.PasswText.Text)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                            "Перший та повторний паролі не співпадають." + Environment.NewLine +
                            "Необхідно повторити введення паролю.";
                          MapOpisViewModel.SelectedFalseLogin();
                          return;
                      }
                      if (ViewModelRegisterAccountUser._GhangePaswUser == true)
                      {
                          GhangePasswordUser();
                          return;
                      }
                      selectedModelAccountUser = new ModelAccountUser();
                      //NewAccountUser();
                      selectedModelAccountUser.id = 0;
                      selectedModelAccountUser.idUser = MapOpisViewModel.CallViewProfilLikar == "ProfilLikar" ? MapOpisViewModel.selectedProfilLikar.kodDoctor : MapOpisViewModel.selectedProfilPacient.kodPacient;
                      selectedModelAccountUser.login = selectProfilLikar.telefon; //_telefon+
                      selectedModelAccountUser.password = WindowAccount.Passw1Text.Text.ToString();
                      selectedModelAccountUser.idStatus = MapOpisViewModel.CallViewProfilLikar == "ProfilLikar"?"3":"2";

                      string json = MapOpisViewModel.pathcontrolerStatusUser + selectedModelAccountUser.idStatus.ToString();
                      CallServer.PostServer(MapOpisViewModel.pathcontrolerStatusUser, json, "GETID");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      NsiStatusUser Idinsert = JsonConvert.DeserializeObject<NsiStatusUser>(CallServer.ResponseFromServer);
                      if (Idinsert != null)
                      {
                          selectedModelAccountUser.kodDostupa = Idinsert.kodDostupa;
                          selectedModelAccountUser.nameStatus = Idinsert.nameStatus;

                      }
                      json = JsonConvert.SerializeObject(selectedModelAccountUser);
                      CallServer.PostServer(pathcontrolerAccountUser, json, "POST");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      AccountUser Insert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
                      if (Insert != null)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                          "Ваш профіль збережено, обліковий запис успішно створено." + Environment.NewLine +
                          "Відтепер ви маєте свій кабінет і вхід до нього за вашим логіном та паролем. ";
                          MapOpisViewModel.SelectedFalseLogin();
                          ViewModelRegisterAccountUser.ReestrOnOff = true;
                          MapOpisViewModel.saveboolAccountLikar = true;
                      }
                      else
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                          "Профіль не створено. За консультацією зверніться до системного адміністратора";
                          MapOpisViewModel.SelectedFalseLogin();
                          ViewModelRegisterAccountUser.ReestrOnOff = false;
                      }
                      
                      WindowAccount.Close();
                  }));
            }
        }

        private void GhangePasswordUser()
        {

            string json = "";
            json = pathcontrolerAccountUser +"0/"+ ViewModelRegisterAccountUser.CountryKod +ViewModelRegisterAccountUser.AccountTel+"/0";
            CallServer.PostServer(pathcontrolerAccountUser, json, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            AccountUser Idinsert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
            if (Idinsert != null)
            {
                Idinsert.password = WindowAccount.Passw1Text.Text.ToString();
                json = JsonConvert.SerializeObject(Idinsert);
                CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                AccountUser Insert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine +
                    "Ваш пароль змінено, обліковий запис успішно модифіковано." + Environment.NewLine +
                    "Відтепер ви реєструєте вхід до кабінету з новим паролем. ";
                    MapOpisViewModel.SelectedFalseLogin();
                }
                else
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine +
                    "Пароль не змінено. Невірно задано логін користувача";
                    MapOpisViewModel.SelectedFalseLogin();
                }

            }
            else
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Пароль не змінено. Невірно задано логін користувача";
                MapOpisViewModel.SelectedFalseLogin();
            }
        }
        public void NewAccountUser()
        {
            CallServer.PostServer(pathcontrolerAccountUser, pathcontrolerAccountUser, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) selectedModelAccountUser.idUser = "CNT.0000000001";
            else
            {
                var result = JsonConvert.DeserializeObject<ListAccountUser>(CmdStroka);
                List<AccountUser> res = result.AccountUser.ToList();
                MapOpisViewModel.ViewAccountUsers = new ObservableCollection<AccountUser>((IEnumerable<AccountUser>)res);
                int _keyAccountUserindex = 0, setindex = 0;
                _keyAccountUserindex = Convert.ToInt32(MapOpisViewModel.ViewAccountUsers[0].idUser.Substring(MapOpisViewModel.ViewAccountUsers[0].idUser.LastIndexOf(".") + 1, MapOpisViewModel.ViewAccountUsers[0].idUser.Length - (MapOpisViewModel.ViewAccountUsers[0].idUser.LastIndexOf(".") + 1)));
                for (int i = 0; i < MapOpisViewModel.ViewAccountUsers.Count; i++)
                {
                    setindex = Convert.ToInt32(MapOpisViewModel.ViewAccountUsers[i].idUser.Substring(MapOpisViewModel.ViewAccountUsers[i].idUser.LastIndexOf(".") + 1, MapOpisViewModel.ViewAccountUsers[i].idUser.Length - (MapOpisViewModel.ViewAccountUsers[i].idUser.LastIndexOf(".") + 1)));
                    if (_keyAccountUserindex < setindex) _keyAccountUserindex = setindex;
                }
                _keyAccountUserindex++;
                string _repl = "0000000000";
                selectedModelAccountUser.idUser = "CNT." + _repl.Substring(0, _repl.Length - _keyAccountUserindex.ToString().Length) + _keyAccountUserindex.ToString();

            }

        }
        
        public static List<string> Countrys { get; set; } = new List<string> { "+380", "+44" };

        private string _SelectedCountry;
        public string SelectedCountry
        {
            get => _SelectedCountry;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedCountry = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCountry)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewCountry(_SelectedCountry);
            }
        }

        public void SetNewCountry(string selected = "")
        {
            ViewModelAccountRecords WindowMen = MainWindow.LinkMainWindow("ViewModelAccountRecords");
            _telefon = selected == "0" ? "+380" : "+44";
        }
    }
}
