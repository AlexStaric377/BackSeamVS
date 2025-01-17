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
// Управление вводом-выводом
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
/// Многопоточность
using System.Threading;
using System.Windows.Threading;


namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ModelUnloadTab
    {
        public string NameTabl { get; set; }
        public string UnloadTabl { get; set; }

        public ModelUnloadTab(string nameTabl = "", string unloadTabl = "")
        {
            this.NameTabl = nameTabl;
            this.UnloadTabl = unloadTabl;
        }

    }
    public class ViewModelUnloadTab : BaseViewModel
    {

        public ModelUnloadTab ModelUnloadTab;
        public ViewModelUnloadTab(ModelUnloadTab modelUnloadTab)
        {
            this.ModelUnloadTab = modelUnloadTab;
        }


        public string NameTabl
        {
            get { return ModelUnloadTab.NameTabl; }
            set { ModelUnloadTab.NameTabl = value; OnPropertyChanged("NameTabl"); }
        }
        public string UnloadTabl
        {
            get { return ModelUnloadTab.UnloadTabl; }
            set { ModelUnloadTab.UnloadTabl = value; OnPropertyChanged("UnloadTabl"); }
        }

 
    }

    public class EventUnloadTab
    {
        public event EventHandler<ViewUnloadTabEventArgs> ViewUnloadTabArrived;
        void FireNewNotification(ModelUnloadTab ntf) =>
            ViewUnloadTabArrived?.Invoke(this, new ViewUnloadTabEventArgs(ntf));
    }

    public class ViewUnloadTabEventArgs : EventArgs
    {
        public readonly ModelUnloadTab ModelUnloadTab;
        public ViewUnloadTabEventArgs(ModelUnloadTab ntf) { ModelUnloadTab = ntf; }
    }
    public class ViewModelUnloadTabListVM
    {
        
        public ObservableCollection<ViewModelUnloadTab> ViewUnloadTabs { get; } =
            new ObservableCollection<ViewModelUnloadTab>();

        public ViewModelUnloadTabListVM()
        {
            ModelUnloadTab ntf = new ModelUnloadTab();
            var ViewUnloadTab = new ViewModelUnloadTab( ntf);


            ViewUnloadTab.PropertyChanged += ModelPropertyChanged; //OnModelPropertyChanged;
 
        }

        // Теперь, что мы делаем, когда приходит нотификация?
        // Для начала, она приходит в каком-то фоновом потоке,
        // но нам нужно работать с публичными свойствами только в UI-потоке.
        // Перебросим выполнение в него. А уж там создадим NotificationVM и добавим в список:
        public event PropertyChangedEventHandler ModelPropertyChanged;
        public void OnModelPropertyChanged(object sender, ViewUnloadTabEventArgs e)
        {
            
            // это происходит в модельном потоке
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                // это в UI-потоке
                ModelUnloadTab modelUnloadTab = new ModelUnloadTab();
                modelUnloadTab.NameTabl = MapOpisViewModel.listtablbd[MapOpisViewModel.itemtable];
                modelUnloadTab.UnloadTabl = MapOpisViewModel.listtablbd[MapOpisViewModel.itemtable];
                var ntfVM = new ViewModelUnloadTab(e.ModelUnloadTab);
                ViewUnloadTabs.Add(ntfVM);
            });
        }

        //private void OnContentPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    ViewUnloadTab(e.PropertyName);
        //}

    }


    


    // -------------------------------------------------------
    public partial class MapOpisViewModel : BaseViewModel
    {
        // ViewModelUnloadDb - Выгрузка таблиц во внешние текстовые файлы
        // клавиша в окне:  Вивантажити

        

        #region Обработка событий и команд вигрузки
        /// <summary>
        /// Стркутура: Команды, 
        /// 
        /// </summary> 

        public static MainWindow WindowUnload = MainWindow.LinkNameWindow("BackMain");
        public static int itemtable = 0, endUnload = 0;
        public static string[] deldstroka = {"-1", "-1", "-1" , "-1", "-1", "-1",
        "-1", "-1", "-1" , "-1", "0/-1", "-1/0/0", "0/-1",
        "-1","-1","-1","-1/0","-1/0","-1/0","-1","-1","-1","-1","-1/0",
        "-1","-1","-1","-1","-1","-1","-1","-1" ,"-1","-1","-1","-1"};
        public static string[] listtablbd = { "Complaint", "Feature", "Detailing" , "ListGrDetailing", "GrDetailing", "ListGroupQualification",
        "Qualification", "Diagnoz", "Recommendation" , "Interview", "ContentInterv", "ColectionInterview", "CompletedInterview",
        "DependencyDiagnoz","Icd","MedicalInstitution","Doctor","Pacient","AccountUser","NsiStatusUser","Sob","GrupDiagnoz","MedicalGrDiagnoz","DoctorGrDiagnoz",
            "PacientMapAnaliz","PacientAnalizKrovi","PacientAnalizUrine","CabinetPacient","LifePacient",
        "RegistrationAppointment","CabinetDoctor","LifeDoctor","AdmissionPatients","VisitingDays","LanguageUI"};

        public static string[] controler = {"/api/ApiControllerComplaint/", "/api/FeatureController/", "/api/DetailingController/" , "/api/ControllerListGroupDetail/",
        "/api/GrDetalingController/", "/api/GroupQualificationController/","/api/QualificationController/", "/api/DiagnozController/",
        "/api/RecommendationController/" , "/api/InterviewController/", "/api/ContentInterviewController/", "/api/ColectionInterviewController/", "/api/CompletedInterviewController/",
        "/api/DependencyDiagnozController/","/api/IcdController/","/api/MedicalInstitutionController/","/api/ApiControllerDoctor/","/api/PacientController/",
        "/api/AccountUserController/","/api/NsiStatusUserController/","/api/SobController/","/api/GrupDiagnozController/" , "/api/MedGrupDiagnozController/", "/api/LikarGrupDiagnozController/",
        "/api/PacientMapAnalizController/","/api/PacientAnalizKroviController/","/api/PacientAnalizUrineController/",
        "/api/CabinetPacientController/","/api/LifePacientController/",
        "/api/RegistrationAppointmentController/","/api/CabinetdoctorController/","/api/LifeDoctorController/","/api/ControllerAdmissionPatients/",
        "/api/VisitingDaysController/","/api/LanguageUIController/" };
        public static string OutFile = "", CmdStroka = "", json = "";

        public static ModelUnloadTab selectViewUnloadTab;
        public ModelUnloadTab SelectedViewUnloadTab
        {
            get { return selectViewUnloadTab; }
            set { selectViewUnloadTab = value; OnPropertyChanged("SelectedViewUnloadTab"); }
        }

 
        public static  UnLoadDb unLoadDb = new UnLoadDb();
        public static BaseUnload[] arrayUnload = { new UnloadComplaint(), new UnloadFeature(), new UnloadDetailing(),
                          new UnloadListGrDetailing(), new UnloadGrDetailing(), new UnloadListGroupQualification(),
                          new UnloadQualification(), new UnloadDiagnoz(), new UnloadRecommendation(), new UnloadInterview(), new UnloadContentInterv(),
                          new UnloadColectionInterview(), new UnloadCompletedInterview(), new UnloadDependencyDiagnoz(), new UnloadIcd(),
                          new UnloadMedicalInstitution(), new UnloadDoctor(), new UnloadPacient(), new UnloadAccountUser(), new UnloadNsiStatusUser(),
                          new UnloadSob(),new UnloadGrupDiagnoz(),new UnloadMedGrupDiagnoz(), new UnloadLikarGrupDiagnoz(), new UnloadPacientMapAnaliz(),
                          new UnloadPacientAnalizKrovi(),new UnloadPacientAnalizUrine()
        };
 
        
        
        private RelayCommand? selectPathUnload;
        public RelayCommand SelectPathUnload
        {
            get
            {
                return selectPathUnload ??
                  (selectPathUnload = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      RegStatusUser = "Адміністратор";
                      if (boolSetAccountUser == false)
                      { 
                        if (RegSetAccountUser() == false) return;
                      }
                      var dlg = new System.Windows.Forms.FolderBrowserDialog();
                      dlg.Description = "Шлях розташування архівних файлів";

                      System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                      if (result == System.Windows.Forms.DialogResult.OK)
                      {
                          FilePath = dlg.SelectedPath + @"\";
                          WindowUnload.UnloadBdTextBox.Text = FilePath.ToString();
                      }

                  }));
            }
        }
        // выгрузка таблиц из БД
        private RelayCommand? unLoadComand;
        public RelayCommand UnLoadComand
        {
            get
            {
                return unLoadComand ??
                (unLoadComand = new RelayCommand(obj =>
                {
                    if (CheckStatusUser() == false) return;
                    if (boolSetAccountUser == true)
                    {
                        //WindowUnload.GifUnloadBd.Visibility = Visibility.Visible;

                        endUnload = itemtable = 0;
                        WindowUnload.Unload.Background = Brushes.LimeGreen;
                        WindowUnload.LineUnLoad.Background = Brushes.LimeGreen;
                        RunGifWait();
                        foreach (var item in arrayUnload)
                        {
                            CallServer.PostServer(controler[itemtable], controler[itemtable], "GET");
                            CmdStroka = CallServer.ServerReturn();
                            if (CmdStroka.Contains("[]") == false)
                            {
                                if (ExistsOutFile.OutfileExists(itemtable) > 0)
                                {
                                    WindowUnload.SetUnLoadTabl.Text = listtablbd[itemtable];  // ViewUnloadTabs[itemtable].UnloadTabl;
                                    // запустить процес отображения в окне информации о выгрузке текущей таблицы
                                    // из коллекции - arrayUnload
                                    unLoadDb.FireUnLoad(item);
                                }
                            }
                            else
                            {

                                MainWindow.MessageError = "Увага!" + Environment.NewLine + "Таблиця: "+ listtablbd[itemtable]+ " не містить записів";
                                SelectedMessageOk(4);
                            }
                            itemtable++;
                            WindowUnload.LineUnLoad.Width += 20;

                        }
                        endUnload = 1;
                        MainWindow.MessageError = "Увага!" + Environment.NewLine + "Вивантаження бази даних завершено!.";
                        SelectedMessageOk(4);
 
                        WindowUnload.UnloadBdTextBox.Text = "";
                        WindowUnload.Unload.Background = Brushes.AliceBlue;
                        WindowUnload.LineUnLoad.Background = Brushes.White;
                        WindowUnload.BorderUnload.BorderBrush = Brushes.AliceBlue;
                    }
                    else
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не зареєстровані як Адміністратор!.";
                        SelectedMessageOk(4);
                    }
 
                }));
            }
        }


        // запуск потока слежения за пасивностью клиента
        public static void RunGifWait()
        {
            bool TimeOut = false;
            MainWindow.RenderInfo Arguments01 = new MainWindow.RenderInfo();
            Thread thread = new Thread(RunWinGifWait);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true; // Фоновый поток
            thread.Start(Arguments01);
            //WindowUnload.BorderUnload.BorderBrush = Brushes.LimeGreen;
            //MainWindow.RenderInfo Arguments01 = new MainWindow.RenderInfo() { };
            //Arguments01.argument1 = "1";
            //Thread thStartTimer01 = new Thread(RunWinGifWait);
            //thStartTimer01.SetApartmentState(ApartmentState.STA);
            //thStartTimer01.IsBackground = true; // Фоновый поток
            //thStartTimer01.Start(Arguments01);

        }


        // открытие окна Close
        public static void RunWinGifWait(object ThreadObj)
        {
            //System.Windows.Application.Current.Dispatcher.Invoke(new Action(delegate ()
            //{
            //    MainWindow ButtonUnload = MainWindow.LinkNameWindow("BackMain");
            //    ButtonUnload.BorderUnload.BorderBrush = Brushes.LimeGreen;
            //}));

            WaitWindow NewOrder = new WaitWindow("",2,10);
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2);
            NewOrder.ShowDialog();

        }

        public class ExistsOutFile
        {
            public static int OutfileExists(int itemtable =0)
            {
                int RezChek = 0;
                OutFile = FilePath + listtablbd[itemtable] + ".json";
                if (!(System.IO.File.Exists(OutFile)))
                {
                    try
                    {
                        using (FileStream NewFile = System.IO.File.Create(OutFile))
                        {
                            NewFile.Close();
                        }
                    }
                    catch (Exception) //error
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine + "Виникла помилка при створені файлу.";
                        SelectedFalseLogin(4);
                        return RezChek;
                    }
                }
                else
                {
                    System.IO.File.Delete(OutFile);
                    using (FileStream NewFile = System.IO.File.Create(OutFile))
                    {
                        NewFile.Close();
                    }
                }
                return RezChek=1;
            }
        }

 

        public class UnLoadDb
        {
            public void FireUnLoad(BaseUnload baseUnload)
            {
                baseUnload.UnloadTable();
            }
        }

        public abstract class BaseUnload
        {
            // абстрактный метод
            public abstract void UnloadTable();
        }
        public class ForeachUnload
        {
            public static void Foreachres(string stroka)
            {
                using (StreamWriter writer = new StreamWriter(OutFile, true))
                {
                    writer.WriteLine(stroka);
                    writer.Close();
                }
            }

        }
        // Complaint 1
        public class UnloadComplaint : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelComplaint>(CmdStroka);
                List<ModelComplaint> res = result.ViewComplaint.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }
        }

        // Feature 2
        public class UnloadFeature : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelFeature>(CmdStroka);
                List<ModelFeature> res = result.ModelFeature.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }
        }

        // Detailing 3
        class UnloadDetailing : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelDetailing>(CmdStroka);
                List<ModelDetailing> res = result.ViewDetailing.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }
        // GrDetailing 4
        class UnloadListGrDetailing : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelListGrDetailing>(CmdStroka);
                List<ModelListGrDetailing> res = result.ViewListGrDetailing.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // GrDetailing 5 
        class UnloadGrDetailing : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelGrDetailing>(CmdStroka);
                List<ModelGrDetailing> res = result.ViewGrDetailing.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }
        // ListGroupQualification 6
        class UnloadListGroupQualification : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelGroupQualification>(CmdStroka);
                List<ModelGroupQualification> res = result.ViewGroupQualification.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }
        // Qualification 7
        class UnloadQualification : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelQualification>(CmdStroka);
                List<ModelQualification> res = result.ViewQualification.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }
        // Diagnoz 8
        class UnloadDiagnoz : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListDiagnoz>(CmdStroka);
                List<Diagnoz> res = result.Diagnoz.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // Recommendation 9
        class UnloadRecommendation : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListRecommendation>(CmdStroka);
                List<Recommendation> res = result.Recommendation.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }
        }

        // Interview 10
        class UnloadInterview : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
                List<ModelInterview> res = result.ModelInterview.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // ContentInterv 11
        class UnloadContentInterv : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelContentInterv>(CmdStroka);
                List<ModelContentInterv> res = result.ModelContentInterv.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // ColectionInterview 12
        class UnloadColectionInterview : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
                List<ColectionInterview> res = result.ColectionInterview.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // CompletedInterview 13
        class UnloadCompletedInterview : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelCompletedInterview>(CmdStroka);
                List<ModelCompletedInterview> res = result.ModelCompletedInterview.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // DependencyDiagnoz 14
        class UnloadDependencyDiagnoz : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelDependency>(CmdStroka);
                List<ModelDependency> res = result.ModelDependency.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // DependencyDiagnoz 15
        class UnloadIcd : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
                List<ModelIcd> res = result.ModelIcd.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // MedicalInstitution 16
        class UnloadMedicalInstitution : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelMedical>(CmdStroka);
                List<MedicalInstitution> res = result.MedicalInstitution.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // Doctor 17
        class UnloadDoctor : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
                List<ModelDoctor> res = result.ModelDoctor.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // Pacient 18
        class UnloadPacient : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
                List<ModelPacient> res = result.ViewPacient.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // Pacient 19
        class UnloadAccountUser : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListAccountUser>(CmdStroka);
                List<AccountUser> res = result.AccountUser.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // Pacient 20
        class UnloadNsiStatusUser : BaseUnload
        {
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListNsiStatusUser>(CmdStroka);
                List<NsiStatusUser> res = result.NsiStatusUser.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }





        //UnloadSob 21
        public class UnloadSob : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelSob>(CmdStroka);
                List<ViewModelSob> res = result.ViewModelSob.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        //UnloadGrupDiagnoz 22
        public class UnloadGrupDiagnoz : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListGrupDiagnoz>(CmdStroka);
                List<GrupDiagnoz> res = result.GrupDiagnoz.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        //UnloadMedGrupDiagnoz 23
        public class UnloadMedGrupDiagnoz : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelMedGrupDiagnoz>(CmdStroka);
                List<ModelMedGrupDiagnoz> res = result.ModelMedGrupDiagnoz.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        //UnloadLikarGrupDiagnoz 24
        public class UnloadLikarGrupDiagnoz : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListModelLikarGrupDiagnoz>(CmdStroka);
                List<ModelLikarGrupDiagnoz> res = result.ModelLikarGrupDiagnoz.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        //UnloadPacientMapAnaliz 25
        public class UnloadPacientMapAnaliz : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListPacientMapAnaliz>(CmdStroka);
                List<PacientMapAnaliz> res = result.PacientMapAnaliz.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        //UnloadPacientMapAnaliz 26
        public class UnloadPacientAnalizKrovi : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListPacientAnalizKrovi>(CmdStroka);
                List<PacientAnalizKrovi> res = result.PacientAnalizKrovi.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }
        //UnloadPacientMapAnaliz 27
        public class UnloadPacientAnalizUrine : BaseUnload
        {
            //public override int Dange { get { return 8; } }
            public override void UnloadTable()
            {
                var result = JsonConvert.DeserializeObject<ListPacientAnalizUrine>(CmdStroka);
                List<PacientAnalizUrine> res = result.PacientAnalizUrine.ToList();
                foreach (var model in res)
                {
                    json = JsonConvert.SerializeObject(model);
                    ForeachUnload.Foreachres(json);
                }
            }

        }

        // ---------------------------------------------------------------
        // Абстрактные классы и методы

        //class ProgramAbs
        //{
        //    static void Main(string[] args)
        //    {
        //        Player player = new Player();

        //        Weapon[] inventary = { new Gun(), new LaserGun(), new Bow() };
        //        foreach (var item in inventary)
        //        {
        //            player.CheckInfo(item);
        //            player.Fire(item);

        //        }

        //        //Gun gun = new Gun();
        //        //player.Fire(gun);
        //    }
        //}
        //// абстрактный класс
        //public abstract class Weapon
        //{
        //    // абстрактное свойство
        //    public abstract int Dange { get;  }

        //    // абстрактный метод
        //    public abstract void Fire();

        //    public void ShowInfo()
        //    {
        //        Console.WriteLine($"{GetType().Name} Dange: {Dange}");
        //    }
        //}

        //class Gun : Weapon
        //{
        //    public override int Dange { get { return 5; } }

        //    public override void Fire()
        //    {
        //        Console.WriteLine("Бах");

        //    }

        //}

        //class LaserGun : Weapon
        //{
        //    public override int Dange => 3; 

        //    public override void Fire()
        //    {
        //        Console.WriteLine("Пиу");

        //    }

        //}

        //class Bow : Weapon
        //{
        //    public override int Dange { get { return 8; } }
        //    public override void Fire()
        //    {
        //        Console.WriteLine("Чмок");

        //    }

        //}

        //class Player
        //{

        //    public void Fire(Weapon weapon)
        //    {
        //        weapon.Fire();

        //    }
        //    public void CheckInfo(Weapon weapon)
        //    {
        //        weapon.ShowInfo();

        //    }
        //}




        #endregion
    }
}
//----------------------------------------------------------


