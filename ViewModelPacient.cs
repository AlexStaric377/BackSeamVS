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
    public partial class MapOpisViewModel : BaseViewModel
    {
        
        // ViewModelPacient справочник пациентов
        // клавиша в окне:  Пациент

        #region Обработка событий и команд вставки, удаления и редектирования справочника "пациентов"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех паціентів из БД
        /// через механизм REST.API
        /// </summary>      
        private bool editboolPacient = false, addboolPacient = false, loadboolPacient = false;
        private string edittextPacient = "";
        public static string pathcontrolerPacient =  "/api/PacientController/", controlerLifePacient = "/api/LifePacientController/"
            , controlerLifeDoctor = "/api/LifeDoctorController/";
        private ModelPacient selectedPacient;
        public static string _pacient = "";
        public static List<string> UnitComb { get; set; } = new List<string> { "чол.", "жін." };
        public static ObservableCollection<ModelPacient> ViewPacients { get; set; }
        public ModelPacient SelectedPacient
        {
            get { return selectedPacient; }
            set { selectedPacient = value; OnPropertyChanged("SelectedPacient"); }
        }

 
        public static void ObservableViewPacient(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
            List<ModelPacient> res = result.ViewPacient.ToList();
            ViewPacients = new ObservableCollection<ModelPacient>((IEnumerable<ModelPacient>)res);
            WindowMen.PacientTablGrid.ItemsSource = ViewPacients;


        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadPacient;
        public RelayCommand PacientListLoad
        {
            get
            {
                return loadPacient ??
                  (loadPacient = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadPacient();
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addPacient;
        public RelayCommand AddPacient
        {
            get
            {
                return addPacient ??
                  (addPacient = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandPacient(); }));
            }
        }

        private void AddComandPacient()
        {
            if (loadboolPacient == false) MethodLoadPacient();
            MethodaddcomPacient();
        }

        private void MethodaddcomPacient()
        {
            WindowMen.PacientTablGrid.SelectedItem = null;            
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolPacient == false)
            {
                SelectedPacient = new ModelPacient();
                BoolTruePacient();
            } 
            else BoolFalsePacient();

           

        }

        private void MethodLoadPacient()
        {
            WindowMen.Loadpac.Visibility = Visibility.Hidden;
            loadboolPacient = true;
            CallServer.PostServer(pathcontrolerPacient, pathcontrolerPacient, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewPacient(CmdStroka);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        private string _SelectedComb;
        public string SelectedComb
        {
            get => _SelectedComb;
            set
            {
                 //меняем значение в обычном порядке
                _SelectedComb = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedComb)));
                //OnPropertyChanged(nameof(SelectedComb));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewComb(_SelectedComb);
            }
        }

        public void SetNewComb(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            WindowMen.Pacientt7.Text = selected == "0" ? "чол." : "жін.";
        }

        private void BoolTruePacient()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            addboolPacient = true;
            editboolPacient = true;
            List<string> Units = new List<string> { "чол.", "жін." };
            WindowMen.Combgender.ItemsSource = Units;
            WindowMen.Combgender.SelectedIndex = Convert.ToInt32(SelectedComb);
            WindowMen.Combgender.IsEnabled = true;
            WindowMen.Pacientt2.IsEnabled = true;
            WindowMen.Pacientt2.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt3.IsEnabled = true;
            WindowMen.Pacientt3.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt4.IsEnabled = true;
            WindowMen.Pacientt4.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt5.IsEnabled = true;
            WindowMen.Pacientt5.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt6.IsEnabled = true;
            WindowMen.Pacientt6.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt7.IsEnabled = true;
            WindowMen.Pacientt7.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt8.IsEnabled = true;
            WindowMen.Pacientt8.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt9.IsEnabled = true;
            WindowMen.Pacientt9.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt11.IsEnabled = true;
            WindowMen.Pacientt11.Background = Brushes.AntiqueWhite;
            WindowMen.Pacientt13.IsEnabled = true;
            WindowMen.Pacientt13.Background = Brushes.AntiqueWhite;
            WindowMen.PacientTablGrid.IsEnabled = false;
        }

        private void BoolFalsePacient()
        {
            addboolPacient = false;
            editboolPacient = false;
            WindowMen.Combgender.IsEnabled = false;
            WindowMen.Pacientt2.IsEnabled = false;
            WindowMen.Pacientt2.Background = Brushes.White;
            WindowMen.Pacientt3.IsEnabled = false;
            WindowMen.Pacientt3.Background = Brushes.White;
            WindowMen.Pacientt4.IsEnabled = false;
            WindowMen.Pacientt4.Background = Brushes.White;
            WindowMen.Pacientt5.IsEnabled = false;
            WindowMen.Pacientt5.Background = Brushes.White;
            WindowMen.Pacientt6.IsEnabled = false;
            WindowMen.Pacientt6.Background = Brushes.White;
            WindowMen.Pacientt7.IsEnabled = false;
            WindowMen.Pacientt7.Background = Brushes.White;
            WindowMen.Pacientt8.IsEnabled = false;
            WindowMen.Pacientt8.Background = Brushes.White;
            WindowMen.Pacientt9.IsEnabled = false;
            WindowMen.Pacientt9.Background = Brushes.White;
            WindowMen.Pacientt11.IsEnabled = false;
            WindowMen.Pacientt11.Background = Brushes.White;
            WindowMen.Pacientt13.IsEnabled = false;
            WindowMen.Pacientt13.Background = Brushes.White;
            WindowMen.PacientTablGrid.IsEnabled = true;

        }

        // команда удаления
        private RelayCommand? removePacient;
        public RelayCommand RemovePacient
        {
            get
            {
                return removePacient ??
                  (removePacient = new RelayCommand(obj =>
                  {
                      if (selectedPacient != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              MetodRemovePrifilPacient(selectedPacient.kodPacient);
                              ViewPacients.Remove(selectedPacient);
                              selectedPacient = new ModelPacient();
                              BoolFalsePacient();
                          }
                      }
                      
                      IndexAddEdit = "";
                  },
                 (obj) => ViewPacients != null));
            }
        }
        public static void MetodRemovePrifilPacient(string kodPacient ="")
        {
            string json = pathcontrolerPacient + "0/" + kodPacient;
            CallServer.PostServer(pathcontrolerPacient, json, "DELETE");

            // удаление анализа крови PacientAnalizKrovi
            json = ViewModelColectionAnalizBlood.pathcontrollerAnalizBlood + "0/" + kodPacient;
            CallServer.PostServer(ViewModelColectionAnalizBlood.pathcontrollerAnalizBlood, json, "DELETE");

            // удаление анализа мочи PacientAnalizUrine
            json = ViewModelColectionAnalizUrine.pathcontrollerAnalizUrine + "0/" + kodPacient;
            CallServer.PostServer(ViewModelColectionAnalizUrine.pathcontrollerAnalizUrine, json, "DELETE");

            // удаление пульс давление ... PacientMapAnaliz
            json = pathcontrollerPacientMapAnaliz + "0/" + kodPacient;
            CallServer.PostServer(pathcontrollerPacientMapAnaliz, json, "DELETE");


            // удаление Жизни пациента и взаимодействие с врачами LifePacient
            json = controlerLifePacient + "0/" + kodPacient + "/0";
            CallServer.PostServer(controlerLifePacient, json, "DELETE");

            // удаление пациентов записавшихся на прием RegistrationAppointment
            json = pathcontrollerAppointment + "0/" + kodPacient + "/0";
            CallServer.PostServer(pathcontrollerAppointment, json, "DELETE");

            // удаление пациентов записавшихся на прием  у доктора LifeDoctor
            json = controlerLifeDoctor + "0/" + kodPacient + "/0";
            CallServer.PostServer(controlerLifeDoctor, json, "DELETE");

            // удаление список приемов пациентов записавшихся на прием у доктора AdmissionPatients
            json = pathcontrolerReceptionPacient + "0/" + kodPacient + "/0";
            CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");

            // удаление проведеных интервью ColectionInterview
            json = ColectioncontrollerIntevPacient + "0/" + kodPacient + "/0";
            CallServer.PostServer(ColectioncontrollerIntevPacient, json, "DELETE");

            // удаление  учетной записи AccountUser
            json = pathcontrolerAccountUser + "0/" + kodPacient;
            CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");

        }

        // команда закрытия окна
        RelayCommand? checkKeyTextTel;
        public RelayCommand CheckKeyTextTel
        {
            get
            {
                return checkKeyTextTel ??
                  (checkKeyTextTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(WindowMen.PacientProfilt8, 12);
                  }));
            }
        }


        // команда  редактировать
        private RelayCommand? editPacient;
        public RelayCommand? EditPacient
        {
            get
            {
                return editPacient ??
                  (editPacient = new RelayCommand(obj =>
                  {
                      if (selectedPacient != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolPacient == false)BoolTruePacient();
                          else
                          {
                              BoolFalsePacient();
                              WindowMen.PacientTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? savePacient;
        public RelayCommand SavePacient
        {
            get
            {
                return savePacient ??
                  (savePacient = new RelayCommand(obj =>
                  {
                      string json = "";
                      BoolFalsePacient();
                      if ( WindowMen.Pacientt4.Text.Length != 0 && WindowMen.Pacientt7.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewPacient();
                              json = JsonConvert.SerializeObject(selectedPacient);
                              CallServer.PostServer(pathcontrolerPacient, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                              int Countins = ViewPacients != null ? ViewPacients.Count : 0;
                              if (ViewPacients == null)
                              { 
                                  ViewPacients = new ObservableCollection<ModelPacient>();
                                  ViewPacients.Add(Idinsert);
                              } 
                              else ViewPacients.Insert(Countins, Idinsert);
                              SelectedPacient = Idinsert;
                              WindowMen.PacientTablGrid.ItemsSource = ViewPacients;

                          }
                          else
                          {
                              json = JsonConvert.SerializeObject(selectedPacient);
                              CallServer.PostServer(pathcontrolerPacient, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("Pacient/", json);
                      }
                      WindowMen.PacientTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));
            }
        }

        public void SelectNewPacient()
        {

            int indexdia = 1, setindex = 0;
            string kodgrup = "";
            if (selectedPacient == null) selectedPacient = new ModelPacient();
            if (ViewPacients != null)
            {
                kodgrup = ViewPacients[0].kodPacient;
                indexdia = Convert.ToInt32(kodgrup.Substring(kodgrup.LastIndexOf(".") + 1, kodgrup.Length - (kodgrup.LastIndexOf(".") + 1)));
                for (int i = 0; i < ViewPacients.Count; i++)
                {
                    setindex = Convert.ToInt32(ViewPacients[i].kodPacient.Substring(ViewPacients[i].kodPacient.LastIndexOf(".") + 1, ViewPacients[i].kodPacient.Length - (ViewPacients[i].kodPacient.LastIndexOf(".") + 1)));
                    if (indexdia < setindex) indexdia = setindex;
                }
                indexdia++;
                string _repl = "000000000";
                selectedPacient.kodPacient = "PCN." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString();
            }
            else { selectedPacient.kodPacient = "PCN.000000001"; }
            selectedPacient.age = Convert.ToInt32(WindowMen.Pacientt4.Text);
            selectedPacient.email = WindowMen.Pacientt11.Text;
            selectedPacient.gender = WindowMen.Pacientt7.Text;
            selectedPacient.growth = WindowMen.Pacientt6.Text.ToString() != "" ? Convert.ToInt32(WindowMen.Pacientt6.Text) :0;
            selectedPacient.pind = WindowMen.Pacientt13.Text.ToString();
            selectedPacient.name = WindowMen.Pacientt2.Text;
            selectedPacient.surname = WindowMen.Pacientt3.Text;
            selectedPacient.profession = WindowMen.Pacientt9.Text;
            selectedPacient.tel = "+"+ WindowMen.Pacientt8.Text;
            selectedPacient.weight = WindowMen.Pacientt5.Text.ToString() != "" ? Convert.ToDecimal(WindowMen.Pacientt5.Text) : 0 ;
        }

        // команда печати
        RelayCommand? printPacient;
        public RelayCommand PrintPacient
        {
            get
            {
                return printPacient ??
                  (printPacient = new RelayCommand(obj =>
                  {
                      WindowMen.PacientTablGrid.SelectedItem = null;
                      if (ViewPacients != null)
                      {
                          MessageBox.Show("Пацієнт :" + ViewPacients[0].name.ToString());
                      }
                  },
                 (obj) => ViewPacients != null));
            }
        }

       

        // Выбор фамилии доктора
        private RelayCommand? searchPacientT;
        public RelayCommand SearchPacientT
        {
            get
            {
                return searchPacientT ??
                  (searchPacientT = new RelayCommand(obj =>
                  {
                      MetodSearchPacientT();
                  }));
            }


        }

        
        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyPacientT;
        public RelayCommand CheckKeyPacientT
        {
            get
            {
                return checkKeyPacientT ??
                  (checkKeyPacientT = new RelayCommand(obj =>
                  {
                      MetodSearchPacientT();
                  }));
            }
        }


        private void MetodSearchPacientT()
        {

            if (CheckStatusUser() == false) return;
            if (WindowMen.PoiskPacientT.Text.Trim() != "")
            {
                string jason = pathcontrolerPacient + "0/0/0/" + WindowMen.PoiskPacientT.Text + "/0";
                CallServer.PostServer(pathcontrolerPacient, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewPacient(CmdStroka);
            }
        }
        #endregion
        #endregion
    }
}

