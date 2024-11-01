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
using System.Diagnostics;

namespace BackSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        // ViewModelDoctor  Справочник докторов
        // клавиша в окне:  Профілі лікарів

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Профілі лікарів"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех карточек описания доктора из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowDoctor = MainWindow.LinkNameWindow("BackMain");
        private bool editboolDoctor = false, addboolDoctor = false, loadboolDoctor = false;
        public static string pathcontrolerDoctor = "/api/ApiControllerDoctor/";
        private static string pathcontrolerMedZaklad = "/api/MedicalInstitutionController/";
        private ModelDoctor selectedDoctor;
        private static ModelGridDoctor selectedGridDoctor;
        public static string CallViewDoctor = "";
        public static ObservableCollection<ModelDoctor> ViewDoctors { get; set; }
        public static ObservableCollection<ModelGridDoctor> ViewGridDoctors { get; set; }
        public ModelDoctor SelectedDoctor
        {
            get { return selectedDoctor; }
            set { selectedDoctor = value; OnPropertyChanged("SelectedDoctor"); }
        }

        public ModelGridDoctor SelectedGridDoctor
        {
            get { return selectedGridDoctor; }
            set { selectedGridDoctor = value; OnPropertyChanged("SelectedGridDoctor"); }
        }
        public static void ObservableViewDoctors(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            ViewDoctors = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);
            IndexAddEdit = "";
            MetodLoadGridDoctor();
        }

        public static void MetodLoadGridDoctor()
        {
            ViewGridDoctors = new ObservableCollection<ModelGridDoctor>();
            foreach (ModelDoctor modelDoctor in ViewDoctors)
            {
                selectedGridDoctor = new ModelGridDoctor();
                selectedGridDoctor.kodDoctor = modelDoctor.kodDoctor;
                selectedGridDoctor.id = modelDoctor.id;
                selectedGridDoctor.name = modelDoctor.name;
                selectedGridDoctor.surname = modelDoctor.surname;
                selectedGridDoctor.specialnoct = modelDoctor.specialnoct;
                selectedGridDoctor.telefon = modelDoctor.telefon;
                selectedGridDoctor.email = modelDoctor.email;
                selectedGridDoctor.edrpou = modelDoctor.edrpou;
                selectedGridDoctor.uriwebDoctor = modelDoctor.uriwebDoctor;
                selectedGridDoctor.napryamok = modelDoctor.napryamok;
                if (modelDoctor.edrpou != "")
                {
                    string json = pathcontrolerMedZaklad + modelDoctor.edrpou.ToString() + "/0/0"; //
                    CallServer.PostServer(pathcontrolerMedZaklad, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    MedicalInstitution Idinsert = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectedGridDoctor.nameZaklad = Idinsert.name;
                        selectedGridDoctor.adrZaklad = Idinsert.adres;
                        selectedGridDoctor.pind = Idinsert.postIndex;
                    }
                }
                ViewGridDoctors.Add(selectedGridDoctor);
            }
            WindowDoctor.DoctorTablGrid.ItemsSource = ViewGridDoctors;
        }

        // команда закрытия окна
        RelayCommand? checkLikarTel;
        public RelayCommand CheckLikarTel
        {
            get
            {
                return checkLikarTel ??
                  (checkLikarTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Doctort6, 12);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilLikarTel;
        public RelayCommand CheckProfilLikarTel
        {
            get
            {
                return checkProfilLikarTel ??
                  (checkProfilLikarTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Likart6, 12);
                  }));
            }
        }
        // команда закрытия окна
        RelayCommand? checkLikarPind;
        public RelayCommand CheckLikarPind
        {
            get
            {
                return checkLikarPind ??
                  (checkLikarPind = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Doctort5, 5);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilLikarPind;
        public RelayCommand CheckProfilLikarPind
        {
            get
            {
                return checkProfilLikarPind ??
                  (checkProfilLikarPind = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Likart5, 5);
                  }));
            }
        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadDoctor;
        public RelayCommand LoadDoctor
        {
            get
            {
                return loadDoctor ??
                  (loadDoctor = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadDoctor();  
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addDoctor;
        public RelayCommand AddDoctor
        {
            get
            {
                return addDoctor ??
                  (addDoctor = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandDoctor(); }));
            }
        }

        private void AddComandDoctor()
        {
            if (loadboolDoctor == false) MethodLoadDoctor();
            MethodaddcomDoctor();
        }

        private void MethodaddcomDoctor()
        {
            MainWindow WindowDoc = MainWindow.LinkNameWindow("BackMain");
            selectedDoctor = new ModelDoctor();
            WindowDoc.DoctorTablGrid.SelectedItem = null;
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolDoctor == false)
            {
                SelectedGridDoctor = new ModelGridDoctor();
                BoolTrueDoctor();
            }
            else BoolFalseDoctor();


        }

        private void MethodLoadDoctor()
        {
            loadboolDoctor = true;
            WindowDoctor.Loadlikar.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerDoctor, pathcontrolerDoctor, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewDoctors(CmdStroka);
        }


        private void BoolTrueDoctor()
        {
            addboolDoctor = true;
            editboolDoctor = true;
            WindowDoctor.Doctort10.IsEnabled = true;
            WindowDoctor.Doctort10.Background = Brushes.AntiqueWhite;
            WindowDoctor.Doctort2.IsEnabled = true;
            WindowDoctor.Doctort2.Background = Brushes.AntiqueWhite;
            WindowDoctor.Doctort3.IsEnabled = true;
            WindowDoctor.Doctort3.Background = Brushes.AntiqueWhite;
            WindowDoctor.Doctort5.IsEnabled = true;
            WindowDoctor.Doctort5.Background = Brushes.AntiqueWhite;
            WindowDoctor.Doctort6.IsEnabled = true;
            WindowDoctor.Doctort6.Background = Brushes.AntiqueWhite;
            WindowDoctor.Doctort7.IsEnabled = true;
            WindowDoctor.Doctort7.Background = Brushes.AntiqueWhite;

            WindowDoctor.DoctortBoxUri.IsEnabled = true;
            WindowDoctor.DoctortBoxUri.Background = Brushes.AntiqueWhite;
            WindowDoctor.FolderDoc5.Visibility = Visibility.Visible;
            WindowDoctor.FolderWebUriLikar.Visibility = Visibility.Visible;
            WindowDoctor.FolderLikarGrDia.Visibility = Visibility.Visible;
            WindowDoctor.DoctorTablGrid.IsEnabled = false;
        }

        private void BoolFalseDoctor()
        {
            addboolDoctor = false;
            editboolDoctor = false;
            WindowDoctor.Doctort10.IsEnabled = false;
            WindowDoctor.Doctort10.Background = Brushes.White;
            WindowDoctor.Doctort2.IsEnabled = false;
            WindowDoctor.Doctort2.Background = Brushes.White;
            WindowDoctor.Doctort3.IsEnabled = false;
            WindowDoctor.Doctort3.Background = Brushes.White;
            WindowDoctor.Doctort5.IsEnabled = false;
            WindowDoctor.Doctort5.Background = Brushes.White;
            WindowDoctor.Doctort6.IsEnabled = false;
            WindowDoctor.Doctort6.Background = Brushes.White;
            WindowDoctor.Doctort7.IsEnabled = false;
            WindowDoctor.Doctort7.Background = Brushes.White;
 
            WindowDoctor.DoctortBoxUri.IsEnabled = false;
            WindowDoctor.DoctortBoxUri.Background = Brushes.White;
            WindowDoctor.FolderDoc5.Visibility = Visibility.Hidden;
            WindowDoctor.FolderWebUriLikar.Visibility = Visibility.Hidden;
            WindowMedical.FolderLikarGrDia.Visibility = Visibility.Hidden;
            WindowDoctor.DoctorTablGrid.IsEnabled = true;

        }

        // команда удаления
        private RelayCommand? removeDoctor;
        public RelayCommand RemoveDoctor
        {
            get
            {
                return removeDoctor ??
                  (removeDoctor = new RelayCommand(obj =>
                  {
                      if (selectedGridDoctor != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              selectedDoctor = ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex];
                              MetodRemovePrifilLikar(selectedDoctor.kodDoctor);

                              ViewDoctors.Remove(selectedDoctor);
                              selectedDoctor = new ModelDoctor();
                              ViewGridDoctors.Remove(selectedGridDoctor);
                              selectedGridDoctor = new ModelGridDoctor();
                              WindowDoctor.Doctort8.Text = "";
                              BoolFalseDoctor();
                              WindowDoctor.DoctorTablGrid.SelectedItem = null;
                          }

                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewDoctors != null));
            }
        }


        public static void MetodRemovePrifilLikar(string kodLikar = "")
        {

            string json = pathcontrolerDoctor + "0/" + kodLikar;
            CallServer.PostServer(pathcontrolerDoctor, json, "DELETE");

            // удаление Жизни пациента и взаимодействие с врачами LifePacient
            json = controlerLifePacient + "0/0/" + kodLikar;
            CallServer.PostServer(controlerLifePacient, json, "DELETE");

            // удаление пациентов записавшихся на прием  у доктора LifeDoctor
            json = controlerLifeDoctor + "0/0/" + kodLikar;
            CallServer.PostServer(controlerLifeDoctor, json, "DELETE");

            // удаление пациентов записавшихся на прием RegistrationAppointment
            json = pathcontrollerAppointment + "0/0/" + kodLikar;
            CallServer.PostServer(pathcontrollerAppointment, json, "DELETE");

            // удаление список приемов пациентов записавшихся на прием у доктора AdmissionPatients
            json = pathcontrolerReceptionPacient + "0/0/" + kodLikar;
            CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");

            // удаление проведеных интервью ColectionInterview
            json = ColectioncontrollerIntevPacient + "0/0/" + kodLikar;
            CallServer.PostServer(ColectioncontrollerIntevPacient, json, "DELETE");

            // удаление  учетной записи AccountUser
            json = pathcontrolerAccountUser + "0/" + kodLikar;
            CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");

            // удаление  учетной записи LikarGrupDiagnoz
            json = controlerLikarGrDiagnoz + "0/" + kodLikar;
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "DELETE");

        }



        // команда  редактировать
        private RelayCommand? editDoctor;
        public RelayCommand? EditDoctor
        {
            get
            {
                return editDoctor ??
                  (editDoctor = new RelayCommand(obj =>
                  {
                      if (selectedGridDoctor != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolDoctor == false)
                          {
                              selectedDoctor = ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex];
                              BoolTrueDoctor();
                          }
                          else
                          {
                              BoolFalseDoctor();
                              WindowDoctor.DoctorTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveDoctor;
        public RelayCommand SaveDoctor
        {
            get
            {
                return saveDoctor ??
                  (saveDoctor = new RelayCommand(obj =>
                  {
                      string json = "";

                      if (WindowDoctor.Doctort10.Text.Length != 0 && WindowDoctor.Doctort2.Text.Length != 0)
                      {

                          selectedDoctor.name = WindowDoctor.Doctort10.Text.ToString();
                          selectedDoctor.surname = WindowDoctor.Doctort2.Text.ToString();
                          selectedDoctor.specialnoct = WindowDoctor.Doctort3.Text.ToString();
                          selectedDoctor.telefon = WindowDoctor.Doctort6.Text.ToString();
                          selectedDoctor.email = WindowDoctor.Doctort7.Text.ToString();
                          selectedDoctor.napryamok = WindowDoctor.DoctorNaprTextBox.Text.ToString();
                          selectedDoctor.uriwebDoctor = WindowDoctor.DoctortBoxUri.Text.ToString();
                          if (IndexAddEdit == "addCommand")
                          {
                              SelectNewDoctor();
                              json = JsonConvert.SerializeObject(selectedDoctor);
                              CallServer.PostServer(pathcontrolerDoctor, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                              ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                              if (ViewDoctors == null) ViewDoctors.Add(Idinsert);
                              else { ViewDoctors.Insert(ViewDoctors.Count, Idinsert); }
                              SelectedDoctor = Idinsert;
                          }
                          else
                          {

                              json = JsonConvert.SerializeObject(selectedDoctor);
                              CallServer.PostServer(pathcontrolerDoctor, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                          }
                          UnloadCmdStroka("Doctor/", json);
                      }
                      SelectedGridDoctor = new ModelGridDoctor();
                      WindowDoctor.DoctorTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseDoctor();

                  }));

            }
        }

        public void SelectNewDoctor()
        {
            if (selectedDoctor == null) selectedDoctor = new ModelDoctor();
            if (ViewGridDoctors == null) ViewGridDoctors = new ObservableCollection<ModelGridDoctor>();
            if (IndexAddEdit == "addCommand")
            {
                if (ViewDoctors != null)
                {
                    int _keyDoctorindex = 0, setindex = 0;
                    _keyDoctorindex = Convert.ToInt32(ViewDoctors[0].kodDoctor.Substring(ViewDoctors[0].kodDoctor.LastIndexOf(".") + 1, ViewDoctors[0].kodDoctor.Length - (ViewDoctors[0].kodDoctor.LastIndexOf(".") + 1)));
                    for (int i = 0; i < ViewDoctors.Count; i++)
                    {
                        setindex = Convert.ToInt32(ViewDoctors[i].kodDoctor.Substring(ViewDoctors[i].kodDoctor.LastIndexOf(".") + 1, ViewDoctors[i].kodDoctor.Length - (ViewDoctors[i].kodDoctor.LastIndexOf(".") + 1)));
                        if (_keyDoctorindex < setindex) _keyDoctorindex = setindex;
                    }
                    _keyDoctorindex++;
                    string _repl = "000000000";
                    selectedDoctor.kodDoctor = "DTR." + _repl.Substring(0, _repl.Length - _keyDoctorindex.ToString().Length) + _keyDoctorindex.ToString();
                }
                else
                {
                    ViewDoctors = new ObservableCollection<ModelDoctor>();
                    selectedDoctor.kodDoctor = "DTR.000000001";
                }

            }

            selectedGridDoctor = new ModelGridDoctor();
            selectedGridDoctor.kodDoctor = selectedDoctor.kodDoctor;
            selectedGridDoctor.id = selectedDoctor.id;
            selectedGridDoctor.name = selectedDoctor.name;
            selectedGridDoctor.surname = selectedDoctor.surname;
            selectedGridDoctor.specialnoct = selectedDoctor.specialnoct;
            selectedGridDoctor.telefon = selectedDoctor.telefon;
            selectedGridDoctor.email = selectedDoctor.email;
            selectedGridDoctor.edrpou = selectedDoctor.edrpou;
            selectedGridDoctor.nameZaklad = WindowDoctor.Doctort9.Text.ToString();
            selectedGridDoctor.pind = WindowDoctor.Doctort5.Text.ToString();
            selectedGridDoctor.adrZaklad = WindowDoctor.Doctort4.Text.ToString();
            selectedGridDoctor.uriwebDoctor = selectedDoctor.uriwebDoctor;
            selectedGridDoctor.napryamok = selectedDoctor.napryamok;
            if (IndexAddEdit == "addCommand") ViewGridDoctors.Add(selectedGridDoctor);
            else ViewGridDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex] = selectedGridDoctor;
            WindowDoctor.DoctorTablGrid.ItemsSource = ViewGridDoctors;


            if (_kodDoctor == "")
            {

                if (ViewModelLikarGrupDiagnoz.LikarGrupDiagnozs.Count != 0)
                {
                    foreach (ModelLikarGrupDiagnoz modelLikarGrupDiagnoz in ViewModelLikarGrupDiagnoz.LikarGrupDiagnozs)
                    {
                        modelLikarGrupDiagnoz.kodDoctor = selectedDoctor.kodDoctor;
                        string json = JsonConvert.SerializeObject(modelLikarGrupDiagnoz);
                        CallServer.PostServer(controlerLikarGrDiagnoz, json, "POST");
                    }

                }
            }
        }

        // команда печати
        RelayCommand? printDoctor;
        public RelayCommand PrintDoctor
        {
            get
            {
                return printDoctor ??
                  (printDoctor = new RelayCommand(obj =>
                  {
                      WindowMen.DetailingTablGrid.SelectedItem = null;
                      if (ViewDoctors != null)
                      {
                          MessageBox.Show("Ім'я та прізвище лікаря :" + ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex].name.ToString() + " " + ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex].surname.ToString());
                      }
                  },
                 (obj) => ViewDoctors != null));
            }
        }

        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? addMedzaklad;
        public RelayCommand AddMedzaklad
        {
            get
            {
                return addMedzaklad ??
                  (addMedzaklad = new RelayCommand(obj =>
                  { AddComandAddMedzaklad(); }));
            }
        }

        private void AddComandAddMedzaklad()
        {
            MainWindow.UrlServer = pathcontrolerMedZaklad;
            CallServer.PostServer(MainWindow.UrlServer, pathcontrolerMedZaklad, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                CallServer.BoolFalseTabl();
                return;
            }

            WinNsiMedZaklad NewOrder = new WinNsiMedZaklad();
            NewOrder.Left = 600;
            NewOrder.Top = 200;
            NewOrder.ShowDialog();

            if (ViewModelNsiMedZaklad.selectedMedZaklad != null)
            {
                selectedDoctor.edrpou = ViewModelNsiMedZaklad.selectedMedZaklad.edrpou;
                WindowDoctor.Doctort9.Text = ViewModelNsiMedZaklad.selectedMedZaklad.name;
                WindowDoctor.Doctort5.Text = ViewModelNsiMedZaklad.selectedMedZaklad.postIndex;
                WindowDoctor.Doctort4.Text = ViewModelNsiMedZaklad.selectedMedZaklad.adres;
            }

        }

        // команда загрузки сайта
        private RelayCommand? gridProfilWebUriLikar;
        public RelayCommand GridProfilWebUriLikar
        {
            get
            {
                return gridProfilWebUriLikar ??
                  (gridProfilWebUriLikar = new RelayCommand(obj =>
                  {

                      if (ViewDoctors != null)
                      {
                          if (WindowDoctor.DoctorTablGrid.SelectedIndex >= 0)
                          {
                              if (ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex].uriwebDoctor != null)
                              {
                                  string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                                  string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                                  string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                                  Process Rungoogle = new Process();
                                  Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                                  Rungoogle.StartInfo.Arguments = selectedDoctor.uriwebDoctor;
                                  //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                                  Rungoogle.StartInfo.UseShellExecute = false;
                                  Rungoogle.EnableRaisingEvents = true;
                                  Rungoogle.Start();
                              }
                          }


                      }

                  }));
            }
        }

        // команда открытия видимости кнопки загрузки сайта
        private RelayCommand? visibleFolderUriWebLikar;
        public RelayCommand VisibleFolderUriWebLikar
        {
            get
            {
                return visibleFolderUriWebLikar ??
                  (visibleFolderUriWebLikar = new RelayCommand(obj =>
                  {
                      if (WindowDoctor.DoctorTablGrid.SelectedIndex >= 0)
                      {
                          selectedDoctor = ViewDoctors[WindowDoctor.DoctorTablGrid.SelectedIndex];
                          WindowMedical.FolderWebUriLikar.Visibility = Visibility.Visible;
                          WindowMedical.FolderLikarGrDia.Visibility = Visibility.Visible;
                          _kodDoctor = selectedDoctor.kodDoctor;


                      }

                  }));
            }
        }

        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? listLikarGrupDiagnoz;
        public RelayCommand ListLikarGrupDiagnoz
        {
            get
            {
                return listLikarGrupDiagnoz ??
                  (listLikarGrupDiagnoz = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.ActCompletedInterview = "NameGrDiagnoz";
                      WinLikarGrupDiagnoz Order = new WinLikarGrupDiagnoz();
                      Order.Left = (MainWindow.ScreenWidth / 2)-50;
                      Order.Top = (MainWindow.ScreenHeight / 2) - 350;
                      Order.ShowDialog();
                      MapOpisViewModel.ActCompletedInterview = "";
                  }));
            }
        }




        // Выбор фамилии доктора
        private RelayCommand? searchDoctor;
        public RelayCommand SearchDoctor
        {
            get
            {
                return searchDoctor ??
                  (searchDoctor = new RelayCommand(obj =>
                  {
                      MetodSearchDoctor();
                  }));
            }


        }

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyDoctor;
        public RelayCommand CheckKeyDoctor
        {
            get
            {
                return checkKeyDoctor ??
                  (checkKeyDoctor = new RelayCommand(obj =>
                  {
                      MetodSearchDoctor();
                  }));
            }
        }


        private void MetodSearchDoctor()
        {
            if (CheckStatusUser() == false) return;
            if (WindowDoctor.PoiskDoctor.Text.Trim() != "")
            {
                string jason = pathcontrolerDoctor + "0/0/" + WindowDoctor.PoiskDoctor.Text;
                CallServer.PostServer(pathcontrolerDoctor, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewDoctors(CmdStroka);
            }

        }
        #endregion
        #endregion
    }
}