// 1. Сначала запускается NotificationListVM в котором определены
// -NotificationVM,NewNotificationEventArgs.  и запускаетя класс NotificationWatcher(); 
// Он должен запускать (и останавливать, но мне лень) модельный NotificationWatcher,
// и подписываться на его события.
//public class NotificationListVM
//{
//    public ObservableCollection<NotificationVM> Notifications { get; } =
//        new ObservableCollection<NotificationVM>();

//    // создание экземпляра класса - class NotificationWatcher в котором реализовано event EventHandler
//    NotificationWatcher watcher = new NotificationWatcher(); 

//    public NotificationListVM()
//    {
//        watcher.NotificationArrived += OnNotificationArrtived;
//        watcher.Start();
//    }

//    // Теперь, что мы делаем, когда приходит нотификация?
//    // Для начала, она приходит в каком-то фоновом потоке,
//    // но нам нужно работать с публичными свойствами только в UI-потоке.
//    // Перебросим выполнение в него. А уж там создадим NotificationVM и добавим в список:

//    void OnNotificationArrtived(object sender, NewNotificationEventArgs e)
//    {
//        // это происходит в модельном потоке
//        Application.Current?.Dispatcher.InvokeAsync(() =>
//        {
//            // это в UI-потоке
//            var ntfVM = new NotificationVM(e.Notification);
//            Notifications.Add(ntfVM);
//        });
//    }

