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
                        json = pathcontrolerMedZaklad + Idinsert.edrpou.ToString()+"/0/0/0";
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
                    CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays+ MapOpisViewModel._kodDoctor+"/0", "GETID");
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
            VisitngDays.DayoftheMonth.IsEnabled = true;
            VisitngDays.ReseptionTimeOn.IsEnabled = true;
            VisitngDays.ReseptionDayBoxLast.IsEnabled = true;
            VisitngDays.ReseptionDayBoxLast.Background = Brushes.AntiqueWhite;
            VisitngDays.ReseptionDayOn.IsEnabled = true;
            VisitngDays.ReseptionDayOn.Background = Brushes.AntiqueWhite;
            VisitngDays.ReseptionTimeOn.Background = Brushes.AntiqueWhite;
            VisitngDays.ReseptionTimeBoxLast.IsEnabled = true;
            VisitngDays.ReseptionTimeBoxLast.Background = Brushes.AntiqueWhite;

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
            VisitngDays.DayoftheMonth.IsEnabled = false;
            VisitngDays.ReseptionTimeOn.IsEnabled = false;
            VisitngDays.ReseptionDayBoxLast.IsEnabled = false;
            VisitngDays.ReseptionDayBoxLast.Background = Brushes.White;
            VisitngDays.ReseptionDayOn.IsEnabled = false;
            VisitngDays.ReseptionDayOn.Background = Brushes.White;
            VisitngDays.ReseptionTimeOn.Background = Brushes.White;
            VisitngDays.ReseptionTimeBoxLast.IsEnabled = false;
            VisitngDays.ReseptionTimeBoxLast.Background = Brushes.White;
            VisitngDays.DayoftheWeek.SelectedIndex = 0;
            VisitngDays.TimeofDay.SelectedIndex = 0;
            VisitngDays.ComboBoxOnoff.SelectedIndex = 0;
            VisitngDays.DayoftheMonth.SelectedIndex = 0;
            VisitngDays.ReseptionBoxMonth.Text = "";
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

                      if (loadthisMonth == true)
                      {
                          AddAppointmentsAdmin();
                          MetodLoadGridViewModelVisitingDays();


                      }
                      else
                      {

                          string json = "";
                          if (VisitngDays.ReseptionPacientVisit.Text.Trim().Length <= 0) return;

                          VisitngDays.ReseptionPacientVisit.Text = VisitngDays.ReseptionPacientVisit.Text.Substring(0, 10);
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
                              if (ViewVisitingDays == null) ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
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
                      }
                      BoolFalseVisitingDays();
                      IndexAddEdit = "";
                  }));

            }
        }


        public void AddAppointmentsAdmin()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            int nawmonth = System.DateTime.Now.Month;
            int nawday = System.DateTime.Now.Day;
            int beginind = Convert.ToInt32(WindowMen.ReseptionDayOn.Text);
            if (beginind < nawday && nawmonth == Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear))
            {
                MainWindow.MessageError = " Перший день прийому меньше поточного дня календарного місяця обраного вами. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            int Daymonth = System.DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear));
            int lastDay = Convert.ToInt32(WindowMen.ReseptionDayBoxLast.Text);
            if (lastDay > Daymonth)
            {
                MainWindow.MessageError = " Крайній день прийому більше останнього дня календарного місяця обраного вами. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            if (lastDay < beginind)
            {
                MainWindow.MessageError = " Перший день прийому більше останнього дня прийому. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            MapOpisViewModel.RunGifWait();

            string ThisDay = "", ThisMonth = "", json = "";
            int itime = 1;
            ObservableCollection<ModelVisitingDays> ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>();
            WindowMen.ReseptionBoxMonth.Text = ViewModelVisitingDays.MonthYear[Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear)];

            selectModelVisitingDays.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
            //if (WindowMen.CabinetReseptionTimeOn.Text != "09.00" || WindowMen.CabinetReseptionTimeBoxLast.Text != "17.00")
            //{
            decimal TimeOn = Convert.ToDecimal(WindowMen.ReseptionTimeOn.Text.Replace(".", ","));
            decimal TimeLast = Convert.ToDecimal(WindowMen.ReseptionTimeBoxLast.Text.Replace(".", ","));
            if (TimeOn > TimeLast)
            {
                MainWindow.MessageError = " Час закінчення прийому меньше часу початку прийому";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }

            for (decimal ind = TimeOn; ind <= TimeLast; ind++)
            {
                string stringTime = ind <= 9 ? "0" + Convert.ToString(ind) : Convert.ToString(ind);
                ViewModelVisitingDays.TimeVizits[itime] = stringTime;
                if (ind < TimeLast)
                {
                    itime++;
                    stringTime = ind <= 9 ? "0" + Convert.ToString(ind + 0.3m) : Convert.ToString(ind + 0.3m);
                    ViewModelVisitingDays.TimeVizits[itime] = stringTime;
                    itime++;
                }

            }
            for (int ind = itime; ind < 19; ind++)
            { ViewModelVisitingDays.TimeVizits[ind] = ""; }

            //}
            // выбранный месяц и год 
            string ThisYear = DateTime.Now.ToShortDateString().Substring(DateTime.Now.ToShortDateString().LastIndexOf(".") + 1, DateTime.Now.ToShortDateString().Length - (DateTime.Now.ToShortDateString().LastIndexOf(".") + 1));
            ThisMonth = ViewModelVisitingDays.selectedIndexMonthYear.Length > 1 ? ViewModelVisitingDays.selectedIndexMonthYear : "0" + ViewModelVisitingDays.selectedIndexMonthYear;
            json = MapOpisViewModel.pathcontrolerVisitingDays + MapOpisViewModel.selectModelVisitingDays.kodDoctor + "/" + ThisMonth + "." + ThisYear;
            CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Розклад по місяцю " + WindowMen.ReseptionBoxMonth.Text + " вже сформовано. Ви бажаєте стерти існуючий розклад?";
                MapOpisViewModel.SelectedDelete();

                if (MapOpisViewModel.DeleteOnOff == false)
                {
                    WindowMen.DayoftheMonth.SelectedIndex = 0;
                    return;
                }
                MapOpisViewModel.LoadInfoPacient("розкладу прийому пацієнтів на " + WindowMen.ReseptionBoxMonth.Text);
                json = MapOpisViewModel.pathcontrolerVisitingDays + "0/" + MapOpisViewModel.selectModelVisitingDays.kodDoctor + "/" + ThisMonth + "." + ThisYear;
                CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "DELETE");
                ViewModeVisitingDays = new ObservableCollection<ViewModelVisitingDays>();
                ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
                VisitngDays.ReseptionPacientTablGrid.ItemsSource = ViewModeVisitingDays;
            }
            if (Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear) == 0) return;
            // количество дней в месяце


            for (int i = beginind; i <= lastDay; i++)
            {
                ThisDay = i > 9 ? Convert.ToString(i) : "0" + Convert.ToString(i);
                string dateVisit = ThisDay + "." + ThisMonth + "." + ThisYear;
                MapOpisViewModel.selectModelVisitingDays.dateVizita = dateVisit;
                DateTime convertedDate = Convert.ToDateTime(dateVisit);
                int theweek = (int)convertedDate.DayOfWeek;
                MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek = ViewModelVisitingDays.AppointmentsDayWeeks[theweek];
                MapOpisViewModel.selectModelVisitingDays.onOff = "Так";
                if (MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek != "Субота" && MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek != "Неділя" && theweek != 0)
                {
                    for (int indtime = 1; indtime < itime; indtime++)  //18
                    {
                        ModelVisitingDays VisitingDays = MapOpisViewModel.selectModelVisitingDays;
                        VisitingDays.timeVizita = ViewModelVisitingDays.TimeVizits[indtime];
                        json = JsonConvert.SerializeObject(VisitingDays);
                        CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "POST");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelVisitingDays Idinsert = JsonConvert.DeserializeObject<ModelVisitingDays>(CallServer.ResponseFromServer);
                        if (ViewVisitingDays == null ) ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
                        ViewVisitingDays.Add(Idinsert);
                    }
                }
            }
            MessageWarning Info = MainWindow.LinkMainWindow("MessageWarning");
            if (Info != null) Info.Close();
            endUnload = 1;

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
