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

        // ViewModelProfilLikar Справочник докторов
        // клавиша в окне:  Профіль лікаря в кабінеті лікаря

        #region Обработка событий и команд вставки, удаления и редектирования Профіль лікаря
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех карточек описания доктора из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowProfilDoctor = MainWindow.LinkNameWindow("BackMain");
        public static bool boolSetAccountUser = false, saveboolAccountLikar = false, boolVisibleMessage = false;
        public bool editboolProfilLikar = false, addboolProfilLikar = false, loadboolProfilLikar = false;
        private string pathcontrollerMedZaklad = "/api/MedicalInstitutionController/";
        private string pathcontrolerProfilLikar =  "/api/ApiControllerDoctor/";
        private static string pathcontrolerMedZakladProfilLikar =  "/api/MedicalInstitutionController/";
        public static ModelDoctor selectedProfilLikar;
        private static ModelGridDoctor selectedGridProfilLikar;
        public static string CallViewProfilLikar = "ProfilLikar";
        public static string _kodDoctor = "";
        public static ObservableCollection<ModelDoctor> ViewProfilLikars { get; set; }
        public static ObservableCollection<ModelGridDoctor> ViewGridProfilLikars { get; set; }
        public ModelDoctor SelectedProfilLikar
        {
            get { return selectedProfilLikar; }
            set { selectedProfilLikar = value; OnPropertyChanged("SelectedProfilLikar"); }
        }

        public ModelGridDoctor SelectedGridProfilLikar
        {
            get { return selectedGridProfilLikar; }
            set { selectedGridProfilLikar = value; OnPropertyChanged("SelectedGridProfilLikar"); }
        }
        public static void ObservableViewProfilLikars(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            ViewDoctors = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);
            IndexAddEdit = "";
            MetodLoadGridProfilLikar();
        }

        public static void NewAccountRecords()
        {
            saveboolAccountLikar = false;
            WinAccountRecords NewOrder = new WinAccountRecords();
            NewOrder.ShowDialog();
        }
        #endregion

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadProfilLikar;
        public RelayCommand LoadProfilLikar
        {
            get
            {
                return loadProfilLikar ??
                  (loadProfilLikar = new RelayCommand(obj =>
                  {
                      MetodLoadProfilLikar();
 
                  }));
            }
        }

        public void MetodLoadProfilLikar()
        {
            WindowMain.FolderLikarProfil.Visibility = Visibility.Visible;
            WindowProfilDoctor.BorderCabLikar.Visibility = Visibility.Hidden;
            WindowProfilDoctor.LikarLoadInf.Visibility = Visibility.Hidden;
            WindowProfilDoctor.LikarLoadinterv.Visibility = Visibility.Hidden;
            EdrpouMedZaklad = "";
            //RegSetAccountUser();
            SelectRegAccountUser();

            if (WindowProfilDoctor.AccountUsert5.Text != "")
            {
                addboolProfilLikar = true;
                _kodDoctor = ViewModelNsiLikar.selectedLikar.kodDoctor;
                selectedProfilLikar = ViewModelNsiLikar.selectedLikar;
                MetodLoadGridProfilLikar();
                SelectedGridProfilLikar = selectedGridProfilLikar;
                WindowProfilDoctor.LikarIntert2.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;
                WindowProfilDoctor.ReceptionPacient2.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;
            }
            else
            {
                WindowProfilDoctor.LikarLoadInf.Visibility = Visibility.Visible;
                WindowProfilDoctor.LikarLoadinterv.Visibility = Visibility.Visible;
            }
        }

        private void SelectRegAccountUser()
        {
            CallServer.PostServer(pathcontrollerMedZaklad, pathcontrollerMedZaklad, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                CallServer.BoolFalseTabl();
                return;
            }
            WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
            MedZaklad.ShowDialog();
            EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();


            if (EdrpouMedZaklad != "")
            {
                CallViewProfilLikar = "ProfilLikar";

                selectedProfilLikar = new ModelDoctor();
                WinNsiLikar NewOrder = new WinNsiLikar();
                NewOrder.ShowDialog();
                CallViewProfilLikar = "";
                if (MapOpisViewModel.nameDoctor != "")
                {
                    LoadInfoPacient("лікаря.");
                    if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();

                    modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
                    modelColectionInterview.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                    WindowIntevLikar.ReceptionLikar2.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
                    _kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                    ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                    WindowProfilDoctor.ReceptionPacientTablGrid.ItemsSource = ViewReceptionPacients;
                    WindowIntevLikar.CabinetNameMedZaklad.Text = LikarAppointments.Likart9.Text;
                    LikarAppointments.CabinetReseptionLikar.Text = modelColectionInterview.nameDoctor;
                    ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>();
                    LikarAppointments.CabinetReseptionPacientTablGrid.ItemsSource = ViewLikarAppointments;
                    SelectedViewModelLikarAppointments = new ViewModelVisitingDays();
                    ColectionInterviewIntevLikars = new ObservableCollection<ModelColectionInterview>();
                    WindowProfilDoctor.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;
                    WindowProfilDoctor.ReceptionPacient4.IsEnabled = false;
                    WindowProfilDoctor.ReceptionPacient4.Background = Brushes.White;
                    WindowProfilDoctor.ReceptionPacient7.IsEnabled = false;
                    WindowProfilDoctor.ReceptionPacient7.Background = Brushes.White;

                    boolVisibleMessage = true;
                    MethodLoadtableColectionIntevLikar();
                    MethodLoadReceptionLikar();
                    MethodLoadLikarAppointments();
                    MethodloadtablWorkDiagnoz();
                    MethodloadtablDiagnoz();

                    MessageWarning Info = MainWindow.LinkMainWindow("MessageWarning");
                    if (Info != null) Info.Close();
                    boolVisibleMessage = false;
                }

            }

        }


        public static void MetodLoadGridProfilLikar()
        {
            if (selectedProfilLikar != null)
            {
                if (selectedProfilLikar.id != 0)
                { 
                    ViewProfilLikars = new ObservableCollection<ModelDoctor>();
                    ViewProfilLikars.Add(selectedProfilLikar);
                    ViewGridProfilLikars = new ObservableCollection<ModelGridDoctor>();
                    foreach (ModelDoctor modelDoctor in ViewProfilLikars)
                    {
                        selectedGridProfilLikar = new ModelGridDoctor();
                        selectedGridProfilLikar.kodDoctor = modelDoctor.kodDoctor;
                        selectedGridProfilLikar.id = modelDoctor.id;
                        selectedGridProfilLikar.name = modelDoctor.name;
                        selectedGridProfilLikar.surname = modelDoctor.surname;
                        selectedGridProfilLikar.specialnoct = modelDoctor.specialnoct;
                        selectedGridProfilLikar.telefon = modelDoctor.telefon;
                        selectedGridProfilLikar.email = modelDoctor.email;
                        selectedGridProfilLikar.edrpou = modelDoctor.edrpou;
                        selectedGridProfilLikar.napryamok = modelDoctor.napryamok;
                        selectedGridProfilLikar.uriwebDoctor = modelDoctor.uriwebDoctor;
                        if (modelDoctor.edrpou != null)
                        {
                            MainWindow.UrlServer = pathcontrolerMedZaklad;
                            string json = pathcontrolerMedZakladProfilLikar + modelDoctor.edrpou.ToString()+"/0/0";
                            CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            MedicalInstitution Idinsert = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);
                            if (Idinsert != null)
                            {
                                selectedGridProfilLikar.nameZaklad = Idinsert.name;
                                selectedGridProfilLikar.adrZaklad = Idinsert.adres;
                                selectedGridProfilLikar.pind = Idinsert.postIndex;
                            }
                        }

                        ViewGridProfilLikars.Add(selectedGridProfilLikar);
                    }               
                }

            }

        }

        private void BoolTrueProfilLikar()
        {

            editboolProfilLikar = true;
            WindowProfilDoctor.Likart10.IsEnabled = true;
            WindowProfilDoctor.Likart10.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart2.IsEnabled = true;
            WindowProfilDoctor.Likart2.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart3.IsEnabled = true;
            WindowProfilDoctor.Likart3.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart5.IsEnabled = true;
            WindowProfilDoctor.Likart5.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart6.IsEnabled = true;
            WindowProfilDoctor.Likart6.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart7.IsEnabled = true;
            WindowProfilDoctor.Likart7.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.LikarNaprt3.IsEnabled = true;
            WindowProfilDoctor.LikarNaprt3.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.LikarUrit7.IsEnabled = true;
            WindowProfilDoctor.LikarUrit7.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.FolderDoc5.Visibility = Visibility.Visible;
            WindowProfilDoctor.FolderDocUri5.Visibility = Visibility.Visible;
            
        }

        private void BoolFalseProfilLikar()
        {
            addboolDoctor = false;
            editboolProfilLikar = false;
            WindowProfilDoctor.Likart10.IsEnabled = false;
            WindowProfilDoctor.Likart10.Background = Brushes.White;
            WindowProfilDoctor.Likart2.IsEnabled = false;
            WindowProfilDoctor.Likart2.Background = Brushes.White;
            WindowProfilDoctor.Likart3.IsEnabled = false;
            WindowProfilDoctor.Likart3.Background = Brushes.White;
            WindowProfilDoctor.Likart5.IsEnabled = false;
            WindowProfilDoctor.Likart5.Background = Brushes.White;
            WindowProfilDoctor.Likart6.IsEnabled = false;
            WindowProfilDoctor.Likart6.Background = Brushes.White;
            WindowProfilDoctor.Likart7.IsEnabled = false;
            WindowProfilDoctor.Likart7.Background = Brushes.White;
            WindowProfilDoctor.LikarNaprt3.IsEnabled = false;
            WindowProfilDoctor.LikarNaprt3.Background = Brushes.White;
            WindowProfilDoctor.LikarUrit7.IsEnabled = false;
            WindowProfilDoctor.LikarUrit7.Background = Brushes.White;
            WindowProfilDoctor.FolderDoc5.Visibility = Visibility.Hidden;
            WindowProfilDoctor.FolderDocUri5.Visibility = Visibility.Hidden;


        }

        // команда  редактировать
        private RelayCommand? editProfilLikar;
        public RelayCommand? EditProfilLikar
        {
            get
            {
                return editProfilLikar ??
                  (editProfilLikar = new RelayCommand(obj =>
                  {
                      
                      //if (selectedProfilLikar != null)
                      //{
                      //    IndexAddEdit = "editCommand";
                      //    if (editboolProfilLikar == false)
                      //    {
                      //        BoolTrueProfilLikar();
                      //    }
                      //    else
                      //    {
                      //        BoolFalseProfilLikar();
                      //        IndexAddEdit = "";
                      //    }
                      //}
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveProfilLikar;
        public RelayCommand SaveProfilLikar
        {
            get
            {
                return saveProfilLikar ??
                  (saveProfilLikar = new RelayCommand(obj =>
                  {
                      //BoolFalseProfilLikar();
                      //if (WindowProfilDoctor.Likart10.Text.Length != 0 && WindowProfilDoctor.Likart2.Text.Length != 0)
                      //{
                      //    SelectProfilLikar();
                      //    if (IndexAddEdit == "addCommand")
                      //    {
                      //        string json = JsonConvert.SerializeObject(selectedProfilLikar);
                      //        CallServer.PostServer(pathcontrolerProfilLikar, json, "POST");
                      //        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      //        ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                      //        if (ViewDoctors == null) ViewDoctors.Add(Idinsert);
                      //        else { ViewDoctors.Insert(ViewDoctors.Count, Idinsert); }
                      //        selectedProfilLikar = Idinsert;
                      //    }
                      //    else
                      //    {
                      //        string json = JsonConvert.SerializeObject(selectedProfilLikar);
                      //        CallServer.PostServer(pathcontrolerProfilLikar, json, "PUT");
                      //    }
                      //}
                      //IndexAddEdit = "";

                  }));

            }
        }

        public void SelectProfilLikar()
        {

            selectedProfilLikar.name = WindowProfilDoctor.Likart10.Text.ToString();
            selectedProfilLikar.surname = WindowProfilDoctor.Likart2.Text.ToString();
            selectedProfilLikar.specialnoct = WindowProfilDoctor.Likart3.Text.ToString();
            selectedProfilLikar.edrpou = WindowProfilDoctor.Likart8.Text.ToString();
            selectedProfilLikar.telefon = WindowProfilDoctor.Likart6.Text.ToString();
            selectedProfilLikar.email = WindowProfilDoctor.Likart7.Text.ToString();
            selectedProfilLikar.uriwebDoctor = WindowProfilDoctor.LikarUrit7.Text.ToString();
            selectedProfilLikar.napryamok = WindowProfilDoctor.LikarNaprt3.Text.ToString();

            selectedGridDoctor = new ModelGridDoctor();
            selectedGridDoctor.kodDoctor = selectedProfilLikar.kodDoctor;
            selectedGridDoctor.id = selectedProfilLikar.id;
            selectedGridDoctor.name = selectedProfilLikar.name;
            selectedGridDoctor.surname = selectedProfilLikar.surname;
            selectedGridDoctor.specialnoct = selectedProfilLikar.specialnoct;
            selectedGridDoctor.telefon = selectedProfilLikar.telefon;
            selectedGridDoctor.email = selectedProfilLikar.email;
            selectedGridDoctor.edrpou = selectedProfilLikar.edrpou;
            selectedGridDoctor.nameZaklad = WindowProfilDoctor.Likart9.Text.ToString();
            selectedGridDoctor.pind = WindowProfilDoctor.Likart5.Text.ToString();
            selectedGridDoctor.adrZaklad = WindowProfilDoctor.Likart4.Text.ToString();
            selectedGridDoctor.napryamok = selectedProfilLikar.napryamok;
            selectedGridDoctor.uriwebDoctor = selectedProfilLikar.uriwebDoctor;
        }

        // команда печати
        RelayCommand? printProfilLikar;
        public RelayCommand PrintProfilLikar
        {
            get
            {
                return printProfilLikar ??
                  (printProfilLikar = new RelayCommand(obj =>
                  {
                      
                      //if (selectedGridProfilLikar != null)
                      //{
                      //    MessageBox.Show("Ім'я та прізвище лікаря :" + selectedGridProfilLikar.name.ToString() + " " + selectedGridProfilLikar.surname.ToString());
                      //}
                  },
                 (obj) => selectedGridProfilLikar != null));
            }
        }

        // команда загрузки сайта
        private RelayCommand? uriWebLikar;
        public RelayCommand UriWebLikar
        {
            get
            {
                return uriWebLikar ??
                  (uriWebLikar = new RelayCommand(obj =>
                  {

                      if (selectedDoctor.uriwebDoctor != "")
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
                  }));
            }
        }

        

        #endregion
    }

}