//}
//// Переходим к VM. Для начала, простая вещь — одна нотификация
//public class NotificationVM
//{
//    public string Text { get; }
//    public NotificationVM(Notification ntf) { Text = ntf.Text; }
//}

//public class Notification
//{
//    public int ID;
//    public string Text;
//}

//// Определяем свой класс, производный от EventArgs:

//public class NewNotificationEventArgs : EventArgs
//{
//    public readonly Notification Notification;
//    public NewNotificationEventArgs(Notification ntf) { Notification = ntf; }
//}


//// 2. Этот класс обеспечивает выполнение NotificationListVM.
//public class NotificationWatcher
//{
//    // Ну, нам нужно начать слежение и окончить его. Сделаем это в фоновом потоке, и используем TPL.
//    // Оформим само слежение в виде метода Watch, мы должны его запустить и остановить по требованию.
//    // Для остановки будем применять CancellationToken, как и положено.

//    CancellationTokenSource cts;
//    public void Start()
//    {
//        cts = new CancellationTokenSource();
//        Task.Run(() => Watch(cts.Token)); 
//    }

//    public void Stop()
//    {
//        cts.Cancel();
//        cts = null;
//    }

//    // В методе Start мы просто запускаем Watch в фоновом потоке с токеном.
//    // А в методе Stop включаем отмену задания по этому токену. Всё просто!
//    // Дальше, сам метод Watch будет просто каждую секунду осуществлять при помощи метода EvaluateList,
//    // и наблюдать за токеном:
//    async void Watch(CancellationToken ct)
//    {
//        var pause = TimeSpan.FromSeconds(1);
//        try
//        {
//            while (!ct.IsCancellationRequested)
//            {
//                await Task.Delay(pause, ct);
//                await EvaluateList(ct); //await 
//            }
//        }
//        catch (OperationCanceledException) when (ct.IsCancellationRequested)
//        { }
//    }

