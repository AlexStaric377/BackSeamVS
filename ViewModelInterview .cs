﻿using System;
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
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public static MainWindow WindowInterv = MainWindow.LinkNameWindow("BackMain");
        public static bool editboolInterview = false, addtboolInterview = false, loadboolInterview = false, addInterviewGrDetail=true;
        private string edittextInterview = "", AddEdit ="";
        public static int selectedInterviewIndex = 0;
        public static string GetidkodProtokola = "";
        public static string Interviewcontroller =  "/api/InterviewController/";
        public static string Controlleroutfile = "/api/UnLoadController/";

        public static ModelInterview selectedInterview;
        public static ObservableCollection<ModelInterview> ModelInterviews { get; set; }

        public ModelInterview SelectedInterview
        {
            get { return selectedInterview; }
            set { selectedInterview = value; OnPropertyChanged("SelectedInterview"); }
        }
        public ModelResultInterview selectedResultInterview;
        public ModelResultInterview SelectedResultInterview

        {
            get { return selectedResultInterview; }
            set { selectedResultInterview = value; OnPropertyChanged("SelectedResultInterview"); }
        }
        public static void ObservableModelInterview(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
            List<ModelInterview> res = result.ModelInterview.ToList();
            ModelInterviews = new ObservableCollection<ModelInterview>((IEnumerable<ModelInterview>)res);
            WindowInterv.InterviewTablGrid.ItemsSource = ModelInterviews.OrderBy(x=>x.kodProtokola);
        }



        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadInterview;
        public RelayCommand LoadInterview
        {
            get
            {
                return loadInterview ??
                  (loadInterview = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadtableInterview();
                  }));
            }
        }

        // команда добавления нового объекта
        private RelayCommand? addInterview;
        public RelayCommand AddInterview
        {
            get
            {
                return addInterview ??
                  (addInterview = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandInterview(); }));
            }
        }

        private void AddComandInterview()
        {
            if (loadboolInterview == true) MetodNewEkzemplar();          
            if (loadboolInterview == false) MethodLoadtableInterview();
            MethodaddcomInterview();
        }

        private void MetodNewEkzemplar()
        {
            selectedInterview = new ModelInterview();
            SelectedInterview = new ModelInterview();
            modelDependency = new ModelDependency();
            SelectedResultInterview = new ModelResultInterview();
        }
        private void MethodLoadtableInterview()
        {
            MetodNewEkzemplar();
            ModelInterviews = new ObservableCollection<ModelInterview>();
            WindowInterv.InterviewGroup.Visibility = Visibility.Hidden;
            CallServer.PostServer(Interviewcontroller, Interviewcontroller, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelInterview(CmdStroka);
            AddEdit = "";
            loadboolInterview = true;

        }

        private void MethodaddcomInterview()
        {
            AddEdit = "addCommand";
            if (addtboolInterview == false) BoolTrueInterview();
            else BoolFalseInterview();

        }


        private void BoolTrueInterview()
        {
            addtboolInterview = true;
            editboolInterview = true;
            WindowInterv.Interviewt2.IsEnabled = true;
            WindowInterv.Interviewt2.Background = Brushes.AntiqueWhite;
            WindowInterv.FolderInterview.Visibility = Visibility.Visible;
            WindowInterv.InterviewOpis.IsEnabled = true;
            WindowInterv.InterviewOpis.Background = Brushes.AntiqueWhite;
            WindowInterv.InterviewTextUri.IsEnabled = true;
            WindowInterv.InterviewTextUri.Background = Brushes.AntiqueWhite;

            WindowInterv.FolderDiagnozInterview.Visibility = Visibility.Visible;
            WindowInterv.FolderRecomenInterview.Visibility = Visibility.Visible;
            WindowInterv.InterviewLab3.Text = AddEdit == "addCommand" ? "Створити" : "Корегувати";
            WindowInterv.InterviewTablGrid.IsEnabled = false;
            if (AddEdit == "addCommand")
            {
                WindowMen.BorderLoadInterview.IsEnabled = false;
                WindowMen.BorderGhangeInterview.IsEnabled = false;
                WindowMen.BorderDeleteInterview.IsEnabled = false;
                WindowMen.BorderPrintInterview.IsEnabled = false;
            }
            if (AddEdit == "editCommand")
            {
                WindowMen.BorderLoadInterview.IsEnabled = false;
                WindowMen.BorderAddInterview.IsEnabled = false;
                WindowMen.BorderDeleteInterview.IsEnabled = false;
                WindowMen.BorderPrintInterview.IsEnabled = false;
            }
        }

        private void BoolFalseInterview()
        {
            addtboolInterview = false;
            editboolInterview = false;
            WindowInterv.Interviewt2.IsEnabled = false;
            WindowInterv.Interviewt2.Background = Brushes.White;
            WindowInterv.InterviewOpis.IsEnabled = false;
            WindowInterv.InterviewOpis.Background = Brushes.White;
            WindowInterv.InterviewTextUri.IsEnabled = false;
            WindowInterv.InterviewTextUri.Background = Brushes.White;

            WindowInterv.FolderInterview.Visibility = Visibility.Hidden;
            WindowInterv.FolderDiagnozInterview.Visibility = Visibility.Hidden;
            WindowInterv.FolderRecomenInterview.Visibility = Visibility.Hidden;
            WindowInterv.FolderUriInterview.Visibility = Visibility.Hidden;
            WindowInterv.InterviewLab3.Text = "Переглянути" + Environment.NewLine + "опитування";
            WindowInterv.InterviewTablGrid.IsEnabled = true;
            WindowMen.BorderLoadInterview.IsEnabled = true;
            WindowMen.BorderGhangeInterview.IsEnabled = true;
            WindowMen.BorderDeleteInterview.IsEnabled = true;
            WindowMen.BorderPrintInterview.IsEnabled = true;
            WindowMen.BorderAddInterview.IsEnabled = true;
            AddEdit = "";
        }
        // команда удаления
        private RelayCommand? removeInterview;
        public RelayCommand RemoveInterview
        {
            get
            {
                return removeInterview ??
                  (removeInterview = new RelayCommand(obj =>
                  {
                      if (selectedInterview != null)
                      {
                          if (selectedInterview.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = ViewModelCreatInterview.pathcontroler + selectedInterview.kodProtokola;
                              CallServer.PostServer(ViewModelCreatInterview.pathcontroler, json, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  json = ViewModelCreatInterview.pathcontroler + selectedInterview.kodProtokola + "/0";
                                  CallServer.PostServer(ViewModelCreatInterview.pathcontroler, json, "DELETE");
                              }

                              json = Interviewcontroller + selectedInterview.id.ToString();
                              CallServer.PostServer(Interviewcontroller, json, "DELETE");
                              ModelInterviews.Remove(selectedInterview);
                              selectedInterview = new ModelInterview();

                              BoolFalseInterview();
                              WindowInterv.InterviewTablGrid.SelectedItem = null;
                              SelectedResultInterview = new ModelResultInterview();
                          }

                      }
                      
                  },
                 (obj) => ModelInterviews != null));
            }
        }

        // команда  редактировать
        private RelayCommand? editInterview;
        public RelayCommand? EditInterview
        {
            get
            {
                return editInterview ??
                  (editInterview = new RelayCommand(obj =>
                  {
                      
                      AddEdit = "editCommand";
                      if (editboolInterview == false && selectedInterview != null && selectedInterview.id !=0)
                      {
                          if (selectedInterview.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          BoolTrueInterview();
                          edittextInterview = WindowInterv.Interviewt2.Text.ToString();
                      }
                      else
                      {
                          BoolFalseInterview();
                          WindowInterv.InterviewLab3.Text = "Переглянути" + Environment.NewLine + "опитування";
                          WindowInterv.InterviewTablGrid.SelectedItem = null;
                      }

                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveInterview;
        public RelayCommand SaveInterview
        {
            get
            {
                return saveInterview ??
                  (saveInterview = new RelayCommand(obj =>
                  {
                      string json = "";

                      if (WindowInterv.Interviewt2.Text.Length != 0 && selectedInterview !=null)
                      {
                            if (WindowInterv.InterviewDependencyt3.Text.ToString().Length == 0)
                            {

                                WinNsiListRecommen NewOrder = new WinNsiListRecommen();
                                NewOrder.Left = (MainWindow.ScreenWidth / 2);
                                NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                                NewOrder.ShowDialog();
                                if (WindowInterv.InterviewDependencyt3.Text.Length ==0)
                                {
                                  MainWindow.MessageError = "Увага!  " + Environment.NewLine +
                                  " Ви не встановили відповідні рекомендації щодо подальших дій." + Environment.NewLine +
                                  "Збереження опитування можливе після встановлення рекомендацій.";
                                  MessageWarn NewEnd = new MessageWarn(MainWindow.MessageError, 2, 8);
                                  NewEnd.ShowDialog();
                                  return;
                                }
                                modelDependency.kodProtokola = selectedInterview.kodProtokola;
                                modelDependency.kodRecommend = WindowInterv.InterviewDependencyt3.Text.Substring(0, WindowMain.InterviewDependencyt3.Text.IndexOf(":"));
                                WindowInterv.InterviewDependencyt3.Text = WindowInterv.InterviewDependencyt3.Text.Substring(WindowMain.InterviewDependencyt3.Text.IndexOf(":")+1, WindowMain.InterviewDependencyt3.Text.Length- (WindowMain.InterviewDependencyt3.Text.IndexOf(":")+1));
                            }                      
 
                          selectedInterview.opistInterview = WindowInterv.InterviewOpis.Text.Length>0 ? WindowInterv.InterviewOpis.Text.Replace("/", " ") : selectedInterview.opistInterview.Replace("/", " ");
                          selectedInterview.uriInterview = WindowInterv.InterviewTextUri.Text.Length>0 ? WindowInterv.InterviewTextUri.Text : selectedInterview.uriInterview;
                          selectedInterview.nametInterview = WindowInterv.Interviewt2.Text.Length>0 ? WindowInterv.Interviewt2.Text : selectedInterview.nametInterview;
                          selectedInterview.idUser = RegIdUser;
                          
                          // ОБращение к серверу добавить или  измнить  запись в БД
                          json = JsonConvert.SerializeObject(selectedInterview);
                          string method = selectedInterview.id == 0 ? "POST" : "PUT";
                          CallServer.PostServer(Interviewcontroller, json, method);
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          ModelInterview Insertstroka = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
  
                          json = CallServer.ResponseFromServer.Replace("/","*").Replace("?", "_");
                          if (json.Length < 1024)
                          {
                                CallServer.PostServer(Controlleroutfile, Controlleroutfile + "Interview/" + json + "/0", "GETID");
                          }

                          // дозапись в справочник взаимосвязи диагнозов рекомендаций и протоколов интервью

                          if (method == "POST")
                          {
                              ModelInterviews.Add(Insertstroka);
                              WindowInterv.InterviewTablGrid.ItemsSource = ModelInterviews;
                              modelDependency.kodProtokola = selectedInterview.kodProtokola;
                              modelDependency.kodDiagnoz = WindowInterv.InterviewDependencyt2.Text.ToString().Length == 0 ? "" : WindowInterv.InterviewDependencyt2.Text.Substring(0, WindowMain.InterviewDependencyt2.Text.IndexOf(":"));
                          }
                          json = JsonConvert.SerializeObject(modelDependency);
                          CallServer.PostServer(pathcontrolerDependency, json, method);
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]") == false)
                          {
                              CallServer.PostServer(Controlleroutfile, Controlleroutfile + "Interview/" + json + "/0", "GETID");
                              MessageOk();
                          }

                      }
                      BoolFalseInterview();
                      WindowInterv.InterviewTablGrid.SelectedItem = null;
                      SelectedResultInterview = new  ModelResultInterview();
                      MapOpisViewModel.ModelCall =  "";

                  }));
            }
        }

        private void MessageOk()
        {

            MainWindow.MessageError = "Встановлення відповідного попереднього діагнозу та рекомендації " + Environment.NewLine +
            " за цим опитаванням  успішно завершено.";
            MessageWarn NewEnd = new MessageWarn(MainWindow.MessageError, 2, 5);
            NewEnd.ShowDialog();
        }

        public void SelectNewInterview()
        {

            // KodProtokola = "PRT.000000001"           
            int _keyInterview = 1, setindex =0;
            if (ModelInterviews.Count >0 )
            {
                if (ModelInterviews[0].kodProtokola.Length != 0)
                { 
                    _keyInterview = Convert.ToInt32(ModelInterviews[0].kodProtokola.Substring(ModelInterviews[0].kodProtokola.LastIndexOf(".") + 1, ModelInterviews[0].kodProtokola.Length - (ModelInterviews[0].kodProtokola.LastIndexOf(".") + 1)));
                }
                for (int i = 0; i < ModelInterviews.Count; i++)
                {
                    if (ModelInterviews[i].kodProtokola.Length != 0)
                    {
                        setindex = Convert.ToInt32(ModelInterviews[i].kodProtokola.Substring(ModelInterviews[i].kodProtokola.LastIndexOf(".") + 1, ModelInterviews[i].kodProtokola.Length - (ModelInterviews[i].kodProtokola.LastIndexOf(".") + 1)));
                    }
                    if (_keyInterview < setindex) _keyInterview = setindex;
                }
                _keyInterview++;
                string _repl = "000000000";
                _repl = _repl.Length - _keyInterview.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - _keyInterview.ToString().Length) : "";
                
                bool breaktrue = false;
                while (breaktrue == false)
                { 
                    CallServer.PostServer(pathcontrolerContent, pathcontrolerContent + "PRT." + _repl + _keyInterview.ToString(), "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]") == true)  break; 
                    _keyInterview++;
                }
                selectedInterview.kodProtokola = "PRT." + _repl + _keyInterview.ToString();
            }
            else selectedInterview.kodProtokola = "PRT.000000001";
            
        }

        // команда печати
        RelayCommand? printInterview;
        public RelayCommand PrintInterview
        {
            get
            {
                return printInterview ??
                  (printInterview = new RelayCommand(obj =>
                  {
                      if (ModelInterviews != null)
                      {
                          MessageBox.Show("Інтерв'ю :" + ModelInterviews[0].nametInterview.ToString());
                      }
                  },
                 (obj) => ModelInterviews != null));
            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? addCreatIntreview;
        public RelayCommand CreatIntreview
        {
            get
            {
                return addCreatIntreview ??
                  (addCreatIntreview = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.ModelCall = AddEdit == ""? "ModelInterview" : "";
                      if (AddEdit == "addCommand") SelectNewInterview();
                      GetidkodProtokola = selectedInterview.kodProtokola;
                      ComandCreatIntreview(); }));
            }
        }

 
        private void ComandCreatIntreview()
        {

            if (WindowInterv.InterviewDependencyt2.Text == "")
            {
                MainWindow.MessageError = "Створення опитування здійснюється " + Environment.NewLine +
                           "після встановлення попереднього діагнозу.";
                MessageWarn NewEnd = new MessageWarn(MainWindow.MessageError, 2, 5);
                NewEnd.ShowDialog();
                return;
            }
            if (WindowInterv.Interviewt2.Text == "")
            {
                MainWindow.MessageError = "Створення опитування неможливо. " + Environment.NewLine +
                           "Не введено назву опитування.";
                MessageWarn NewEnd = new MessageWarn(MainWindow.MessageError, 2, 5);
                NewEnd.ShowDialog();
                return;
            }
            
            WinCreatIntreview NewOrder = new WinCreatIntreview();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) - 220;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            MapOpisViewModel.ModelCall = "";

        }

        private RelayCommand? onVisibleFolderInterv;
        public RelayCommand OnVisibleFolderInterv
        {
            get
            {
                return onVisibleFolderInterv ??
                  (onVisibleFolderInterv = new RelayCommand(obj =>
                  {
                       BoolFalseInterview();
                       WindowInterv.InterviewLab3.Text = "Переглянути" + Environment.NewLine + "опитування";
   
                      if (WindowInterv.InterviewTablGrid.SelectedIndex > -1)
                      {
                            int indexintev = WindowInterv.InterviewTablGrid.SelectedIndex;
                            if (ModelInterviews.Count != WindowInterv.InterviewTablGrid.Items.Count)
                            {
                                MethodLoadtableInterview();
                            }

                            if (ModelInterviews[indexintev].kodProtokola == null || ModelInterviews[indexintev].kodProtokola == "") return;

                            WindowInterv.FolderInterview.Visibility = Visibility.Visible;
                            WindowInterv.InterviewLab3.Visibility = Visibility.Visible;
                            WindowInterv.InterviewUri.Visibility = Visibility.Visible;
                            WindowInterv.FolderUriInterview.Visibility = Visibility.Visible;
                            selectedInterview = ModelInterviews[indexintev];
                            WindowInterv.Interviewt2.Text = selectedInterview.nametInterview;
                            WindowInterv.InterviewOpis.Text = selectedInterview.opistInterview;
                            WindowInterv.InterviewTextUri.Text = selectedInterview.uriInterview;
                                
                            selectedInterviewIndex = WindowInterv.InterviewTablGrid.SelectedIndex;
                            string json = pathcontrolerDependency + "0/" + selectedInterview.kodProtokola + "/0";
                            CallServer.PostServer(pathcontrolerDependency, json, "GETID");
                            string CmdStroka = CallServer.ServerReturn();
                            if (CmdStroka.Contains("[]") == false)
                            {
                                  
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                modelDependency = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                                if (modelDependency.kodDiagnoz != "")
                                {
                                    json = controlerViewDiagnoz + modelDependency.kodDiagnoz + "/0/0";
                                    CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                                    { 
                                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                        ModelDiagnoz NameDiagnoz = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                                        WindowInterv.InterviewDependencyt2.Text = NameDiagnoz.nameDiagnoza.ToString();

                                    }
                                }
                                json = controlerModelRecommendation + modelDependency.kodRecommend + "/0";
                                CallServer.PostServer(controlerModelRecommendation, json, "GETID");
                                if (CallServer.ResponseFromServer.Contains("[]") == false)
                                {
                                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                    ModelRecommendation NameRecomen = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                                    WindowInterv.InterviewDependencyt3.Text = NameRecomen.contentRecommendation.ToString();
                                }
                            }
                      }
                      
                  }));
            }
        }

        // команда выбора 
        private RelayCommand? ghangeDiagnozInterview;
        public RelayCommand GhangeDiagnozInterview
        {
            get
            {
                return ghangeDiagnozInterview ??
                  (ghangeDiagnozInterview = new RelayCommand(obj =>
                  { ComandGhangeDiagnozInterview(); }));
            }
        }

        private void ComandGhangeDiagnozInterview()
        {
            selectedDependency = new ModelDependencyDiagnoz();
            MapOpisViewModel.ModelCall = "Dependency";
            MapOpisViewModel.SelectActivGrupDiagnoz = "";
            WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-100;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ModelCall = "";
            if (selectedInterview != null) 
            { 
               if(selectedInterview.kodProtokola !=""  && selectedDependency.kodDiagnoz !="")
               {
                    json = pathcontrolerDependency + "0/" + selectedInterview.kodProtokola + "/0";
                    CallServer.PostServer(pathcontrolerDependency, json, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]") == false)
                    {
                        WindowInterv.Interviewt2.Text = selectedInterview.nametInterview;
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        modelDependency = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                        modelDependency.kodDiagnoz = MapOpisViewModel.selectedDependency.kodDiagnoz;
                        //modelDependency.kodDiagnoz = WindowInterv.InterviewDependencyt2.Text.Substring(0, WindowInterv.InterviewDependencyt2.Text.IndexOf(":")).Trim();
                        json = JsonConvert.SerializeObject(modelDependency);
                        CallServer.PostServer(pathcontrolerDependency, json, "PUT");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                        CmdStroka = CallServer.ServerReturn();
                        if (CmdStroka.Contains("[]") == false)
                        {
                            UnloadCmdStroka("DependencyDiagnoz/", json);
                            MessageOk();
                        }
                    }            
               }
            }
        }

        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? ghangeRecomenInterview;
        public RelayCommand GhangeRecomenInterview
        {
            get
            {
                return ghangeRecomenInterview ??
                  (ghangeRecomenInterview = new RelayCommand(obj =>
                  { ComandghangeRecomenInterview(); }));
            }
        }

        private void ComandghangeRecomenInterview()
        {
            selectedDependency = new ModelDependencyDiagnoz();
            WinNsiListRecommen NewOrder = new WinNsiListRecommen();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
            NewOrder.ShowDialog();
            if(WindowInterv.InterviewDependencyt3.Text.Length >0)
            {
                if (WindowInterv.InterviewDependencyt3.Text.Contains(":"))
                { 
                    modelDependency.kodRecommend = WindowInterv.InterviewDependencyt3.Text.Substring(0, WindowMain.InterviewDependencyt3.Text.IndexOf(":"));
                    WindowInterv.InterviewDependencyt3.Text = WindowInterv.InterviewDependencyt3.Text.Substring(WindowMain.InterviewDependencyt3.Text.IndexOf(":") + 1, WindowMain.InterviewDependencyt3.Text.Length - (WindowMain.InterviewDependencyt3.Text.IndexOf(":") + 1));                
                }

            }

        }

        
        private RelayCommand? loadUriInterview;
        public RelayCommand LoadUriInterview
        {
            get
            {
                return loadUriInterview ??
                  (loadUriInterview = new RelayCommand(obj =>
                  {
                      if (selectedInterview != null)
                      {
                          if (selectedInterview.uriInterview != null)
                          {
                              MainWindow.MessageError = "Увага!" + Environment.NewLine + "Чекайте завантаження сторінки.";
                              SelectedWirning(3);

                              string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                              string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                              string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                              Process Rungoogle = new Process();
                              Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                              Rungoogle.StartInfo.Arguments = selectedInterview.uriInterview;
                              Rungoogle.StartInfo.UseShellExecute = false;
                              Rungoogle.EnableRaisingEvents = true;
                              Rungoogle.Start();                          
                          }
                      
                      }
  


                  }));
            }
        }
        // Выбор названия интервью диагностики 
        private RelayCommand? searchInterveiw;
        public RelayCommand SearchInterveiw
        {
            get
            {
                return searchInterveiw ??
                  (searchInterveiw = new RelayCommand(obj =>
                  {
                      MetodKeyEnterInterveiw();
                  }));
            }
        }

        
        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyTextInterveiw;
        public RelayCommand CheckKeyTextInterveiw
        {
            get
            {
                return checkKeyTextInterveiw ??
                  (checkKeyTextInterveiw = new RelayCommand(obj =>
                  {
                      MetodKeyEnterInterveiw();
                  }));
            }
        }

        private void MetodKeyEnterInterveiw()
        {
            if (CheckStatusUser() == false) return;
            if (WindowInterv.PoiskInterveiw.Text.Trim() != "")
            {
                string jason = Interviewcontroller + "0/0/0/" + WindowInterv.PoiskInterveiw.Text;
                CallServer.PostServer(Interviewcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableModelInterview(CmdStroka);
            }
        }


        

        // команда контроля нажатия клавиши enter
        RelayCommand? selectedListComplaint;
        public RelayCommand SelectedListComplaint
        {
            get
            {
                return selectedListComplaint ??
                  (selectedListComplaint = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.ActCompletedInterview = "NameCompl";
                      NsiComplaint NewOrder = new NsiComplaint();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 90;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.ActCompletedInterview = "";

                      if (MapOpisViewModel.nameFeature3 != "")
                      {
                          string keyComplaint = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));
                          string jason = Interviewcontroller + "0/" + keyComplaint + "/-1/0";
                          CallServer.PostServer(Interviewcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableModelInterview(CmdStroka);
                          AddEdit = "";
                          loadboolInterview = true;

                      }
                  }));
            }
        }
        #endregion
        #endregion
    }
}
