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
        // ViewModelReestrCompletedInterview модель ViewQualification
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>

        private bool editboolColectionCompl = false, loadboolColectionCompl = false;
        public static string ModelCall = "";
        public static string Colectioncontroller =  "/api/ColectionInterviewController/";
        public static string Completedcontroller =  "/api/CompletedInterviewController/";
        public static string Doctorcontroller =  "/api/ApiControllerDoctor/";
        public static ModelColectionInterview selectedColectionInterview;
        public static ColectionInterview selectedColection;
        public static ModelDependency Insert;
        public static ObservableCollection<ModelColectionInterview> ColectionInterviewInterviews { get; set; }
        public static ObservableCollection<ColectionInterview> ColectionInterviews { get; set; }
        public ModelColectionInterview SelectedColectionInterview
        {
            get { return selectedColectionInterview; }
            set { selectedColectionInterview = value; OnPropertyChanged("SelectedColectionInterview"); }
        }


        public static void ObservablelColectionInterview(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ColectionInterviews = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
            BildModelColection();
            WindowMen.ColectionTablGrid.ItemsSource = ColectionInterviewInterviews;
        }

        private static void BildModelColection()
        {

            ObservableCollection<ColectionInterview> tmpColection = new ObservableCollection<ColectionInterview>();


            ColectionInterviewInterviews = new ObservableCollection<ModelColectionInterview>();
            foreach (ColectionInterview colectionInterview in ColectionInterviews)
            {
                if (colectionInterview.kodPacient == null)
                {
                    string json = Colectioncontroller + colectionInterview.id.ToString() + "/0/0";
                    CallServer.PostServer(Colectioncontroller, json, "DELETE");
                }
                else tmpColection.Add(colectionInterview);
            }
            ColectionInterviews = tmpColection;
            foreach (ColectionInterview colectionInterview in ColectionInterviews)
            {
                selectedColectionInterview = new ModelColectionInterview();
                if (colectionInterview.kodPacient != "Гість" && colectionInterview.kodPacient.Length != 0) MethodPacient(colectionInterview, false);
                else { WindowMen.Intert3.Text = "Гість"; selectedColectionInterview.namePacient = "Гість"; }
                if (colectionInterview.kodDoctor != null && colectionInterview.kodDoctor.Length != 0) MethodDoctor(colectionInterview, false);
                if (colectionInterview.kodProtokola != null && colectionInterview.kodProtokola.Length != 0) MethodProtokola(colectionInterview, false);

                selectedColectionInterview.id = colectionInterview.id;
                selectedColectionInterview.kodComplInterv = colectionInterview.kodComplInterv;
                selectedColectionInterview.kodProtokola = colectionInterview.kodProtokola;
                selectedColectionInterview.dateInterview = colectionInterview.dateInterview;
                selectedColectionInterview.resultDiagnoz = colectionInterview.resultDiagnoz;
                ColectionInterviewInterviews.Add(selectedColectionInterview);  
            }

        }

        private static void MethodPacient(ColectionInterview colectionInterview, bool boolname)
        {
            if (colectionInterview.kodPacient != "Гість")
            {

                string json = Pacientcontroller + colectionInterview.kodPacient.ToString() + "/0/0/0/0";
                CallServer.PostServer(Pacientcontroller, json, "GETID");
                if (CallServer.ResponseFromServer.Contains("[]") == false)
                {
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                    selectedColectionInterview.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
                    if (boolname == true) WindowMen.Intert3.Text = selectedColectionInterview.namePacient;
                }
            }
            else
            {
                WindowMen.Intert3.Text = "Гість"; selectedColectionInterview.namePacient = "Гість";
            }
         
        }

        private static void MethodDoctor(ColectionInterview colectionInterview, bool boolname)
        {

            
            var json = Doctorcontroller + colectionInterview.kodDoctor.ToString() + "/0/0";
            CallServer.PostServer(Doctorcontroller, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") ==false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                selectedColectionInterview.nameDoctor = Insert.name + " " + Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                if (boolname == true) WindowMen.Intert2.Text = selectedColectionInterview.nameDoctor;
            }

        }

        private static void MethodProtokola(ColectionInterview colectionInterview, bool boolname)
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
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString()+ "/0/0";
                    CallServer.PostServer(Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectedColectionInterview.nameDiagnoz = Insert1.nameDiagnoza;
                        if (boolname == true) WindowMen.Interviewt6.Text = Insert1.nameDiagnoza;
                    }

                    MainWindow.UrlServer = Recomencontroller;
                    json = Recomencontroller + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedColectionInterview.nameRecomen = Insert2.contentRecommendation;
                        if (boolname == true) WindowMen.Interviewt5.Text = Insert2.contentRecommendation;
                    }
                }    

            }
        }


        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadColectionInterview;
        public RelayCommand LoadColectionInterview
        {
            get
            {
                return loadColectionInterview ??
                  (loadColectionInterview = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadtableColectionInterview(); //if (loadboolColectionCompl == false)
                  }));
            }
        }

  
        private void MethodLoadtableColectionInterview()
        {
            IndexAddEdit = "editCommand";
            WindowMen.Loadinterv.Visibility = Visibility.Hidden;
            CallServer.PostServer(Colectioncontroller, Colectioncontroller, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionInterview(CmdStroka);
        }

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? addColectionInterview;
        public RelayCommand AddColectionInterview
        {
            get
            {
                return addColectionInterview ??
                  (addColectionInterview = new RelayCommand(obj =>
                  {
                      MainWindow.MessageError = "Увага!" + Environment.NewLine +
                        "Додавання протоколів опитування не виконується! ";
                      SelectedFalseLogin();
                  }));
            }
        }



        private void BoolTrueColectionCompl()
        {
            //addtboolInterview = true;
            editboolColectionCompl = true;
            WindowMen.FoldInterv.Visibility = Visibility.Visible;
            WindowMen.FolderLikar.Visibility = Visibility.Visible;
            WindowMen.FolderPacient.Visibility = Visibility.Visible;
            WindowMen.FolderRecomen.Visibility = Visibility.Visible;
            WindowMen.FolderDiagn.Visibility = Visibility.Visible;
            WindowMen.Interviewt7.IsEnabled = true;
            WindowMen.Interviewt7.Background = Brushes.AntiqueWhite;

        }

        private void BoolFalseColectionCompl()
        {
            //addtboolInterview = false;
            editboolColectionCompl = false;
            WindowMen.FoldInterv.Visibility = Visibility.Hidden;
            WindowMen.FolderLikar.Visibility = Visibility.Hidden;
            WindowMen.FolderPacient.Visibility = Visibility.Hidden;
            WindowMen.FolderRecomen.Visibility = Visibility.Hidden;
            WindowMen.FolderDiagn.Visibility = Visibility.Hidden;
            WindowMen.Interviewt7.IsEnabled = false;
            WindowMen.Interviewt7.Background = Brushes.White;
        }
        // команда удаления
        private RelayCommand? removeColectionCompl;
        public RelayCommand RemoveColectionCompl
        {
            get
            {
                return removeColectionCompl ??
                  (removeColectionCompl = new RelayCommand(obj =>
                  {
                      if (selectedColectionInterview != null)
                      {
                          MessageDeleteData();
                          string json = Completedcontroller + selectedColectionInterview.kodComplInterv.ToString()+"/0";
                          CallServer.PostServer(Completedcontroller, json, "DELETE");
                          MainWindow.UrlServer = Colectioncontroller;
                          json = Colectioncontroller + selectedColectionInterview.id.ToString()+"/0/0";
                          CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                          ColectionInterviewInterviews.Remove(selectedColectionInterview);
                          selectedColectionInterview = new ModelColectionInterview();
                      }
                      BoolFalseColectionCompl();
                      WindowMen.ColectionTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                  },
                 (obj) => ColectionInterviewInterviews != null));
            }
        }

        // команда  редактировать
        private RelayCommand? editColectionCompl;
        public RelayCommand? EditColectionCompl
        {
            get
            {
                return editColectionCompl ??
                  (editColectionCompl = new RelayCommand(obj =>
                  {
                      if (WindowMen.ColectionTablGrid.SelectedIndex >= 0)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                        "Редагування протоколів опитування не виконується! ";
                          SelectedFalseLogin();

                          //IndexAddEdit = "editCommand";
                          //if (editboolColectionCompl == false && selectedColectionInterview != null)
                          //{
                          //    selectedColection = new ColectionInterview();
                          //    selectedColection = ColectionInterviews[WindowMen.ColectionTablGrid.SelectedIndex];
                          //    BoolTrueColectionCompl();
                          //}
                          //else
                          //{
                          //    BoolFalseColectionCompl();
                          //    IndexAddEdit = "";
                          //    GetidkodProtokola = "";
                          //    WindowMen.ColectionTablGrid.SelectedItem = null;
                          //}                     
                      }


                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveColectionCompl;
        public RelayCommand SaveColectionCompl
        {
            get
            {
                return saveColectionCompl ??
                  (saveColectionCompl = new RelayCommand(obj =>
                  {
                      BoolFalseColectionCompl();
                      // ОБращение к серверу измнить корректируемую запись в БД
                      if (selectedColectionInterview != null)
                      {
                          MainWindow.UrlServer = Protocolcontroller;
                          var json = Protocolcontroller + "0/" + WindowMen.Intert1.Text.ToString();
                          CallServer.PostServer(Protocolcontroller, json, "GETID");
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                          if (Insert == null) Insert = new ModelDependency();
                         
                          if (Insert.kodRecommend != WindowMen.Interviewt5.Text.ToString().Substring(0, WindowMen.Interviewt5.Text.ToString().IndexOf(":")) ||
                          Insert.kodDiagnoz != WindowMen.Interviewt6.Text.ToString().Substring(0, WindowMen.Interviewt6.Text.ToString().IndexOf(":")))
                          {
                              Insert.kodProtokola = WindowMen.Intert1.Text.ToString();
                              Insert.kodDiagnoz = WindowMen.Interviewt6.Text.ToString().Substring(0, WindowMen.Interviewt6.Text.ToString().IndexOf(":"));
                              Insert.kodRecommend = WindowMen.Interviewt5.Text.ToString().Substring(0, WindowMen.Interviewt5.Text.ToString().IndexOf(":"));
                              string Method = Insert.id == 0 ? "POST" : "PUT";
                              MainWindow.UrlServer = Protocolcontroller;
                              var jsonDepency = JsonConvert.SerializeObject(Insert);
                              CallServer.PostServer(Protocolcontroller, jsonDepency, Method);

                          }
                          string Intert20 = WindowMen.Intert2.Text.ToString().Contains(":") ? WindowMen.Intert2.Text.ToString().Substring(0, WindowMen.Intert2.Text.ToString().IndexOf(":")) : WindowMen.Intert2.Text.ToString();
                          selectedColection.kodDoctor = WindowMen.Intert2.Text.Trim().Length==0 ? "" : Intert20;
                          string Intert30 = WindowMen.Intert3.Text.ToString().Contains(":") ? WindowMen.Intert3.Text.ToString().Substring(0, WindowMen.Intert3.Text.ToString().IndexOf(":")) : WindowMen.Intert3.Text.ToString();
                          selectedColection.kodPacient = WindowMen.Intert3.Text.Trim().Length == 0 ? "" : Intert30; 
                          selectedColection.kodProtokola = WindowMen.Intert1.Text.ToString();
                          selectedColection.resultDiagnoz = WindowMen.Interviewt7.Text.ToString();
                          selectedColection.kodComplInterv = selectedColectionInterview.kodComplInterv;
                          
                          MainWindow.UrlServer = Colectioncontroller;
                          json = JsonConvert.SerializeObject(selectedColection);
                          CallServer.PostServer(MainWindow.UrlServer, json, "PUT");
                      }
     
                      IndexAddEdit = "";
                      WindowMen.ColectionTablGrid.SelectedItem = null;

                  }));
            }
        }

        
        // команда печати
        RelayCommand? printColectionCompl;
        public RelayCommand PrintColectionCompl
        {
            get
            {
                return printColectionCompl ??
                  (printColectionCompl = new RelayCommand(obj =>
                  {
                      if (ColectionInterviewInterviews != null)
                      {
                          MessageBox.Show("Результат діагнозу :" + ColectionInterviewInterviews[0].kodProtokola.ToString());
                      }
                  },
                 (obj) => ColectionInterviewInterviews != null));
            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntreview;
        public RelayCommand ReadColectionIntreview
        {
            get
            {
                return readColectionIntreview ??
                  (readColectionIntreview = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = selectedColectionInterview.kodComplInterv+"/0";
                      ComandreadColectionIntreview();
                      BoolFalseInterview();
                      editboolColectionCompl = false;
                  }));
            }
        }


        private void ComandreadColectionIntreview()
        {

            WinCreatIntreview NewOrder = new WinCreatIntreview();
            NewOrder.Left = 600;
            NewOrder.Top = 130;
            NewOrder.ShowDialog();


        }

        private RelayCommand? listLikar;
        public RelayCommand ListLikar
        {
            get
            {
                return listLikar ??
                  (listLikar = new RelayCommand(obj =>
                  {
                      WinNsiLikar NewOrder = new WinNsiLikar();
                      NewOrder.Left = 450;
                      NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? listPacient;
        public RelayCommand ListPacient
        {
            get
            {
                return listPacient ??
                  (listPacient = new RelayCommand(obj =>
                  {
                      WinNsiPacient NewOrder = new WinNsiPacient();
                      NewOrder.Left = 450;
                      NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? listRecomendaciya;
        public RelayCommand ListRecomendaciya
        {
            get
            {
                return listRecomendaciya ??
                  (listRecomendaciya = new RelayCommand(obj =>
                  {
                      WinNsiListRecommen NewOrder = new WinNsiListRecommen();
                      NewOrder.Left = 450;
                      NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? listDiagnoz;
        public RelayCommand ListDiagnoz
        {
            get
            {
                return listDiagnoz ??
                  (listDiagnoz = new RelayCommand(obj =>
                  {
                      WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                      NewOrder.Left = 450;
                      NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? onVisibleObj;
        public RelayCommand OnVisibleObj
        {
            get
            {
                return onVisibleObj ??
                  (onVisibleObj = new RelayCommand(obj =>
                  {
                      if (ColectionInterviews != null)
                      { 
                          WindowMen.FoldInterv.Visibility = Visibility.Visible;
                          if (editboolColectionCompl == true) BoolFalseColectionCompl();
                          if (WindowMen.ColectionTablGrid.SelectedIndex < 0) return;
                          ColectionInterview selectedColection = ColectionInterviews[WindowMen.ColectionTablGrid.SelectedIndex];
                          if (selectedColection.kodPacient != null && selectedColection.kodPacient.Length != 0) MethodPacient(selectedColection,true);
                          if (selectedColection.kodDoctor != null && selectedColection.kodDoctor.Length != 0) MethodDoctor(selectedColection, true);
                          if (selectedColection.kodProtokola != null && selectedColection.kodProtokola.Length != 0) MethodProtokola(selectedColection, true);
                      
                      }
 

                  }));
            }
        }
        

        #endregion
        #endregion
    }
}

