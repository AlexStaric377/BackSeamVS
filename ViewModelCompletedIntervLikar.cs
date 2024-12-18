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
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        // ViewModelReestrCompletedInterview 
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("BackMain");
        private bool editboolIntevLikar = false, loadboolIntevLikar = false;
        public static string ModelCallIntevLikar = "";
        public static string ColectioncontrollerIntevLikar =  "/api/ColectionInterviewController/";
        public static string CompletedcontrollerIntevLikar =  "/api/CompletedInterviewController/";
        public static string PacientcontrollerIntevLikar =  "/api/PacientController/";
        public static string DoctorcontrollerIntevLikar =  "/api/ApiControllerDoctor/";
        public static string ProtocolcontrollerIntevLikar = "/api/DependencyDiagnozController/";
        public static string DiagnozcontrollerIntevLikar =  "/api/DiagnozController/";
        public static string RecomencontrollerIntevLikar =  "/api/RecommendationController/";

        public static ModelColectionInterview selectedIntevLikar;
        public static ColectionInterview selectedColectionIntevLikar;
        public static ModelDependency InsertIntevLikar;
        public static ObservableCollection<ModelColectionInterview> ColectionInterviewIntevLikars { get; set; }
        public static ObservableCollection<ColectionInterview> ColectionIntevLikars { get; set; }
        public ModelColectionInterview SelectedColectionIntevLikar
        {
            get { return selectedColectionInterview; }
            set { selectedColectionInterview = value; OnPropertyChanged("SelectedColectionIntevLikar"); }
        }


        public static void ObservablelColectionIntevLikar(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ColectionIntevLikars = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
            BildModelColectionIntevLikar();
            WindowMen.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;
        }

        private static void BildModelColectionIntevLikar()
        {
            ColectionInterviewIntevLikars  = new ObservableCollection<ModelColectionInterview>();
            foreach (ColectionInterview colectionInterview in ColectionIntevLikars)
            {
                selectedIntevLikar = new ModelColectionInterview();
                if (colectionInterview.kodPacient != null && colectionInterview.kodPacient.Length != 0) MethodPacientIntevLikars(colectionInterview, false);
                //else { WindowMen.LikarIntert3.Text = "Гість"; selectedIntevLikar.namePacient = "Гість"; }
                if (colectionInterview.kodDoctor != null && colectionInterview.kodDoctor.Length != 0) MethodDoctorIntevLikars(colectionInterview, false);
                if (colectionInterview.kodProtokola != null && colectionInterview.kodProtokola.Length != 0) MethodProtokolaIntevLikars(colectionInterview, false);

                selectedIntevLikar.id = colectionInterview.id;
                selectedIntevLikar.kodComplInterv = colectionInterview.kodComplInterv;
                selectedIntevLikar.kodProtokola = colectionInterview.kodProtokola;
                selectedIntevLikar.dateInterview = colectionInterview.dateInterview;
                selectedIntevLikar.resultDiagnoz = colectionInterview.resultDiagnoz;
                ColectionInterviewIntevLikars.Add(selectedIntevLikar);
            }
        }

        private static void MethodPacientIntevLikars(ColectionInterview colectionInterview, bool boolname)
        {

          
            string json = Pacientcontroller + colectionInterview.kodPacient.ToString()+ "/0/0/0/0";
            CallServer.PostServer(Pacientcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                selectedIntevLikar.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
                selectedIntevLikar.kodPacient = Idinsert.kodPacient;
                WindowMen.LikarInterviewAvtort7.Text = selectedIntevLikar.namePacient;
                if (boolname == true) WindowMen.LikarIntert3.Text = selectedIntevLikar.namePacient;

            }
        }

        private static void MethodDoctorIntevLikars(ColectionInterview colectionInterview, bool boolname)
        {

            var json = Doctorcontroller + colectionInterview.kodDoctor.ToString() + "/0/0";
            CallServer.PostServer(Doctorcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                selectedIntevLikar.nameDoctor = Insert.name+" "+ Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                selectedIntevLikar.kodDoctor = Insert.kodDoctor;
                WindowMen.LikarInterviewAvtort7.Text = selectedIntevLikar.nameDoctor;
                //selectedIntevLikar.namePacient = "";
                if (boolname == true) WindowMen.Intert2.Text = selectedIntevLikar.nameDoctor;
                //WindowMen.LikarIntert3.Text = "";
            }

        }

        private static void MethodProtokolaIntevLikars(ColectionInterview colectionInterview, bool boolname)
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
                        selectedIntevLikar.nameDiagnoz = Insert1.nameDiagnoza;
                        if (boolname == true) WindowMen.LikarInterviewt6.Text = Insert1.nameDiagnoza;
                    }

                    json = Recomencontroller + Insert.kodRecommend.ToString() + "/0"; ;
                    CallServer.PostServer(Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedIntevLikar.nameRecomen = Insert2.contentRecommendation;
                        if (boolname == true) WindowMen.LikarInterviewt5.Text = Insert2.contentRecommendation;
                    }
                }

            }
        }


        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadColectionIntevLikar;
        public RelayCommand LoadColectionIntevLikar
        {
            get
            {
                return loadColectionIntevLikar ??
                  (loadColectionIntevLikar = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      if (_kodDoctor == "") { MetodLoadProfilLikar(); }  
                        MethodLoadtableColectionIntevLikar();
                      
                  }));
            }
        }


        public static void MethodLoadtableColectionIntevLikar()
        {
            IndexAddEdit = "";
            WindowMen.LikarLoadinterv.Visibility = Visibility.Hidden;
            WindowMen.LikarInterGridSave.Visibility = Visibility.Hidden;
            WindowMen.LikarColectionGridGhange.Visibility = Visibility.Hidden;

            CallServer.PostServer(ColectioncontrollerIntevLikar, ColectioncontrollerIntevLikar+ "0/" + _kodDoctor + "/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionIntevLikar(CmdStroka);
            
        }


        private void BoolTrueIntevLikarCompl()
        {
            editboolIntevLikar = true;
            WindowMen.LikarFoldInterv.Visibility = Visibility.Visible;
            WindowMen.LikarFolderRecomen.Visibility = Visibility.Visible;
            WindowMen.LikarFolderDiagn.Visibility = Visibility.Visible;
            WindowMen.LikarInterviewt7.IsEnabled = true;
            WindowMen.LikarInterviewt7.Background = Brushes.AntiqueWhite;

        }

        private void BoolFalseIntevLikarCompl()
        {
            editboolIntevLikar = false;
            WindowMen.LikarFolderRecomen.Visibility = Visibility.Hidden;
            WindowMen.LikarFolderDiagn.Visibility = Visibility.Hidden;
            WindowMen.LikarInterviewt7.IsEnabled = false;
            WindowMen.LikarInterviewt7.Background = Brushes.White;
        }
        // команда удаления
        private RelayCommand? removeColectionIntevLikar;
        public RelayCommand RemoveColectionIntevLikar
        {
            get
            {
                return removeColectionIntevLikar ??
                  (removeColectionIntevLikar = new RelayCommand(obj =>
                  {
                      if (selectedIntevLikar != null)
                      {
                          MessageDeleteData();
                          string json = CompletedcontrollerIntevLikar + selectedIntevLikar.kodComplInterv + "/0";
                          CallServer.PostServer(CompletedcontrollerIntevLikar, json, "DELETE");

                          json = ColectioncontrollerIntevLikar + selectedIntevLikar.id.ToString() + "/0/0";
                          CallServer.PostServer(ColectioncontrollerIntevLikar, json, "DELETE");
                          ColectionInterviewIntevLikars.Remove(selectedIntevLikar);
                          ColectionIntevLikars.Remove(selectedColectionIntevLikar);
                          selectedIntevLikar = new ModelColectionInterview();
                          selectedColectionIntevLikar = new ColectionInterview();
                      }
                      BoolFalseIntevLikarCompl();
                      WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedItem = null;


                  }));
            }
        }

        // команда  редактировать
        private RelayCommand? editColectionIntevLikar;
        public RelayCommand? EditColectionIntevLikar
        {
            get
            {
                return editColectionIntevLikar ??
                  (editColectionIntevLikar = new RelayCommand(obj =>
                  {
                      if (WindowMen.ColectionIntevLikarTablGrid.SelectedIndex >= 0)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolIntevLikar == false && selectedIntevLikar != null)
                          {
                              selectedColection = new ColectionInterview();
                              selectedColection = ColectionIntevLikars[WindowMen.ColectionIntevLikarTablGrid.SelectedIndex];
                              BoolTrueIntevLikarCompl();
                          }
                          else
                          {
                              BoolFalseIntevLikarCompl();
                              IndexAddEdit = "";
                              GetidkodProtokola = "";
                              WindowMen.ColectionIntevLikarTablGrid.SelectedItem = null;
                          }
                      }


                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveColectionIntevLikar;
        public RelayCommand SaveColectionIntevLikar
        {
            get
            {
                return saveColectionIntevLikar ??
                  (saveColectionIntevLikar = new RelayCommand(obj =>
                  {
                      string json = "";
                      BoolFalseIntevLikarCompl();
                      // ОБращение к серверу измнить корректируемую запись в БД
                      if (selectedIntevLikar != null)
                      {
                          if (selectedIntevLikar.kodProtokola != "") 
                          { 
                           
                            //json = ProtocolcontrollerIntevLikar + "0/" + WindowMen.LikarIntert1.Text.ToString();
                            CallServer.PostServer(ProtocolcontrollerIntevLikar, json, "GETID");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                            
                          }
                          if (Insert == null)
                          { 
                              Insert = new ModelDependency();
                              json = Interviewcontroller + "0/0/0/0";
                              CallServer.PostServer(Interviewcontroller, json, "GETID");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                              int Numbkey = Convert.ToInt32(Insert.kodProtokola.Substring(Insert.kodProtokola.IndexOf(".") + 1, Insert.kodProtokola.Length - (Insert.kodProtokola.IndexOf(".") + 1)));
                              Numbkey++;
                              string _repl = "000000000";
                             _repl = _repl.Length - Numbkey.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - Numbkey.ToString().Length) : "";
                              Insert.kodProtokola = Insert.kodProtokola.Substring(0,Insert.kodProtokola.IndexOf(".")+1) + _repl + Numbkey.ToString();
                              
                          }

                          if (WindowMen.LikarInterviewt5.Text.ToString().Contains(":") == true )
                          { 
                              if (WindowMen.LikarInterviewt5.Text.ToString().Contains(":") == true) Insert.kodRecommend = WindowMen.LikarInterviewt5.Text.ToString().Substring(0, WindowMen.LikarInterviewt5.Text.ToString().IndexOf(":"));
                              if (WindowMen.LikarInterviewt6.Text.ToString().Contains(":") == true) Insert.kodDiagnoz = WindowMen.LikarInterviewt6.Text.ToString().Substring(0, WindowMen.LikarInterviewt6.Text.ToString().IndexOf(":"));
                              Insert.id = selectedIntevLikar.kodProtokola == "" ? 0 : Insert.id;
                              var jsonDepency = JsonConvert.SerializeObject(Insert);
                              CallServer.PostServer(ProtocolcontrollerIntevLikar, jsonDepency, "PUT");
                              if (selectedColection == null) selectedColection = new ColectionInterview();
                              selectedColection.kodDoctor = selectedIntevLikar.kodDoctor;        
                          }
 
                          json = JsonConvert.SerializeObject(selectedColection);
                          CallServer.PostServer(ColectioncontrollerIntevLikar, json, "PUT");
                      }

                      IndexAddEdit = "";
                      WindowMen.ColectionIntevLikarTablGrid.SelectedItem = null;

                  }));
            }
        }


        // команда печати
        RelayCommand? printColectionIntevLikar;
        public RelayCommand PrintColectionIntevLikar
        {
            get
            {
                return printColectionIntevLikar ??
                  (printColectionIntevLikar = new RelayCommand(obj =>
                  {
                      if (ColectionInterviewIntevLikars != null)
                      {
                          MessageBox.Show("Результат діагнозу :" + ColectionInterviewIntevLikars[0].resultDiagnoz.ToString());
                      }
                  }));
            }
        }

        // команда 
        private RelayCommand? readColectionIntevLikars;
        public RelayCommand ReadColectionIntevLikars
        {
            get
            {
                return readColectionIntevLikars ??
                  (readColectionIntevLikars = new RelayCommand(obj =>
                  {
                      if (WindowMen.ColectionIntevLikarTablGrid.SelectedIndex >= 0)
                      { 
                          IndexAddEdit = "editCommand";
                          ModelCall = "ModelColectionInterview";
                          GetidkodProtokola = ColectionInterviewIntevLikars[WindowMen.ColectionIntevLikarTablGrid.SelectedIndex].kodComplInterv + "/0";
                          ComandreadColectionIntevLikars();
                          BoolFalseIntevLikarCompl();
                          editboolIntevLikar = false;                     
                      }

                  }));
            }
        }


        private void ComandreadColectionIntevLikars()
        {

            WinCreatIntreview NewOrder = new WinCreatIntreview();
            NewOrder.Left = 600;
            NewOrder.Top = 130;
            NewOrder.ShowDialog();


        }

        private RelayCommand? listIntevLikars;
        public RelayCommand ListIntevLikars
        {
            get
            {
                return listIntevLikars ??
                  (listIntevLikars = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.CallViewProfilLikar = "WinNsiLikar";
                      WinNsiLikar NewOrder = new WinNsiLikar();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.CallViewProfilLikar = "";
                      if (selectedIntevLikar != null && WindowMain.LikarIntert2.Text.ToString().Trim() != "")
                      { 
                          selectedIntevLikar.kodDoctor = WindowMain.LikarIntert2.Text.Substring(0, WindowMain.LikarIntert2.Text.IndexOf(":"));
                          selectedIntevLikar.nameDoctor = WindowMain.LikarIntert2.Text.Substring(WindowMain.LikarIntert2.Text.IndexOf(":"), WindowMain.LikarIntert2.Text.Length - WindowMain.LikarIntert2.Text.IndexOf(":"));

                      }
                  }));
            }
        }

        private RelayCommand? listPacientIntevLikars;
        public RelayCommand ListPacientIntevLikars
        {
            get
            {
                return listPacientIntevLikars ??
                  (listPacientIntevLikars = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.CallViewProfilLikar = "WinNsiPacient";
                      WinNsiPacient NewOrder = new WinNsiPacient();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.CallViewProfilLikar = "";
                      if (selectedIntevLikar != null && WindowMain.LikarIntert3.Text.ToString().Trim() != "")
                      { 
                        selectedIntevLikar.kodPacient = WindowMain.LikarIntert3.Text.Substring(0, WindowMain.LikarIntert3.Text.IndexOf(":"));
                        selectedIntevLikar.namePacient = WindowMain.LikarIntert3.Text.Substring(WindowMain.LikarIntert3.Text.IndexOf(":"), WindowMain.LikarIntert3.Text.Length-WindowMain.LikarIntert3.Text.IndexOf(":"));
                      } 

                  }));
            }
        }

        private RelayCommand? listRecomendaciyaIntevLikars;
        public RelayCommand ListRecomendaciyaIntevLikars
        {
            get
            {
                return listRecomendaciyaIntevLikars ??
                  (listRecomendaciyaIntevLikars = new RelayCommand(obj =>
                  {
                      WinNsiListRecommen NewOrder = new WinNsiListRecommen();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? listDiagnozIntevLikars;
        public RelayCommand ListDiagnozIntevLikars
        {
            get
            {
                return listDiagnozIntevLikars ??
                  (listDiagnozIntevLikars = new RelayCommand(obj =>
                  {
                      WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? onVisibleObjIntevLikars;
        public RelayCommand OnVisibleObjIntevLikars
        {
            get
            {
                return onVisibleObjIntevLikars ??
                  (onVisibleObjIntevLikars = new RelayCommand(obj =>
                  {
                      if (IndexAddEdit == "")
                      { 
                           if (ColectionInterviewIntevLikars != null)
                          {
                              MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("BackMain");

                              if (editboolIntevLikar == true) BoolFalseIntevLikarCompl();
                              if (ColectionInterviewIntevLikars.Count != 0 && WindowMen.ColectionIntevLikarTablGrid.SelectedIndex>=0)
                              { 
                                  WindowIntevLikar.LikarFoldInterv.Visibility = Visibility.Visible;
                                  selectedColectionIntevLikar = ColectionIntevLikars[WindowMen.ColectionIntevLikarTablGrid.SelectedIndex];
                                  selectedIntevLikar= ColectionInterviewIntevLikars[WindowMen.ColectionIntevLikarTablGrid.SelectedIndex];
                                  if (selectedColectionIntevLikar.kodPacient != null && selectedColectionIntevLikar.kodPacient.Length != 0) MethodPacientIntevLikars(selectedColectionIntevLikar, true);
                                  if (selectedColectionIntevLikar.kodDoctor != null && selectedColectionIntevLikar.kodDoctor.Length != 0) MethodDoctorIntevLikars(selectedColectionIntevLikar, true);
                                  if (selectedColectionIntevLikar.kodProtokola != null && selectedColectionIntevLikar.kodProtokola.Length != 0) MethodProtokolaIntevLikars(selectedColectionIntevLikar, true);
                               
                              }

 
                           }                    
                      }
 


                  }));
            }
        }

        
        
        private RelayCommand? selectedListWorkDiagnoz;
        public RelayCommand SelectedListWorkDiagnoz
        {
            get
            {
                return selectedListWorkDiagnoz ??
                  (selectedListWorkDiagnoz = new RelayCommand(obj =>
                  {
                      AddAllWorkDiagnozs();
                      MapOpisViewModel.SelectActivGrupDiagnoz = "WorkDiagnozs";
                      WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 150;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        #endregion
        #endregion
    }
}


