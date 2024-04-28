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
    public class ViewModelNsiLikar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        WinNsiLikar WindowMen = MainWindow.LinkMainWindow("WinNsiLikar");
        private  static string pathcontroller =  "/api/ApiControllerDoctor/";
        public static ModelDoctor selectedLikar;
        public static  ObservableCollection<ModelDoctor> NsiLikars { get; set; }
 

        public ModelDoctor SelectedLikar
        { get { return selectedLikar; } set { selectedLikar = value; OnPropertyChanged("SelectedLikar"); } }
        // конструктор класса
        public ViewModelNsiLikar()
        {

            if (MapOpisViewModel.EdrpouMedZaklad == "")
            {
                switch (MapOpisViewModel.CallViewProfilLikar)
                {
                    case "ProfilLikar":
                        CallServer.PostServer(pathcontroller, pathcontroller, "GET");
                        break;
                    case "WinNsiLikar":
                        CallServer.PostServer(pathcontroller, pathcontroller, "GET");
                        break;
                    default:
                        CallServer.PostServer(pathcontroller, pathcontroller + MapOpisViewModel.selectedColectionInterview.kodDoctor+"/0", "GETID");
                        break;
                }
            }
            else
            {
                CallServer.PostServer(pathcontroller, pathcontroller + "0/" + MapOpisViewModel.EdrpouMedZaklad, "GETID");
            }
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelLikar(CmdStroka);
        }
        public static void ObservableNsiModelLikar(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            NsiLikars = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);
            if (MapOpisViewModel.selectIcdGrDiagnoz != "")
            { 
                ObservableCollection<ModelDoctor> TmpNsiLikars = new ObservableCollection<ModelDoctor>();
                foreach (ModelDoctor modelDoctor in NsiLikars)
                {
                    string json = ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz + modelDoctor.kodDoctor+"/"+ MapOpisViewModel.selectIcdGrDiagnoz.ToString();
                    CallServer.PostServer(ViewModelLikarGrupDiagnoz.controlerLikarGrDiagnoz, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)TmpNsiLikars.Add(modelDoctor);
                }
                if (TmpNsiLikars.Count != 0) NsiLikars = TmpNsiLikars;
            }
        }



        // команда закрытия окна
        RelayCommand? closeLikar;
        public RelayCommand CloseLikar
        {
            get
            {
                return closeLikar ??
                  (closeLikar = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WindowMain.LikarIntert2.Text = "";
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectLikar;
        public RelayCommand SelectLikar
        {
            get
            {
                return selectLikar ??
                  (selectLikar = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("BackMain");
                      WindowMain.AccountUsert5.Text = "";
                      MapOpisViewModel.nameDoctor = "";
                      WindowMain.Intert2.Text = "";
                      if (selectedLikar != null)
                      {
                          MapOpisViewModel._kodDoctor = selectedLikar.kodDoctor.ToString();
                          MapOpisViewModel.nameDoctor = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString() + " " + selectedLikar.telefon.ToString();
                          WindowMain.Intert2.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString();
                          WindowMain.LikarIntert2.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString() + " " + selectedLikar.telefon.ToString();
                          WindowMain.AccountUsert5.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString();

                          if (MapOpisViewModel.CallViewProfilLikar == "ProfilLikar") MapOpisViewModel.selectedProfilLikar = selectedLikar;
                      }
                      WindowMen.Close();
                 
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? viewUriWebProfilLikar;
        public RelayCommand ViewUriWebProfilLikar
        {
            get
            {
                return viewUriWebProfilLikar ??
                  (viewUriWebProfilLikar = new RelayCommand(obj =>
                  {
                      if (selectedLikar.uriwebDoctor != "")
                      {
                          string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                          string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                          string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                          Process Rungoogle = new Process();
                          Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                          Rungoogle.StartInfo.Arguments = selectedLikar.uriwebDoctor;
                          //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                          Rungoogle.StartInfo.UseShellExecute = false;
                          Rungoogle.EnableRaisingEvents = true;
                          Rungoogle.Start();
                      }
                  }));
            }
        }
       

    }
 
}