//    // Теперь, метод EvaluateList.Он должен загрузить JSON-список, распарсить его в объектное
//    // представление, и найти, какие же из нотификаций новые.Для этого нам понадобится класс,
//    // соответствующий одной нотификации.Отвлечёмся от класса NotificationWatcher на время,
//    // и напишем его.Он у нас будет совсем простым (а у вас наверное сложнее, т.к.он должен будет
//    // отражать реальный JSON с сервера):



//    //HashSet<int> currentIds = new HashSet<int>(); // текущие ID

//    async Task   EvaluateList(CancellationToken ct) //async Task
//    {
//        // получаем JSON
//        //string json = await DownloadNotifications(ct);
//        //// превращаем его в список объектов
//        //var currentNotifications = JsonConvert.DeserializeObject<List<Notification>>(json);
//        // отбираем из него новые
//        Notification newNotifications = new Notification();
//        newNotifications.ID = 1;
//        newNotifications.Text = MapOpisViewModel.listtablbd[MapOpisViewModel.itemtable];
//                //currentNotifications.Where(ntf => !currentIds.Contains(ntf.ID));
//        // для каждого из новых объектов оповещаем о нём мир
//        //foreach (var ntf in newNotifications)
//        //{
//            /*currentIds.Add(ntf.ID);*/ // и запоминаем его ID на будущее, чтобы
//                                    // в следующий раз он не считался новым
//            FireNewNotification(newNotifications);
//        //}
//    }

