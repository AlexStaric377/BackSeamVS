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
        public static bool loadboolLikarAppointments = false, addboolLikarAppointments = false, editboolLikarAppointments = false, loadthisMonth = false;
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
                    CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays + MapOpisViewModel._kodDoctor+"/0", "GETID");
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
            
 
            if (addboolLikarAppointments == false) BoolTrueLikarAppointments();
            else BoolFalseLikarAppointments();
            LikarAppointments.CabinetReseptionPacientTablGrid.SelectedItem = null;
            selectModelVisitingDays = new ModelVisitingDays();
            selectModelLikarAppointments = new ModelVisitingDays();
            selectViewModelLikarAppointments = new ViewModelVisitingDays();
            LikarAppointments.CabinetNameMedZaklad.Text = LikarAppointments.Likart9.Text.ToString();
            LikarAppointments.CabinetReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            selectViewModelLikarAppointments.nameZaklad = LikarAppointments.Likart9.Text;
            selectViewModelLikarAppointments.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            SelectedViewModelLikarAppointments = selectViewModelLikarAppointments;
            LikarAppointments.CabinetDayoftheWeek.SelectedIndex = 0;
            LikarAppointments.CabinetTimeofDay.SelectedIndex = 0;
            LikarAppointments.CabinetComboBoxOnoff.SelectedIndex = 0;
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
            LikarAppointments.CabinetDayoftheMonth.IsEnabled = true;
            LikarAppointments.CabinetReseptionTimeOn.IsEnabled = true;
            LikarAppointments.CabinetReseptionTimeOn.Background = Brushes.AntiqueWhite;
            LikarAppointments.CabinetReseptionTimeBoxLast.IsEnabled = true;
            LikarAppointments.CabinetReseptionTimeBoxLast.Background = Brushes.AntiqueWhite;
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
            LikarAppointments.CabinetDayoftheMonth.IsEnabled = false;
            LikarAppointments.CabinetReseptionTimeOn.IsEnabled = false;
            LikarAppointments.CabinetReseptionTimeBoxLast.IsEnabled = false;
            LikarAppointments.CabinetReseptionTimeOn.Background = Brushes.White;
            LikarAppointments.CabinetReseptionTimeBoxLast.Background = Brushes.White;
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
                      if (loadthisMonth == true)
                      {
                          MapOpisViewModel.RunGifWait();
                          MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
                          string ThisDay = "", ThisMonth = "", json = "";
                          int itime = 1;
                          ObservableCollection<ModelVisitingDays> ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>();
                          WindowMen.CabinetReseptionBoxMonth.Text = ViewModelVisitingDays.MonthYear[Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear)];

                          MapOpisViewModel.selectModelVisitingDays.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                          //if (WindowMen.CabinetReseptionTimeOn.Text != "09.00" || WindowMen.CabinetReseptionTimeBoxLast.Text != "17.00")
                          //{
                          decimal TimeOn = Convert.ToDecimal(WindowMen.CabinetReseptionTimeOn.Text.Replace(".", ","));
                          decimal TimeLast = Convert.ToDecimal(WindowMen.CabinetReseptionTimeBoxLast.Text.Replace(".", ","));
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
                              "Розклад по місяцю " + WindowMen.CabinetReseptionBoxMonth.Text + " вже сформовано. Ви бажаєте стерти існуючий розклад?";
                              MapOpisViewModel.SelectedDelete();

                              if (MapOpisViewModel.DeleteOnOff == false)
                              {
                                  WindowMen.CabinetDayoftheMonth.SelectedIndex = 0;
                                  return;
                              }
                              MapOpisViewModel.LoadInfoPacient("розкладу прийому пацієнтів на " + WindowMen.CabinetReseptionBoxMonth.Text);
                              json = MapOpisViewModel.pathcontrolerVisitingDays + "0/" + MapOpisViewModel.selectModelVisitingDays.kodDoctor + "/" + ThisMonth + "." + ThisYear;
                              CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "DELETE");
                              MapOpisViewModel.ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>();
                              MapOpisViewModel.ViewModeLikarAppointments = new ObservableCollection<ViewModelVisitingDays>();
                              WindowMen.CabinetReseptionPacientTablGrid.ItemsSource = MapOpisViewModel.ViewModeLikarAppointments;

                          }
                          if (Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear) == 0) return;
                          // количество дней в месяце
                          int indexDay = System.DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear));
                          for (int i = 1; i < indexDay; i++)
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
                                      int Countins = ViewLikarAppointments != null ? ViewLikarAppointments.Count : 0;
                                      MapOpisViewModel.ViewLikarAppointments.Add(Idinsert);
                                  }
                              }
                          }
                          MessageWarning Info = MainWindow.LinkMainWindow("MessageWarning");
                          if (Info != null) Info.Close();
                          endUnload = 1;
                          MetodLoadGridViewModelLikarAppointments();


                      }
                      else
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
                      }
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

