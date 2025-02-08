using System;
using System.IO;
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
using Microsoft.Extensions.Configuration;
/// Многопоточность
using System.Threading;
using System.Windows.Threading;


namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {
        private int IdItemSelected = 0;
        public static string DiagnozRecomendaciya = "", NameDiagnoz = "", NameRecomendaciya = "", OpistInterview = "", UriInterview = "", StrokaInterview = "";
        public static bool endwhileselected = false, OnOffStartGuest = false, DeleteOnOff = false, ViewAnalogDiagnoz = false,
            PrintComplInterview = false, StopDialog = false, SaveAnalogDiagnoz = false, loadboolPacientProfil=false, loadTreeInterview = false;
        public static string ActCompletedInterview = "null", ActCreatInterview="", IndikatorSelected = "",InfoSborka="", 
                             selectedComplaintname = "", selectFeature = "", selectGrDetailing = "", selectQualification = "", selectIcdGrDiagnoz ="";
        public static string InputContent = "", PacientContent = "", LikarContent = "", RegIdUser = "", RegUserStatus = "";
        public static MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
        public static int NumberstrokaGuest = 0, IdItemGuestInterv = 0;
        private bool endwhile = false;
        public static string pathcontrolerContent = "/api/ContentInterviewController/";
        public static string pathcontroler = "/api/CompletedInterviewController/";
        public static string pathcontrolerColection = "/api/ColectionInterviewController/";
        public static string pathcontrolerInterview = "/api/InterviewController/";

        public static ModelCompletedInterview selectedGuestInterv;
        public static ModelCompletedInterview selectedCompletedInterv = new ModelCompletedInterview();
        public static ModelColectionInterview modelColectionInterview = new ModelColectionInterview();
        public static ModelPacient selectedProfilPacient;
        public static ColectionInterview colectionInterview;

        public static ObservableCollection<ModelCompletedInterview> GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
        public static ObservableCollection<ModelCompletedInterview> TmpGuestIntervs = new ObservableCollection<ModelCompletedInterview>();
        public static ObservableCollection<ModelInterview> AnalogInterviews = new ObservableCollection<ModelInterview>();
        public static ObservableCollection<ModelDetailing> listgrdetaling = new ObservableCollection<ModelDetailing>();
        public ModelCompletedInterview SelectedGuestInterv
        { get { return selectedGuestInterv; } set { selectedGuestInterv = value; OnPropertyChanged("SelectedGuestInterv"); } }

        public ModelPacient SelectedProfilPacient
        { get { return selectedProfilPacient; } set { selectedProfilPacient = value; OnPropertyChanged("SelectedProfilPacient"); } }


        public MapOpisViewModel()
        {
            CallViewProfilLikar = "";
            RegStatusUser = "Адміністратор";
            if (boolSetAccountUser == false)
            { if (RegSetAccountUser() == false) Environment.Exit(0);}
        }

        public static string ConfigPath()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true);

            IConfigurationRoot config = builder.Build();
            return config.GetConnectionString("UnloadString");
        }


        // команда вывзова окна  интервью
        RelayCommand? addCompletedInterview;
        public RelayCommand AddCompletedInterview
        {
            get
            {
                return addCompletedInterview ??
                  (addCompletedInterview = new RelayCommand(obj =>
                  {
                      //IsnableButton();
                      selectedCompletedInterv = new ModelCompletedInterview();
                      modelColectionInterview = new ModelColectionInterview();
                      GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                      InputContent = InputContent == "" ? WindowMain.InputNameInterview.Content.ToString() : InputContent;
                      WindowMain.NameInterv.Text = "Загальне опитування: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                      modelColectionInterview.nameInterview = WindowMain.NameInterv.Text; 
                      WindowMain.StackPanelGuestInterview.Visibility = Visibility.Hidden;
                      IndexAddEdit = "addCommand";
                      ActCompletedInterview = "Guest";
                      StrokaInterview = "";
                      OnOffStartGuest = false;
                      StopDialog = false;
                      loadTreeInterview = false;
                      ZagolovokInterview();
                      AutoSelectedInterview();
                      WindowMain.TablGuestInterviews.SelectedItem = null;
                      if (GuestIntervs.Count != 0 && StopDialog == false)MetodSaveInterview();
                      WindowMain.NameInterv.Text = "";
                  }));
            }
        }



        // команда вывзова окна со списком жалоб для выбора строки  и записи в интервью
        RelayCommand? editCompletedInterview;
        public RelayCommand EditCompletedInterview
        {
            get
            {
                return editCompletedInterview ??
                  (editCompletedInterview = new RelayCommand(obj =>
                  {
                      if (GuestIntervs.Count != 0)
                      { 
                          IndexAddEdit = "editCommand";
                          ActCompletedInterview = "Guest";
                          StopDialog = false;
                          WindowMain.StackPanelGuestInterview.Visibility = Visibility.Hidden;
                          EditSelectedInterview();
                          SuccessEndEditInterview();
                      }

                  }));
            }
        }


        // команда закрытия окна
        RelayCommand? saveCompletedInterview;
        public RelayCommand SaveCompletedInterview
        {
            get
            {
                return saveCompletedInterview ??
                  (saveCompletedInterview = new RelayCommand(obj =>
                  {
                    if (GuestIntervs.Count != 0)MetodSaveInterview();
                      WindowMain.NameInterv.Text = "";
                  }));
            }
        }

        // команда удаления строки интервью
        RelayCommand? deleteCompletedInterview;
        public RelayCommand DeleteCompletedInterview
        {
            get
            {
                return deleteCompletedInterview ??
                  (deleteCompletedInterview = new RelayCommand(obj =>
                  {
                      
                      if (GuestIntervs.Count != 0)
                      {
                          MessageDeleteData();
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              int IdItemSelected = WindowMain.TablGuestInterviews.SelectedIndex;
                              RemovStrIntervs(IdItemSelected);
                              WindowMain.StackPanelGuestInterview.Visibility = Visibility.Hidden;
                          }
                      }
                  }));
            }
        }

        // команда печати
        RelayCommand? printCompletedInterview;
        public RelayCommand PrintCompletedInterview
        {
            get
            {
                return printCompletedInterview ??
                  (printCompletedInterview = new RelayCommand(obj =>
                  {
                      PrintComplInterview = true;
                      PrintDiagnoz();
 
                  },
                 (obj) => GuestIntervs != null));
            }
        }

        public void ZagolovokInterview()
        {

            modelColectionInterview.dateInterview = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            switch (ActCompletedInterview)
            {
                case "Likar":
                    modelColectionInterview.kodDoctor = _kodDoctor;
                    modelColectionInterview.kodPacient = "";
                    if (selectedProfilPacient != null) modelColectionInterview.kodPacient = selectedProfilPacient.kodPacient;
                    modelColectionInterview.nameInterview = WindowMain.KabLikarNameInterv.Text.ToString();
                    break;
                case "Pacient":
                    modelColectionInterview.kodDoctor = "";
                    modelColectionInterview.kodPacient = _pacientProfil;
                    modelColectionInterview.nameInterview = WindowMain.KabPacientNameInterv.Text.ToString();
                    break;
                case "Guest":
                    modelColectionInterview.kodDoctor = "";
                    modelColectionInterview.kodPacient = "Гість";
                    modelColectionInterview.nameInterview = WindowMain.NameInterv.Text.ToString();
                    break;

            }
        }



        // Дозапись или корретировка проведенного интервью
        public void EditSelectedInterview()
        {

            SelectedswitchEdit();
            if (selectedGuestInterv != null && IdItemGuestInterv > 0)
            {

                switch (selectedGuestInterv.kodDetailing.Length)
                {
                    case 5:
                        OpenNsiFeature();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                    case 9:
                        OpenNsiDetailing();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                    case > 9:
                        OpenNsiGrDetailing();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                }
            }
            else
            {
                IdItemGuestInterv = GuestIntervs.Count;
                OpenNsiComplaint();
            }
        }

        public static void SelectedswitchEdit()
        {
            switch (ActCompletedInterview)
            {
                case "Guest":
                    IdItemGuestInterv = WindowMain.TablGuestInterviews.SelectedIndex;
                    break;
                case "Pacient":
                    IdItemGuestInterv = WindowMain.TablPacientInterviews.SelectedIndex;
                    break;
                case "Likar":
                    IdItemGuestInterv = WindowMain.TablLikarInterviews.SelectedIndex;
                    break;
            }
            IdItemGuestInterv++;
        }

        public  void MetodSaveInterview()
        {
            if (GuestIntervs != null)
            {
                if (GuestIntervs.Count != 0)
                {
                    MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                    WindowMain.StackPanelGuestInterview.Visibility = Visibility.Hidden;
                    SelectNewKodComplInteriew();
                    modelColectionInterview.detailsInterview = "";
                    NumberstrokaGuest = 0;
                    // Очистка коллекции от двойных  кодов
                    if (modelColectionInterview.kodProtokola != "")
                    { 
                        string json = pathcontroler + modelColectionInterview.kodProtokola + "/0";
                        CallServer.PostServer(pathcontroler, json, "DELETE");                   
                    }

                    CreatDetailsInterview();
                    SelectedDiagnozRecom();
                    if (ViewAnalogDiagnoz == true )
                    { 
                        foreach (ModelCompletedInterview modelCompletedInterview in GuestIntervs)
                        {

                            modelCompletedInterview.id = 0;
                            modelCompletedInterview.numberstr = NumberstrokaGuest++;
                            modelCompletedInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
                            json = JsonConvert.SerializeObject(modelCompletedInterview);
                            CallServer.PostServer(pathcontroler, json, "POST");
                        }
                        CopycolectionInterview();
                        SaveInterviewProtokol();
                        TmpGuestIntervs = GuestIntervs;
                   
                    }
                    OnOffStartGuest = true;
                    WindowMain.TablGuestInterviews.ItemsSource = null;
                    WindowMain.TablLikarInterviews.ItemsSource = null;
                    WindowMain.TablPacientInterviews.ItemsSource = null;
                    GuestIntervs = new ObservableCollection<ModelCompletedInterview>();     
                }
            }
            WindowMain.NameInterv.Text = "";

        }

        private void CreatDetailsInterview()
        {

            // ОБращение к серверу добавляем запись в соответствии с сформированным списком
            foreach (ModelCompletedInterview modelCompletedInterview in GuestIntervs.OrderBy(x => x.kodDetailing))
            {
                modelColectionInterview.detailsInterview = modelColectionInterview.detailsInterview.Length == 0
                                ? modelCompletedInterview.kodDetailing + ";" : modelColectionInterview.detailsInterview + modelCompletedInterview.kodDetailing + ";";
            }
            DiagnozRecomendaciya = modelColectionInterview.detailsInterview;
        }
        private void RemovStrIntervs(int IdItemSelected)
        {
            if (selectedGuestInterv != null)
            {
                switch (selectedGuestInterv.kodDetailing.Length)
                {
                    case 0:
                        ClearGuestIntervs(0, IdItemSelected);
                        break;
                    case 5:
                        ClearGuestIntervs(5, IdItemSelected);
                        break;
                    case 9:
                        ClearGuestIntervs(9, IdItemSelected);
                        break;
                    case > 9:
                        GuestIntervs.Remove(selectedGuestInterv);
                        break;
                }
                Selectedswitch();
            }

        }
        private void ClearGuestIntervs(int Lenghkod = 0, int IdItemSelected = 0)
        {
            endwhile = false;
            ModelCompletedInterview modelContentInterv = new ModelCompletedInterview();
            while (endwhile == false)
            {
                if (IdItemSelected == 0)
                {
                    GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                    endwhile = true;
                    break;
                }

                string remkodDetailing = "";
                int indexrem = 0, Lengthkodrem = 0;
                int CountInterv = GuestIntervs.Count;
                for (int index = 0; index <= CountInterv; index++)
                {
                    if (IdItemSelected <= index)
                    {
                        if (IdItemSelected == index)
                        {
                            indexrem = index;
                            Lengthkodrem = GuestIntervs[index].kodDetailing.Length;
                            remkodDetailing = GuestIntervs[index].kodDetailing;
                        }
                        if (GuestIntervs.Count == indexrem) break;
                        modelContentInterv = GuestIntervs[indexrem];
                        if ((modelContentInterv.kodDetailing.Length < Lengthkodrem) || (remkodDetailing != modelContentInterv.kodDetailing && modelContentInterv.kodDetailing.Length == Lengthkodrem))
                        {
                            break;
                        }
                        GuestIntervs.Remove(modelContentInterv);
                    }
                }
                endwhile = true;
            }
            TmpGuestIntervs = GuestIntervs;

        }

        // Дальнейший автовыбор характеристик жалоб на основании ранее установленных.
        // Автоматическое продолжение интервью

        public  void AutoSelectedInterview()
        {
            bool endwhileFeature = false;
            OpenNsiComplaint();
            if (GuestIntervs.Count == 0)
            {
                WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;
                return;
            }
            
            int countFeature = GuestIntervs.Count;
            while (endwhileFeature == false)
            {
                int shag = 1, key = 0;

                foreach (ModelCompletedInterview modelContentInterv in GuestIntervs)
                {
                    selectedGuestInterv = modelContentInterv;
                    switch (IndikatorSelected)
                    {
                        case "NsiComplaint":
                            key = 5;
                            break;
                        case "NsiFeature":
                            key = 9;
                            break;
                    }
                    AutoSelectedFeature(selectedGuestInterv, shag, countFeature, key);
                    shag++;
                }
                if (IndikatorSelected == "NsiDetailing") endwhileFeature = true;
                switch (IndikatorSelected)
                {
                    case "NsiComplaint":
                        IndikatorSelected = "NsiFeature";
                        break;
                    case "NsiFeature":
                        IndikatorSelected = "NsiDetailing";
                        break;
                }
                if (countFeature == GuestIntervs.Count && endwhileFeature == false)
                {

                    CreatDetailsInterview();
                    if (DiagnozRecomendaciya.Length <= 6)
                    {
                        StopDialog = true;
                        MessageSmalInfo();
                        if (MapOpisViewModel.DeleteOnOff == false)
                        {
                            GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                            Selectedswitch();
                            return;
                        }
                        StopDialog = false;
                    }
                    else MessageEndDialog();
                    endwhileFeature = true;
                    if (MapOpisViewModel.StopDialog == true)
                    {
                        GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                        Selectedswitch();
                        return;
                    } 
                    if (GuestIntervs.Count != 0) MetodSaveInterview();
                    StopDialog = true;
                }
                countFeature = GuestIntervs.Count;
            }

        }

        public static void AutoSelectedFeature(ModelCompletedInterview modelContentInterv, int shag, int countFeature, int key)
        {
            if (modelContentInterv.kodDetailing.Length == key)  // заменить цифру или показатель на независимый показатель
            {
                IdItemGuestInterv = GuestIntervs.Count - countFeature + shag;
                if (IndikatorSelected == "NsiComplaint") OpenNsiFeature();
                if (IndikatorSelected == "NsiFeature") OpenNsiDetailing();
            }
        }

        public static void OpenNsiComplaint()
        {
            MapOpisViewModel.ActCreatInterview = "CreatInterview";
            NsiComplaint NewOrder = new NsiComplaint();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-90;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; 
            NewOrder.ShowDialog();
            IndikatorSelected = "NsiComplaint";
            Selectedswitch();
 
        }

        public static void OpenNsiFeature()
        {
            MapOpisViewModel.ActCreatInterview = "CreatInterview";
            selectedComplaintname = GuestIntervs[IdItemGuestInterv - 1].detailsInterview;
            ViewModelNsiFeature.jasonstoka = ViewModelNsiFeature.pathFeatureController + "0/" + selectedGuestInterv.kodDetailing + "/0"; ;
            ViewModelNsiFeature.Method = "GETID";
            WinNsiFeature NewOrder = new WinNsiFeature();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
        }

        public static void OpenNsiDetailing()
        {

            if (loadTreeInterview == false) LoadTreeInterview(); // загрузка деревьв подобных интервью для настройки груповых детализаций
 
                MapOpisViewModel.ActCreatInterview = "CreatInterview";
                selectFeature = GuestIntervs[IdItemGuestInterv - 1].detailsInterview;
                string pathcontroller = "/api/DetailingController/";
                string jason = pathcontroller + "0/" + MapOpisViewModel.selectedGuestInterv.kodDetailing + "/0";
                CallServer.PostServer(pathcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]") == false)
                {

                    ViewModelNsiDetailing.ObservableNsiModelFeatures(CmdStroka);
                    LoadNsiGrDetailing();
                    if (ViewModelNsiDetailing.NsiModelDetailings.Count() > 0)
                    { 
                        NsiDetailing NewNsi = new NsiDetailing();
                        NewNsi.Left = (MainWindow.ScreenWidth / 2)-100;
                        NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                        NewNsi.ShowDialog();            
               
                    }
                    ViewModelNsiDetailing.NsiModelDetailings = null;
                }            
 


       
        }

        public static void LoadTreeInterview()
        {
            string detailsInterview = "";
            loadTreeInterview = true;
            foreach (ModelCompletedInterview modelCompletedInterview in GuestIntervs.OrderBy(x => x.kodDetailing))
            {
                 detailsInterview +=  modelCompletedInterview.kodDetailing + ";";
            }
            string jason = Interviewcontroller +"0/"+ detailsInterview +"/-1/0";
            CallServer.PostServer(Interviewcontroller, jason, "GETID");
            StrokaInterview = CallServer.ServerReturn();
        }
        public static void LoadNsiGrDetailing()
        {
            listgrdetaling = new ObservableCollection<ModelDetailing>();
            foreach (ModelDetailing modelDetailing in ViewModelNsiDetailing.NsiModelDetailings)
            {
                if (modelDetailing.keyGrDetailing != null && modelDetailing.keyGrDetailing != "")
                {
                    listgrdetaling.Add(modelDetailing);
                }
            }
            if (listgrdetaling.Count > 0)
            {
                foreach (ModelDetailing modelDetailing in listgrdetaling)
                {
                    ViewModelNsiDetailing.NsiModelDetailings.Remove(modelDetailing);
                    if (StrokaInterview.Contains(modelDetailing.keyGrDetailing) == true || StrokaInterview.Contains("[]") == true) //+";"
                    { 
                        ViewModelNsiDetailing.selectedDetailing = modelDetailing;
                        MapOpisViewModel.selectGrDetailing = selectFeature+" "+ modelDetailing.nameDetailing.ToString().ToUpper();
                        WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                        NewOrder.Left = (MainWindow.ScreenWidth / 2) -100;
                        NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
                        NewOrder.ShowDialog();                   
                    }
 
                }
            }

        }

        public static void OpenNsiGrDetailing()
        {
            MapOpisViewModel.ActCreatInterview = "CreatInterview";
            WinNsiGrDetailing NewNsi = new WinNsiGrDetailing();
            NewNsi.Left = (MainWindow.ScreenWidth / 2)-100;
            NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewNsi.ShowDialog();
        }

        public static void Selectedswitch()
        {
            switch (ActCompletedInterview)
            {
                case "Guest":
                    WindowMain.TablGuestInterviews.ItemsSource = GuestIntervs;
                    break;
                case "Pacient":
                    WindowMain.TablPacientInterviews.ItemsSource = GuestIntervs;
                    break;
                case "Likar":
                    WindowMain.TablLikarInterviews.ItemsSource = GuestIntervs;
                    break;
            }

        }


        // метод дозаписи выбранной строки жалобы в общий контекст интервью.
        public static void SelectContentCompleted()
        {
            int indexcontent = 0;
            bool booladdContent = false, addcontent = false;

            TmpGuestIntervs = new ObservableCollection<ModelCompletedInterview>();
            foreach (ModelCompletedInterview ModelCompletedInterview in GuestIntervs)   //.OrderBy(x => x.kodDetailing)
            {
                indexcontent++;
                if (IdItemGuestInterv == indexcontent && selectedGuestInterv != null && addcontent==false) booladdContent = true;
                TmpGuestIntervs.Add(ModelCompletedInterview);
                if (booladdContent == true)
                { 
                    AddselectedColection();
                    addcontent = true;
                    booladdContent = false;
                } 
            }
            if (GuestIntervs.Count == TmpGuestIntervs.Count) AddselectedColection();
            GuestIntervs = TmpGuestIntervs;
            Selectedswitch();

        }

        private static void AddselectedColection()
        {
            ModelCompletedInterview selectedaddContent = new ModelCompletedInterview();
            selectedaddContent.kodDetailing = nameFeature3.Substring(0, nameFeature3.IndexOf(":"));
            selectedaddContent.detailsInterview = nameFeature3.Substring(nameFeature3.IndexOf(":") + 1, nameFeature3.Length - (nameFeature3.IndexOf(":") + 1));
            TmpGuestIntervs.Add(selectedaddContent);
            IdItemGuestInterv++;

        }

        public static void SelectNewKodComplInteriew()
        {
            string indexcmp = "CMP.000000000001";
            CallServer.PostServer(pathcontroler, pathcontroler + "0/1", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                var result = JsonConvert.DeserializeObject<ListModelCompletedInterview>(CmdStroka);
                if (result.ModelCompletedInterview[0].kodComplInterv != null)
                { 
                    List<ModelCompletedInterview> res = result.ModelCompletedInterview.ToList();
                    int indexdia = Convert.ToInt32(res[0].kodComplInterv.Substring(res[0].kodComplInterv.LastIndexOf(".") + 1, res[0].kodComplInterv.Length - (res[0].kodComplInterv.LastIndexOf(".") + 1)));
                    string _repl = "000000000000";
                    indexcmp = "CMP." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + (indexdia + 1).ToString();                    
                }
            }
            modelColectionInterview.kodComplInterv = indexcmp;
        }



        // метод дозаписи данных формируемого интервью в таблицу проведенных интервью
        public static void AddInterviewProtokol()
        {
            CopycolectionInterview();
            SaveInterviewProtokol();
        }

        public static void CopycolectionInterview()
        { 
            colectionInterview = new ColectionInterview();
            colectionInterview.kodProtokola = modelColectionInterview.kodProtokola;
            colectionInterview.kodPacient = modelColectionInterview.kodPacient;
            colectionInterview.kodDoctor = modelColectionInterview.kodDoctor;
            colectionInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
            colectionInterview.dateInterview = modelColectionInterview.dateInterview;
            colectionInterview.detailsInterview = modelColectionInterview.detailsInterview;
            colectionInterview.nameInterview = modelColectionInterview.nameInterview;
            colectionInterview.resultDiagnoz = modelColectionInterview.resultDiagnoz;        
        
        }
        public static void SaveInterviewProtokol()
        {

            string Method = colectionInterview.id != 0 ? "PUT" : "POST";
            var json = JsonConvert.SerializeObject(colectionInterview);
            CallServer.PostServer(pathcontrolerColection, json, Method);
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                colectionInterview = JsonConvert.DeserializeObject<ColectionInterview>(CallServer.ResponseFromServer);
                modelColectionInterview.id = colectionInterview.id;
            }
        }

  


        // команда старт интервью пацента
        RelayCommand? addStartPacient;
        public RelayCommand AddStartPacient
        {
            get
            {
                return addStartPacient ??
                  (addStartPacient = new RelayCommand(obj =>
                  {
                      MetodStartPacient();
                  }));
                      
            }
        }

        public void MetodStartPacient()
        {

            if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
            selectedCompletedInterv = new ModelCompletedInterview();
            modelColectionInterview = new ModelColectionInterview();
            if (_pacientProfil == "") { WarningMessageOfProfilPacient(); return; }
            PacientContent = PacientContent == "" ? WindowMain.PacientKabinetInterv.Content.ToString() : PacientContent;
            WindowMain.KabPacientNameInterv.Text = "Опитування пацієнта: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            modelColectionInterview.nameInterview = WindowMain.KabPacientNameInterv.Text;
            WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;
            GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
            IndexAddEdit = "addCommand";
            ActCompletedInterview = "Pacient";
            StopDialog = false;
            loadTreeInterview = false;
            ZagolovokInterview();
            AutoSelectedInterview();
            if (GuestIntervs.Count != 0 && StopDialog == false)MetodSaveInterview();

        }

        // команда корректировки интервью пацента
        RelayCommand? methodEditInteviewPacient;
        public RelayCommand MethodEditInteviewPacient
        {
            get
            {
                return methodEditInteviewPacient ??
                  (methodEditInteviewPacient = new RelayCommand(obj =>
                  {
                      if (GuestIntervs.Count != 0)
                      {
                          IndexAddEdit = "editCommand";
                          ActCompletedInterview = "Pacient";
                          StopDialog = false;
                          EditSelectedInterview();
                          SuccessEndEditInterview();
                      }

                  }));
            }
        }

 
        // команда старт интервью пацента
        RelayCommand? saveStartPacient;
        public RelayCommand SaveStartPacient
        {
            get
            {
                return saveStartPacient ??
                  (saveStartPacient = new RelayCommand(obj =>
                  {
                      if (GuestIntervs.Count != 0) MetodSaveInterview();
                      WindowMain.KabPacientNameInterv.Text = "";
                  }));
            }
        }

        RelayCommand? deleteCompletedPacient;
        public RelayCommand DeleteCompletedPacient
        {
            get
            {
                return deleteCompletedPacient ??
                  (deleteCompletedPacient = new RelayCommand(obj =>
                  {
                      MessageDeleteData();
                      if (MapOpisViewModel.DeleteOnOff == true)
                      { 
                          int IdItemSelected = WindowMain.TablPacientInterviews.SelectedIndex;
                          RemovStrIntervs(IdItemSelected);
                          WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;

                      }

                  }));
            }
        }
        // команда старт интервью пацента
        RelayCommand? addStartLikar;
        public RelayCommand AddStartLikar
        {
            get
            {
                return addStartLikar ??
                  (addStartLikar = new RelayCommand(obj =>
                  {
                      MetodInterviewStartLikar();
                  }));
            }
        }

        public void MetodInterviewStartLikar()
        {
            if (RegUserStatus == "2") return;
            selectedCompletedInterv = new ModelCompletedInterview();
            modelColectionInterview = new ModelColectionInterview();
            WindowMain.KabLikarNameInterv.Text = "Лікарське опитування: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            modelColectionInterview.nameInterview = WindowMain.KabLikarNameInterv.Text;
            if (_kodDoctor == "")
            {
                WarningMessageOfProfilLikar();
                return;
            }
            LikarContent = LikarContent == "" ? WindowMain.InputNameProfilLikar.Content.ToString() : LikarContent;
            IndexAddEdit = "addCommand";
            ActCompletedInterview = "Likar";
            StopDialog = false;
            loadTreeInterview = false;
            WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
            GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
            LoadProfPacient();
            if (_pacientProfil.Length != 0)
            {
                StopDialog = false;
                ZagolovokInterview();
                AutoSelectedInterview();
                if (GuestIntervs.Count != 0 && StopDialog == false)MetodSaveInterview();

            }
        }

        private void LoadProfPacient()
        {

            CallViewProfilLikar = "ProfilPacient";
            selectedProfilPacient = new ModelPacient();
            WinNsiPacient NewOrder = new WinNsiPacient();
            NewOrder.ShowDialog();
            CallViewProfilLikar = "";
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                WarningMessageOfProfilPacient();
                return;
            }
            SetProfilPacient();
        }

        public void SetProfilPacient()
        {

            if (selectedProfilPacient != null)
            {
                _pacientProfil = selectedProfilPacient.kodPacient;
                SelectedProfilPacient = selectedProfilPacient;
                if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();
                modelColectionInterview.namePacient = selectedProfilPacient.name + selectedProfilPacient.surname;
                modelColectionInterview.kodPacient = selectedProfilPacient.kodPacient;
                selectedPacientProfil = selectedProfilPacient;
            }
            WindowMain.LikarIntert3.Text = "";

        }

        // команда вывзова окна со списком жалоб для выбора строки  и записи в интервью
        RelayCommand? editStartLikar;
        public RelayCommand EditStartLikar
        {
            get
            {
                return editStartLikar ??
                  (editStartLikar = new RelayCommand(obj =>
                  {
                      if (GuestIntervs.Count != 0)
                      {
                          IndexAddEdit = "editCommand";
                          ActCompletedInterview = "Likar";
                          StopDialog = false;
                          WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
                          EditSelectedInterview();
                          SuccessEndEditInterview();

                      }

                  }));
            }
        }


        // команда сохранения интервью пацента
        RelayCommand? saveStartLikar;
        public RelayCommand SaveStartLikar
        {
            get
            {
                return saveStartLikar ??
                  (saveStartLikar = new RelayCommand(obj =>
                  {
                     if(GuestIntervs.Count !=0) MetodSaveInterview();
                      WindowMain.KabLikarNameInterv.Text = "";
                  }));
            }
        }

        RelayCommand? deleteCompletedLikar;
        public RelayCommand DeleteCompletedLikar
        {
            get
            {
                return deleteCompletedLikar ??
                  (deleteCompletedLikar = new RelayCommand(obj =>
                  {
                      //if (GuestIntervs != null)
                      //{
                      //    MessageDeleteData();
                      //    if (MapOpisViewModel.DeleteOnOff == true)
                      //    {
                      //        int IdItemSelected = WindowMain.TablLikarInterviews.SelectedIndex;
                      //        RemovStrIntervs(IdItemSelected);
                      //        WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
                      //    }
                      //}
 
                  }));
            }
        }
        public static void SuccessEndEditInterview()
        {
            MainWindow.MessageError = "Для збереження змін і завершення опитування" + Environment.NewLine +
            "необхідно натиснути на кнопку <Зберегти>.";
            SelectedFalseLogin(4);
        }
        public static void SuccessEndInterview()
        {

            MainWindow.MessageError = "Опитування успішно закінчено." + Environment.NewLine + "Почекайте будьласка." + Environment.NewLine +
                "Здійснюється формування попередньої діагностичної гіпотези " + Environment.NewLine + "та відповідних рекомендацій щодо подальших дій";
            MapOpisViewModel.SelectedFalseLogin(5);
            //"Для збереження рузультатів та одержання рекомендацій " + Environment.NewLine + "натисніть на кнопку <Зберегти>";
        }


        public  void SelectedDiagnozRecom()
        {
            //MainWindow.MessageError = "Почекайте будьласка." + Environment.NewLine +
            // "Здійснюється формування попередньої діагностичної гіпотези " + Environment.NewLine + "та відповідних рекомендацій щодо подальших дій";
            //MapOpisViewModel.SelectedFalseLogin();
            ViewAnalogDiagnoz = false;
            SelectedFalseDiagnoz();
            //var json = pathcontrolerInterview + "0/" + DiagnozRecomendaciya + "/0/0";
            //CallServer.PostServer(pathcontrolerInterview, json, "GETID");
            //string CmdStroka = CallServer.ServerReturn();
            //if (CmdStroka.Contains("[]"))
            //{
            //    SelectedFalseDiagnoz();
            //}
            //else
            //{
            //    SelectInterviewDiagnoz();
            //    TrueNaimDiagnoz();
            //}


        }

        public static void SelectInterviewDiagnoz()
        {
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelInterview Idinsert = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
            // Дозапись найденого протокола в коллекции протоколов интервью
            modelColectionInterview.kodProtokola = Idinsert.kodProtokola;
            colectionInterview = new ColectionInterview();
            colectionInterview.kodProtokola = Idinsert.kodProtokola;
            string json = JsonConvert.SerializeObject(colectionInterview);
            CallServer.PostServer(pathcontrolerColection, json, "PUT");

            LoadDiagnozRecomen(Idinsert.kodProtokola);

        }

        public static void LoadDiagnozRecomen(string KodProtokola)
        {
            string json = Protocolcontroller + "0/" + KodProtokola + "/0";
            CallServer.PostServer(Protocolcontroller, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                MessageOfDagnoz();
                MapOpisViewModel.DeleteOnOff = false;
                return;
            } 
            else
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);

                if (Insert != null)
                {
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(MapOpisViewModel.Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        MapOpisViewModel.NameDiagnoz = Insert1.nameDiagnoza;
                        selectIcdGrDiagnoz = Insert1.icdGrDiagnoz;

                        json = ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz + "0/"+ Insert1.icdGrDiagnoz.ToString() ;
                        CallServer.PostServer(ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz, json, "GETID");
                        if (CallServer.ResponseFromServer.Contains("[]") == false)
                        {
                            //CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            //ModelGrupDiagnoz insertGrDiagnoz = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                            selectIcdGrDiagnoz = Insert1.icdGrDiagnoz;
                        }
                    }

                    json = MapOpisViewModel.Recomencontroller + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(MapOpisViewModel.Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        MapOpisViewModel.NameRecomendaciya = Insert2.contentRecommendation;

                    }
                }
            }

        }

        public void TrueNaimDiagnoz()
        {
            if (NameDiagnoz.Length != 0)
            {
                MapOpisViewModel.IndexAddEdit = "editCommand";
                MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodComplInterv + "/0";
                WinResultInterview NewResult = new WinResultInterview();
                NewResult.ShowDialog();
                if (ViewAnalogDiagnoz == true) SelectReception();

            }

        }

        public void SelectReception()
        {
            switch (ActCompletedInterview)
            {
                case "Likar":

                    MethodLoadReceptionLikar();
                    if (ViewModelReceptionPatients == null) ViewModelReceptionPatients = new ObservableCollection<ModelReceptionPatient>();
                     ModelReceptionPatient modelReceptionPatient = new ModelReceptionPatient();
                    modelReceptionPatient.dateInterview = modelColectionInterview.dateInterview;
                    modelReceptionPatient.nameDiagnoz = modelColectionInterview.nameDiagnoz;
                    modelReceptionPatient.namePacient = modelColectionInterview.namePacient;
                    modelReceptionPatient.nameRecomen = modelColectionInterview.nameRecomen;
                    modelReceptionPatient.kodProtokola = modelColectionInterview.kodProtokola;
                    modelReceptionPatient.kodPacient = modelColectionInterview.kodPacient;
                    modelReceptionPatient.kodComplInterv = modelColectionInterview.kodComplInterv;
                    modelReceptionPatient.topictVizita = modelColectionInterview.resultDiagnoz;
                    ViewModelReceptionPatients.Add(modelReceptionPatient);
                    WindowReceptionPacient.ReceptionPacientTablGrid.ItemsSource = ViewModelReceptionPatients;
                    SelectedReceptionPacient = modelReceptionPatient;

                    if (ViewReceptionPacients == null) ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                    if (selectedReceptionPacient == null) selectedReceptionPacient = new AdmissionPatient();
                    selectedReceptionPacient.kodPacient = modelColectionInterview.kodPacient;
                    selectedReceptionPacient.kodDoctor = modelColectionInterview.kodDoctor;
                    selectedReceptionPacient.kodComplInterv = modelColectionInterview.kodComplInterv;
                    selectedReceptionPacient.kodProtokola = modelColectionInterview.kodProtokola;
                    selectedReceptionPacient.topictVizita = modelColectionInterview.resultDiagnoz;
                    selectedReceptionPacient.dateInterview = modelColectionInterview.dateInterview;
                    ViewReceptionPacients.Add(selectedReceptionPacient);
                    IndexAddEdit = "addCommand";
                    break;
                case "Pacient":

                    // записать заявку на прием к врачу
                    SelectedColectionIntevLikar = modelColectionInterview;
                    // дописать в проведенные интервью
                    if (ColectionInterviewIntevPacients == null) ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>(); ;
                    ColectionInterviewIntevPacients.Add(modelColectionInterview);
                    SelectedColectionIntevPacient = modelColectionInterview;
                    SelectedColectionReceptionPatient = modelColectionInterview;
                    break;
                case "Guest":

                    SelectReceptionLIkarGuest = modelColectionInterview;
                    break;
            }
            SelectIcdGrDiagnoz();
            ChangeContentCompletedInterview();

        }

        public void ChangeContentCompletedInterview()
        {
            // Очистка коллекции от двойных  кодов
            string json = pathcontroler + modelColectionInterview.kodComplInterv + "/0";
            CallServer.PostServer(pathcontroler, json, "DELETE");

            CallServer.PostServer(pathcontrolerContent, pathcontrolerContent + modelColectionInterview.kodProtokola, "GETID");
            string CmdStroka = CallServer.ServerReturn();

            if (CmdStroka.Contains("[]")) ViewModelCreatInterview.selectedContentInterv = new ModelContentInterv();
            else ViewModelCreatInterview.ObservableContentInterv(CmdStroka);

            foreach (ModelContentInterv modelContentInterv in ViewModelCreatInterview.ContentIntervs)
            {
                ModelCompletedInterview modelCompletedInterview = new ModelCompletedInterview();
                modelCompletedInterview.id = 0;
                modelCompletedInterview.numberstr = modelContentInterv.numberstr;
                modelCompletedInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
                modelCompletedInterview.kodDetailing = modelContentInterv.kodDetailing;
                modelCompletedInterview.detailsInterview = modelContentInterv.detailsInterview;

                json = JsonConvert.SerializeObject(modelCompletedInterview);
                CallServer.PostServer(pathcontroler, json, "POST");
            }

        }

        public void SelectIcdGrDiagnoz()
        {
            string json = Protocolcontroller + "0/" + modelColectionInterview.kodProtokola + "/0";
            CallServer.PostServer(MapOpisViewModel.Protocolcontroller, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0";
                    CallServer.PostServer(MapOpisViewModel.Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectIcdGrDiagnoz = Insert1.icdGrDiagnoz.ToString();
                        json = ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz + "0/"+Insert1.icdGrDiagnoz.ToString();
                        CallServer.PostServer(ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz, json, "GETID");
                        if (CallServer.ResponseFromServer.Contains("[]") == false)
                        {
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelGrupDiagnoz insertGrDiagnoz = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                            selectIcdGrDiagnoz = insertGrDiagnoz.icdGrDiagnoz;
                        }
                    }
                }
            }
        }


        public  void SelectedFalseDiagnoz()
        {
            string json = "", CmdStroka ="";
            endUnload = 0;
            DeleteOnOff = true;
            RunGifWait();
            while (DiagnozRecomendaciya.Contains(";") == true)
            {
                json = pathcontrolerInterview + "0/" + DiagnozRecomendaciya + "/-1/0";
                CallServer.PostServer(pathcontrolerInterview, json, "GETID");
                CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]") == false) { break; }
                DiagnozRecomendaciya = DiagnozRecomendaciya.Substring(0, DiagnozRecomendaciya.Length - 1);
                DiagnozRecomendaciya = DiagnozRecomendaciya.Substring(0, DiagnozRecomendaciya.LastIndexOf(";")+1);
            }
            endUnload = 1;
            Thread.Sleep(800);
            if (CmdStroka.Contains("[]") == true) { MessageOfDagnoz(); return;  }
            var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
            List<ModelInterview> res = result.ModelInterview.ToList();
            AnalogInterviews = new ObservableCollection<ModelInterview>((IEnumerable<ModelInterview>)res);
            if (AnalogInterviews.Count == 1)LoadDiagnozRecomen(AnalogInterviews[0].kodProtokola);
           
            if (MapOpisViewModel.DeleteOnOff == true)
            {               
                WinAnalogDiagnoz NewResult = new WinAnalogDiagnoz();
                NewResult.ShowDialog();
 
                if (SaveAnalogDiagnoz == true || ViewAnalogDiagnoz == true)
                {
                    SetContent();
                    switch (ActCompletedInterview)
                    {
                        case "Likar":
                            WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                            WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;
                            break;
                        case "Pacient":
                            WindowMain.StackPanelPacient.Visibility = Visibility.Visible;
                            WindowMain.PacientKabinetInterv.Visibility = Visibility.Visible;
                            break;
                        case "Guest":
                            WindowMain.InputNameInterview.Visibility = Visibility.Visible;
                            WindowMain.StackPanelGuestInterview.Visibility = Visibility.Visible;
                            break;
                    }

                }
            }

        }

        private static void MessageOfDagnoz()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
                    "за результатами проведеного опитування відповідний діагноз відсутній.";
            MapOpisViewModel.SelectedFalseLogin(4);
        }
        private void MessageSmalInfo()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
                             "За результатами вашого опитування в базі знань знайдені " + Environment.NewLine +
                             "попередні діагнози за місцем прояву але без" + Environment.NewLine +
                             "визначення характеру прояву нездужання. " + Environment.NewLine + "Опитування не змістовне. ";
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            NewOrder.Width = NewOrder.Width + 250;
            NewOrder.BorderYes.Margin = new Thickness(180, 0, 0, 0);
            NewOrder.BorderYes.Width = NewOrder.BorderYes.Width + 40;
            NewOrder.Yes.Width = NewOrder.Yes.Width + 40;
            NewOrder.Yes.Height = NewOrder.Yes.Height + 6;
            NewOrder.Yes.FontSize = 15;
            NewOrder.Yes.Content = "Переглянути";
            NewOrder.BorderNo.Margin = new Thickness(0, 0, 180, 0);
            NewOrder.BorderNo.Width = NewOrder.BorderNo.Width + 120;
            NewOrder.No.Width = NewOrder.No.Width + 120;
            NewOrder.No.Height = NewOrder.No.Height + 6;
            NewOrder.No.FontSize = 15;
            NewOrder.No.Content = "Припинити";
            NewOrder.ShowDialog();
        }

        public static void ContinueCompletedInterview()
        {
            GuestIntervs = TmpGuestIntervs;
            string ContentMasage = Environment.NewLine + "Для продовження деталізації опитування та редагування необхідно" + Environment.NewLine +
                    "     натиснути кнопку 'Додати' або  встановити курсор миши " + Environment.NewLine + " на обраний рядок опитування та натиснути кнопку 'Змінити'. ";
            switch (ActCompletedInterview)
            {
                case "Likar":
                    WindowMain.TablLikarInterviews.ItemsSource = GuestIntervs;
                    WindowMain.InputNameProfilLikar.Content = ContentMasage;
                    WindowMain.InputNameProfilLikar.Height = 100;
                    WindowMain.InputNameProfilLikar.Foreground = Brushes.Blue;
                    WindowMain.StackPanelLikar.Height = 100;
                    WindowMain.StackPanelLikar.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.BorderLikar.Height = 100;
                    WindowMain.BorderLikar.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                    WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;

                    break;
                case "Pacient":
                    WindowMain.TablPacientInterviews.ItemsSource = GuestIntervs;
                    WindowMain.PacientKabinetInterv.Content = ContentMasage;
                    WindowMain.PacientKabinetInterv.Height = 100;
                    WindowMain.PacientKabinetInterv.Foreground = Brushes.Blue;
                    WindowMain.StackPanelCabPacient.Height = 100;
                    WindowMain.StackPanelCabPacient.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.BorderCabPacient.Height = 100;
                    WindowMain.BorderCabPacient.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.StackPanelCabPacient.Visibility = Visibility.Visible;
                    WindowMain.PacientKabinetInterv.Visibility = Visibility.Visible;

                    break;
                case "Guest":
                    WindowMain.TablGuestInterviews.ItemsSource = GuestIntervs;
                    WindowMain.InputNameInterview.Content = ContentMasage;
                    WindowMain.InputNameInterview.Height = 100;
                    WindowMain.InputNameInterview.Foreground = Brushes.Blue;
                    WindowMain.StackPanelGuestInterview.Height = 100;
                    WindowMain.StackPanelGuestInterview.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.BorderGuestInterview.Height = 100;
                    WindowMain.BorderGuestInterview.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.StackPanelGuestInterview.Visibility = Visibility.Visible;
                    WindowMain.InputNameInterview.Visibility = Visibility.Visible;
                    break;
            }

        }
        public static void SetContent()
        {
            switch (ActCompletedInterview)
            {
                case "Likar":
                    if (LikarContent.Length > 0) WindowMain.InputNameProfilLikar.Content = LikarContent;
                    WindowMain.InputNameProfilLikar.Height = 282;
                    WindowMain.InputNameProfilLikar.Foreground = Brushes.Black;
                    WindowMain.StackPanelLikar.Height = 300;
                    WindowMain.StackPanelLikar.Margin = new Thickness(0, 0, 10, 0);
                    WindowMain.BorderLikar.Height = 300;
                    WindowMain.BorderLikar.Margin = new Thickness(0, 0, 0, 10);

                    break;
                case "Pacient":
                    if (PacientContent.Length > 0) WindowMain.PacientKabinetInterv.Content = PacientContent;
                    WindowMain.PacientKabinetInterv.Height = 282;
                    WindowMain.PacientKabinetInterv.Foreground = Brushes.Black;
                    WindowMain.StackPanelPacient.Height = 300;
                    WindowMain.StackPanelPacient.Margin = new Thickness(0, 0, 10, 0);
                    WindowMain.BorderPacient.Height = 300;
                    WindowMain.BorderPacient.Margin = new Thickness(0, 0, 10, 0);
                    break;
                case "Guest":
                    if (InputContent.Length > 0) WindowMain.InputNameInterview.Content = InputContent;
                    WindowMain.InputNameInterview.Height = 282;
                    WindowMain.InputNameInterview.Foreground = Brushes.Black;
                    WindowMain.StackPanelGuestInterview.Height = 300;
                    WindowMain.StackPanelGuestInterview.Margin = new Thickness(0, 0, 10, 0);
                    WindowMain.BorderGuestInterview.Height = 300;//182
                    WindowMain.BorderGuestInterview.Margin = new Thickness(0, 0, 10, 0);
                    break;
            }

        }
        public static void UnloadCmdStroka(string model = "", string CmdStroka = "")
        {
            CallServer.PostServer(Controlleroutfile, Controlleroutfile + model + CmdStroka + "/0", "GETID");
        }

       
    }

  
}

