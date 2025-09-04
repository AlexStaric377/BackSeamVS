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

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    class ViewModelNsiMedZaklad : BaseViewModel
    {
        private WinNsiMedZaklad WindowMedZaklad = MainWindow.LinkMainWindow("WinNsiMedZaklad");
        MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
        private string pathcontrollerMedZaklad =  "/api/MedicalInstitutionController/";
        private bool closeWin = false;
        public  static MedicalInstitution selectedMedZaklad;
        public static ObservableCollection<MedicalInstitution> NsiModelMedZaklads { get; set; }
        public static ObservableCollection<ModelMedGrupDiagnoz> GrupMedZaklads { get; set; }

        public MedicalInstitution SelectedMedZaklad
        { get { return selectedMedZaklad; } set { selectedMedZaklad = value; OnPropertyChanged("SelectedMedZaklad"); } }
        // конструктор класса
        public ViewModelNsiMedZaklad()
        {
            string CmdStroka = "", tmpedrpou = "";
            if (ViewModelAnalogDiagnoz.Likar == "ReseptionAnalogLikar")
            {
                CallServer.PostServer(pathcontrollerMedZaklad, pathcontrollerMedZaklad + "0/0/0/2", "GETID");
                CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelMedical(CmdStroka);
            }
            else 
            {
                if (MapOpisViewModel.selectIcdGrDiagnoz != "" && MapOpisViewModel.selectIcdGrDiagnoz != null)
                {
                   
                    NsiModelMedZaklads = new ObservableCollection<MedicalInstitution>();
                    string json = ViewModelMedicalGrDiagnoz.controlerGrDiagnoz + "0/" + MapOpisViewModel.selectIcdGrDiagnoz+"/0";
                    if (ViewModelAnalogDiagnoz.Likar == "ListProfilMedical")
                    {
                        string grupdiagnoz = MapOpisViewModel.selectIcdGrDiagnoz.Substring(MapOpisViewModel.selectIcdGrDiagnoz.IndexOf(".")+1, MapOpisViewModel.selectIcdGrDiagnoz.Length-(MapOpisViewModel.selectIcdGrDiagnoz.IndexOf(".") + 1));
                        string keygrupicd = MapOpisViewModel.selectIcdGrDiagnoz.Substring(0, MapOpisViewModel.selectIcdGrDiagnoz.IndexOf(".")+1);
                        keygrupicd += grupdiagnoz.Substring(0, grupdiagnoz.IndexOf(".")+1);
                        grupdiagnoz = grupdiagnoz.Substring(grupdiagnoz.IndexOf(".") + 1, grupdiagnoz.Length - (grupdiagnoz.IndexOf(".") + 1));
                        keygrupicd += grupdiagnoz.Substring(0, grupdiagnoz.IndexOf("."));
                        json = ViewModelMedicalGrDiagnoz.controlerGrDiagnoz + "0/0/" + keygrupicd+"/0";
                    } 
                    CallServer.PostServer(ViewModelMedicalGrDiagnoz.controlerGrDiagnoz, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CmdStroka = CallServer.ServerReturn();
                        var GrDiagnoz = JsonConvert.DeserializeObject<ListModelMedGrupDiagnoz>(CmdStroka);
                        List<ModelMedGrupDiagnoz> grupDiagnoz = GrDiagnoz.ModelMedGrupDiagnoz.ToList();
                        GrupMedZaklads = new ObservableCollection<ModelMedGrupDiagnoz>((IEnumerable<ModelMedGrupDiagnoz>)grupDiagnoz);
                        

                        foreach (ModelMedGrupDiagnoz medGrupDiagnoz in GrupMedZaklads)
                        {
                            if (MapOpisViewModel.VeiwModelMedicals != null)
                            {
                                
                                foreach (MedicalInstitution medicalInstitution in MapOpisViewModel.VeiwModelMedicals)
                                {
                                    if (medGrupDiagnoz.kodZaklad == medicalInstitution.kodZaklad)
                                    {
                                        NsiModelMedZaklads.Add(medicalInstitution);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (tmpedrpou != medGrupDiagnoz.kodZaklad)
                                {
                                  
                                    json = pathcontrollerMedZaklad + medGrupDiagnoz.kodZaklad + "/0/0/0";
                                    CallServer.PostServer(pathcontrollerMedZaklad, json, "GETID");
                                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                                    {
                                        CmdStroka = CallServer.ServerReturn();
                                        var Medical = JsonConvert.DeserializeObject<ListModelMedical>(CmdStroka);
                                        List<MedicalInstitution> medzaklad = Medical.MedicalInstitution.ToList();
 
                                        foreach (MedicalInstitution medicalInstitution in medzaklad)
                                        {
                                            NsiModelMedZaklads.Add(medicalInstitution);
                                        }
                                        tmpedrpou = NsiModelMedZaklads[0].kodZaklad;
                                    }                                                                  
                                   
 
                                }
                            }
                        }
                        
                        if (MapOpisViewModel.PacientPostIndex != "")
                        {
                            ObservableCollection<MedicalInstitution> tmpNsiModelMedZaklads = new ObservableCollection<MedicalInstitution>();
                            foreach (MedicalInstitution Institution in NsiModelMedZaklads)
                            {
                                if (Institution.postIndex == MapOpisViewModel.PacientPostIndex ||
                                Institution.postIndex.Substring(0, 2) == MapOpisViewModel.PacientPostIndex.Substring(0, 2) ||
                                (Institution.postIndex.Substring(0, 1) == MapOpisViewModel.PacientPostIndex.Substring(0, 1) && MapOpisViewModel.PacientPostIndex.Trim().Length == 4) ||
                                (Institution.postIndex.Substring(0, 1) == "0" && MapOpisViewModel.PacientPostIndex.Substring(0, 1) == "0" && MapOpisViewModel.PacientPostIndex.Trim().Length == 5) ||
                               (Institution.postIndex.Substring(0, 1) == "0" && MapOpisViewModel.PacientPostIndex.Trim().Length == 4) ||
                               (Institution.postIndex.Trim().Length == 4 && MapOpisViewModel.PacientPostIndex.Substring(0, 1) == "0" && MapOpisViewModel.PacientPostIndex.Trim().Length == 5)) tmpNsiModelMedZaklads.Add(Institution);
                            }
                            if (tmpNsiModelMedZaklads.Count != 0) NsiModelMedZaklads = tmpNsiModelMedZaklads;
                        }
                    }
                    else AddNsiModelMedZaklads();
                }
                else AddNsiModelMedZaklads();            
            
            }

        }

        private void AddNsiModelMedZaklads()
        {
            CallServer.PostServer(pathcontrollerMedZaklad, pathcontrollerMedZaklad, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableNsiModelMedical(CmdStroka);
        }

        public static void ObservableNsiModelMedical(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelMedical>(CmdStroka);
            List<MedicalInstitution> res = result.MedicalInstitution.ToList();
            NsiModelMedZaklads = new ObservableCollection<MedicalInstitution>((IEnumerable<MedicalInstitution>)res);
            selectedMedZaklad = new MedicalInstitution();

        }


        // команда закрытия окна
        RelayCommand? closeModelMedZaklad;
        public RelayCommand CloseModelMedZaklad
        {
            get
            {
                return closeModelMedZaklad ??
                  (closeModelMedZaklad = new RelayCommand(obj =>
                  {
                      closeWin = true;
                      MetodSelectMedzaklad();
                  }));
            }
        }

        RelayCommand? viewUriWebMedzaklad;
        public RelayCommand ViewUriWebMedzaklad
        {
            get
            {
                return viewUriWebMedzaklad ??
                  (viewUriWebMedzaklad = new RelayCommand(obj =>
                  {
                      if (selectedMedZaklad.uriwebZaklad != "")
                      {
                          string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                          string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                          string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                          Process Rungoogle = new Process();
                          Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                          Rungoogle.StartInfo.Arguments = selectedMedZaklad.uriwebZaklad;
                          //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                          Rungoogle.StartInfo.UseShellExecute = false;
                          Rungoogle.EnableRaisingEvents = true;
                          Rungoogle.Start();
                      }
                  }));
            }
        }

        

        // Выбор названия мед закладу
        private RelayCommand? searchNameMedical;
        public RelayCommand SearchNameMedical
        {
            get
            {
                return searchNameMedical ??
                  (searchNameMedical = new RelayCommand(obj =>
                  {
                      if (WindowMedZaklad.PoiskMedical.Text.Trim() != "")
                      {
                          string jason = pathcontrollerMedZaklad + "0/0/" + WindowMedZaklad.PoiskMedical.Text + "/0";
                          CallServer.PostServer(pathcontrollerMedZaklad, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableNsiModelMedical(CmdStroka);
                          WindowMedZaklad.TablMedzaklad.ItemsSource = NsiModelMedZaklads;
                      }

                  }));
            }
        }

        

        RelayCommand? selectMedzaklad;
        public RelayCommand SelectMedzaklad
        {
            get
            {
                return selectMedzaklad ??
                  (selectMedzaklad = new RelayCommand(obj =>
                  {
                      MetodSelectMedzaklad();
                  }));
            }
        }

        private void MetodSelectMedzaklad()
        {
            if (selectedMedZaklad != null && selectedMedZaklad.id != 0)
            {
                closeWin = true;
                WindowMain.Likart9.Text = selectedMedZaklad.name;
                WindowMain.Likart8.Text = selectedMedZaklad.kodZaklad;
                WindowMain.Likart4.Text = selectedMedZaklad.adres;
                WindowMain.Likart5.Text = selectedMedZaklad.postIndex;
                
            }
            if(closeWin == true) WindowMedZaklad.Close();       
        }

    }
}
