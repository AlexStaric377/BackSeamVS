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

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {

        // ViewModelPacient справочник пациентов
        // клавиша в окне:  Пациент

        #region Обработка событий и команд вставки, удаления и редектирования справочника "пациентов"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех паціентів из БД
        /// через механизм REST.API
        /// </summary> 
        
        private bool editboolPacientProfil = false, addboolPacientProfil = false, loadboolPacientProfil = false;
        private string edittextPacientProfil = "";
        private string pathcontrolerPacientProfil =  "/api/PacientController/";
        public static ModelPacient selectedPacientProfil;
        public static string _pacientProfil = "", _pacientName="",_pacientGender = "",RegStatusUser= "Пацїєнт";
        public static List<string> UnitCombProfil { get; set; } = new List<string> { "чол.", "жін." };
        public static ObservableCollection<ModelPacient> ViewPacientProfils { get; set; }
        public ModelPacient SelectedPacientProfil
        {
            get { return selectedPacientProfil; }
            set { selectedPacientProfil = value; OnPropertyChanged("SelectedPacientProfil"); }
        }


        public static void ObservableViewPacientProfil(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
            List<ModelPacient> res = result.ViewPacient.ToList();
            ViewPacientProfils = new ObservableCollection<ModelPacient>((IEnumerable<ModelPacient>)res);



        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadPacientProfil;
        public RelayCommand ProfilLoadPacient
        {
            get
            {
                return loadPacientProfil ??
                  (loadPacientProfil = new RelayCommand(obj =>
                  {
                      MethodLoadPacientProfil();
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addPacientProfil;
        public RelayCommand AddPacientProfil
        {
            get
            {
                return addPacientProfil ??
                  (addPacientProfil = new RelayCommand(obj =>
                  { MethodaddcomPacientProfil(); }));
            }
        }

 
        private void MethodaddcomPacientProfil()
        {
          
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";

            if (addboolPacientProfil == false)
            {
                WindowMain.BorderCabPacient.Visibility = Visibility.Hidden;
                 SelectedPacientProfil = new ModelPacient();
                selectedPacientProfil = new ModelPacient();
                BoolTruePacientProfil();
            }
            else BoolFalsePacientProfil();



        }

        private void MethodLoadPacientProfil()
        {
            WindowMain.BorderCabPacient.Visibility = Visibility.Hidden;
            loadboolPacientProfil = true;
            SelectRegPacientProfil();
 
        }

        private void SelectRegPacientProfil()
        {
            MainWindow WindowPacientProfil = MainWindow.LinkNameWindow("BackMain");
            CallViewProfilLikar = "PacientProfil";
            selectedPacientProfil = new ModelPacient();
            WinNsiPacient NewOrder = new WinNsiPacient();
            NewOrder.ShowDialog();
            CallViewProfilLikar = "";
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                WarningMessageOfProfilPacient();
                return;
            }
            ObservableViewPacientProfil(CmdStroka);
            if (selectedPacientProfil != null)
            {
                LoadInfoPacient("пацієнта.");

                _pacientProfil = selectedPacientProfil.kodPacient;
                _pacientGender = selectedPacientProfil.gender;
                _pacientName = selectedPacientProfil.name + " " + selectedPacientProfil.surname + " " + selectedPacientProfil.profession + " " + selectedPacientProfil.tel;
                SelectedPacientProfil = selectedPacientProfil;
                WindowPacientProfil.PacientIntert3.Text = _pacientName;
                WindowPacientProfil.KabPacientNameInterv.Text = "Опитування пацієнта: ";
                WindowPacientProfil.ReceptionPacientzap3.Text = _pacientName;
                
                WindowPacientProfil.StatusHealth3.Text = _pacientName;
                SelectedProfilPacient = selectedProfilPacient;
                if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();
                modelColectionInterview.namePacient = selectedPacientProfil.name + selectedPacientProfil.surname;
                modelColectionInterview.kodPacient = selectedPacientProfil.kodPacient;

                WindowPacientProfil.LikarInterviewAvtort7.Text = "";
                ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>();
                WindowPacientProfil.ColectionIntevPacientTablGrid.ItemsSource = ColectionInterviewIntevPacients;
                ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
                WindowPacientProfil.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;
                ViewPacientMapAnalizs = new ObservableCollection<PacientMapAnaliz>();
                WindowPacientProfil.StatusHealthTablGrid.ItemsSource = ViewPacientMapAnalizs;

                boolVisibleMessage = true;
                MethodLoadtableColectionIntevPacient();
                LoadReceptionPacients();
                MethodLoadStanHealthPacient();


                MessageWarning Info = MainWindow.LinkMainWindow("MessageWarning");
                if (Info != null) Info.Close();
                boolVisibleMessage = false;

            }

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
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            WindowMen.PacientProfilt7.Text = selected == "0" ? "чол." : "жін.";
        }

        private void BoolTruePacientProfil()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            addboolPacientProfil = true;
            editboolPacientProfil = true;
            List<string> Units = new List<string> { "чол.", "жін." };
            WindowMen.CombgenderProfil.ItemsSource = Units;
            WindowMen.CombgenderProfil.SelectedIndex = Convert.ToInt32(SelectedComb);
            WindowMen.CombgenderProfil.IsEnabled = true;
            WindowMen.PacientProfilt2.IsEnabled = true;
            WindowMen.PacientProfilt2.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt3.IsEnabled = true;
            WindowMen.PacientProfilt3.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt4.IsEnabled = true;
            WindowMen.PacientProfilt4.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt5.IsEnabled = true;
            WindowMen.PacientProfilt5.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt6.IsEnabled = true;
            WindowMen.PacientProfilt6.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt7.IsEnabled = true;
            WindowMen.PacientProfilt7.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt8.IsEnabled = true;
            WindowMen.PacientProfilt8.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt9.IsEnabled = true;
            WindowMen.PacientProfilt9.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt11.IsEnabled = true;
            WindowMen.PacientProfilt11.Background = Brushes.AntiqueWhite;
            WindowMen.PacientProfilt13.IsEnabled = true;
            WindowMen.PacientProfilt13.Background = Brushes.AntiqueWhite;
        }

        private void BoolFalsePacientProfil()
        {
            addboolPacientProfil = false;
            editboolPacientProfil = false;
            WindowMen.Combgender.IsEnabled = false;
            WindowMen.PacientProfilt2.IsEnabled = false;
            WindowMen.PacientProfilt2.Background = Brushes.White;
            WindowMen.PacientProfilt3.IsEnabled = false;
            WindowMen.PacientProfilt3.Background = Brushes.White;
            WindowMen.PacientProfilt4.IsEnabled = false;
            WindowMen.PacientProfilt4.Background = Brushes.White;
            WindowMen.PacientProfilt5.IsEnabled = false;
            WindowMen.PacientProfilt5.Background = Brushes.White;
            WindowMen.PacientProfilt6.IsEnabled = false;
            WindowMen.PacientProfilt6.Background = Brushes.White;
            WindowMen.PacientProfilt7.IsEnabled = false;
            WindowMen.PacientProfilt7.Background = Brushes.White;
            WindowMen.PacientProfilt8.IsEnabled = false;
            WindowMen.PacientProfilt8.Background = Brushes.White;
            WindowMen.PacientProfilt9.IsEnabled = false;
            WindowMen.PacientProfilt9.Background = Brushes.White;
            WindowMen.PacientProfilt11.IsEnabled = false;
            WindowMen.PacientProfilt11.Background = Brushes.White;
            WindowMen.PacientProfilt13.IsEnabled = false;
            WindowMen.PacientProfilt13.Background = Brushes.White;

        }

        // команда удаления
        private RelayCommand? removePacientProfil;
        public RelayCommand RemovePacientProfil
        {
            get
            {
                return removePacientProfil ??
                  (removePacientProfil = new RelayCommand(obj =>
                  {
                      if (selectedPacientProfil != null)
                      {
                          MetodRemovePrifilPacient(selectedPacientProfil.kodPacient);
                          selectedPacientProfil = new ModelPacient();
                      }
                      BoolFalsePacientProfil();
                      IndexAddEdit = "";
                  }));
            }
        }


        // команда  редактировать
        private RelayCommand? editPacientProfil;
        public RelayCommand? EditPacientProfil
        {
            get
            {
                return editPacientProfil ??
                  (editPacientProfil = new RelayCommand(obj =>
                  {
                      if (selectedPacientProfil != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolPacientProfil == false) BoolTruePacientProfil();
                          else
                          {
                              BoolFalsePacientProfil();
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? savePacientProfil;
        public RelayCommand SavePacientProfil
        {
            get
            {
                return savePacientProfil ??
                  (savePacientProfil = new RelayCommand(obj =>
                  {
                      BoolFalsePacientProfil();
                      if (WindowMen.PacientProfilt4.Text.Length != 0 && WindowMen.PacientProfilt7.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewPacientProfil();
                              string json = JsonConvert.SerializeObject(selectedPacientProfil);
                              CallServer.PostServer(pathcontrolerPacient, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                              int Countins = ViewPacientProfils != null ? ViewPacientProfils.Count : 0;
                              if (ViewPacientProfils == null)
                              {
                                  ViewPacientProfils = new ObservableCollection<ModelPacient>();
                                  ViewPacientProfils.Add(Idinsert);
                              }
                              else ViewPacientProfils.Insert(Countins, Idinsert);
                              SelectedPacientProfil = Idinsert;
                              _pacientProfil = Idinsert.kodPacient;
                              WindowMen.PacientIntert3.Text = selectedPacientProfil.name + " " + selectedPacientProfil.surname + " " + selectedPacientProfil.profession + " " + selectedPacientProfil.tel;


                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedPacientProfil);
                              CallServer.PostServer(pathcontrolerPacient, json, "PUT");
                          }
                      }
                      IndexAddEdit = "";

                  }));
            }
        }

        public void SelectNewPacientProfil()
        {

            CallServer.PostServer(pathcontrolerPacient, pathcontrolerPacient+ "0/0/0/0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            selectedPacientProfil.kodPacient = "PCN.000000001";
            if (CmdStroka.Contains("[]") == false)
            { 
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                int indexdia = Convert.ToInt32(Idinsert.kodPacient.Substring(Idinsert.kodPacient.LastIndexOf(".") + 1, Idinsert.kodPacient.Length - (Idinsert.kodPacient.LastIndexOf(".") + 1)))+1;
                string _repl = "000000000";
                selectedPacientProfil.kodPacient = "PCN." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString();
            }
            MapOpisViewModel.PacientPostIndex = WindowMen.PacientProfilt13.Text.ToString();
            selectedPacientProfil.age = Convert.ToInt32(WindowMen.PacientProfilt4.Text);
            selectedPacientProfil.email = WindowMen.PacientProfilt11.Text;
            selectedPacientProfil.gender = WindowMen.PacientProfilt7.Text;
            selectedPacientProfil.growth = WindowMen.PacientProfilt6.Text.ToString() != "" ? Convert.ToInt32(WindowMen.PacientProfilt6.Text) : 0;
            selectedPacientProfil.pind = WindowMen.PacientProfilt13.Text.ToString();
            selectedPacientProfil.name = WindowMen.PacientProfilt2.Text;
            selectedPacientProfil.surname = WindowMen.PacientProfilt3.Text;
            selectedPacientProfil.profession = WindowMen.PacientProfilt9.Text;
            selectedPacientProfil.tel = "+" + WindowMen.PacientProfilt8.Text;
            selectedPacientProfil.weight = WindowMen.PacientProfilt5.Text.ToString() != "" ? Convert.ToDecimal(WindowMen.PacientProfilt5.Text) : 0;
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
                      "Ви бажаєте створити кабінет пацієнта для зберігання" + Environment.NewLine + " результатів ваших опитувань та записів до лікаря?";
            SelectedOkNo();
            if (MapOpisViewModel.DeleteOnOff == true)
            {
                MapOpisViewModel.selectedProfilPacient = selectedPacientProfil;
                MapOpisViewModel.CallViewProfilLikar = "PacientProfil";
                NewAccountRecords();

            }
        }

        // команда печати
        RelayCommand? printPacientProfil;
        public RelayCommand PrintPacientProfil
        {
            get
            {
                return printPacientProfil ??
                  (printPacientProfil = new RelayCommand(obj =>
                  {
                     
                      if (selectedPacientProfil != null)
                      {
                          MessageBox.Show("Пацієнт :" + selectedPacientProfil.name.ToString());
                      }
                  },
                 (obj) => selectedPacientProfil != null));
            }
        }


        #endregion
        #endregion
    }
}

