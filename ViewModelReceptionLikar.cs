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
        private static  MainWindow ReceptionLIkar = MainWindow.LinkNameWindow("BackMain");
        private bool addboolAppointment = false, editboolAppointment = false;
        public static string NameInterviewPacient = "", KodInterviewPacient = "", KodProtokola = "", DateInterview = "", DetalishIterv="";
        public static string pathcontrollerAppointment = "/api/RegistrationAppointmentController/";
        private string controllerColectionInterview = "/api/ColectionInterviewController/";
        public static ModelRegistrationAppointment selectRegistrationAppointment;
        public static ModelVisitingDays selectVisitingDays;
        public ModelColectionInterview SelectedColectionReceptionPatient
        {
            get { return modelColectionInterview; }
            set { modelColectionInterview = value; OnPropertyChanged("SelectedColectionReceptionPatient"); }
        }
        public static ObservableCollection<ModelRegistrationAppointment> ViewRegistrAppoints { get; set; }
        public static ObservableCollection<ModelColectionInterview> ViewReceptionPatients { get; set; }

        // добавить запись к врачу
        private RelayCommand? methodLoadReceptionPacient;
        public RelayCommand MethodLoadReceptionPacient
        {
            get
            {
                return methodLoadReceptionPacient ??
                  (methodLoadReceptionPacient = new RelayCommand(obj =>
                  {
                      if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
                      if (_pacientProfil == "") { MethodLoadPacientProfil(); }
                      LoadReceptionPacients(); }));
            }
        }

   
        public static void LoadReceptionPacients()
        {
            CallServer.PostServer(pathcontrollerAppointment, pathcontrollerAppointment + _pacientProfil+"/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionRegistrationAppointment(CmdStroka);
        }


        public static void ObservablelColectionRegistrationAppointment(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelRegistrationAppointment>(CmdStroka);
            List<ModelRegistrationAppointment> res = result.ModelRegistrationAppointment.ToList();
            ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>((IEnumerable<ModelRegistrationAppointment>)res);
            BildModelReceptionPatient();
            ReceptionLIkar.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;


        }

        public static void BildModelReceptionPatient()
        {

            ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
            foreach (ModelRegistrationAppointment modelReceptionPatient in ViewRegistrAppoints)
            {
                modelColectionInterview = new ModelColectionInterview();
                if (modelReceptionPatient.kodDoctor != null && modelReceptionPatient.kodDoctor.Length != 0) MethodReceptionDoctor(modelReceptionPatient);
                if (modelReceptionPatient.kodPacient != null && modelReceptionPatient.kodPacient.Length != 0) MethodReceptionPacient(modelReceptionPatient);
                if (modelReceptionPatient.kodProtokola != null && modelReceptionPatient.kodProtokola.Length != 0) MethodReceptionProtokol(modelReceptionPatient);
                modelColectionInterview.kodComplInterv = modelReceptionPatient.kodComplInterv;
                modelColectionInterview.dateInterview = modelReceptionPatient.dateInterview;
                modelColectionInterview.dateDoctor = modelReceptionPatient.dateDoctor;
                modelColectionInterview.kodProtokola = modelReceptionPatient.kodProtokola;
                modelColectionInterview.resultDiagnoz = modelReceptionPatient.topictVizita;
                ViewReceptionPatients.Add(modelColectionInterview);
                
            }

        }

        public static void MethodReceptionDoctor(ModelRegistrationAppointment colectionInterview)
        {
            var json = DoctorcontrollerIntev + colectionInterview.kodDoctor.ToString() + "/0/0";
            CallServer.PostServer(DoctorcontrollerIntev, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                modelColectionInterview.nameDoctor = Insert.name + " " + Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                modelColectionInterview.kodDoctor = Insert.kodDoctor;
            }

        }

        public static void MethodReceptionPacient(ModelRegistrationAppointment colectionInterview)
        {

            string json = PacientcontrollerIntevLikar + colectionInterview.kodPacient.ToString() + "/0/0/0/0";
            CallServer.PostServer(PacientcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Insert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                modelColectionInterview.namePacient = Insert.name + " " + Insert.surname + " " + Insert.profession + " " + Insert.tel;
                modelColectionInterview.kodPacient = Insert.kodPacient;
            }
        }

        public static void MethodReceptionProtokol(ModelRegistrationAppointment colectionInterview)
        {
            var json = ProtocolcontrollerIntevLikar + "0/" + colectionInterview.kodProtokola.ToString() + "/0";
            CallServer.PostServer(ProtocolcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = DiagnozcontrollerIntevLikar + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(DiagnozcontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        modelColectionInterview.nameDiagnoz = Insert1.nameDiagnoza;
                    }

                    json = RecomencontrollerIntevLikar + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(RecomencontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        modelColectionInterview.nameRecomen = Insert2.contentRecommendation;
                    }
                }

            }
        }

        // добавить запись к врачу
        private RelayCommand? addReceptionPacient;
        public RelayCommand AddReceptionPacient
        {
            get
            {
                return addReceptionPacient ??
                  (addReceptionPacient = new RelayCommand(obj =>
                  { if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
                      if (_pacientProfil == "") { MethodLoadPacientProfil(); }
                      MethodAddReceptionPacient();
                  }));
            }
        }
        public void MethodAddReceptionPacient()
        {
            IndexAddEdit = "addCommand";
            SelectedColectionReceptionPatient = null;
            ReceptionLIkar.ReceptionPacientzap3.Text = _pacientName;
            if (addboolAppointment == false) BoolTrueAppointment();
            else BoolFalseAppointment();

        }

        public void BoolTrueAppointment()
        {
            addboolAppointment = true;
            MethodEditTrue();
        }

        private void BoolFalseAppointment()
        {
            IndexAddEdit = "";
            addboolAppointment = false;
            ReceptionLIkar.ReceptionLikarCompInterview.Visibility = Visibility.Hidden;
            MethodEditFalse();
        }

        public void MethodEditTrue()
        {
            editboolAppointment = true;
            //ReceptionLIkar.ReceptionLikar4.IsEnabled = true;
            //ReceptionLIkar.ReceptionLikar4.Background = Brushes.AntiqueWhite;
            ReceptionLIkar.ReceptionLikar7.IsEnabled = true;
            ReceptionLIkar.ReceptionLikar7.Background = Brushes.AntiqueWhite;
            ReceptionLIkar.ReceptionLikarFolderLikar.Visibility = Visibility.Visible;
            ReceptionLIkar.ReceptionLikarAddInterv.Visibility = Visibility.Visible;
            ReceptionLIkar.ReceptionLikarFolder.Visibility = Visibility.Visible;
        }


        private void MethodEditFalse()
        {
            editboolAppointment = false;
            //ReceptionLIkar.ReceptionLikar4.IsEnabled = false;
            //ReceptionLIkar.ReceptionLikar4.Background = Brushes.White;
            ReceptionLIkar.ReceptionLikar7.IsEnabled = false;
            ReceptionLIkar.ReceptionLikar7.Background = Brushes.White;
            ReceptionLIkar.ReceptionLikarAddInterv.Visibility = Visibility.Hidden;
            ReceptionLIkar.ReceptionLikarFolderLikar.Visibility = Visibility.Hidden;
            ReceptionLIkar.ReceptionLikarFolder.Visibility = Visibility.Hidden;
        }

        // добавить запись к врачу
        private RelayCommand? editReceptionPacient;
        public RelayCommand EditReceptionPacient
        {
            get
            {
                return editReceptionPacient ??
                  (editReceptionPacient = new RelayCommand(obj =>
                  { MethodEditReceptionPacient(); }));
            }
        }

        public void MethodEditReceptionPacient()
        {
            if (modelColectionInterview != null)
            {
                IndexAddEdit = "editCommand";
                if (editboolAppointment == true) { MethodEditFalse(); return; }
                MethodEditTrue();
                ReceptionLIkar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
            }

        }

        // удалить запись к врачу
        private RelayCommand? removeReceptionPacient;
        public RelayCommand RemoveReceptionPacient
        {
            get
            {
                return removeReceptionPacient ??
                  (removeReceptionPacient = new RelayCommand(obj =>
                  { MethodRemoveReceptionPacient(); }));
            }
        }
        public void MethodRemoveReceptionPacient()
        {
            if (modelColectionInterview != null)
            {
                MessageDeleteData();
                if (MapOpisViewModel.DeleteOnOff == true)
                {
                    if (ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex >= 0)
                    {
                        string json = pathcontrollerAppointment + ViewRegistrAppoints[ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex].id.ToString()+"/0/0";
                        CallServer.PostServer(pathcontrollerAppointment, json, "DELETE");
                        modelColectionInterview = ViewReceptionPatients[ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex];
                        selectRegistrationAppointment = ViewRegistrAppoints[ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex];

                        json = pathcontrolerAdmissionPatients + modelColectionInterview.kodPacient.ToString() + "/" + modelColectionInterview.kodDoctor.ToString() + "/" + modelColectionInterview.kodComplInterv + "/0";
                        CallServer.PostServer(pathcontrolerAdmissionPatients, json, "GETID");
                        string CmdStroka = CallServer.ServerReturn();
                        if (CmdStroka.Contains("[]") == false) 
                        {
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);
                            json = pathcontrolerAdmissionPatients + admissionPatient.id+"/0/0";
                            CallServer.PostServer(pathcontrolerAdmissionPatients, json, "DELETE");
                            
                        }
                        ViewRegistrAppoints.Remove(selectRegistrationAppointment);
                        ViewReceptionPatients.Remove(modelColectionInterview);
                        BildModelReceptionPatient();
                        ReceptionLIkar.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;
                        modelColectionInterview = new  ModelColectionInterview();
                        BoolFalseAppointment();
                        IndexAddEdit = "";
                    }
          
                }

            }
            
        }
        // сохранить запись к врачу
        private RelayCommand? saveReceptionPacientLikar;
        public RelayCommand SaveReceptionPacientLikar
        {
            get
            {
                return saveReceptionPacientLikar ??
                  (saveReceptionPacientLikar = new RelayCommand(obj =>
                  { MethodSaveReceptionPacient(); }));
            }
        }


        public void MethodSaveReceptionPacient()
        {
            if (modelColectionInterview == null) return;
            if (modelColectionInterview.dateInterview != "")
            {
                if (ReceptionLIkar.ReceptionLikar2.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                    SelectedFalseLogin();
                    return;
                }
                if (ReceptionLIkar.ReceptionLikar4.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено дату та час прийому";
                    SelectedFalseLogin();
                    return;
                }
                if (ReceptionLIkar.ReceptionLikar7.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено текст звернення";
                    SelectedFalseLogin();
                    return;
                }
               
                modelColectionInterview.kodPacient = _pacientProfil;
                modelColectionInterview.dateDoctor = WindowIntevLikar.ReceptionLikar4.Text.ToString();
                modelColectionInterview.resultDiagnoz = WindowIntevLikar.ReceptionLikar7.Text.ToString();
                selectRegistrationAppointment = new ModelRegistrationAppointment();

                selectRegistrationAppointment.kodDoctor = modelColectionInterview.kodDoctor; //nameDoctor.Substring(0, modelColectionInterview.nameDoctor.IndexOf(":"));
                selectRegistrationAppointment.kodPacient = modelColectionInterview.kodPacient; //.namePacient;
                selectRegistrationAppointment.kodProtokola = modelColectionInterview.kodProtokola;
                selectRegistrationAppointment.kodComplInterv = modelColectionInterview.kodComplInterv;
                selectRegistrationAppointment.topictVizita = modelColectionInterview.resultDiagnoz;
                selectRegistrationAppointment.dateInterview = modelColectionInterview.dateInterview;
                selectRegistrationAppointment.dateDoctor = modelColectionInterview.dateDoctor; // selectReceptionPatient.dateDoctor;
                selectRegistrationAppointment.KodDiagnoz = kodDiagnoz;

                var json = JsonConvert.SerializeObject(selectRegistrationAppointment);
                string Method = "POST";
                if (IndexAddEdit == "editCommand" && ViewRegistrAppoints != null && WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex >= 0)
                {
                    Method = "PUT";
                    selectRegistrationAppointment.id = ViewRegistrAppoints[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex].id;
                }
                CallServer.PostServer(pathcontrollerAppointment, json, Method);
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) { CallServer.FalseServerGet(); return; }
                {
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    selectRegistrationAppointment = JsonConvert.DeserializeObject<ModelRegistrationAppointment>(CallServer.ResponseFromServer);

                }

                admissionPatient = new AdmissionPatient();
                if (IndexAddEdit == "editCommand")
                { 
                    json = pathcontrolerAdmissionPatients + modelColectionInterview.kodPacient.ToString() + "/" + modelColectionInterview.kodDoctor.ToString() +"/"+ modelColectionInterview.kodComplInterv+"/0";
                    CallServer.PostServer(pathcontrolerAdmissionPatients, json, "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) { CallServer.FalseServerGet(); return; }
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);
                    }                
                }
 
                admissionPatient.kodDoctor = modelColectionInterview.kodDoctor; ;
                admissionPatient.kodPacient = modelColectionInterview.kodPacient;
                admissionPatient.kodProtokola = modelColectionInterview.kodProtokola;
                admissionPatient.kodComplInterv = modelColectionInterview.kodComplInterv;
                admissionPatient.topictVizita = modelColectionInterview.resultDiagnoz;
                admissionPatient.dateInterview = modelColectionInterview.dateInterview;
                admissionPatient.dateVizita = modelColectionInterview.dateDoctor;
                admissionPatient.kodDiagnoz = kodDiagnoz;

                json = JsonConvert.SerializeObject(admissionPatient);
                CallServer.PostServer(pathcontrolerAdmissionPatients, json, Method);
                CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
                {
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);

                }
                admissionPatient = new AdmissionPatient();
                CopycolectionInterview();
                SaveInterviewProtokol();
                MethodEditFalse();
                ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>();
                modelColectionInterview = new ModelColectionInterview();
                SelectedColectionReceptionPatient = null;
                LoadReceptionPacients();
                
                
            }

        }


        

        // сохранить запись к врачу
        private RelayCommand? printReceptionPacient;
        public RelayCommand PrintReceptionPacient
        {
            get
            {
                return printReceptionPacient ??
                  (printReceptionPacient = new RelayCommand(obj =>
                  { MethodSaveReceptionPacient(); }));
            }
        }
        public void MethodPrintReceptionPacient()
        {
            if (ViewModelReceptionPatients != null)
            {
                MessageBox.Show("Пацієнт :" + ViewModelReceptionPatients[0].namePacient.ToString());
            }
        }

        // команда просмотра содержимого интервью
        private RelayCommand? readColectionIntreviewReception;
        public RelayCommand ReadColectionIntreviewReception
        {
            get
            {
                return readColectionIntreviewReception ??
                  (readColectionIntreviewReception = new RelayCommand(obj =>
                  {
                      string TempIndexAddEdit = IndexAddEdit;
                      IndexAddEdit = "editCommand";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = modelColectionInterview.kodComplInterv + "/0";

                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      IndexAddEdit = TempIndexAddEdit;
                  }));
            }
        }

        // команда выбора доктора
        private RelayCommand? reseptionPacientLikars;
        public RelayCommand ReseptionPacientLikars
        {
            get
            {
                return reseptionPacientLikars ??
                  (reseptionPacientLikars = new RelayCommand(obj =>
                  {
                      
                      WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                      MedZaklad.ShowDialog();

                      EdrpouMedZaklad = ReceptionLIkar.Likart8.Text.ToString();
                      if (EdrpouMedZaklad.Length > 0)
                      { 
                          CallViewProfilLikar = "WinNsiLikar";
                          WinNsiLikar NewOrder = new WinNsiLikar();
                          NewOrder.ShowDialog();
                          if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();

                          modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
                          modelColectionInterview.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                          ReceptionLIkar.ReceptionLikar2.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);                    
                      }
 
                  }));
            }
        }

        // команда выбора доктора
        private RelayCommand? reseptionPacientInterview;
        public RelayCommand ReseptionPacientInterview
        {
            get
            {
                return reseptionPacientInterview ??
                  (reseptionPacientInterview = new RelayCommand(obj =>
                  {
                      if (_pacientProfil != "")
                      { 
                          CallServer.PostServer(controllerColectionInterview, controllerColectionInterview + "0/0/" + _pacientProfil, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]") == false)
                          {
                              WinListInteviewPacient NewOrder = new WinListInteviewPacient();
                              NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                              NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                              NewOrder.ShowDialog();
                              if (KodProtokola.Length != 0)
                              {
                                  if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();
                                  modelColectionInterview.kodComplInterv = KodInterviewPacient;
                                  modelColectionInterview.nameInterview = NameInterviewPacient;
                                  modelColectionInterview.dateInterview = DateInterview;
                                  modelColectionInterview.kodProtokola = KodProtokola;
                                  modelColectionInterview.detailsInterview =DetalishIterv;

                                  ModelRegistrationAppointment modelReceptionPatient = new ModelRegistrationAppointment();
                                  modelReceptionPatient.kodProtokola = KodProtokola;
                                  MethodReceptionProtokol(modelReceptionPatient);
                                  ReceptionLIkar.ReceptionLikar1.Text = modelColectionInterview.dateInterview;
                                  ReceptionLIkar.ReceptionLikar5.Text = modelColectionInterview.nameRecomen;
                                  ReceptionLIkar.ReceptionLikar6.Text = modelColectionInterview.nameDiagnoz;
                                  ReceptionLIkar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                                  ReceptionLIkar.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                              }

                          }                      
                      }

                  }));
            }
        }

        private RelayCommand? onVisibleReceptionPacients;
        public RelayCommand OnVisibleReceptionPacients
        {
            get
            {
                return onVisibleReceptionPacients ??
                  (onVisibleReceptionPacients = new RelayCommand(obj =>
                  {

                      if (ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex == -1) return;
                      if (ViewReceptionPatients != null)
                      {
                          //MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("BackMain");
                          modelColectionInterview = ViewReceptionPatients[ReceptionLIkar.ReceptionLikarTablGrid.SelectedIndex];
                          modelColectionInterview.namePacient = _pacientName;
                          ReceptionLIkar.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                          ReceptionLIkar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                          ReceptionLIkar.ReceptionLikarLoadinterv.Visibility = Visibility.Hidden;
                          _kodDoctor = modelColectionInterview.kodDoctor;
                      }


                  }));
            }
        }

        // команда выбора даты приема
        private RelayCommand? selectDayVisitingLikar;
        public RelayCommand SelectDaysVisitingLikar
        {
            get
            {
                return selectDayVisitingLikar ??
                  (selectDayVisitingLikar = new RelayCommand(obj =>
                  {

                      if (_kodDoctor.Length == 0)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                          SelectedFalseLogin();
                          return;
                      }

                      WinVisitingDays NewOrder = new WinVisitingDays();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2)  ;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      if (selectVisitingDays != null)
                      {
                          ReceptionLIkar.ReceptionLikar4.Text = selectVisitingDays.dateVizita + " :" + selectVisitingDays.timeVizita;
                      }

                  }));
            }
        }
    }
}
