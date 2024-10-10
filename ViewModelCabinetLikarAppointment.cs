using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;
using System.ComponentModel;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {

        private static MainWindow LikarAppointments = MainWindow.LinkNameWindow("BackMain");
        private bool loadboolLikarAppointments = false, addboolLikarAppointments = false, editboolLikarAppointments = false;
        public static string pathcontrolerLikarAppointments = "/api/ApiControllerVisitingDays/";
        public static ModelVisitingDays selectModelLikarAppointments;
        public static ViewModelVisitingDays selectViewModelLikarAppointments;

        public ViewModelVisitingDays SelectedViewModelLikarAppointments
        {
            get { return selectViewModelLikarAppointments; }
            set { selectViewModelLikarAppointments = value; OnPropertyChanged("SelectedViewModelLikarAppointments"); }
        }
        public static ObservableCollection<ModelVisitingDays> ViewLikarAppointments { get; set; }
        public static ObservableCollection<ViewModelVisitingDays> ViewModeLikarAppointments { get; set; }

        public static void ObservableModelLikarAppointments(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelVisitingDays>(CmdStroka);
            List<ModelVisitingDays> res = result.ModelVisitingDays.ToList();
            ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>((IEnumerable<ModelVisitingDays>)res);
            IndexAddEdit = "";
            MetodLoadGridViewModelLikarAppointments();
        }

        public static void MetodLoadGridViewModelLikarAppointments()
        {
            ViewModeLikarAppointments = new ObservableCollection<ViewModelVisitingDays>();
            foreach (ModelVisitingDays modelLikarAppointments in ViewLikarAppointments)
            {
                selectModelLikarAppointments = modelLikarAppointments;
                selectViewModelLikarAppointments = new ViewModelVisitingDays();
                selectViewModelLikarAppointments.kodDoctor = modelLikarAppointments.kodDoctor;
                selectViewModelLikarAppointments.id = modelLikarAppointments.id;
                selectViewModelLikarAppointments.daysOfTheWeek = modelLikarAppointments.daysOfTheWeek;
                selectViewModelLikarAppointments.dateVizita = modelLikarAppointments.dateVizita;
                selectViewModelLikarAppointments.timeVizita = modelLikarAppointments.timeVizita;
                selectViewModelLikarAppointments.onOff = modelLikarAppointments.onOff;

                if (modelLikarAppointments.kodDoctor != "")
                {
                    string json = pathcontrolerDoctor + modelLikarAppointments.kodDoctor.ToString() + "/0/0";
                    CallServer.PostServer(pathcontrolerMedZaklad, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectViewModelLikarAppointments.nameDoctor = Idinsert.name + Idinsert.telefon;
                        selectViewModelLikarAppointments.edrpou = Idinsert.edrpou;
                        json = pathcontrolerMedZaklad + Idinsert.edrpou.ToString() + "/0/0";
                        CallServer.PostServer(pathcontrolerMedZaklad, json, "GETID");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        MedicalInstitution Idzaklad = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);

                        if (Idzaklad != null)
                        {
                            selectViewModelLikarAppointments.nameZaklad = Idzaklad.name;
                        }
                    }
                }
                ViewModeLikarAppointments.Add(selectViewModelLikarAppointments);
            }
            LikarAppointments.CabinetReseptionPacientTablGrid.ItemsSource = ViewModeLikarAppointments;
        }

        #region Команды вставки, удаления и редектирования справочника "расписаний приёмов пациентов"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadLikarAppointments;
        public RelayCommand LoadLikarAppointments
        {
            get
            {
                return loadLikarAppointments ??
                  (loadLikarAppointments = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      MethodLoadLikarAppointments();
                  }));
            }
        }

        private void MethodLoadLikarAppointments()
        {
            if (_kodDoctor == "") { MetodLoadProfilLikar(); }
            LikarAppointments.CabinetNameMedZaklad.Text = LikarAppointments.Likart9.Text.ToString();
            LikarAppointments.CabinetReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
                    LikarAppointments.CabinetReseptionPacientLab.Visibility = Visibility.Hidden;
                    CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays + MapOpisViewModel._kodDoctor, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                    else ObservableModelLikarAppointments(CmdStroka);
                    loadboolLikarAppointments = true;

                    LikarAppointments.CabinetDayoftheWeek.ItemsSource = ViewModelVisitingDays.DayWeeks;
                    LikarAppointments.CabinetDayoftheWeek.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexDayWeek);
                    LikarAppointments.CabinetTimeofDay.ItemsSource = ViewModelVisitingDays.TimeVizits;
                    LikarAppointments.CabinetTimeofDay.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexTimeVizita);
                    LikarAppointments.CabinetComboBoxOnoff.ItemsSource = ViewModelVisitingDays.VizitsOnOff;
                    LikarAppointments.CabinetComboBoxOnoff.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexVizitsOnOff);
            //    }
            //}

        }


        // команда добавления нового объекта

        private RelayCommand? addLikarAppointments;
        public RelayCommand AddLikarAppointments
        {
            get
            {
                return addLikarAppointments ??
                  (addLikarAppointments = new RelayCommand(obj =>
                  { if (RegUserStatus == "2") return;  AddComandLikarAppointments(); }));
            }
        }

        private void AddComandLikarAppointments()
        {
            if (loadboolLikarAppointments == false) MethodLoadLikarAppointments();
            MethodaddcomLikarAppointments();
        }

        private void MethodaddcomLikarAppointments()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectModelLikarAppointments = new ModelVisitingDays();
            selectViewModelLikarAppointments = new ViewModelVisitingDays();
           
            ;
            if (addboolLikarAppointments == false) BoolTrueLikarAppointments();
            else BoolFalseLikarAppointments();
            //LikarAppointments.CabinetReseptionPacientTablGrid.SelectedItem = null;
            LikarAppointments.CabinetDayoftheWeek.SelectedIndex = 0;
            LikarAppointments.CabinetTimeofDay.SelectedIndex = 0;
            LikarAppointments.CabinetComboBoxOnoff.SelectedIndex = 0;
            LikarAppointments.CabinetNameMedZaklad.Text = LikarAppointments.Likart9.Text.ToString();
            LikarAppointments.CabinetReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            selectViewModelLikarAppointments.nameZaklad = LikarAppointments.Likart9.Text;
            selectViewModelLikarAppointments.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            SelectedViewModelLikarAppointments = selectViewModelLikarAppointments;
        }



        private void BoolTrueLikarAppointments()
        {
            addboolLikarAppointments = true;
            editboolLikarAppointments = true;
            LikarAppointments.CabinetReseptionTime.IsEnabled = true;
            LikarAppointments.CabinetReseptionTime.Background = Brushes.AntiqueWhite;
            LikarAppointments.CabinetDayoftheWeek.IsEnabled = true;
            LikarAppointments.CabinetDatePicker.IsEnabled = true;
            LikarAppointments.CabinetTimeofDay.IsEnabled = true;
            LikarAppointments.CabinetComboBoxOnoff.IsEnabled = true;

        }


        private void BoolFalseLikarAppointments()
        {

            addboolLikarAppointments = false;
            editboolLikarAppointments = false;
            LikarAppointments.CabinetReseptionTime.IsEnabled = false;
            LikarAppointments.CabinetReseptionTime.Background = Brushes.White;
            LikarAppointments.CabinetDayoftheWeek.IsEnabled = false;
            LikarAppointments.CabinetDatePicker.IsEnabled = false;
            LikarAppointments.CabinetTimeofDay.IsEnabled = false;
            LikarAppointments.CabinetComboBoxOnoff.IsEnabled = false;
        }

        // команда  редактировать
        private RelayCommand? editLikarAppointments;
        public RelayCommand? EditLikarAppointments
        {
            get
            {
                return editLikarAppointments ??
                  (editLikarAppointments = new RelayCommand(obj =>
                  {
                      if (selectViewModelLikarAppointments != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolLikarAppointments == false) { BoolTrueLikarAppointments(); }
                          else
                          {
                              BoolFalseLikarAppointments();
                              LikarAppointments.CabinetReseptionPacientTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда удаления
        private RelayCommand? removeLikarAppointments;
        public RelayCommand RemoveLikarAppointments
        {
            get
            {
                return removeLikarAppointments ??
                  (removeLikarAppointments = new RelayCommand(obj =>
                  {
                      if (LikarAppointments.CabinetReseptionPacientTablGrid.SelectedIndex >= 0)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              if (LikarAppointments.CabinetReseptionPacientTablGrid.SelectedIndex >= 0)
                              {
                                  int _indexremove = LikarAppointments.CabinetReseptionPacientTablGrid.SelectedIndex;
                                  selectViewModelLikarAppointments = ViewModeLikarAppointments[_indexremove];                                
                                  string json = pathcontrolerLikarAppointments + selectViewModelLikarAppointments.id.ToString();
                                  CallServer.PostServer(pathcontrolerVisitingDays, json, "DELETE");
                                  ViewLikarAppointments.Remove(ViewLikarAppointments[_indexremove]);
                                  ViewModeLikarAppointments.Remove(ViewModeLikarAppointments[_indexremove]);
                                  LikarAppointments.CabinetReseptionPacientTablGrid.SelectedItem = null;
                                  selectViewModelLikarAppointments = new ViewModelVisitingDays();
                                  SelectedViewModelLikarAppointments = selectViewModelLikarAppointments;                            
                              }
 
                          }
                      }
                      IndexAddEdit = "";
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveLikarAppointments;
        public RelayCommand SaveLikarAppointments
        {
            get
            {
                return saveLikarAppointments ??
                  (saveLikarAppointments = new RelayCommand(obj =>
                  {
                      string json = "";
                      LikarAppointments.CabinetReseptionPacientVisit.Text = LikarAppointments.CabinetReseptionPacientVisit.Text.Substring(0, 10);
                      selectModelLikarAppointments = new ModelVisitingDays();
                      selectModelLikarAppointments.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                      selectModelLikarAppointments.daysOfTheWeek = LikarAppointments.CabinetReseptionPacient.Text;
                      selectModelLikarAppointments.dateVizita = LikarAppointments.CabinetReseptionPacientVisit.Text;
                      selectModelLikarAppointments.timeVizita = LikarAppointments.CabinetReseptionTime.Text;
                      selectModelLikarAppointments.onOff = LikarAppointments.CabinetReseptionTextBoxOnoff.Text;

                      if (IndexAddEdit == "addCommand")
                      {


                          json = JsonConvert.SerializeObject(selectModelLikarAppointments);
                          CallServer.PostServer(pathcontrolerVisitingDays, json, "POST");
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          ModelVisitingDays Idinsert = JsonConvert.DeserializeObject<ModelVisitingDays>(CallServer.ResponseFromServer);
                          int Countins = ViewLikarAppointments != null ? ViewLikarAppointments.Count : 0;
                          ViewLikarAppointments.Insert(Countins, Idinsert);
                          selectViewModelLikarAppointments = new ViewModelVisitingDays();
                          selectViewModelLikarAppointments.kodDoctor = Idinsert.kodDoctor;
                          selectViewModelLikarAppointments.id = Idinsert.id;
                          selectViewModelLikarAppointments.daysOfTheWeek = Idinsert.daysOfTheWeek;
                          selectViewModelLikarAppointments.dateVizita = Idinsert.dateVizita;
                          selectViewModelLikarAppointments.timeVizita = Idinsert.timeVizita;
                          selectViewModelLikarAppointments.onOff = Idinsert.onOff;
                          if (ViewModeLikarAppointments == null) ViewModeLikarAppointments = new ObservableCollection<ViewModelVisitingDays>();
                          ViewModeLikarAppointments.Add(selectViewModelLikarAppointments);
                          LikarAppointments.CabinetReseptionPacientTablGrid.ItemsSource = ViewLikarAppointments;

                      }
                      else
                      {
                          selectModelLikarAppointments.id = selectViewModelLikarAppointments.id;
                          json = JsonConvert.SerializeObject(selectModelLikarAppointments);
                          CallServer.PostServer(pathcontrolerVisitingDays, json, "PUT");
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer;
                      }
                      UnloadCmdStroka("VisitingDays/", json);
                      BoolFalseLikarAppointments();
                      IndexAddEdit = "";

                  }));

            }
        }

        // команда печати
        RelayCommand? printLikarAppointments;
        public RelayCommand PrintLikarAppointments
        {
            get
            {
                return printLikarAppointments ??
                  (printLikarAppointments = new RelayCommand(obj =>
                  {

                      
                          MessageBox.Show("Лікар :" + ViewModeLikarAppointments[0].nameDoctor.ToString());
 
                  },
                 (obj) => ViewModeLikarAppointments != null));
            }
        }

        private RelayCommand? onVisibleObjLikarAppointments;
        public RelayCommand OnVisibleObjLikarAppointments
        {
            get
            {
                return onVisibleObjLikarAppointments ??
                  (onVisibleObjLikarAppointments = new RelayCommand(obj =>
                  {
                      if (ViewLikarAppointments != null && LikarAppointments.CabinetReseptionPacientTablGrid.SelectedIndex>=0)
                      {
                          selectViewModelLikarAppointments = ViewModeLikarAppointments[LikarAppointments.CabinetReseptionPacientTablGrid.SelectedIndex];
                          SelectedViewModelLikarAppointments = selectViewModelLikarAppointments;
                          LikarAppointments.CabinetDayoftheWeek.SelectedIndex = 0;
                          LikarAppointments.CabinetTimeofDay.SelectedIndex = 0;
                          LikarAppointments.CabinetComboBoxOnoff.SelectedIndex = 0;
                      }
                  }));
            }
        }
        #endregion
    }
}