//    // Теперь, нотификация. Нотификацию напишем совершенно обыкновенную, через event.
//    // Как примерные программисты, реализуем паттерн с EventArgs.
//    // Определяем свой класс, производный от EventArgs:



//    // реализуем отправление нотификации:

//    public event EventHandler<NewNotificationEventArgs> NotificationArrived;
//    public void FireNewNotification(Notification ntf) =>
//        NotificationArrived?.Invoke(this, new NewNotificationEventArgs(ntf));

// реализовать метод DownloadNotifications. У вас он будет читать данные с сервера,
// но я просто закодирую фейковые данные прямо в коде для простоты примера.

// фейк, тут нужно читать с сервера
// ------------------------------------------------
//int notificationNo = 0;
//string[] strings =
//    {
//    "[ { \"ID\": 1, \"Text\": \"Привет\" } ]",
//    "[ { \"ID\": 1, \"Text\": \"Привет\" }, { \"ID\": 2, \"Text\": \"Привет\" } ]",
//    "[ { \"ID\": 1, \"Text\": \"Привет\" }, { \"ID\": 2, \"Text\": \"Привет\" }, " +
//      "{ \"ID\": 3, \"Text\": \"Пока\" } ]",
//    "[ { \"ID\": 4, \"Text\": \"Пока\" } ]"
//};
//async Task<string> DownloadNotifications(CancellationToken ct)
//{
//    var index = notificationNo >= strings.Length ? strings.Length - 1 : notificationNo;
//    await Task.Delay(200);
//    notificationNo++;
//    return strings[index];
//}
// ---------------------------------------------

// Переходим к VM. Для начала, простая вещь — одна нотификация.
// Она умеет конструироваться из модельной нотификации, но ей не нужен ID,
// так что она его не запоминает. Кроме того, она будет выставлять свойство,
// а не поле, как и всякий уважающий себя VM-объект (поскольку к нему будет проводиться привязка):

//   public class NotificationVM

// Затем, список нотификаций. Положим в него немного логики.
// Во-первых, он должен выставлять ObservableCollection нотификаций.
// Затем, он должен запускать (и останавливать, но мне лень) модельный NotificationWatcher,
// и подписываться на его события.
//}
