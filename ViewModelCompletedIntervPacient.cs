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
        // ViewModelReestrCompletedInterview модель ViewQualification
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public static string namePacient = "", nameDoctor = "", nameFeature3 = "", nameDetailing = "", nameGrDetailing = "";
               
        public static MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("BackMain");
        private bool editboolIntevPacient = false, loadboolIntevPacient = false;
        
        public static string ColectioncontrollerIntevPacient =  "/api/ColectionInterviewController/";
        public static string CompletedcontrollerIntevPacient =  "/api/CompletedInterviewController/";
        public static string PacientcontrollerIntev =  "/api/PacientController/";
        public static string DoctorcontrollerIntev =  "/api/ApiControllerDoctor/";
        public static string ProtocolcontrollerDependency =  "/api/DependencyDiagnozController/";
        public static string DiagnozcontrollerIntev =  "/api/DiagnozController/";
        public static string RecomencontrollerIntev =  "/api/RecommendationController/";

        public static ModelColectionInterview selectedIntevPacient;
        public static ColectionInterview selectedColectionIntevPacient;
        public static ModelDependency InsertIntevPacient;
        public static ObservableCollection<ModelColectionInterview> ColectionInterviewIntevPacients { get; set; }
        public static ObservableCollection<ColectionInterview> ColectionIntevPacients { get; set; }
        public ModelColectionInterview SelectedColectionIntevPacient
        {
            get { return selectedIntevPacient; }
            set { selectedIntevPacient = value; OnPropertyChanged("SelectedColectionIntevPacient"); }
        }


        public static void ObservablelColectionIntevPacient(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ColectionIntevPacients = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
            BildModelColectionIntevPacient();
            WindowIntevPacient.ColectionIntevPacientTablGrid.ItemsSource = ColectionInterviewIntevPacients;
        }

        private static void BildModelColectionIntevPacient()
        {
            ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>();
            foreach (ColectionInterview colectionInterview in ColectionIntevPacients)
            {
                selectedIntevPacient = new ModelColectionInterview();
                if (colectionInterview.kodPacient != null && colectionInterview.kodPacient.Length != 0) MethodPacientIntevPacient(colectionInterview, false);
                else { WindowIntevPacient.PacientIntert3.Text = "Гість"; selectedIntevPacient.namePacient = "Гість"; }
                if (colectionInterview.kodDoctor != null && colectionInterview.kodDoctor.Length != 0) MethodDoctorIntevPacient(colectionInterview, false);
                if (colectionInterview.kodProtokola != null && colectionInterview.kodProtokola.Length != 0) MethodProtokolaIntevPacient(colectionInterview, false);

                selectedIntevPacient.id = colectionInterview.id;
                selectedIntevPacient.kodComplInterv = colectionInterview.kodComplInterv;
                selectedIntevPacient.kodProtokola = colectionInterview.kodProtokola;
                selectedIntevPacient.dateInterview = colectionInterview.dateInterview;
                selectedIntevPacient.nameInterview = colectionInterview.nameInterview;
                ColectionInterviewIntevPacients.Add(selectedIntevPacient);
            }
        }

        private static void MethodPacientIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            MainWindow.UrlServer = Pacientcontroller;
            string json = Pacientcontroller + colectionInterview.kodPacient.ToString()+ "/0/0/0/0";
            CallServer.PostServer(Pacientcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                selectedIntevPacient.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
                selectedIntevPacient.kodPacient = Idinsert.kodPacient;
                if (boolname == true) WindowIntevPacient.PacientIntert3.Text = selectedIntevPacient.namePacient;
            }
        }

        private static void MethodDoctorIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            
            var json = Doctorcontroller + colectionInterview.kodDoctor.ToString()+"/0/0";
            CallServer.PostServer(Doctorcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                selectedIntevPacient.nameDoctor = Insert.name + " " + Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                selectedIntevPacient.kodDoctor = Insert.kodDoctor;
                if (boolname == true) WindowIntevPacient.PacientIntert2.Text = selectedIntevPacient.nameDoctor;
            }

        }

        private static void MethodProtokolaIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            
            var json = Protocolcontroller + "0/" + colectionInterview.kodProtokola.ToString() + "/0";
            CallServer.PostServer(Protocolcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    MainWindow.UrlServer = Diagnozcontroller;
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectedIntevPacient.nameDiagnoz = Insert1.nameDiagnoza;
                        if (boolname == true) WindowIntevPacient.PacientInterviewt6.Text = Insert1.nameDiagnoza;
                        selectedIntevPacient.resultDiagnoz = Insert1.uriDiagnoza;
                    }

                    MainWindow.UrlServer = Recomencontroller;
                    json = Recomencontroller + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedIntevPacient.nameRecomen = Insert2.contentRecommendation;
                        if (boolname == true) WindowIntevPacient.PacientInterviewt5.Text = Insert2.contentRecommendation;
                    }
                }

            }
        }


        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadColectionIntevPacient;
        public RelayCommand LoadColectionIntevPacient
        {
            get
            {
                return loadColectionIntevPacient ??
                  (loadColectionIntevPacient = new RelayCommand(obj =>
                  {
                      if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
                      if (_pacientProfil == "") MethodLoadPacientProfil();
                      MethodLoadtableColectionIntevPacient(); 
                  }));
            }
        }


        public static void MethodLoadtableColectionIntevPacient()
        {
            IndexAddEdit = "";
            CallServer.PostServer(ColectioncontrollerIntevPacient, ColectioncontrollerIntevPacient + "0/0/" + _pacientProfil, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionIntevPacient(CmdStroka);
        }


        private void BoolTrueIntevPacientCompl()
        {
            WindowMen.PacientFoldInterv.Visibility = Visibility.Visible;
        }

        private void BoolFalseIntevPacientCompl()
        {
            WindowMen.PacientFoldInterv.Visibility = Visibility.Hidden;
        }
        // команда удаления
        private RelayCommand? removeColectionIntevPacient;
        public RelayCommand RemoveColectionIntevPacient
        {
            get
            {
                return removeColectionIntevPacient ??
                  (removeColectionIntevPacient = new RelayCommand(obj =>
                  {
                      if (selectedIntevPacient != null)
                      {
                          MessageDeleteData();
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                               
                               
                               string json = CompletedcontrollerIntevPacient + selectedIntevPacient.kodProtokola.ToString() + "/0";
                               CallServer.PostServer(CompletedcontrollerIntevPacient, json, "DELETE");
                               json = ColectioncontrollerIntevPacient + selectedIntevPacient.id.ToString() + "/0/0"; 
                               CallServer.PostServer(ColectioncontrollerIntevPacient, json, "DELETE");
                               ColectionInterviewIntevPacients.Remove(selectedIntevPacient);
                               selectedIntevPacient = new ModelColectionInterview(); 
                              if (selectedIntevPacient.kodDoctor != "")
                              {

                                  json = pathcontrolerReceptionPacient + "0/"+ selectedIntevPacient.kodDoctor + "/0";
                                  CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");

                              }
                          }

                      }
                      
                      WindowMen.ColectionIntevPacientTablGrid.SelectedItem = null;

                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveColectionIntevPacient;
        public RelayCommand SaveColectionIntevPacient
        {
            get
            {
                return saveColectionIntevPacient ??
                  (saveColectionIntevPacient = new RelayCommand(obj =>
                  {
                      BoolFalseIntevPacientCompl();
                      WindowMen.ColectionIntevPacientTablGrid.SelectedItem = null;

                  }));
            }
        }


        // команда печати
        RelayCommand? printColectionIntevPacient;
        public RelayCommand PrintColectionIntevPacient
        {
            get
            {
                return printColectionIntevPacient ??
                  (printColectionIntevPacient = new RelayCommand(obj =>
                  {
                      if (ColectionInterviewIntevPacients != null)
                      {
                          MessageBox.Show("Результат діагнозу :" + ColectionInterviewIntevPacients[0].resultDiagnoz.ToString());
                      }
                  }));
            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntevPacients;
        public RelayCommand ReadColectionIntevPacients
        {
            get
            {
                return readColectionIntevPacients ??
                  (readColectionIntevPacients = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = selectedIntevPacient.kodComplInterv +"/0"; 
                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? onVisibleObjIntevPacients;
        public RelayCommand OnVisibleObjIntevPacients
        {
            get
            {
                return onVisibleObjIntevPacients ??
                  (onVisibleObjIntevPacients = new RelayCommand(obj =>
                  {
                      if (IndexAddEdit == "")
                      {
                            if (WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedIndex >= 0)
                            { 
                                ColectionInterview selectedColection = ColectionIntevPacients[WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedIndex];
                                SelectedColectionIntevPacient = ColectionInterviewIntevPacients[WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedIndex];
                                GetidkodProtokola = selectedColection.kodComplInterv + "/0";
                                MetodSearchContentInterv(GetidkodProtokola, CompletedcontrollerIntevPacient);
                                WindowIntevPacient.PacientInterviewUriText.Text = modelColectionInterview.nameInterview;
                                WindowIntevPacient.PacientFoldUrlHtpps.Visibility = Visibility.Visible;
                                WindowIntevPacient.PacientFoldMixUrlHtpps.Visibility = Visibility.Visible;
                                WindowMen.PacientFoldInterv.Visibility = Visibility.Visible;
                            }
                      }

                  }));
            }
        }

        private RelayCommand? pacientReadOpisIntevUrl;
        public RelayCommand PacientReadOpisIntevUrl
        {
            get
            {
                return pacientReadOpisIntevUrl ??
                  (pacientReadOpisIntevUrl = new RelayCommand(obj =>
                  {
                      if (selectedIntevPacient.resultDiagnoz == "") return;
                      MetodRunGoogle(selectedIntevPacient.resultDiagnoz);
                  }));
            }
        }

        private RelayCommand? pacientReadOpisMixUrlHtpps;
        public RelayCommand PacientReadOpisMixUrlHtpps
        {
            get
            {
                return pacientReadOpisMixUrlHtpps ??
                  (pacientReadOpisMixUrlHtpps = new RelayCommand(obj =>
                  {
                      if (modelColectionInterview.nameInterview == "") return;
                      MetodRunGoogle(modelColectionInterview.nameInterview);
                  }));
            }
        }

        public static void MetodRunGoogle(string TextContentInterv = "")
        {
            string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
            string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
            Process Rungoogle = new Process();
            Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
            Rungoogle.StartInfo.Arguments = TextContentInterv.Trim();
            //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            Rungoogle.StartInfo.UseShellExecute = false;
            Rungoogle.EnableRaisingEvents = true;
            Rungoogle.Start();
        }

        public static void MetodSearchContentInterv(string GetidkodProtokola = "", string controler = "")
        {
            int Index = 0;
            string ContentIntervText = "https://www.google.com/search?q=";
            
            CallServer.PostServer(controler, controler + GetidkodProtokola, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == true) return;
            ViewModelCreatInterview.ObservableContentInterv(CmdStroka);
            foreach (ModelContentInterv modelContentInterv in ViewModelCreatInterview.ContentIntervs)
            {
                string textDetalish = modelContentInterv.detailsInterview.TrimStart().TrimEnd();
                if (Index > 0) ContentIntervText += textDetalish.Replace(" ", "+");
                Index++;
                if (Index > 2) break;
            }
            modelColectionInterview.nameInterview = ContentIntervText;

        }
        #endregion
        #endregion
    }
}
