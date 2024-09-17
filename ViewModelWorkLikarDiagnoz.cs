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
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public static MainWindow WindowWorkGrDiagnoz = MainWindow.LinkNameWindow("BackMain");
        static bool activWorkViewDiagnoz = false, loadboolWorkDiagnoz = false, loadWorkGrupDiagnoz = false;
        public static string controlerLikarGrDiagnoz = "/api/LikarGrupDiagnozController/", SelectActivWorkGrupDiagnoz = "";
        public static ModelDiagnoz selectedWorkDiagnoz;

        public static ObservableCollection<ModelDiagnoz> ViewWorkDiagnozs { get; set; }
        public static ObservableCollection<ModelDiagnoz> TmpWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public static ObservableCollection<ModelDiagnoz> AllWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public ModelDiagnoz SelectedViewWorkDiagnoz
        { get { return selectedWorkDiagnoz; } set { selectedWorkDiagnoz = value; OnPropertyChanged("SelectedViewWorkDiagnoz"); } }

        public static void ObservableViewWorkDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            TmpWorkDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadViewWorkDiagnoz;
        public RelayCommand LoadViewWorkDiagnoz
        {
            get
            {
                return loadViewWorkDiagnoz ??
                  (loadViewWorkDiagnoz = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      if (_kodDoctor == "") SelectRegAccountUser();
                      if (_kodDoctor == "") return;
                      MethodloadtablWorkDiagnoz();
                  }));
            }
        }

        private void MethodloadtablWorkDiagnoz()
        {
            loadWorkGrupDiagnoz = false;
            AllWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
            WindowWorkGrDiagnoz.WorkLoadDia.Visibility = Visibility.Hidden;
            WindowWorkGrDiagnoz.WorkFoldInterv.Visibility = Visibility.Hidden;
            WindowWorkGrDiagnoz.WorkCompInterviewLab.Visibility = Visibility.Hidden;
            string json = controlerLikarGrDiagnoz + _kodDoctor + "/0";
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ViewModelLikarGrupDiagnoz.ObservableViewLikarGrDiagnoz(CmdStroka);
            ViewWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
            foreach (ModelLikarGrupDiagnoz likarGrupDiagnoz in ViewModelLikarGrupDiagnoz.LikarGrupDiagnozs)
            {
                ModelDiagnoz likarGrupDiagnozs = new ModelDiagnoz();
                if(likarGrupDiagnoz.icdGrDiagnoz != "")
                { 
                    likarGrupDiagnozs.icdGrDiagnoz = likarGrupDiagnoz.icdGrDiagnoz;
                    likarGrupDiagnozs.nameDiagnoza = likarGrupDiagnoz.icdGrDiagnoz;
                    likarGrupDiagnozs.id = likarGrupDiagnoz.id;
                    ViewWorkDiagnozs.Add(likarGrupDiagnozs);
                    ModelDiagnoz Idinsert = new ModelDiagnoz();
                    if (likarGrupDiagnoz.icdGrDiagnoz != "")
                    { 
                        json = controlerViewDiagnoz+ "0/" + likarGrupDiagnoz.icdGrDiagnoz + "/0";
                        CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                        CmdStroka = CallServer.ServerReturn();
                        ObservableViewWorkDiagnoz(CmdStroka);
                        foreach (ModelDiagnoz modelDiagnoz in TmpWorkDiagnozs)
                        { 
                            AllWorkDiagnozs.Add(modelDiagnoz);
                        }               
                    }
                }
                
 
            }
            WindowWorkGrDiagnoz.DiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
            WindowWorkGrDiagnoz.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
            loadboolWorkDiagnoz = true;
        }

 
        // команда добавления нового объекта

        private RelayCommand addViewViewWorkDiagnoz;
        public RelayCommand AddViewWorkDiagnoz
        {
            get
            {
                return addViewViewWorkDiagnoz ??
                  (addViewViewWorkDiagnoz = new RelayCommand(obj =>
                  {
                      if (RegUserStatus == "2") return;
                      if (loadWorkGrupDiagnoz == false) MethodloadtablWorkDiagnoz();
                      MethodaddcomWorkDiagnoz();
                  }));
            }
        }

       

        private void MethodaddcomWorkDiagnoz()
        {
            selectedDiagnoz = new ModelDiagnoz();
            WindowWorkGrDiagnoz.DiagnozTablGrid.SelectedItem = null;
 
            SelectActivGrupDiagnoz = "";
            WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (WindowWorkGrDiagnoz.Diagnozt1.Text.Length != 0)
            {
                string AddWorkGrDiagnoz = WindowWorkGrDiagnoz.Diagnozt1.Text;
                if (loadWorkGrupDiagnoz == true) MethodloadtablWorkDiagnoz();

                SelectActivGrupDiagnoz = "WorkGrupDiagnoz";
                ViewModelLikarGrupDiagnoz.MetodAddLikarGrDiagnoz(AddWorkGrDiagnoz);

                foreach (ModelLikarGrupDiagnoz modelLikarGrupDiagnoz in ViewModelLikarGrupDiagnoz.AddLikarGrupDiagnozs)
                {
                    ModelDiagnoz Idinsert = new ModelDiagnoz();
                    Idinsert.icdGrDiagnoz = modelLikarGrupDiagnoz.icdGrDiagnoz;
                    Idinsert.nameDiagnoza = modelLikarGrupDiagnoz.icdGrDiagnoz;
                    Idinsert.id = modelLikarGrupDiagnoz.id;
                    ViewWorkDiagnozs.Add(Idinsert);
                    if (Idinsert.icdGrDiagnoz != "")
                    {
                        json = controlerViewDiagnoz + "0/" + Idinsert.icdGrDiagnoz + "/0";
                        CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                        CmdStroka = CallServer.ServerReturn();
                        ObservableViewWorkDiagnoz(CmdStroka);
                        foreach (ModelDiagnoz modelDiagnoz in TmpWorkDiagnozs)
                        {
                            AllWorkDiagnozs.Add(modelDiagnoz);
                        }
                    }

                }
                WindowWorkGrDiagnoz.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
                loadWorkGrupDiagnoz = false;
            }
            SelectActivGrupDiagnoz = "";
        }

        // команда удаления
        private RelayCommand? removeViewWorkDiagnoz;
        public RelayCommand RemoveViewWorkDiagnoz
        {
            get
            {
                return removeViewWorkDiagnoz ??
                  (removeViewWorkDiagnoz = new RelayCommand(obj =>
                  {
                      if (loadWorkGrupDiagnoz == false)
                      {
                          if (WindowWorkGrDiagnoz.WorkDiagnozTablGrid.SelectedIndex >= 0)
                          {
                              SelectedRemove();
                              // Видалення данных о гостях, пациентах, докторах, учетных записях
                              if (MapOpisViewModel.DeleteOnOff == true)
                              {
                                  selectedWorkDiagnoz = ViewWorkDiagnozs[WindowWorkGrDiagnoz.WorkDiagnozTablGrid.SelectedIndex];
                                  string json = controlerLikarGrDiagnoz + selectedWorkDiagnoz.id.ToString();
                                  CallServer.PostServer(controlerLikarGrDiagnoz, json, "DELETE");
                                  ViewWorkDiagnozs.Remove(selectedWorkDiagnoz);
                                  AllWorkDiagnozs.Remove(selectedWorkDiagnoz);
                                  selectedWorkDiagnoz = new ModelDiagnoz();
                                  WindowWorkGrDiagnoz.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
                              }
                          }
                      }
                  }, (obj) => ViewWorkDiagnozs != null));
            }
        }


        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? findNameWorkMkx;
        public RelayCommand FindNameWorkMkx
        {
            get
            {
                return findNameWorkMkx ??
                  (findNameWorkMkx = new RelayCommand(obj =>
                  { ComandFindNameWorkMkx(); }));
            }
        }

        private void ComandFindNameWorkMkx()
        {
            if (selectedWorkDiagnoz != null)
            {
                if (ViewWorkDiagnozs != null)
                {
                    if (WindowWorkGrDiagnoz.WorkDiagnozTablGrid.SelectedIndex >= 0)
                    {
                        selectedWorkDiagnoz = ViewWorkDiagnozs[WindowWorkGrDiagnoz.WorkDiagnozTablGrid.SelectedIndex];
                        if (loadWorkGrupDiagnoz == false)
                        {
                            SelectActivGrupDiagnoz = selectedWorkDiagnoz.icdGrDiagnoz;
                            SelectedViewWorkDiagnoz = new ModelDiagnoz();
                            WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                            NewOrder.Left = (MainWindow.ScreenWidth / 2);
                            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                            NewOrder.ShowDialog();

                        }
                        else
                        {
                            WindowWorkGrDiagnoz.WorkDiagnozt3.Text =  "";

                            selectedInterview = new ModelInterview();
                            selectedInterview.uriInterview = selectedWorkDiagnoz.uriDiagnoza;
                            if (selectedWorkDiagnoz.keyIcd != "")
                            {

                                string json = VeiwModelNsiIcd.controlerNsiIcd + selectedWorkDiagnoz.keyIcd.ToString() + "/0";
                                CallServer.PostServer(VeiwModelNsiIcd.controlerNsiIcd, json, "GETID");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                ModelIcd Idinsert = JsonConvert.DeserializeObject<ModelIcd>(CallServer.ResponseFromServer);
                                if (Idinsert != null)WindowWorkGrDiagnoz.WorkDiagnozt3.Text = Idinsert.name;
                            }
                            WindowWorkGrDiagnoz.WorkFoldInterv.Visibility = Visibility.Visible;
                            WindowWorkGrDiagnoz.WorkCompInterviewLab.Visibility = Visibility.Visible;
                        }



                    }
                }


            }
        }


        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? selectedListWorkGrDiagnoz;
        public RelayCommand SelectedListWorkGrDiagnoz
        {
            get
            {
                return selectedListWorkGrDiagnoz ??
                  (selectedListWorkGrDiagnoz = new RelayCommand(obj =>
                  { ComandFindNameWorkGrDiagnoz(); }));
            }
        }

        private void ComandFindNameWorkGrDiagnoz()
        {
            SelectActivGrupDiagnoz = "GrupDiagnoz";
            if (_kodDoctor != "")
            {
                WinLikarGrupDiagnoz Order = new WinLikarGrupDiagnoz();
                Order.Left = (MainWindow.ScreenWidth / 2);
                Order.Top = (MainWindow.ScreenHeight / 2) - 350;
                Order.ShowDialog();
            }
            if (WindowWorkGrDiagnoz.Diagnozt1.Text != "")
            {
                ViewWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
                loadWorkGrupDiagnoz = true;
                foreach (ModelDiagnoz modelDiagnoz in AllWorkDiagnozs)
                { 
                    if(modelDiagnoz.icdGrDiagnoz == WindowWorkGrDiagnoz.Diagnozt1.Text.Trim()) ViewWorkDiagnozs.Add(modelDiagnoz);
                    WindowWorkGrDiagnoz.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
                }
            }
 
            SelectActivGrupDiagnoz = "";
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntev;
        public RelayCommand ReadColectionIntev
        {
            get
            {
                return readColectionIntev ??
                  (readColectionIntev = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";
                      string json = pathcontrolerDependency + selectedWorkDiagnoz.kodDiagnoza + "/0";
                      CallServer.PostServer(pathcontrolerDependency, json, "GETID");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      ModelDependencyDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelDependencyDiagnoz>(CallServer.ResponseFromServer);
                      if (Idinsert != null)
                      { 
                          MapOpisViewModel.GetidkodProtokola = Idinsert.kodProtokola;
                          WinCreatIntreview NewOrder = new WinCreatIntreview();
                          NewOrder.Left = 600;
                          NewOrder.Top = 130;
                          NewOrder.ShowDialog();                     
                      }


                  }));
            }
        }
        #endregion
        #endregion

    }
}
