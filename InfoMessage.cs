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

using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;



namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {
        public static  MainWindow WindowInfo = MainWindow.LinkNameWindow("BackMain");
        protected Process[] procs;
        public static void SelectedDelete(int HeightWidth =0)
        {
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);            
            if (HeightWidth == -1)
            {

                //Random r = new Random();
                //Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)r.Next(1, 200), (byte)r.Next(1, 224), (byte)r.Next(1, 247)));
                //NewOrder.Yes.Background = brush;

                NewOrder.Height = NewOrder.Height + 200;
                NewOrder.Width = NewOrder.Width + 350;
                NewOrder.BorderNo.Margin = new Thickness(0, 0, 250, 0);
                NewOrder.BorderYes.Margin = new Thickness(250, 0, 0, 0);
            }

            NewOrder.ShowDialog();
        }
        public static void SelectedFalseLogin(int TimePauza=0)
        {
            TimePauza = TimePauza == 0 ? 7 : TimePauza;
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, TimePauza);
            NewOrder.Height = NewOrder.Height + 90;
            NewOrder.grid2.Height = NewOrder.grid2.Height + 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height + 60;
            NewOrder.ShowDialog();
        }

        public static void SelectedMessageOk(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 7 : TimePauza;
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, TimePauza);
            NewOrder.Height = NewOrder.Height - 40;
            NewOrder.grid2.Height = NewOrder.grid2.Height - 30;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height - 30;
            NewOrder.ShowDialog();
        }

        public static void SelectedRemove()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви дійсно бажаєте стерти облікові данні?";
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            NewOrder.Height = NewOrder.Height - 70;
            NewOrder.grid2.Height = NewOrder.grid2.Height - 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height - 60;
            NewOrder.ShowDialog();
        }

        public static void SelectedOkNo()
        {
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            NewOrder.Height = NewOrder.Height + 90;
            NewOrder.grid2.Height = NewOrder.grid2.Height + 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height + 60;
            NewOrder.ShowDialog();
        }
        public static void SelectedWirning(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 7 : TimePauza;
            MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, TimePauza);
            NewOrder.ShowDialog();
        }

        public static void SelectedMessageDialog()
        {
            WinMessageDialog NewOrder = new WinMessageDialog(MainWindow.MessageError);
            NewOrder.ShowDialog();
        }
        public void MessageDeleteData()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви дійсно бажаєте стерти облікові данні?";
            SelectedDelete();
        }
        public static void LoadInfoPacient(string user = "")
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Будь ласка зачекайте, завантажується вся інформація щодо " + user;
            SelectedLoad();
        }
        public static void SelectedLoad(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 100 : TimePauza;
            MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, TimePauza);
            NewOrder.Height = NewOrder.Height - 90;
            NewOrder.grid2.Height = NewOrder.grid2.Height - 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height - 60;
            NewOrder.Show();
        }
        public void WarningMessageOfCompletedINterviewPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації необхідно завантажити профіль паціента ";
            SelectedFalseLogin();
        }

        public void WarningMessageSelectComplaint()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації про характер прояву нездужання необхідно завантажити інформацію про те як або де проявляеться нездужання ";
            SelectedFalseLogin();
        }
        public void MessageOnOffKabinetPacient()
        { 
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету пацієнта необхідно вийти з кабінету лікаря " + Environment.NewLine +
            "Закрити кабінет лікаря?";
            SelectedDelete();
        }

        public void WarningMessageLoadProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації необхідно завантажити профіль паціента ";
            SelectedFalseLogin();
        }
        public void WarningMessageLoadLanguageUI()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач необхідно перезавантажити програму. ";
            SelectedWirning();
        }
        public void MessageOnOffKabinetLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету лікаря треба вийти з кабінету пацієнта " + Environment.NewLine +
            "Закрити кабінет пацієнта?"; ;
            SelectedDelete();
        }
        
        public void WarningMessageOfProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для проведення опитування необхідно завантажити профіль лікаря ";
            SelectedFalseLogin();
        }

        public void WarningMessageLoadProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації необхідно завантажити профіль лікаря ";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 4);
            SelectedFalseLogin();
        }
        public void WarningMessageOfProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для проведення опитування  необхідно завантажити профіль паціента ";
            SelectedFalseLogin();
        }
        public void WarningMessageOfNameInterview()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Ви не ввели назву опитування " + Environment.NewLine + "Будь ласка для виконання опитування введіть його назву";
            SelectedFalseLogin();
        }

        public void WarningMessageReceptionLIkarGuest()
        { 
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для запису на прийом до лікаря необхідно пройти опитування." + Environment.NewLine +
            "Для цього натиснути на кнопку 'Кімната для опитування' потім натиснути на кнопку 'Додати'.";
            SelectedFalseLogin();       
        }

        public void WarningMessageReceptionLIkar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для запису на прийом до лікаря необхідно пройти опитування." + Environment.NewLine +
            "Для цього натиснути на кнопку 'Провести опитування' потім натиснути на кнопку 'Додати'.";
            SelectedFalseLogin();
        }

        public static void MessageDialogFeature()
        {
            MainWindow.MessageError = "Дуже добре!" + Environment.NewLine +
            "У наступному меню треба визначтити, як ця хворобливість себе проявляє.";
            SelectedMessageDialog();
        }

        public static void MessageDialogDetailing()
        {
            MainWindow.MessageError = "Дуже добре!" + Environment.NewLine +
            "У наступному меню треба визначтити, як ця хворобливість проявляється у часі," + Environment.NewLine + " в яких ситуаціях, при якій температурі,та в яких ситуаціях.";
            SelectedMessageDialog();
        }

        public static void MessageEndDialog()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
           "Подальша детелізація характеру зовнішніх проявів хворобливості закінчена." + Environment.NewLine +
           "Для одержання рекомендацій щодо поточного стану опитування натисніть на кнопку < Далі > або відмовитися на кнопку <Припинити> ";
            SelectedFalseLogin(10);
        }

        public static void InfoRemoveZapis()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач коригування цього запису для заборонено. ";
            SelectedWirning();
        }

        public static void InfoOfPind()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач,  введено не існуючий поштовий індекс. ";
            SelectedWirning();
        }

        public static void InfoEditZapis()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач коригування цього запису для заборонено. ";
            SelectedWirning();
        }
        public static void VersiyaBack()
        {
            // Информация для администрирования Версия программы, путь архивирования
            //string PuthConecto = Process.GetCurrentProcess().MainModule.FileName;
            //string Versia = FileVersionInfo.GetVersionInfo(PuthConecto).ToString();  // версия файла.
            //string VersiaT = Versia.Substring(Versia.IndexOf("FileVersion") + 12, Versia.IndexOf("FileDescription") - (Versia.IndexOf("FileVersion") + 12)).Replace("\r\n", "").Replace(" ", "");

            MainWindow.ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            MainWindow.ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Process[] ObjModulesList = Process.GetProcessesByName("BackSeam"); 
            foreach (Process nobjModule in ObjModulesList)
            {
                // Заполнить коллекцию модулей
                ProcessModuleCollection ObjModules = ObjModulesList[0].Modules;
                // Итерация по коллекции модулей.
                foreach (ProcessModule objModule in ObjModules)
                {
                    //Получить правильный путь к модулю
                    string strModulePath = objModule.FileName.ToString();
                    //Если модуль существует
                    if (System.IO.File.Exists(objModule.FileName.ToString()))
                    {
                        //Читать версию
                        string strFileVersion = objModule.FileVersionInfo.FileVersion.ToString();
                        //Читать размер файла
                        string strFileSize = objModule.ModuleMemorySize.ToString();
                        //Читать дату модификации
                        FileInfo objFileInfo = new FileInfo(objModule.FileName.ToString());
                        string strFileModificationDate = objFileInfo.LastWriteTime.ToShortDateString();
                        //Читать описание файла
                        string strFileDescription = objModule.FileVersionInfo.FileDescription.ToString();
                        //Читать имя продукта
                        string strProductName = objModule.FileVersionInfo.ProductName.ToString();
                        //Читать версию продукта
                        string strProductVersion = objModule.FileVersionInfo.ProductVersion.ToString();
                        MapOpisViewModel.InfoSborka = strFileVersion;
                        WindowAccountUser.InfoSeamVer.Text = WindowAccountUser.InfoSeamVer.Text + strFileVersion + " Дата зборки: "+ strFileModificationDate;
                        WindowInfo.Title += " Версія: " + strFileVersion;
                        break;
                    }
                }
            }
            CallServer._UrlAdres = ConfigBuild();
            LoadSelectLanguageUI();
            ViewAccountUser();
        }

        public static string ConfigBuild()
        {
           var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true);

            IConfigurationRoot config = builder.Build();
            CallServer.UnloadString = config.GetSection("ConnectionStrings:UnloadString").Value;
            MainWindow.SelectLanguageUI = config.GetSection("ConnectionStrings:LanguageUI").Value;
            //WindowInfo.LanguageUIt1.Text = MainWindow.SelectLanguageUI;
            return config.GetConnectionString("Urls");
        }
        
        public static void LoadSelectLanguageUI()
        {
            
            string LoadLanguageUI = "";
            if (MainWindow.SelectLanguageUI != "Ukraine")
            {
                LoadLanguageUI = Directory.GetCurrentDirectory() + @"\LanguageUI\";
                OutFile = LoadLanguageUI + MainWindow.SelectLanguageUI + ".json"; ;
                if (Directory.Exists(LoadLanguageUI))
                {
                    if (System.IO.File.Exists(OutFile))
                    {
                        
                        Encoding code = Encoding.Default;
                        string[] fileLines = System.IO.File.ReadAllLines(OutFile, code);
                        foreach (string str in fileLines)
                        {
                            string[] data = str.Split('=');
                            if (data[0].Trim() == "Online") WindowInfo.Online.Header = data[1].Trim();
                            
                        }
                    }
                }
            }
            WindowInfo.LanguageUIt1.Text = MainWindow.SelectLanguageUI;

        }

        public static void ViewAccountUser()
        {
            if (RegUserStatus != "1" )
            {
                WindowInfo.AccountZap.Visibility = Visibility.Hidden;
                WindowInfo.NsiStatusUser.Visibility = Visibility.Hidden;
                WindowInfo.UnLoadBd.Visibility = Visibility.Hidden;
                WindowInfo.LoadBd.Visibility = Visibility.Hidden;
                WindowInfo.AnalizDiagnoz.Visibility = Visibility.Hidden;
                WindowInfo.SetupGrid.Visibility = Visibility.Hidden;
                


            }
            else
            {
                WindowInfo.AccountZap.Visibility = Visibility.Visible;
                WindowInfo.NsiStatusUser.Visibility = Visibility.Visible;
                WindowInfo.UnLoadBd.Visibility = Visibility.Visible;
                WindowInfo.LoadBd.Visibility = Visibility.Visible;
                WindowInfo.AnalizDiagnoz.Visibility = Visibility.Visible;
                WindowInfo.SetupGrid.Visibility = Visibility.Visible;
            }

        }
        public static void ViewNsiFeature()
        {
            WinNsiFeature zagolovok = MainWindow.LinkMainWindow("WinNsiFeature");
            zagolovok.Zagolovok.Content +=  MapOpisViewModel.selectedComplaintname.ToUpper();
        }

        public static void ViewNsiDetaling()
        {
            NsiDetailing zagolovok = MainWindow.LinkMainWindow("NsiDetailing");
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Detailing":
                    zagolovok.Zagolovok.Content += ViewModelNsiFeature.selectedFeature.name.ToUpper();
                    break;
                case "ViewDetailing":
                    zagolovok.Zagolovok.Content += selectedViewFeature.nameFeature.ToUpper();
                    break;
                default:
                    zagolovok.Zagolovok.Content += MapOpisViewModel.selectFeature.ToUpper();
                    break;
            }

        }

        public static void ViewNsiGrDetaling()
        {
            WinNsiGrDetailing zagolovok = MainWindow.LinkMainWindow("WinNsiGrDetailing");
            zagolovok.BorderAddAll.Visibility = Visibility.Hidden;
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Guest":
                    zagolovok.Zagolovok.Content += MapOpisViewModel.selectGrDetailing.ToUpper();
                    break;
                case "Complaint":
                    zagolovok.Zagolovok.Content += ViewModelNsiDetailing.selectedDetailing.nameDetailing.ToUpper();
                    break;
                case "ViewGrDetailing":
                    zagolovok.Zagolovok.Content += selectedViewDetailingFeature.nameFeature.ToUpper();
                    break;
                case "GrDetailing":
                    zagolovok.Zagolovok.Content += selectedListGroupDeliting.nameGrup.ToUpper();
                    break;
                case null:
                    zagolovok.Zagolovok.Content += MapOpisViewModel.selectedComplaintname.ToUpper();
                    zagolovok.BorderAddAll.Visibility = Visibility.Visible;
                    break;
                default:
                    zagolovok.Zagolovok.Content += MapOpisViewModel.selectedComplaintname.ToUpper();
                    break;
            }
        }
        public static void ViewNsiQualification()
        {
            WinNsiQualification zagolovok = MainWindow.LinkMainWindow("WinNsiQualification");
            zagolovok.Zagolovok.Content += MapOpisViewModel.ActCompletedInterview == "Complaint" ? ViewModelNsiGrDetailing.selectedGrDetailing.nameGrDetailing : MapOpisViewModel.selectQualification.ToUpper();
        }

        public static void VisibleOffSelect()
        {
            WinMedicalGrDiagnoz winMedicalGrDiagnoz = MainWindow.LinkMainWindow("WinMedicalGrDiagnoz");
            winMedicalGrDiagnoz.ButtonSelect.Visibility = Visibility.Hidden;
            //winMedicalGrDiagnoz.ButtonExit.Visibility = Visibility.Hidden;
        }

        public static void VisibleLikarOffSelect()
        {
            WinLikarGrupDiagnoz winLikarGrDiagnoz = MainWindow.LinkMainWindow("WinLikarGrupDiagnoz");
            if (SelectActivGrupDiagnoz == "GrupDiagnoz")
            {
                winLikarGrDiagnoz.ButtonAdd.Visibility = Visibility.Hidden;
                winLikarGrDiagnoz.ButtonDelete.Visibility = Visibility.Hidden;
            }
            else
            { winLikarGrDiagnoz.ButtonSelect.Visibility = Visibility.Hidden; }
            
            
        }

        public static void VisibleNsiDiagnoz()
        {
            if (MapOpisViewModel.ModelCall != "Dependency" )
            { 
                WinNsiListDiagnoz winLikarGrDiagnoz = MainWindow.LinkMainWindow("WinNsiListDiagnoz");
                winLikarGrDiagnoz.BorderSelect.Visibility = Visibility.Hidden;
                winLikarGrDiagnoz.BorderExit.Visibility = Visibility.Hidden;            
            }
 
        }

        public static void NotVisitingDays()
        {
            if (MapOpisViewModel.boolVisibleMessage == true) return;
            MainWindow.MessageError = "Розклад прийому пацієнтів не введено" + Environment.NewLine +
            "Дата та час прийому буде визначена лікарем додатково";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 5);
            NewOrder.ShowDialog();

        }
    }
}
