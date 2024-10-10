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
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {

        // 
        // 

        #region Обработка событий и команд вставки, удаления и редектирования справочника "пациентов"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех паціентів из БД
        /// через механизм REST.API
        /// </summary>
        public static MainWindow WindowReceptionPacient = MainWindow.LinkNameWindow("BackMain");
        private bool editboolReceptionPacient = false, addboolReceptionPacient = false, loadboolReceptionPacient = false;
        public static bool _readOnlyProfil=false;
        private string edittextReceptionPacient = "";
        public static string pathcontrolerReceptionPacient = "/api/ControllerAdmissionPatients/";
        public static string Pacientcontroller = "/api/PacientController/";
        public static string Protocolcontroller = "/api/DependencyDiagnozController/";
        public static string Diagnozcontroller = "/api/DiagnozController/";
        public static string Recomencontroller = "/api/RecommendationController/";
        public AdmissionPatient selectedReceptionPacient;
        public static ModelReceptionPatient selectedModelReceptionPatient;
        public static ObservableCollection<AdmissionPatient> ViewReceptionPacients { get; set; }
        public static ObservableCollection<ModelReceptionPatient> ViewModelReceptionPatients { get; set; }
        public ModelReceptionPatient SelectedReceptionPacient
        {
            get { return selectedModelReceptionPatient; }
            set { selectedModelReceptionPatient = value; OnPropertyChanged("SelectedReceptionPacient"); }
        }


        public static void ObservableViewReceptionPacient(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListAdmissionPatient>(CmdStroka);
            List<AdmissionPatient> res = result.AdmissionPatient.ToList();
            ViewReceptionPacients = new ObservableCollection<AdmissionPatient>((IEnumerable<AdmissionPatient>)res);
            BildViewModelReceptionPacients();
            WindowMen.ReceptionPacientTablGrid.ItemsSource = ViewModelReceptionPatients;


        }

        private static void BildViewModelReceptionPacients()
        {
            ViewModelReceptionPatients = new ObservableCollection<ModelReceptionPatient>();
            foreach (AdmissionPatient admissionPatient in ViewReceptionPacients)
            {
                selectedModelReceptionPatient = new ModelReceptionPatient();
                selectedModelReceptionPatient.dateInterview = admissionPatient.dateInterview;
                selectedModelReceptionPatient.dateVizita = admissionPatient.dateVizita;
                selectedModelReceptionPatient.topictVizita = admissionPatient.topictVizita;
                selectedModelReceptionPatient.kodPacient = admissionPatient.kodPacient;
                selectedModelReceptionPatient.kodProtokola = admissionPatient.kodProtokola;
                selectedModelReceptionPatient.kodComplInterv = admissionPatient.kodComplInterv;
                MethodReceptionPacients(admissionPatient);
                MethodProtokolaReception(admissionPatient);
                ViewModelReceptionPatients.Add(selectedModelReceptionPatient);
            }
        }

        private static void MethodReceptionPacients(AdmissionPatient colectionInterview)
        {

            selectedModelReceptionPatient.namePacient = "";
            string json = Pacientcontroller + colectionInterview.kodPacient.ToString() + "/0/0/0/0";
            CallServer.PostServer(Pacientcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                selectedModelReceptionPatient.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
            }
            else
            {
                selectedModelReceptionPatient.namePacient = colectionInterview.kodPacient.ToString();
                selectedModelReceptionPatient.kodPacient = colectionInterview.kodPacient.ToString();
            }
        }

        private static void MethodProtokolaReception(AdmissionPatient colectionInterview)
        {
            selectedModelReceptionPatient.nameDiagnoz = "";
            selectedModelReceptionPatient.nameRecomen = "";

            var json = Protocolcontroller + "0/" + colectionInterview.kodProtokola.ToString() + "/0";
            CallServer.PostServer(Protocolcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {

                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString()+"/0/0";
                    CallServer.PostServer(Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectedModelReceptionPatient.nameDiagnoz = Insert1.nameDiagnoza;

                    }


                    json = Recomencontroller + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedModelReceptionPatient.nameRecomen = Insert2.contentRecommendation;
                    }
                }

            }

        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadReceptionLikar;
        public RelayCommand LoadReceptionLikar
        {
            get
            {
                return loadReceptionLikar ??
                  (loadReceptionLikar = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      if (_kodDoctor == "") { MetodLoadProfilLikar(); }
                      MethodLoadReceptionLikar();
                  }));
            }
        }


        private void MethodLoadReceptionLikar()
        {

            ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
            WindowReceptionPacient.ReceptionPacientLoadinterv.Visibility = Visibility.Hidden;
            json =  pathcontrolerReceptionPacient + "0/" + _kodDoctor+"/0/0";
            CallServer.PostServer(pathcontrolerReceptionPacient, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else ObservableViewReceptionPacient(CmdStroka);           
 
        }
 
    
        // команда добавления нового объекта

        private RelayCommand? addReceptionLikar;
        public RelayCommand AddReceptionLikar
        {
            get
            {
                return addReceptionLikar ??
                  (addReceptionLikar = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      AddComandReceptionLikar(); 
                  }));
            }
        }

        private void AddComandReceptionLikar()
        {
            if(ViewReceptionPacients == null) MethodLoadReceptionLikar();
            if (ViewReceptionPacients.Count == 0) MethodLoadReceptionLikar();
            MethodaddcomReceptionPacient();
        }

        private void MethodaddcomReceptionPacient()
        {
            WindowReceptionPacient.PacientTablGrid.SelectedItem = null;
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolReceptionPacient == true) { BoolFalseReceptionLikar(); return; }

            SelectedReceptionPacient = new ModelReceptionPatient();
            BoolTrueReceptionLikar();
        }

        private string _SelectedCombPriyomOnOff;
        public string SelectedCombPriyomOnOff
        {
            get => _SelectedCombPriyomOnOff;
            set
            {
                //меняем значение в обычном порядке
                _SelectedCombProfil = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCombProfil)));
                //OnPropertyChanged(nameof(SelectedComb));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewCombPriyomOnOff(_SelectedCombProfil);
            }
        }

        public void SetNewCombPriyomOnOff(string selected = "")
        {
            MainWindow WindowProfilPacient = MainWindow.LinkNameWindow("BackMain");
            WindowProfilPacient.ReceptionPacientPriyomOnOff.Text = selected == "0" ? "Так" : "Ні";
        }
        private void BoolTrueReceptionLikar()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            addboolReceptionPacient = true;
            editboolReceptionPacient = true;
            List<string> TakNi = new List<string> { "Так", "Ні" };
            WindowMen.CombPriyomOnOff.ItemsSource = TakNi;
            WindowMen.CombPriyomOnOff.SelectedIndex = Convert.ToInt32(SelectedCombPriyomOnOff);
            WindowMen.ReceptionPacient7.IsEnabled = true;
            WindowMen.ReceptionPacient7.Background = Brushes.AntiqueWhite;
           
        }

        private void BoolFalseReceptionLikar()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            addboolReceptionPacient = false;
            editboolReceptionPacient = false;
            WindowMen.ReceptionPacient4.IsEnabled = false;
            WindowMen.ReceptionPacient4.Background = Brushes.White;
            WindowMen.ReceptionPacient7.IsEnabled = false;
            WindowMen.ReceptionPacient7.Background = Brushes.White;
            
        }

        // команда удаления
        private RelayCommand? removeReceptionLikar;
        public RelayCommand RemoveReceptionLikar
        {
            get
            {
                return removeReceptionLikar ??
                  (removeReceptionLikar = new RelayCommand(obj =>
                  {
                      if (selectedModelReceptionPatient != null)
                      {
                          MessageDeleteData();
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              int _setindex = WindowReceptionPacient.ReceptionPacientTablGrid.SelectedIndex; // == 0? WindowMen.ReceptionPacientTablGrid.SelectedIndex : WindowMen.ReceptionPacientTablGrid.SelectedIndex-1;
                              string json = pathcontrolerReceptionPacient + ViewReceptionPacients[_setindex].id.ToString()+"/0/0";
                              CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");
                              selectedReceptionPacient = ViewReceptionPacients[_setindex];
                              ViewReceptionPacients.Remove(selectedReceptionPacient);
                              selectedReceptionPacient = new AdmissionPatient();
                              selectedModelReceptionPatient = ViewModelReceptionPatients[_setindex];
                              ViewModelReceptionPatients.Remove(selectedModelReceptionPatient);
                              BoolFalseReceptionLikar();
                              IndexAddEdit = "";
                          }

                      }
                  }));
            }
        }


        // команда  редактировать
        private RelayCommand? editReceptionLikar;
        public RelayCommand? EditReceptionLikar
        {
            get
            {
                return editReceptionLikar ??
                  (editReceptionLikar = new RelayCommand(obj =>
                  {
                      if (selectedReceptionPacient != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolReceptionPacient == false) BoolTrueReceptionLikar();
                          else
                          {
                              BoolFalseReceptionLikar();
                              WindowReceptionPacient.ReceptionPacientTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveReceptionLikar;
        public RelayCommand SaveReceptionLikar
        {
            get
            {
                return saveReceptionLikar ??
                  (saveReceptionLikar = new RelayCommand(obj =>
                  {

                      if (selectedModelReceptionPatient.dateInterview != "")
                      {
                          MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("BackMain");

                          if (WindowIntevPacient.ReceptionPacient4.Text.Length == 0)
                          {
                              MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено дату та час прийому";
                              SelectedFalseLogin();
                              return;
                          }
                          if (WindowIntevPacient.ReceptionPacient7.Text.Length == 0)
                          {
                              MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено текст звернення";
                              SelectedFalseLogin();
                              return;
                          }
                          MetodPutReceptionLikar();
                          if (selectedReceptionPacient != null)
                          {
                              string Method = (IndexAddEdit == "addCommand" || (IndexAddEdit == "editCommand" && selectedReceptionPacient.id == 0)) ? "POST" : "PUT";
                              string json = JsonConvert.SerializeObject(selectedReceptionPacient);
                              CallServer.PostServer(pathcontrolerReceptionPacient, json, Method);
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              AdmissionPatient Idinsert = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);
                              if (Idinsert == null)
                              {
                                  MainWindow.MessageError = "Увага!" + Environment.NewLine + "Данні не записані в базу даних";
                                  MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, 10);
                                  return;
                              }
                              if ((IndexAddEdit == "editCommand" ) || IndexAddEdit == "addCommand")
                              {

                                  json = JsonConvert.SerializeObject(selectedReceptionPacient);
                                  CallServer.PostServer(pathcontrolerAdmissionPatients, json, Method);
                                  CmdStroka = CallServer.ServerReturn();
                                  if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
                                  {
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      selectedReceptionPacient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);

                                  }
                              }
                              else
                              {
                                  json = pathcontrolerAdmissionPatients + selectedReceptionPacient.kodPacient.ToString() + "/" + selectedReceptionPacient.kodDoctor.ToString() + "/" + selectedReceptionPacient.kodComplInterv + "/0";
                                  CallServer.PostServer(pathcontrolerAdmissionPatients, json, "GETID");
                                  CmdStroka = CallServer.ServerReturn();
                                  if (CmdStroka.Contains("[]") == false)
                                  {

                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      var result = JsonConvert.DeserializeObject<ListAdmissionPatient>(CallServer.ResponseFromServer);
                                      List<AdmissionPatient> listAdmission = result.AdmissionPatient.ToList();

                                      foreach (AdmissionPatient admissionPatient in listAdmission)
                                      {
                                          selectedReceptionPacient.id = admissionPatient.id;
                                          selectedReceptionPacient.topictVizita = admissionPatient.topictVizita;
                                      }
                                      json = JsonConvert.SerializeObject(selectedReceptionPacient);
                                      CallServer.PostServer(pathcontrolerAdmissionPatients, json, "PUT");
                                  }
                              }
                              if (ViewReceptionPacients == null) ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                              ViewReceptionPacients.Add(selectedReceptionPacient);
                              if (ViewModelReceptionPatients == null) ViewModelReceptionPatients = new ObservableCollection<ModelReceptionPatient>();
                              ViewModelReceptionPatients.Add(selectedModelReceptionPatient);
                              WindowReceptionPacient.ReceptionPacientTablGrid.ItemsSource = ViewModelReceptionPatients;
                          }



                          
                      }
                      BoolFalseReceptionLikar();
                      IndexAddEdit = "";
                      selectedReceptionPacient = new AdmissionPatient();
                      selectedModelReceptionPatient = new ModelReceptionPatient();
                      WindowIntevPacient.ReceptionPacientTablGrid.SelectedItem = null;
                      SelectedReceptionPacient = new ModelReceptionPatient();

                  }));
            }
        }

        private void MetodPutReceptionLikar()
        {
            if (IndexAddEdit == "addCommand")
            {
                selectedReceptionPacient = new AdmissionPatient();
                selectedReceptionPacient.kodComplInterv = selectedModelReceptionPatient.kodComplInterv;
                selectedReceptionPacient.kodDoctor = _kodDoctor;
                selectedReceptionPacient.kodPacient = selectedModelReceptionPatient.kodPacient;
                selectedReceptionPacient.kodProtokola = selectedModelReceptionPatient.kodProtokola;
                selectedReceptionPacient.dateInterview = selectedModelReceptionPatient.dateInterview;
            }
            else
            {
                selectedReceptionPacient = ViewReceptionPacients[WindowReceptionPacient.ReceptionPacientTablGrid.SelectedIndex];

            }
            selectedReceptionPacient.topictVizita = WindowReceptionPacient.ReceptionPacient7.Text.ToString();
            selectedReceptionPacient.dateVizita = WindowReceptionPacient.ReceptionPacient4.Text.ToString();
        }
        
        
 
        // команда печати
        RelayCommand? printReceptionLikar;
        public RelayCommand PrintReceptionLikar
        {
            get
            {
                return printReceptionLikar ??
                  (printReceptionLikar = new RelayCommand(obj =>
                  {
                      WindowReceptionPacient.PacientTablGrid.SelectedItem = null;
                      if (ViewReceptionPacients != null)
                      {
                          MessageBox.Show("Лікар :" + ViewReceptionPacients[0].kodDoctor.ToString());
                      }
                  },
                 (obj) => ViewReceptionPacients != null));
            }
        }

        

        // команда открытия профиля пациента
        RelayCommand? readColectionPatients;
        public RelayCommand ReadColectionPatients
        {
            get
            {
                return readColectionPatients ??
                  (readColectionPatients = new RelayCommand(obj =>
                  {
                      if (selectedModelReceptionPatient != null && selectedModelReceptionPatient.kodPacient !="") //kodPacient.Contains("PCN") == true
                      {
                          _pacientProfil = selectedModelReceptionPatient.kodPacient;
                          _readOnlyProfil = true;
                          WinProfilPacient NewOrder = new WinProfilPacient();
                          NewOrder.AddProfil.Visibility = Visibility.Hidden;
                          NewOrder.SaveProfil.Visibility = Visibility.Hidden;
                          NewOrder.ShowDialog();
                      }
                      else
                      {
                          MapOpisViewModel.CallViewProfilLikar = "ProfilPacient";
                          LoadProfPacient();

                          selectedModelReceptionPatient = new ModelReceptionPatient();
                          selectedModelReceptionPatient.namePacient = selectedProfilPacient.name.ToString();
                          selectedModelReceptionPatient.kodPacient = selectedProfilPacient.kodPacient.ToString();
                          _pacientProfil = selectedModelReceptionPatient.kodPacient;
                          MapOpisViewModel.CallViewProfilLikar = "";


                          string json = PacientcontrollerIntevLikar + selectedModelReceptionPatient.kodPacient.ToString() + "/0/0/0/0";
                          CallServer.PostServer(PacientcontrollerIntevLikar, json, "GETID");

                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]") == false)
                          {

                              WinListInteviewPacient NewOrder = new WinListInteviewPacient();
                              NewOrder.Left = (MainWindow.ScreenWidth / 2);
                              NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                              NewOrder.ShowDialog();
                              if (MapOpisViewModel.KodInterviewPacient == "")
                              {
                                  _pacientProfil = "";
                                  selectedModelReceptionPatient.kodPacient = "";
                                  SelectedProfilPacient = new ModelPacient();
                                  return;
                              }
                              WindowReceptionPacient.ReceptionPacient1.Text = MapOpisViewModel.DateInterview;
                              selectedModelReceptionPatient.dateVizita = WindowReceptionPacient.ReceptionPacient4.Text;
                              selectedModelReceptionPatient.dateInterview = MapOpisViewModel.DateInterview;
                              selectedModelReceptionPatient.kodProtokola = MapOpisViewModel.KodProtokola;
                              selectedModelReceptionPatient.kodComplInterv = MapOpisViewModel.KodInterviewPacient;
                              AdmissionPatient colectionInterview = new AdmissionPatient();
                              colectionInterview.kodPacient = selectedModelReceptionPatient.kodPacient;
                              colectionInterview.kodProtokola = selectedModelReceptionPatient.kodProtokola;
                              MethodReceptionPacients(colectionInterview);
                              MethodProtokolaReception(colectionInterview);
                              //if (ViewModelReceptionPatients == null) ViewModelReceptionPatients = new ObservableCollection<ModelReceptionPatient>();
                              //ViewModelReceptionPatients.Add(selectedModelReceptionPatient);
                              //WindowReceptionPacient.ReceptionPacientTablGrid.ItemsSource = ViewModelReceptionPatients;
                              SelectedReceptionPacient = ViewModelReceptionPatients[ViewModelReceptionPatients.Count - 1];
                              if (selectedReceptionPacient == null) selectedReceptionPacient = new AdmissionPatient();
                              selectedReceptionPacient.kodComplInterv = selectedModelReceptionPatient.kodComplInterv;
                              selectedReceptionPacient.kodDoctor = _kodDoctor;
                              selectedReceptionPacient.kodPacient = selectedModelReceptionPatient.kodPacient;
                              selectedReceptionPacient.kodProtokola = selectedModelReceptionPatient.kodProtokola;
                              selectedReceptionPacient.dateInterview = selectedModelReceptionPatient.dateInterview;
                              selectedReceptionPacient.dateVizita = selectedModelReceptionPatient.dateVizita;
                              selectedReceptionPacient.topictVizita = selectedModelReceptionPatient.topictVizita;
                              //if (ViewReceptionPacients == null) ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                              //ViewReceptionPacients.Add(selectedReceptionPacient); 

                          }
                      }
                  }));
            }
        }

        

        // команда открытия профиля пациента
        RelayCommand? readIntevPatients;
        public RelayCommand ReadIntevPatients
        {
            get
            {
                return readIntevPatients ??
                  (readIntevPatients = new RelayCommand(obj =>
                  {
                      IndexAddEdit = selectedModelReceptionPatient.kodComplInterv != "" ? "editCommand" : "";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = selectedModelReceptionPatient.kodComplInterv != "" ? selectedModelReceptionPatient.kodComplInterv + "/0" : selectedModelReceptionPatient.kodProtokola;
                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                  },
                 (obj) => ViewReceptionPacients != null));
            }
        }

        private RelayCommand? onVisibleObjProfilPacient;
        public RelayCommand OnVisibleObjProfilPacient
        {
            get
            {
                return onVisibleObjProfilPacient ??
                  (onVisibleObjProfilPacient = new RelayCommand(obj =>
                  {
                      if (IndexAddEdit == "")
                      {
                          if (WindowIntevLikar.ReceptionPacientTablGrid.SelectedIndex == -1) return;
                          if (ViewModelReceptionPatients != null)
                          {
                              MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("BackMain");
                              WindowIntevLikar.CombPriyomOnOff.IsEnabled = true;
                              if (editboolIntevLikar == true) BoolFalseReceptionLikar();
                              selectedReceptionPacient = ViewReceptionPacients[WindowIntevLikar.ReceptionPacientTablGrid.SelectedIndex];
                              selectedModelReceptionPatient = ViewModelReceptionPatients[WindowIntevLikar.ReceptionPacientTablGrid.SelectedIndex];
                              
                              if (selectedModelReceptionPatient.kodPacient != null && selectedModelReceptionPatient.kodPacient.Length != 0) MethodReceptionPacients(selectedReceptionPacient);
                              if (selectedModelReceptionPatient.kodProtokola != null && selectedModelReceptionPatient.kodProtokola.Length != 0) MethodProtokolaReception(selectedReceptionPacient);
                              _pacientProfil = selectedModelReceptionPatient.kodPacient;
                              _kodDoctor = selectedReceptionPacient.kodDoctor;
                          } 
                      }



                  }));
            }

        }

        
        // команда открытия окна расписания приема пациентов
        RelayCommand? rozkladPatientsLikar;
        public RelayCommand RozkladPatientsLikar
        {
            get
            {
                return rozkladPatientsLikar ??
                  (rozkladPatientsLikar = new RelayCommand(obj =>
                  {
                      if (_kodDoctor.Length == 0)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                          SelectedFalseLogin();
                          return;
                      }

                      WinVisitingDays NewOrder = new WinVisitingDays();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      if (selectVisitingDays != null)
                      {
                          WindowReceptionPacient.ReceptionPacient4.Text = selectVisitingDays.dateVizita + " :" + selectVisitingDays.timeVizita;
                      }
                  }));
            }
        }

        #endregion
        #endregion
    }
}
