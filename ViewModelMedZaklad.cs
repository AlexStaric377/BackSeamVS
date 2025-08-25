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
    public partial class MapOpisViewModel : BaseViewModel
    {
        //  модель MedicalInstitution
        //  клавиша в окне: "Довідник мед закладів"

        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех закладів из БД
        /// через механизм REST.API
        /// </summary>
        private static MainWindow WindowMedical = MainWindow.LinkNameWindow("BackMain");
        public static bool activVeiwMedical = false;
        bool   loadbooltablMedical = false, activeditVeiwModelMedical = false;
        public static bool cikl = true;
        public static string controlerMedical =  "/api/MedicalInstitutionController/";
        public static string controlerStatusZaklad = "/api/ControllerStatusMedZaklad/";
        public static MedicalInstitution selectedMedical;

        public static ObservableCollection<MedicalInstitution> VeiwModelMedicals { get; set; }

        public MedicalInstitution SelectedMedical
        { get { return selectedMedical; } set { selectedMedical = value; OnPropertyChanged("SelectedMedical"); } }

        public static void ObservableVeiwMedical(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelMedical>(CmdStroka);
            List<MedicalInstitution> res = result.MedicalInstitution.ToList();
            VeiwModelMedicals = new ObservableCollection<MedicalInstitution>((IEnumerable<MedicalInstitution>)res);
            MetodAddNameStatus();
            WindowMedical.MedicalTablGrid.ItemsSource = VeiwModelMedicals;
        }


        public static void MetodAddNameStatus()
        {
            int indexrepl = 0;
            foreach (MedicalInstitution medicalInstitution in VeiwModelMedicals)
            {
                if (medicalInstitution.kodZaklad == null) NewkodZaklad();
 
                string json = controlerStatusZaklad + medicalInstitution.idstatus;
                CallServer.PostServer(controlerStatusZaklad, json, "GETID");
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                StatusMedZaklad Idinsert = JsonConvert.DeserializeObject<StatusMedZaklad>(CallServer.ResponseFromServer);
                VeiwModelMedicals[indexrepl].idstatus += ":"+Idinsert.nameStatus;
                indexrepl++;
            }
        
        }


        private static void NewkodZaklad()
        {
            int indexdia = 1, setindex = 1;
            string _repl = "000000000";
            string kodzaklad = "ZKL." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString(); // "ZKL.000000001";
           
            
                foreach (MedicalInstitution medical in VeiwModelMedicals)
                {
                    if (medical.kodZaklad == null)
                    {
                        medical.kodZaklad = "ZKL." + _repl.Substring(0, _repl.Length - setindex.ToString().Length) + setindex.ToString();
                        json = JsonConvert.SerializeObject(medical);
                        CallServer.PostServer(controlerMedical, json, "PUT");
                    }
                    else setindex = Convert.ToInt32(medical.kodZaklad.Substring(medical.kodZaklad.LastIndexOf(".") + 1, medical.kodZaklad.Length - (medical.kodZaklad.LastIndexOf(".") + 1)));
                    setindex++;
                }
        }
        #region Команды вставки, удаления и редектирования справочника "Мед заклади"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadVeiwModelMedical;
        public RelayCommand LoadVeiwModelMedical
        {
            get
            {
                return loadVeiwModelMedical ??
                  (loadVeiwModelMedical = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadtabMedical();
                  }));
            }
        }


        // команда добавления нового объекта

        private RelayCommand addVeiwModelMedical;
        public RelayCommand AddVeiwModelMedical
        {
            get
            {
                return addVeiwModelMedical ??
                  (addVeiwModelMedical = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComVeiwModelMedical(); }));
            }
        }

        private void AddComVeiwModelMedical()
        {
            if (loadbooltablMedical == false) MethodloadtabMedical();
            MethodaddcomMedical();
        }

        private void MethodaddcomMedical()
        {
            WindowMedical.FolderWorkzaklad.Visibility = Visibility.Visible;
            WindowMedical.FolderStatuszaklad.Visibility = Visibility.Visible;
            SelectedMedical = new MedicalInstitution();
            selectedMedical = new MedicalInstitution();
            IndexAddEdit = "addCommand";
            if (activVeiwMedical == false) ModelMedicaltrue();
            else ModelMedicalfalse();
        }


        private void MethodloadtabMedical()
        {
           loadbooltablMedical = true;
           CallServer.PostServer(controlerMedical, controlerMedical, "GET");
           string CmdStroka = CallServer.ServerReturn();
           if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
           else ObservableVeiwMedical(CmdStroka);
        }

        // команда удаления
        private RelayCommand? removeVeiwModelMedical;
        public RelayCommand RemoveVeiwModelMedical
        {
            get
            {
                return removeVeiwModelMedical ??
                  (removeVeiwModelMedical = new RelayCommand(obj =>
                  {
                      if (selectedMedical != null)
                      {

                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerMedical + selectedMedical.id.ToString();
                              CallServer.PostServer(controlerMedical, json, "DELETE");
                              VeiwModelMedicals.Remove(selectedMedical);
                              selectedMedical = new MedicalInstitution();
                              ModelMedicalfalse();
                          }
                      }
                      
                      IndexAddEdit = "";
                  },
                 (obj) => VeiwModelMedicals != null));
            }
        }


        // команда  редактировать
       
        private RelayCommand? editVeiwModelMedical;
        public RelayCommand? EditVeiwModelMedical
        {
            get
            {
                return editVeiwModelMedical ??
                  (editVeiwModelMedical = new RelayCommand(obj =>
                  {
                      if (WindowMedical.MedicalTablGrid.SelectedIndex >= 0)
                      {
                          
                          IndexAddEdit = "editCommand";
                          if (activeditVeiwModelMedical == false)
                          {
                              selectedMedical = VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex];
                              ModelMedicaltrue();
                          }
                          else
                          {
                              ModelMedicalfalse();
                              WindowMedical.MedicalTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveVeiwModelMedical;
        public RelayCommand SaveVeiwModelMedical
        {
            get
            {
                return saveVeiwModelMedical ??
                  (saveVeiwModelMedical = new RelayCommand(obj =>
                  {
                      
                      if (WindowMedical.Medicalt2.Text.Trim().Length != 0 & WindowMedical.Medicalt3.Text.Trim().Length != 0)
                      {
                          if (selectedMedical == null)
                          {
                              selectedMedical = new  MedicalInstitution();
                              selectedMedical.id = 0;
                          }
                          selectedMedical.edrpou = WindowMedical.Medicalt2.Text.ToString();
                          selectedMedical.name = WindowMedical.Medicalt3.Text.ToString();
                          selectedMedical.adres = WindowMedical.Medicalt5.Text.ToString();
                          selectedMedical.email = WindowMedical.Medicalt9.Text.ToString();
                          selectedMedical.postIndex = WindowMedical.Medicalt4.Text.ToString();
                          selectedMedical.telefon = WindowMedical.Medicalt8.Text.ToString();
                          selectedMedical.uriwebZaklad = WindowMedical.MedicalBoxUriWeb.Text.ToString();
                          selectedMedical.idstatus = selectedMedical.idstatus.Substring(0, selectedMedical.idstatus.IndexOf(":"));
                          var json = JsonConvert.SerializeObject(selectedMedical);
                          string Method = IndexAddEdit == "addCommand" ? "POST" : "PUT";
                          CallServer.PostServer(controlerMedical, json, Method);
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_"); 
                          MedicalInstitution Idinsert = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);
                          if (VeiwModelMedicals == null)VeiwModelMedicals = new ObservableCollection<MedicalInstitution>();
                          if (IndexAddEdit == "addCommand")
                          { 
                            VeiwModelMedicals.Add(Idinsert);
                            WindowMedical.MedicalTablGrid.ItemsSource = VeiwModelMedicals;
                          } 
                          
                          UnloadCmdStroka("MedicalInstitution/", json);
                      }
                      IndexAddEdit = "";
                      SelectedMedical = new MedicalInstitution();
                      ModelMedicalfalse();
                  }));
            }
        }

        private void ModelMedicalfalse()
        {
            activVeiwMedical = false;
            activeditVeiwModelMedical = false;
            IndexAddEdit = "";
            WindowMedical.Medicalt2.IsEnabled = false;
            WindowMedical .Medicalt2.Background = Brushes.White;
            WindowMedical .Medicalt3.IsEnabled = false;
            WindowMedical .Medicalt3.Background = Brushes.White;
            WindowMedical .Medicalt4.IsEnabled = false;
            WindowMedical .Medicalt4.Background = Brushes.White;
            WindowMedical .Medicalt5.IsEnabled = false;
            WindowMedical .Medicalt5.Background = Brushes.White;
            WindowMedical .Medicalt8.IsEnabled = false;
            WindowMedical .Medicalt8.Background = Brushes.White;
            WindowMedical .Medicalt9.IsEnabled = false;
            WindowMedical .Medicalt9.Background = Brushes.White;
            WindowMedical.MedicalBoxUriWeb.IsEnabled = false;
            WindowMedical.MedicalBoxUriWeb.Background = Brushes.White;
            WindowMedical.FolderWebUriZaklad.Visibility = Visibility.Hidden;
            WindowMedical.FolderStatuszaklad.Visibility = Visibility.Hidden;
            WindowMedical.MedicalTablGrid.IsEnabled = true;
            WindowMedical.BorderLoadMedical.IsEnabled = true;
            WindowMedical.BorderGhangeMedical.IsEnabled = true;
            WindowMedical.BorderDeleteMedical.IsEnabled = true;
            WindowMedical.BorderPrintMedical.IsEnabled = true;
            WindowMedical.BorderAddMedical.IsEnabled = true;
        }

        private void ModelMedicaltrue()
        {
            activVeiwMedical = true;
            activeditVeiwModelMedical = true;
            WindowMedical .Medicalt2.IsEnabled = true;
            WindowMedical .Medicalt2.Background = Brushes.AntiqueWhite;
            WindowMedical .Medicalt3.IsEnabled = true;
            WindowMedical .Medicalt3.Background = Brushes.AntiqueWhite;
            WindowMedical .Medicalt4.IsEnabled = true;
            WindowMedical .Medicalt4.Background = Brushes.AntiqueWhite;
            WindowMedical .Medicalt5.IsEnabled = true;
            WindowMedical .Medicalt5.Background = Brushes.AntiqueWhite;
            WindowMedical .Medicalt8.IsEnabled = true;
            WindowMedical .Medicalt8.Background = Brushes.AntiqueWhite;
            WindowMedical .Medicalt9.IsEnabled = true;
            WindowMedical .Medicalt9.Background = Brushes.AntiqueWhite;
            WindowMedical.MedicalBoxUriWeb.IsEnabled = true;
            WindowMedical.MedicalBoxUriWeb.Background = Brushes.AntiqueWhite;
            WindowMedical.FolderWebUriZaklad.Visibility = Visibility.Visible;
            WindowMedical.FolderStatuszaklad.Visibility = Visibility.Visible;
            WindowMedical.MedicalTablGrid.IsEnabled = false;

            if (IndexAddEdit == "addCommand")
            {
                WindowMedical.BorderLoadMedical.IsEnabled = false;
                WindowMedical.BorderGhangeMedical.IsEnabled = false;
                WindowMedical.BorderDeleteMedical.IsEnabled = false;
                WindowMedical.BorderPrintMedical.IsEnabled = false;
            }
            if (IndexAddEdit == "editCommand")
            {
                WindowMedical.BorderLoadMedical.IsEnabled = false;
                WindowMedical.BorderAddMedical.IsEnabled = false;
                WindowMedical.BorderDeleteMedical.IsEnabled = false;
                WindowMedical.BorderPrintMedical.IsEnabled = false;
            }
        }
        // команда печати
        RelayCommand? printVeiwModelMedical;
        public RelayCommand PrintVeiwModelMedical
        {
            get
            {
                return printVeiwModelMedical ??
                  (printVeiwModelMedical = new RelayCommand(obj =>
                  {
                      if (VeiwModelMedicals != null)
                      {
                          MessageBox.Show("Назва мед закладу :" + VeiwModelMedicals[0].name.ToString());
                      }
                  },
                 (obj) => VeiwModelMedicals != null));
            }
        }

        // команда загрузки сайта по ссылке 
        RelayCommand? webUriMedzaklad;
        public RelayCommand WebUriMedzaklad
        {
            get
            {
                return webUriMedzaklad ??
                  (webUriMedzaklad = new RelayCommand(obj =>
                  {
                      if (VeiwModelMedicals != null)
                      {
                          if (WindowMedical.MedicalTablGrid.SelectedIndex >= 0)
                          { 
                              if (VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex].uriwebZaklad != null)
                              { 
                                  string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                                  string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                                  string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                                  Process Rungoogle = new Process();
                                  Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                                  Rungoogle.StartInfo.Arguments = selectedMedical.uriwebZaklad;
                                  //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                                  Rungoogle.StartInfo.UseShellExecute = false;
                                  Rungoogle.EnableRaisingEvents = true;
                                  Rungoogle.Start();                         
                              }                          
                          }


                      }
                  },
                 (obj) => VeiwModelMedicals != null));
            }
        }

        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? visibleFolderUriWeb;
        public RelayCommand VisibleFolderUriWeb
        {
            get
            {
                return visibleFolderUriWeb ??
                  (visibleFolderUriWeb = new RelayCommand(obj =>
                  {
                      if (VeiwModelMedicals != null && WindowMedical.MedicalTablGrid.SelectedIndex >= 0)
                      { 
                          WindowMedical.FolderWebUriZaklad.Visibility = Visibility.Visible;
                          WindowMedical.FolderWorkzaklad.Visibility = Visibility.Visible;
                          WindowMedical.FolderStatuszaklad.Visibility = Visibility.Visible;
                          selectedMedical = VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex];

                          if (VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex].idstatus.Length<=2)
                          { 
                              string json = controlerStatusZaklad + VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex].idstatus;
                              CallServer.PostServer(controlerStatusZaklad, json, "GETID");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              StatusMedZaklad Idinsert = JsonConvert.DeserializeObject<StatusMedZaklad>(CallServer.ResponseFromServer);
 
                              VeiwModelMedicals[WindowMedical.MedicalTablGrid.SelectedIndex].idstatus += ":"+ Idinsert.nameStatus;

                          }
                          EdrpouMedZaklad = WindowMedical.Medicalt2.Text;                      
                      }

                  }));
            }
        }

        // команда загрузки  окна профилей медучреждений
        private RelayCommand? listProfilZaklad;
        public RelayCommand ListProfilZaklad
        {
            get
            {
                return listProfilZaklad ??
                  (listProfilZaklad = new RelayCommand(obj =>
                  {
                      if (WindowMedical.Medicalt2.Text != "")
                      { 
                           EdrpouMedZaklad = WindowMedical.Medicalt2.Text;
                           WinMedicalGrDiagnoz Order = new WinMedicalGrDiagnoz();
                           Order.Left = (MainWindow.ScreenWidth / 2)-150;
                           Order.Top = (MainWindow.ScreenHeight / 2) - 350;
                           Order.ShowDialog();
 
                      }
                          

                  }));
            }
        }

        

        // Выбор названия мед закладу
        private RelayCommand? searchMedical;
        public RelayCommand SearchMedical
        {
            get
            {
                return searchMedical ??
                  (searchMedical = new RelayCommand(obj =>
                  {
                      MetodSearchMedical();
                  }));
            }
        }

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyMedical;
        public RelayCommand CheckKeyMedical
        {
            get
            {
                return checkKeyMedical ??
                  (checkKeyMedical = new RelayCommand(obj =>
                  {
                      MetodSearchMedical();
                  }));
            }
        }


        private void MetodSearchMedical()
        {
            if (CheckStatusUser() == false) return;
            if (WindowMedical.PoiskMedical.Text.Trim() != "")
            {
                string jason = controlerMedical + "0/0/" + WindowMedical.PoiskMedical.Text;
                CallServer.PostServer(controlerMedical, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableVeiwMedical(CmdStroka);
            }
        }



        RelayCommand? checkMedicalPind;
        public RelayCommand CheckMedicalPind
        {
            get
            {
                return checkMedicalPind ??
                  (checkMedicalPind = new RelayCommand(obj =>
                  {

                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Medicalt4, 5);
                      if (MapOpisViewModel.WindowMen.Medicalt4.Text.Length >= 4)
                      {
                          string jason = MapOpisViewModel.pathcontrolerSob + "0/0/0/" + MapOpisViewModel.WindowMen.Medicalt4.Text;
                          CallServer.PostServer(MapOpisViewModel.pathcontrolerSob, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) MapOpisViewModel.InfoOfPind();
                      }
                  }));
            }
        }

        
        // команда загрузки  окна профилей медучреждений
        private RelayCommand? listStatusZaklad;
        public RelayCommand ListStatusZaklad
        {
            get
            {
                return listStatusZaklad ??
                  (listStatusZaklad = new RelayCommand(obj =>
                  {
                      if (WindowMedical.Medicalt2.Text != "")
                      {
                          EdrpouMedZaklad = "0";
                          selectedMedical.idstatus = "";
                          WinMedicalGrDiagnoz Order = new WinMedicalGrDiagnoz();
                          Order.Left = (MainWindow.ScreenWidth / 2) - 150;
                          Order.Top = (MainWindow.ScreenHeight / 2) - 350;
                          Order.ShowDialog();
                          SelectedMedical.idstatus = selectedMedical.idstatus;
                          WindowMedical.Medicalt11.Text = selectedMedical.idstatus;
                      }


                  }));
            }
        }

        #endregion
        #endregion

    }
}
