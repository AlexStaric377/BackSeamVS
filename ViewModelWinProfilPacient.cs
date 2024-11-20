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
/// Многопоточность
using System.Threading;
using System.Windows.Threading;
using System.ServiceProcess;
using System.Diagnostics;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    class ViewModelWinProfilPacient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private string pathcontrolerPacientProfil = "/api/PacientController/";
        public static ModelPacient selectedPacientProfil;


        public static List<string> UnitCombProfil { get; set; } = new List<string> { "чол.", "жін." };
        public static ObservableCollection<ModelPacient> ViewPacientProfils { get; set; }
        public ModelPacient SelectPacientProfil
        {
            get { return selectedPacientProfil; }
            set { selectedPacientProfil = value; OnPropertyChanged("SelectPacientProfil"); }
        }


        public ViewModelWinProfilPacient()
        {
            if (MapOpisViewModel._pacientProfil != "")
            {
                CallServer.PostServer(pathcontrolerPacientProfil, pathcontrolerPacientProfil + MapOpisViewModel._pacientProfil + "/0/0/0/0", "GETID");
                string CmdStroka = CallServer.ServerReturn();
                ObservableViewPacientProfil(CmdStroka);
            }
            else selectedPacientProfil = new ModelPacient();

        }

        public static void ObservableViewPacientProfil(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
            List<ModelPacient> res = result.ViewPacient.ToList();
            ViewPacientProfils = new ObservableCollection<ModelPacient>((IEnumerable<ModelPacient>)res);
            selectedPacientProfil = ViewPacientProfils[0];
        }

        private string _SelectedCombProfil;
        public string SelectedCombProfil
        {
            get => _SelectedCombProfil;
            set
            {
                //меняем значение в обычном порядке
                _SelectedCombProfil = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCombProfil)));
                //OnPropertyChanged(nameof(SelectedComb));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewCombProfil(_SelectedCombProfil);
            }
        }

        public void SetNewCombProfil(string selected = "")
        {
            WinProfilPacient WindowResult = MainWindow.LinkMainWindow("WinProfilPacient");
            WindowResult.PacientProfilt7.Text = selected == "0" ? "чол." : "жін.";
        }


        // команда закрытия окна
        RelayCommand? closeProfil;
        public RelayCommand CloseProfil
        {
            get
            {
                return closeProfil ??
                  (closeProfil = new RelayCommand(obj =>
                  {
                      WinProfilPacient WindowResult = MainWindow.LinkMainWindow("WinProfilPacient");
                      WindowResult.Close();
                  }));
            }
        }


        // команда закрытия окна
        RelayCommand? insertProfilCommand;
        public RelayCommand InsertProfilCommand
        {
            get
            {
                return insertProfilCommand ??
                  (insertProfilCommand = new RelayCommand(obj =>
                  {

                      WinProfilPacient WindowResult = MainWindow.LinkMainWindow("WinProfilPacient");
                      List<string> Units = new List<string> { "чол.", "жін." };
                      WindowResult.CombgenderProfil.SelectedIndex = 0;
                      WindowResult.CombgenderProfil.ItemsSource = Units;
                      WindowResult.PacientProfilt7.Text = WindowResult.CombgenderProfil.SelectedIndex == 0 ? "чол." : "жін.";
                      WindowResult.PacientProfilLab1.Visibility = Visibility.Hidden;
                      WindowResult.PacientProfilt2.IsReadOnly = false;
                      WindowResult.PacientProfilt2.Background = Brushes.White;
                      WindowResult.PacientProfilt3.IsReadOnly = false;
                      WindowResult.PacientProfilt3.Background = Brushes.White;
                      WindowResult.PacientProfilt4.IsReadOnly = false;
                      WindowResult.PacientProfilt4.Background = Brushes.White;
                      WindowResult.PacientProfilt5.IsReadOnly = false;
                      WindowResult.PacientProfilt5.Background = Brushes.White;
                      WindowResult.PacientProfilt6.IsReadOnly = false;
                      WindowResult.PacientProfilt6.Background = Brushes.White;
                      WindowResult.PacientProfilt7.IsReadOnly = false;
                      WindowResult.PacientProfilt7.Background = Brushes.White;
                      WindowResult.PacientProfilt8.IsReadOnly = false;
                      WindowResult.PacientProfilt8.Background = Brushes.White;
                      WindowResult.PacientProfilt9.IsReadOnly = false;
                      WindowResult.PacientProfilt9.Background = Brushes.White;
                      WindowResult.PacientProfilt11.IsReadOnly = false;
                      WindowResult.PacientProfilt11.Background = Brushes.White;
                      WindowResult.PacientProfilt13.IsReadOnly = false;
                      WindowResult.PacientProfilt13.Background = Brushes.White;

                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? saveProfilPacientCommand;
        public RelayCommand SaveProfilPacientCommand
        {
            get
            {
                return saveProfilPacientCommand ??
                  (saveProfilPacientCommand = new RelayCommand(obj =>
                  {
                      WinProfilPacient WindowResult = MainWindow.LinkMainWindow("WinProfilPacient");

                      WindowResult.PacientProfilt2.IsReadOnly = true;
                      WindowResult.PacientProfilt3.IsReadOnly = true;
                      WindowResult.PacientProfilt4.IsReadOnly = true;
                      WindowResult.PacientProfilt5.IsReadOnly = true;
                      WindowResult.PacientProfilt6.IsReadOnly = true;
                      WindowResult.PacientProfilt7.IsReadOnly = true;
                      WindowResult.PacientProfilt8.IsReadOnly = true;
                      WindowResult.PacientProfilt9.IsReadOnly = true;
                      WindowResult.PacientProfilt11.IsReadOnly = true;
                      WindowResult.PacientProfilt13.IsReadOnly = true;

                      WindowResult.PacientProfilt2.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt3.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt4.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt5.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt6.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt7.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt8.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt9.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt11.Background = Brushes.Transparent;
                      WindowResult.PacientProfilt13.Background = Brushes.Transparent;

                      //  формирование кода Detailing по значениею группы выранного храктера жалобы
                      WinNsiPacient winNsiPacient = MainWindow.LinkMainWindow("WinNsiPacient");
                      MapOpisViewModel.CallViewProfilLikar = "PacientProfil";
                      MapOpisViewModel.PacientPostIndex = WindowResult.PacientProfilt13.Text.ToString();
                      SelectNewPacientProfil();
                      string json = JsonConvert.SerializeObject(selectedPacientProfil);
                      CallServer.PostServer(pathcontrolerPacientProfil, json, "POST");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                      if (Idinsert != null)
                      {
                          MapOpisViewModel._pacientProfil = Idinsert.kodPacient;
                          ViewModelNsiPacient.LoadNsiPacient();
                          if (winNsiPacient != null) winNsiPacient.TablPacients.ItemsSource = ViewModelNsiPacient.NsiPacients;
                          MapOpisViewModel.selectedProfilPacient = MapOpisViewModel.selectedPacientProfil = Idinsert;
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                          "Ви бажаєте створити кабінет пацієнта для зберігання" + Environment.NewLine + " результатів ваших опитувань та записів до лікаря?";
                          MapOpisViewModel.SelectedDelete(-1);
                          if (MapOpisViewModel.DeleteOnOff == true) MapOpisViewModel.NewAccountRecords();
                      }
                      WindowResult.Close();
                      if (winNsiPacient != null) winNsiPacient.Close();

                  }));
            }

        }

        private void SelectNewPacientProfil()
        {
            if (selectedPacientProfil == null) selectedPacientProfil = new ModelPacient();
            CallServer.PostServer(pathcontrolerPacientProfil, pathcontrolerPacientProfil + "0/0/0/0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            selectedPacientProfil.id = 0;
            selectedPacientProfil.kodPacient = "PCN.0000000001";
            if (CmdStroka.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                int indexdia = Convert.ToInt32(Idinsert.kodPacient.Substring(Idinsert.kodPacient.LastIndexOf(".") + 1, Idinsert.kodPacient.Length - (Idinsert.kodPacient.LastIndexOf(".") + 1))) + 1;
                string _repl = "0000000000";
                selectedPacientProfil.kodPacient = "PCN." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString();
            }

        }

        // команда открытия  окна анализа мочи
        RelayCommand? pacientUrineSelect;
        public RelayCommand PacientUrineSelect
        {
            get
            {
                return pacientUrineSelect ??
                  (pacientUrineSelect = new RelayCommand(obj =>
                  {
                      WinColectionAnalizUrine Urine = new WinColectionAnalizUrine();
                      Urine.ShowDialog();
                  }));
            }
        }

        // команда открытия  окна анализа крови
        RelayCommand? pacientBloodSelect;
        public RelayCommand PacientBloodSelect
        {
            get
            {
                return pacientBloodSelect ??
                  (pacientBloodSelect = new RelayCommand(obj =>
                  {
                      WinColectionAnalizBlood Blood = new WinColectionAnalizBlood();
                      Blood.ShowDialog();
                  }));
            }
        }

        // команда открытия  окна анализа крови
        RelayCommand? pacientStanSelect;
        public RelayCommand PacientStanSelect
        {
            get
            {
                return pacientStanSelect ??
                  (pacientStanSelect = new RelayCommand(obj =>
                  {
                      WinColectionStanHealth Blood = new WinColectionStanHealth();
                      Blood.ShowDialog();
                  }));
            }
        }

    }
}
