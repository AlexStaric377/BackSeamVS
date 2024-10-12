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

        private string pathcontrollerMedZaklad =  "/api/MedicalInstitutionController/";
        public  static MedicalInstitution selectedMedZaklad;
        public static ObservableCollection<MedicalInstitution> NsiModelMedZaklads { get; set; }
        public static ObservableCollection<ModelMedGrupDiagnoz> GrupMedZaklads { get; set; }

        public MedicalInstitution SelectedMedZaklad
        { get { return selectedMedZaklad; } set { selectedMedZaklad = value; OnPropertyChanged("SelectedMedZaklad"); } }
        // конструктор класса
        public ViewModelNsiMedZaklad()
        {
            string CmdStroka = "";
            if (MapOpisViewModel.selectIcdGrDiagnoz != "" && MapOpisViewModel.selectIcdGrDiagnoz != null)
            {

                NsiModelMedZaklads = new ObservableCollection<MedicalInstitution>();
                string json = ViewModelMedicalGrDiagnoz.controlerGrDiagnoz +"0/"+ MapOpisViewModel.selectIcdGrDiagnoz;
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
                                if (medGrupDiagnoz.edrpou == medicalInstitution.edrpou)
                                {
                                    NsiModelMedZaklads.Add(medicalInstitution);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            json = pathcontrollerMedZaklad + medGrupDiagnoz.edrpou + "/0/0";
                            CallServer.PostServer(pathcontrollerMedZaklad, json, "GETID");
                            if (CallServer.ResponseFromServer.Contains("[]") == false)
                            {
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                MedicalInstitution medzaklad = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);
                                NsiModelMedZaklads.Add(medzaklad);
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
                          string jason = pathcontrollerMedZaklad + "0/0/" + WindowMedZaklad.PoiskMedical.Text;
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
            MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
            if (selectedMedZaklad != null)
            {
                WindowMain.Likart9.Text = selectedMedZaklad.name.ToString();
                WindowMain.Likart8.Text = selectedMedZaklad.edrpou.ToString();
                WindowMain.Likart4.Text = selectedMedZaklad.adres.ToString();
                WindowMain.Likart5.Text = selectedMedZaklad.postIndex.ToString();
            }
            WindowMedZaklad.Close();        
        }

    }
}
