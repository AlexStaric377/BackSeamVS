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
        private static MainWindow VisitngDays = MainWindow.LinkNameWindow("BackMain");
        private bool loadboolVisitingDays = false, addboolVisitingDays = false, editboolVisitingDays = false;
        public static string pathcontrolerVisitingDays = "/api/ApiControllerVisitingDays/";
        public static ModelVisitingDays selectModelVisitingDays;
        public static ViewModelVisitingDays selectViewModelVisitingDays;

        public ViewModelVisitingDays SelectedViewModelVisitingDays
        { 
            get { return selectViewModelVisitingDays; } 
            set { selectViewModelVisitingDays = value; OnPropertyChanged("SelectedViewModelVisitingDays"); } 
        }
        public static ObservableCollection<ModelVisitingDays> ViewVisitingDays { get; set; }
        public static ObservableCollection<ViewModelVisitingDays> ViewModeVisitingDays { get; set; }

        public static void ObservableModelVisitingDays(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelVisitingDays>(CmdStroka);
            List<ModelVisitingDays> res = result.ModelVisitingDays.ToList();
            ViewVisitingDays = new ObservableCollection<ModelVisitingDays>((IEnumerable<ModelVisitingDays>)res);
            IndexAddEdit = "";
            MetodLoadGridViewModelVisitingDays();
        }

        public static void MetodLoadGridViewModelVisitingDays()
        {
            ViewModeVisitingDays = new ObservableCollection<ViewModelVisitingDays>();
            foreach (ModelVisitingDays modelVisitingDays in ViewVisitingDays)
            {
                selectModelVisitingDays = modelVisitingDays;
                selectViewModelVisitingDays = new ViewModelVisitingDays();
                selectViewModelVisitingDays.kodDoctor = modelVisitingDays.kodDoctor;
                selectViewModelVisitingDays.id = modelVisitingDays.id;
                selectViewModelVisitingDays.daysOfTheWeek = modelVisitingDays.daysOfTheWeek;
                selectViewModelVisitingDays.dateVizita = modelVisitingDays.dateVizita;
                selectViewModelVisitingDays.timeVizita = modelVisitingDays.timeVizita;
                selectViewModelVisitingDays.onOff = modelVisitingDays.onOff;
 
                if (modelVisitingDays.kodDoctor != "")
                {
                    string json = pathcontrolerDoctor + modelVisitingDays.kodDoctor.ToString()+"/0/0";
                    CallServer.PostServer(pathcontrolerMedZaklad, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectViewModelVisitingDays.nameDoctor = Idinsert.name+ Idinsert.telefon;
                        selectViewModelVisitingDays.edrpou = Idinsert.edrpou;
                        json = pathcontrolerMedZaklad + Idinsert.edrpou.ToString()+"/0/0";
                        CallServer.PostServer(pathcontrolerMedZaklad, json, "GETID");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        MedicalInstitution Idzaklad = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);

                        if (Idinsert != null)
                        {
                            selectViewModelVisitingDays.nameZaklad = Idzaklad.name;
                        }
                    }
                }
                ViewModeVisitingDays.Add(selectViewModelVisitingDays);
            }
            VisitngDays.ReseptionPacientTablGrid.ItemsSource = ViewModeVisitingDays;
        }

        #region Команды вставки, удаления и редектирования справочника "расписаний приёмов пациентов"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadVisitingDays;
        public RelayCommand LoadVisitingDays
        {
            get
            {
                return loadVisitingDays ??
                  (loadVisitingDays = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadVisitingDays(); 
                  }));
            }
        }

        private void MethodLoadVisitingDays()
        {

            WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
            MedZaklad.Left = 550;
            MedZaklad.Top = 100;
            MedZaklad.ShowDialog();


            EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();
            if (EdrpouMedZaklad.Length > 0)
            {
                WinNsiLikar NewOrder = new WinNsiLikar();
                NewOrder.Left = 550;
                NewOrder.Top = 100;
                NewOrder.ShowDialog();
                if (nameDoctor.Length > 0)
                {
                    VisitngDays.NameMedZaklad.Text = VisitngDays.Likart9.Text;
                    VisitngDays.ReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":")+1, MapOpisViewModel.nameDoctor.Length- (MapOpisViewModel.nameDoctor.IndexOf(":")+1));
                    VisitngDays.ReseptionPacientLab.Visibility = Visibility.Hidden;
                    CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays+ MapOpisViewModel._kodDoctor, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                    else ObservableModelVisitingDays(CmdStroka);
                    loadboolVisitingDays = true;

                    VisitngDays.DayoftheWeek.ItemsSource = ViewModelVisitingDays.DayWeeks;
                    VisitngDays.DayoftheWeek.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexDayWeek);
                    VisitngDays.TimeofDay.ItemsSource = ViewModelVisitingDays.TimeVizits;
                    VisitngDays.TimeofDay.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexTimeVizita);
                    VisitngDays.ComboBoxOnoff.ItemsSource = ViewModelVisitingDays.VizitsOnOff;
                    VisitngDays.ComboBoxOnoff.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexVizitsOnOff);
                }
            }

        }


        // команда добавления нового объекта

        private RelayCommand? addVisitingDays;
        public RelayCommand AddVisitingDays
        {
            get
            {
                return addVisitingDays ??
                  (addVisitingDays = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandVisitingDays(); }));
            }
        }

        private void AddComandVisitingDays()
        {
            if (IndexAddEdit != "addCommand") MethodLoadVisitingDays();     // 
            MethodaddcomVisitingDays();
        }

        private void MethodaddcomVisitingDays()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectModelVisitingDays = new ModelVisitingDays();
            selectViewModelVisitingDays = new ViewModelVisitingDays();
            selectViewModelVisitingDays.nameZaklad = VisitngDays.Likart9.Text;
            selectViewModelVisitingDays.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            SelectedViewModelVisitingDays = selectViewModelVisitingDays;
            if (addboolVisitingDays == false) BoolTrueVisitingDays();
            else BoolFalseVisitingDays();
            //VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
            VisitngDays.DayoftheWeek.SelectedIndex = 0;
            VisitngDays.TimeofDay.SelectedIndex = 0;
            VisitngDays.ComboBoxOnoff.SelectedIndex = 0;

        }


      
        private void BoolTrueVisitingDays()
        {
            addboolVisitingDays = true;
            editboolVisitingDays = true;
            VisitngDays.ReseptionTime.IsEnabled = true;
            VisitngDays.ReseptionTime.Background = Brushes.AntiqueWhite;
            VisitngDays.DayoftheWeek.IsEnabled = true;
            VisitngDays.DatePicker.IsEnabled = true;
            VisitngDays.TimeofDay.IsEnabled = true;
            VisitngDays.ComboBoxOnoff.IsEnabled = true;
            VisitngDays.ReseptionPacientTablGrid.IsEnabled = false;
            if (IndexAddEdit == "addCommand")
            {
                VisitngDays.BorderLoadVisitngDays.IsEnabled = false;
                VisitngDays.BorderGhangeVisitngDays.IsEnabled = false;
                VisitngDays.BorderDeleteVisitngDays.IsEnabled = false;
                VisitngDays.BorderPrintVisitngDays.IsEnabled = false;
            }
            if (IndexAddEdit == "editCommand")
            {
                VisitngDays.BorderLoadVisitngDays.IsEnabled = false;
                VisitngDays.BorderAddVisitngDays.IsEnabled = false;
                VisitngDays.BorderDeleteVisitngDays.IsEnabled = false;
                VisitngDays.BorderPrintVisitngDays.IsEnabled = false;
            }

        }


        private void BoolFalseVisitingDays()
        {

            addboolVisitingDays = false;
            editboolVisitingDays = false;
            VisitngDays.ReseptionTime.IsEnabled = false;
            VisitngDays.ReseptionTime.Background = Brushes.White;
            VisitngDays.DayoftheWeek.IsEnabled = false;
            VisitngDays.DatePicker.IsEnabled = false;
            VisitngDays.TimeofDay.IsEnabled = false;
            VisitngDays.ComboBoxOnoff.IsEnabled = false;
            VisitngDays.ReseptionPacientTablGrid.IsEnabled = true;
            VisitngDays.BorderLoadVisitngDays.IsEnabled = true;
            VisitngDays.BorderGhangeVisitngDays.IsEnabled = true;
            VisitngDays.BorderDeleteVisitngDays.IsEnabled = true;
            VisitngDays.BorderPrintVisitngDays.IsEnabled = true;
            VisitngDays.BorderAddVisitngDays.IsEnabled = true;
        }

        // команда  редактировать
        private RelayCommand? editVisitingDays;
        public RelayCommand? EditVisitingDays
        {
            get
            {
                return editVisitingDays ??
                  (editVisitingDays = new RelayCommand(obj =>
                  {
                      if (selectViewModelVisitingDays != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolVisitingDays == false) { BoolTrueVisitingDays(); }
                          else 
                          {
                              BoolFalseVisitingDays();
                              VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда удаления
        private RelayCommand? removeVisitingDays;
        public RelayCommand RemoveVisitingDays
        {
            get
            {
                return removeVisitingDays ??
                  (removeVisitingDays = new RelayCommand(obj =>
                  {
                      if (VisitngDays.ReseptionPacientTablGrid.SelectedIndex >= 0)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              if (VisitngDays.ReseptionPacientTablGrid.SelectedIndex >= 0)
                              {
                                  int removeindex = VisitngDays.ReseptionPacientTablGrid.SelectedIndex;
                                  string json = pathcontrolerVisitingDays+selectViewModelVisitingDays.id.ToString();
                                  CallServer.PostServer(pathcontrolerVisitingDays, json, "DELETE");
                                  ViewVisitingDays.Remove(ViewVisitingDays[removeindex]);
                                  ViewModeVisitingDays.Remove(ViewModeVisitingDays[removeindex]);
                                  VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                                  selectViewModelVisitingDays = new ViewModelVisitingDays();
                                  SelectedViewModelVisitingDays = selectViewModelVisitingDays;                             
                              }

                          }
                      }
                      IndexAddEdit = "";
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveVisitingDays;
        public RelayCommand SaveVisitingDays
        {
            get
            {
                return saveVisitingDays ??
                  (saveVisitingDays = new RelayCommand(obj =>
                  {
                      string json = "";
                      if (VisitngDays.ReseptionPacientVisit.Text.Trim().Length <= 0) return;

                      VisitngDays.ReseptionPacientVisit.Text =VisitngDays.ReseptionPacientVisit.Text.Substring(0, 10);
                      selectModelVisitingDays = new ModelVisitingDays();
                      selectModelVisitingDays.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                      selectModelVisitingDays.daysOfTheWeek = VisitngDays.ReseptionPacient.Text;
                      selectModelVisitingDays.dateVizita = VisitngDays.ReseptionPacientVisit.Text;
                      selectModelVisitingDays.timeVizita = VisitngDays.ReseptionTime.Text;
                      selectModelVisitingDays.onOff = VisitngDays.ReseptionTextBoxOnoff.Text;

                      if (IndexAddEdit == "addCommand")
                      {
                          
                          
                          json = JsonConvert.SerializeObject(selectModelVisitingDays);
                          CallServer.PostServer(pathcontrolerVisitingDays, json, "POST");
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          ModelVisitingDays Idinsert = JsonConvert.DeserializeObject<ModelVisitingDays>(CallServer.ResponseFromServer);
                          if(ViewVisitingDays == null) ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
                          ViewVisitingDays.Add(Idinsert);
                          selectViewModelVisitingDays = new ViewModelVisitingDays();
                          selectViewModelVisitingDays.kodDoctor = Idinsert.kodDoctor;
                          selectViewModelVisitingDays.id = Idinsert.id;
                          selectViewModelVisitingDays.daysOfTheWeek = Idinsert.daysOfTheWeek;
                          selectViewModelVisitingDays.dateVizita = Idinsert.dateVizita;
                          selectViewModelVisitingDays.timeVizita = Idinsert.timeVizita;
                          selectViewModelVisitingDays.onOff = Idinsert.onOff;
                          ViewModeVisitingDays.Add(selectViewModelVisitingDays);
                          VisitngDays.ReseptionPacientTablGrid.ItemsSource = ViewVisitingDays;
                           
                      }
                      else
                      {
                          selectModelVisitingDays.id = selectViewModelVisitingDays.id;
                          json = JsonConvert.SerializeObject(selectModelVisitingDays);
                          CallServer.PostServer(pathcontrolerVisitingDays, json, "PUT");
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer;
                      }
                      UnloadCmdStroka("VisitingDays/", json);
                      BoolFalseVisitingDays();
                      VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));

            }
        }

        // команда печати
        RelayCommand? printVisitingDays;
        public RelayCommand PrintVisitingDays
        {
            get
            {
                return printVisitingDays ??
                  (printVisitingDays  = new RelayCommand(obj =>
                  {


                      MessageBox.Show("Лікар :" + ViewModeVisitingDays[0].nameDoctor.ToString());

                  },
                 (obj) => ViewModeVisitingDays != null));
            }
        }

        private RelayCommand? onVisibleObjVisitingDays;
        public RelayCommand OnVisibleObjVisitingDays
        {
            get
            {
                return onVisibleObjVisitingDays ??
                  (onVisibleObjVisitingDays = new RelayCommand(obj =>
                  {
                      if (ViewVisitingDays != null && VisitngDays.ReseptionPacientTablGrid.SelectedIndex >= 0)
                      {
    
                              selectViewModelVisitingDays = ViewModeVisitingDays[VisitngDays.ReseptionPacientTablGrid.SelectedIndex];
                              SelectedViewModelVisitingDays = selectViewModelVisitingDays;
                              VisitngDays.DayoftheWeek.SelectedIndex = 0; 
                              VisitngDays.TimeofDay.SelectedIndex = 0; 
                              VisitngDays.ComboBoxOnoff.SelectedIndex = 0;                           
    

                      }
                  }));
            }
        }
        #endregion
    }
    
}
