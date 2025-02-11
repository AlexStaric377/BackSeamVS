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
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    class ViewModelAnalogDiagnoz : BaseViewModel
    {
        private MainWindow ReceptionLIkarGuest = MainWindow.LinkNameWindow("BackMain");
        WinAnalogDiagnoz WinAnalog = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
        public static string KodProtokola = "", Likar="";
        private static string pathcontrolerCompleted = "/api/CompletedInterviewController/";
        public static string pathcontrolerContent = "/api/ContentInterviewController/";
        public static ModelInterview selectedResultInterview;
        public static ModelResultInterview selectItogInterview;
        public static ObservableCollection<ModelResultInterview> AnalogDiagnozs { get; set; }
        public ModelResultInterview SelectedAnalogDiagnoz
        { get { return selectItogInterview; } set { selectItogInterview = value; OnPropertyChanged("SelectedAnalogDiagnoz"); } }
        // конструктор класса
        public ViewModelAnalogDiagnoz()
        {
            AnalogDiagnozs = new ObservableCollection<ModelResultInterview>();
            foreach (ModelInterview modelInterview in MapOpisViewModel.AnalogInterviews)
            {
                selectItogInterview = new ModelResultInterview();
                selectItogInterview.nametInterview = modelInterview.nametInterview;
                selectItogInterview.opistInterview = modelInterview.opistInterview;
                selectItogInterview.uriInterview = modelInterview.uriInterview;
                MapOpisViewModel.LoadDiagnozRecomen(modelInterview.kodProtokola);
                selectItogInterview.kodProtokola = modelInterview.kodProtokola;
                selectItogInterview.nameDiagnoza = MapOpisViewModel.NameDiagnoz;
                selectItogInterview.nameRecommendation = MapOpisViewModel.NameRecomendaciya;
                KodProtokola = modelInterview.kodProtokola;
                MapOpisViewModel.GetidkodProtokola = selectItogInterview.kodProtokola;
                MapOpisViewModel.MetodSearchContentInterv(MapOpisViewModel.GetidkodProtokola, pathcontrolerContent);
                selectItogInterview.detailsInterview = MapOpisViewModel.modelColectionInterview.nameInterview;
                AnalogDiagnozs.Add(selectItogInterview);
                
            }

            //selectItogInterview = new ModelResultInterview();
            //KodProtokola = null;
        }

        // загрузка описания диагноза
        private RelayCommand? onVisibleObjDiagnozs;
        public RelayCommand OnVisibleObjDiagnozs
        {
            get
            {
                return onVisibleObjDiagnozs ??
                  (onVisibleObjDiagnozs = new RelayCommand(obj =>
                  {

                      if (WinAnalog.ColectionDiagnozTablGrid.SelectedIndex == -1) return;
                      if (AnalogDiagnozs != null)
                      {
                          selectItogInterview = AnalogDiagnozs[WinAnalog.ColectionDiagnozTablGrid.SelectedIndex];
                          SelectedAnalogDiagnoz = selectItogInterview;
                          KodProtokola = selectItogInterview.kodProtokola;
                          MapOpisViewModel.NameDiagnoz = selectItogInterview.nameDiagnoza;
                          MapOpisViewModel.NameRecomendaciya = selectItogInterview.nameRecommendation;
                          MapOpisViewModel.OpistInterview = selectItogInterview.opistInterview;
                          MapOpisViewModel.UriInterview = selectItogInterview.uriInterview;
                          MapOpisViewModel.modelColectionInterview.dateInterview = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                          MapOpisViewModel.modelColectionInterview.nameDiagnoz = selectItogInterview.nameDiagnoza;
                          MapOpisViewModel.modelColectionInterview.nameInterview = selectItogInterview.nametInterview;
                          MapOpisViewModel.modelColectionInterview.nameRecomen = selectItogInterview.nameRecommendation;
                          MapOpisViewModel.modelColectionInterview.detailsInterview = selectItogInterview.detailsInterview;
                          MapOpisViewModel.modelColectionInterview.kodProtokola = selectItogInterview.kodProtokola;
                          IcdGrDiagnoz();
                          MapOpisViewModel.GetidkodProtokola = selectItogInterview.kodProtokola;
                          MapOpisViewModel.MetodSearchContentInterv(MapOpisViewModel.GetidkodProtokola, pathcontrolerContent);
                          WinAnalog.TextMixUriInterview.Text = MapOpisViewModel.modelColectionInterview.nameInterview;
                      }
                  }));
            }
        }

        private void IcdGrDiagnoz()
        {
            string json = MapOpisViewModel.Protocolcontroller + "0/" + MapOpisViewModel.modelColectionInterview.kodProtokola + "/0";
            CallServer.PostServer(MapOpisViewModel.Protocolcontroller, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = MapOpisViewModel.Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(MapOpisViewModel.Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        MapOpisViewModel.selectIcdGrDiagnoz = Insert1.icdGrDiagnoz.Substring(0, Insert1.icdGrDiagnoz.IndexOf(".")); 
                        MapOpisViewModel.modelColectionInterview.resultDiagnoz = Insert1.UriDiagnoza;
                        json = ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz + "0/" + Insert1.icdGrDiagnoz; // + "/0";
                        CallServer.PostServer(ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz, json, "GETID");
                        if (CallServer.ResponseFromServer.Contains("[]") == false)
                        {
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelGrupDiagnoz insertGrDiagnoz = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                            MapOpisViewModel.selectIcdGrDiagnoz = insertGrDiagnoz.icdGrDiagnoz.Substring(0, insertGrDiagnoz.icdGrDiagnoz.IndexOf(".")); 
                        }
                    }
                }
            }
        }

        // команда закрытия окна
        RelayCommand? closeResult;
        public RelayCommand CloseResult
        {
            get
            {
                return closeResult ??
                  (closeResult = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.SaveAnalogDiagnoz = false;
                      WinAnalogDiagnoz WindowResult = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
                      WindowResult.Close();
                  }));
            }
        }


        // команда закрытия окна
        private RelayCommand? reseptionLikar;
        public RelayCommand ReseptionAnalogLikar
        {
            get
            {
                return reseptionLikar ??
                  (reseptionLikar = new RelayCommand(obj =>
                  {

                      if (MapOpisViewModel.modelColectionInterview.kodProtokola != "")
                      {
                          
                          MapOpisViewModel.ViewAnalogDiagnoz = true;
                          MapOpisViewModel.SaveAnalogDiagnoz = true;
                          MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("BackMain");
                          if (MapOpisViewModel.ViewReceptionPatients == null) MapOpisViewModel.ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
                          MapOpisViewModel.ViewReceptionPatients.Add(MapOpisViewModel.modelColectionInterview);
                          MapOpisViewModel.IndexAddEdit = "addCommand";
                          switch (MapOpisViewModel.ActCompletedInterview)
                          {
                              case "Likar":

                                  WindowIntevLikar.ReceptionPacient7.IsEnabled = true;
                                  WindowIntevLikar.ReceptionPacient7.Background = Brushes.AntiqueWhite;
                                  WindowIntevLikar.ReceptionPacientFoldProfil.Visibility = Visibility.Visible;
                                  break;
                              case "Pacient":
                                  WindowIntevLikar.ReceptionLikarTablGrid.ItemsSource = MapOpisViewModel.ViewReceptionPatients;
                                  WindowIntevLikar.ReceptionLikar7.IsEnabled = true;
                                  WindowIntevLikar.ReceptionLikar7.Background = Brushes.AntiqueWhite;
                                  WindowIntevLikar.ReceptionLikarFolderLikar.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarAddCompInterview.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.ReceptionLikarAddInterv.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarFolderTime.Visibility = Visibility.Visible;

                                  WindowIntevLikar.ReceptionLikarLoadinterv.Content = "Ваші дії: - натиснути на кнопку <Додати>; -вибрати лікаря натиснув" + Environment.NewLine + " на малюнок папки; -ввести дату, час прийому та зміст звернення;-натиснути кнопку 'Зберегти'. ";
                                  WindowIntevLikar.ReceptionLikarLoadinterv.Width = 630;
                                  WindowIntevLikar.ReceptionLikarLoadinterv.Height = 70;
                                  WindowIntevLikar.ReceptionLikarLoadinterv.HorizontalAlignment = HorizontalAlignment.Left;
                                  WindowIntevLikar.ReceptionLikarLoadinterv.FontSize = 12;
                                  WindowIntevLikar.ReceptionLikarLoadinterv.FontWeight = FontWeights.Black;
                                  break;
                              case "Guest":
                                  MapOpisViewModel.addReceptionLIkarGuest = false;
                                  MapOpisViewModel.OnOffStartGuest = true;
                                  MapOpisViewModel.modelColectionInterview.namePacient = "";

                                  WindowIntevLikar.ReceptionLikarFolderLikarGuest.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarFolderGuestTime.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarGuestFoldInterv.Visibility = Visibility.Visible;
                                  WindowIntevLikar.ReceptionLikarGuestCompInterview.Visibility = Visibility.Visible;

                                  WindowIntevLikar.ReseptionZapisLikar.Text = "Ваші дії: - натиснути на кнопку <Додати>; -вибрати лікаря натиснув на малюнок папки; -ввести дату, час прийому та зміст звернення; -натиснути кнопку 'Зберегти'. ";
                                  WindowIntevLikar.ReceptionLikarGuest3.Text = "";
                                  WindowIntevLikar.ReceptionLikarGuest7.IsEnabled = true;
                                  WindowIntevLikar.ReceptionLikarGuest7.Background = Brushes.AntiqueWhite;
                                  break;
                          }


                          if (MapOpisViewModel.ViewRegistrAppoints == null) MapOpisViewModel.ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>();
                          MapOpisViewModel.selectRegistrationAppointment = new ModelRegistrationAppointment();
                          MapOpisViewModel.selectRegistrationAppointment.kodComplInterv = MapOpisViewModel.modelColectionInterview.kodComplInterv;
                          MapOpisViewModel.selectRegistrationAppointment.kodPacient = MapOpisViewModel.modelColectionInterview.kodPacient;
                          MapOpisViewModel.selectRegistrationAppointment.dateInterview = MapOpisViewModel.modelColectionInterview.dateInterview; ;
                          MapOpisViewModel.selectRegistrationAppointment.kodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
                          MapOpisViewModel.selectRegistrationAppointment.kodDoctor = MapOpisViewModel.modelColectionInterview.kodDoctor;
                          MapOpisViewModel.ViewRegistrAppoints.Add(MapOpisViewModel.selectRegistrationAppointment);

                          MethodSelectDoctor("ReseptionAnalogLikar");
                          WinAnalogDiagnoz WindowResult = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
                          if(WindowResult !=null) WindowResult.Close();
 
                      }
                      else InfoNoDiagnoz();
                  }));
            }
        }

        public void MessageRegistrationLikar()
        {
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Likar":
                    MainWindow.MessageError = "Вибрана вами інформація щодо попереднього діагнозу" + Environment.NewLine +
                    "переміщена в закладку 'Розклад прийому пацієнтів'." + Environment.NewLine + "Для подальшого їх використання натисніть на закладку 'Розклад прийому пацієнтів'";
                    break;
                default:
                    MainWindow.MessageError = "Вибрана вами інформація щодо попереднього діагнозу" + Environment.NewLine +
                    "переміщена в закладку 'Запис на прийом до лікаря'." + Environment.NewLine + "Для подальшого їх використання натисніть на закладку 'Запис на прийом до лікаря'";
                    break;
            }

            MapOpisViewModel.SelectedWirning();
        }


        // команда просмотра содержимого интервью
        private RelayCommand? readColectionIntreview;
        public RelayCommand ReadColectionIntreview
        {
            get
            {
                return readColectionIntreview ??
                  (readColectionIntreview = new RelayCommand(obj =>
                  {
                      if (KodProtokola != null && KodProtokola != "")
                      {
                          MapOpisViewModel.IndexAddEdit = "";
                          MapOpisViewModel.GetidkodProtokola = KodProtokola;

                          WinCreatIntreview NewOrder = new WinCreatIntreview();
                          NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                          NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                          NewOrder.ShowDialog();
                      }

                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? continueInterview;
        public RelayCommand ContinueAnalogInterview
        {
            get
            {
                return continueInterview ??
                  (continueInterview = new RelayCommand(obj =>
                  {
                      if (selectItogInterview.nameDiagnoza == "") { MessageNoOpis(); return; }
                      WinAnalogDiagnoz WindowResult = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
                      MapOpisViewModel.ContinueCompletedInterview();
                      WindowResult.Close();
                  }));
            }
        }

        // команда вывода списка профильных мед. учреждений
        RelayCommand? listprofilMedical;
        public RelayCommand ListProfilMedical
        {
            get
            {
                return listprofilMedical ??
                  (listprofilMedical = new RelayCommand(obj =>
                  {
                      MethodSelectDoctor("ListProfilMedical");

                  }));
            }
        }

        private void MethodSelectDoctor(string typeLikar = "")
        {
            if (MapOpisViewModel.modelColectionInterview.kodProtokola != "")
            {
                if (selectItogInterview.kodProtokola != "")
                {
                    Likar = typeLikar;
                    WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                    MedZaklad.ShowDialog();
                }
                MapOpisViewModel.EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();
                if (MapOpisViewModel.EdrpouMedZaklad.Length > 0)
                {
                    MapOpisViewModel.ModelCall = "ReceptionLIkar";
                    WinNsiLikar NewOrder = new WinNsiLikar();
                    NewOrder.ShowDialog();
                    if (MapOpisViewModel.nameDoctor.Length > 0)
                    {
                        MapOpisViewModel.modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":"), MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));

                    }
                    MapOpisViewModel.ModelCall = "";
                }
            }
            else InfoNoDiagnoz();

        }

        // команда закрытия окна
        RelayCommand? selectInterview;
        public RelayCommand SelectInterview
        {
            get
            {
                return selectInterview ??
                  (selectInterview = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.modelColectionInterview.kodProtokola != "")
                      {
                          WinAnalogDiagnoz WindowResult = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
                          MetodSaveIntervievAnalog();
                          MainWindow.MessageError = "Увага! вибраний вами попередній діагноз " + Environment.NewLine +
                           " збережений у реєстрі проведених опитуваннь. Для  його перегляду " + Environment.NewLine +
                           "вам необхідно натиснути закладку 'Перегляд проведених опитуваннь'.";
                          MapOpisViewModel.SelectedFalseLogin(10);
                          WindowResult.Close();
                      }
                      else InfoNoDiagnoz();
                  }));
            }
        }

        public static void MetodSaveIntervievAnalog()
        {
            CallServer.PostServer(MapOpisViewModel.pathcontrolerContent, MapOpisViewModel.pathcontrolerContent + MapOpisViewModel.modelColectionInterview.kodProtokola, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else ObservableContentInterv(CmdStroka);
            MapOpisViewModel.SaveAnalogDiagnoz = true;
            MapOpisViewModel.IndexAddEdit = "addCommand";
            SaveInterview();
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Likar":
                    MapOpisViewModel.MethodLoadtableColectionIntevLikar();
                    break;
                case "Pacient":
                    MapOpisViewModel.MethodLoadtableColectionIntevPacient();
                    break;
            }
        }


        private void InfoNoDiagnoz()
        {
            MainWindow.MessageError = "Увага! Ви не вибрали  попередній діагноз " + Environment.NewLine +
           " Для  його  збереження або перегляду " + Environment.NewLine +
           "вам необхідно натиснути на обраний рядок в списку попередніх діагнозів.";
            MapOpisViewModel.SelectedFalseLogin(10);

        }

        public static void ObservableContentInterv(string CmdStroka)
        {
            MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
            var result = JsonConvert.DeserializeObject<ListModelCompletedInterview>(CmdStroka);
            List<ModelCompletedInterview> res = result.ModelCompletedInterview.ToList();
            MapOpisViewModel.GuestIntervs = new ObservableCollection<ModelCompletedInterview>((IEnumerable<ModelCompletedInterview>)res);
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Likar":
                    WindowMain.TablLikarInterviews.ItemsSource = MapOpisViewModel.GuestIntervs;
                    break;
                case "Pacient":
                    WindowMain.TablPacientInterviews.ItemsSource = MapOpisViewModel.GuestIntervs;
                    break;
            }

        }

        public static void SaveInterview()
        {
            if (MapOpisViewModel.GuestIntervs != null)
            {
                if (MapOpisViewModel.GuestIntervs.Count != 0)
                {
                    MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                    MapOpisViewModel.SelectNewKodComplInteriew();
                    MapOpisViewModel.modelColectionInterview.detailsInterview = "";
                    MapOpisViewModel.NumberstrokaGuest = 0;
                    // Очистка коллекции от двойных  кодов
                    string json = pathcontrolerCompleted + MapOpisViewModel.modelColectionInterview.kodProtokola + "/0";
                    CallServer.PostServer(pathcontrolerCompleted, json, "DELETE");
                    // ОБращение к серверу добавляем запись в соответствии с сформированным списком
                    foreach (ModelCompletedInterview modelCompletedInterview in MapOpisViewModel.GuestIntervs.OrderBy(x => x.kodDetailing))
                    {
                        MapOpisViewModel.modelColectionInterview.detailsInterview = MapOpisViewModel.modelColectionInterview.detailsInterview.Length == 0
                                        ? modelCompletedInterview.kodDetailing + ";" : MapOpisViewModel.modelColectionInterview.detailsInterview + modelCompletedInterview.kodDetailing + ";";
                    }

                    foreach (ModelCompletedInterview modelCompletedInterview in MapOpisViewModel.GuestIntervs)
                    {

                        modelCompletedInterview.id = 0;
                        modelCompletedInterview.numberstr = MapOpisViewModel.NumberstrokaGuest++;
                        modelCompletedInterview.kodComplInterv = MapOpisViewModel.modelColectionInterview.kodComplInterv;

                        json = JsonConvert.SerializeObject(modelCompletedInterview);
                        CallServer.PostServer(pathcontrolerCompleted, json, "POST");
                    }
                    MapOpisViewModel.DiagnozRecomendaciya = MapOpisViewModel.modelColectionInterview.detailsInterview;
                    MapOpisViewModel.AddInterviewProtokol();

                    MapOpisViewModel.OnOffStartGuest = true;
                    WindowMain.TablGuestInterviews.ItemsSource = null;
                    WindowMain.TablLikarInterviews.ItemsSource = null;
                    WindowMain.TablPacientInterviews.ItemsSource = null;
                    MapOpisViewModel.GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                    MapOpisViewModel.TmpGuestIntervs = MapOpisViewModel.GuestIntervs;
                    MapOpisViewModel.IndexAddEdit = "";
                }
            }
        }

        // команда закрытия окна
        private RelayCommand? printDiagnoz;
        public RelayCommand PrintDiagnoz
        {
            get
            {
                return printDiagnoz ??
                  (printDiagnoz = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "";
                      MapOpisViewModel.GetidkodProtokola = KodProtokola;
                      MapOpisViewModel.PrintDiagnoz();

                  }));
            }
        }

        private void MessageNoOpis()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
           "Ви не завантажили детелізацію опису діагнозу із списка попредніх діагнозів ";
            MapOpisViewModel.SelectedFalseLogin(4);
        }

        private RelayCommand? runMixGooglUri;
        public RelayCommand RunMixGooglUri
        {
            get
            {
                return runMixGooglUri ??
                  (runMixGooglUri = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.modelColectionInterview.nameInterview == "") return;
                      MapOpisViewModel.MetodRunGoogle(MapOpisViewModel.modelColectionInterview.nameInterview);

                  }));
            }
        }
        // команда просмотра содержимого интервью
        private RelayCommand? runGoogleUri;
        public RelayCommand RunGooglUri
        {
            get
            {
                return runGoogleUri ??
                  (runGoogleUri = new RelayCommand(obj =>
                  {
                      if (selectItogInterview.uriInterview == "") return;
                      MapOpisViewModel.MetodRunGoogle(selectItogInterview.uriInterview);

                  }));
            }
        }
    }
}
