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
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        static bool activViewDiagnoz = false,  addboolGrDiagnoz = false, loadGrupDiagnoz = false;
        public static string controlerViewDiagnoz =  "/api/DiagnozController/", SelectActivGrupDiagnoz = "", GrupDiagnoz="", KeiIcdGrup = "";
        public static ModelDiagnoz selectedDiagnoz;

        public static ObservableCollection<ModelDiagnoz> ViewDiagnozs { get; set; }
        public static ObservableCollection<ModelDiagnoz> TmpDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public ModelDiagnoz SelectedViewDiagnoz
        { get { return selectedDiagnoz; } set { selectedDiagnoz = value; OnPropertyChanged("SelectedViewDiagnoz"); } }

        public static void ObservableViewDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            ViewDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);
            if(addboolGrDiagnoz == false && loadGrupDiagnoz == false) GrupIcdGrDiagnoz();
            WindowMen.DiagnozTablGrid.ItemsSource = ViewDiagnozs;
            WindowMen.LibDiagnozTablGrid.ItemsSource = ViewDiagnozs;
        }

        public static  void GrupIcdGrDiagnoz()
        {
            string strokaIcdGrDiagnoz = "";
            
            foreach (ModelDiagnoz modelDiagnoz in ViewDiagnozs)
            {
                
                if (strokaIcdGrDiagnoz != modelDiagnoz.IcdGrDiagnoz )
                {
                    modelDiagnoz.nameDiagnoza = modelDiagnoz.icdGrDiagnoz;
                    modelDiagnoz.opisDiagnoza = "";
                    modelDiagnoz.uriDiagnoza = "";
                    TmpDiagnozs.Add(modelDiagnoz);
                    strokaIcdGrDiagnoz = modelDiagnoz.icdGrDiagnoz;

                }
                if (modelDiagnoz.IcdGrDiagnoz == "")
                { 
                    TmpDiagnozs.Add(modelDiagnoz);
                    addboolGrDiagnoz = true;
                } 
            }
            ViewDiagnozs = TmpDiagnozs;
        }




        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadViewDiagnoz;
        public RelayCommand LoadViewDiagnoz
        {
            get
            {
                return loadViewDiagnoz ??
                  (loadViewDiagnoz = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadtablDiagnoz();
                  }));
            }
        }


        // команда добавления нового объекта
        
        private RelayCommand addViewViewDiagnoz;
        public RelayCommand AddViewDiagnoz
        {
            get
            {
                return addViewViewDiagnoz ??
                  (addViewViewDiagnoz = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComViewViewDiagnoz(); }));
            }
        }

        private void AddComViewViewDiagnoz()
        {
            if (addboolGrDiagnoz == false && loadGrupDiagnoz == false) 
            {
                addboolGrDiagnoz = true;
                CallServer.PostServer(controlerViewDiagnoz, controlerViewDiagnoz, "GET");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewDiagnoz(CmdStroka);
               
            }
            MethodaddcomDiagnoz();
        }

        private void MethodaddcomDiagnoz()
        {
            selectedDiagnoz = new ModelDiagnoz();
            WindowMen.DiagnozTablGrid.SelectedItem = null;
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activViewDiagnoz == false) BoolTrueDiagnoz();
            else BoolFalseDiagnoz();
        }
        private void MethodloadtablDiagnoz()
        {
            GrupDiagnoz = "";
            addboolGrDiagnoz = false;
            loadGrupDiagnoz = false;
            
            WindowMen.LoadDia.Visibility = Visibility.Hidden;
            WindowMen.LibFoldInterv.Visibility = Visibility.Hidden;
            WindowMen.LibCompInterviewLab.Visibility = Visibility.Hidden;
            CallServer.PostServer(controlerViewDiagnoz, controlerViewDiagnoz, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewDiagnoz(CmdStroka);
        }
        private void BoolTrueDiagnoz()
        {
            activViewDiagnoz = true;
            WindowMen.FolderMKX.Visibility = Visibility.Visible;
            WindowMen.FolderGrDiagnoz.Visibility = Visibility.Visible;
            
            WindowMen.Diagnozt2.IsEnabled = true;
            WindowMen.Diagnozt2.Background = Brushes.AntiqueWhite;
            WindowMen.DiagnozOpis.IsEnabled = true;
            WindowMen.DiagnozOpis.Background = Brushes.AntiqueWhite;
            WindowMen.DiagnozTextUri.IsEnabled = true;
            WindowMen.DiagnozTextUri.Background = Brushes.AntiqueWhite;
            WindowMen.DiagnozTablGrid.IsEnabled = false;
        }

        private void BoolFalseDiagnoz()
        {
            WindowMen.FolderMKX.Visibility = Visibility.Hidden;
            WindowMen.FolderGrDiagnoz.Visibility = Visibility.Hidden;
            WindowMen.Diagnozt2.IsEnabled = false;
            WindowMen.Diagnozt2.Background = Brushes.White;
            WindowMen.DiagnozOpis.IsEnabled = false;
            WindowMen.DiagnozOpis.Background = Brushes.White;
            WindowMen.DiagnozTextUri.IsEnabled = false;
            WindowMen.DiagnozTextUri.Background = Brushes.White;
            WindowMen.DiagnozTablGrid.IsEnabled = true;
            activViewDiagnoz = false;
            WindowMen.Diagnozt3.Text = "";
            WindowMen.Diagnozt1.Text = "";
            WindowMen.Diagnozt4.Text = "";
        }
        // команда удаления
        private RelayCommand? removeViewDiagnoz;
        public RelayCommand RemoveViewDiagnoz
        {
            get
            {
                return removeViewDiagnoz ??
                  (removeViewDiagnoz = new RelayCommand(obj =>
                  {
                      if (selectedDiagnoz != null)
                      {
                          if (selectedDiagnoz.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerViewDiagnoz + selectedDiagnoz.id.ToString();
                              CallServer.PostServer(controlerViewDiagnoz, json, "DELETE");
                              ViewDiagnozs.Remove(selectedDiagnoz);
                              selectedDiagnoz = new ModelDiagnoz();
                              
                          }
                      }
                      IndexAddEdit = "";
                  }, (obj) => ViewDiagnozs != null));
            }
        }


        // команда  редактировать
        
        
        private RelayCommand? editViewDiagnoz;
        public RelayCommand? EditViewDiagnoz
        {
            get
            {
                return editViewDiagnoz ??
                  (editViewDiagnoz = new RelayCommand(obj =>
                  {
                      if (selectedDiagnoz != null)
                      {
                          if (selectedDiagnoz.idUser != RegIdUser && selectedDiagnoz.idUser != "Admin")
                          {
                              InfoEditZapis();
                              return;
                          }
                          IndexAddEdit = "editCommand";
                          if (activViewDiagnoz == false) {BoolTrueDiagnoz(); }
                          else
                          {
                              BoolFalseDiagnoz();
                              WindowMen.DiagnozTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveViewDiagnoz;
        public RelayCommand SaveViewDiagnoz
        {
            get
            {
                return saveViewDiagnoz ??
                (saveViewDiagnoz = new RelayCommand(obj =>
                {
 
                    
                    if (WindowMen.Diagnozt2.Text.Trim().Length != 0)
                    {
                          string json = "";
                          if (IndexAddEdit == "addCommand")
                          {
                                SelectNewDiagnoz();
                                json = JsonConvert.SerializeObject(selectedDiagnoz);
                                CallServer.PostServer(controlerViewDiagnoz, json, "POST");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_") ;
                                ModelDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
 
                                  if (ViewDiagnozs == null)
                                  {
                                        ViewDiagnozs = new ObservableCollection<ModelDiagnoz>();
                                        ViewDiagnozs.Add(Idinsert);
                                  }
                                    else ViewDiagnozs.Insert(ViewDiagnozs.Count, Idinsert);
                                    SelectedViewDiagnoz = Idinsert;
                          }
                          else
                          {
                                json = JsonConvert.SerializeObject(selectedDiagnoz);
                                CallServer.PostServer(controlerViewDiagnoz, json, "PUT");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                             

                          }
                        if (json.Length > 1024)
                        {
                            selectedDiagnoz.uriDiagnoza = "";
                            json = JsonConvert.SerializeObject(selectedDiagnoz);
                            if (json.Length > 1024)
                            {
                                selectedDiagnoz.opisDiagnoza = WindowMen.DiagnozOpis.Text.ToString().Substring(0, WindowMen.DiagnozOpis.Text.Length - (json.Length - 1024));
                                json = JsonConvert.SerializeObject(selectedDiagnoz);
                            }
                        }
                        CallServer.PostServer(Controlleroutfile, Controlleroutfile + "Diagnoz/" + json + "/0", "GETID");
                        //UnloadCmdStroka("Diagnoz/", json);

                    }
   
                    WindowMen.DiagnozTablGrid.SelectedItem = null;
                    IndexAddEdit = "";
                    BoolFalseDiagnoz();

                }));
            }
        }

        private void SelectNewDiagnoz()
        {
            int indexdia = 1, setindex =0;
            if (selectedDiagnoz == null) selectedDiagnoz = new ModelDiagnoz();
            if (ViewDiagnozs != null && ViewDiagnozs.Count>0)
            {
                indexdia = Convert.ToInt32(ViewDiagnozs[0].kodDiagnoza.Substring(ViewDiagnozs[0].kodDiagnoza.LastIndexOf(".") + 1, ViewDiagnozs[0].kodDiagnoza.Length - (ViewDiagnozs[0].kodDiagnoza.LastIndexOf(".")+1)));
                for (int i = 0; i < ViewDiagnozs.Count; i++)
                {
                  setindex = Convert.ToInt32(ViewDiagnozs[i].kodDiagnoza.Substring(ViewDiagnozs[i].kodDiagnoza.LastIndexOf(".") + 1, ViewDiagnozs[i].kodDiagnoza.Length - ( ViewDiagnozs[i].kodDiagnoza.LastIndexOf(".")+1)));
                  if (indexdia < setindex) indexdia = setindex; 
                }
                indexdia++;
                string _repl = "000000000";
                selectedDiagnoz.kodDiagnoza = "DIA." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString();
            }
            else { selectedDiagnoz.kodDiagnoza = "DIA.000000001"; }
            selectedDiagnoz.nameDiagnoza = WindowMen.Diagnozt2.Text.ToString();
            selectedDiagnoz.opisDiagnoza = WindowMen.DiagnozOpis.Text.ToString();
            selectedDiagnoz.uriDiagnoza = WindowMen.DiagnozTextUri.Text.ToString();
            selectedDiagnoz.idUser = RegIdUser;
        }

        // команда печати
        RelayCommand? printViewDiagnoz;
        public RelayCommand PrintViewDiagnoz
        {
            get
            {
                return printViewDiagnoz ??
                  (printViewDiagnoz = new RelayCommand(obj =>
                  {
                      if (ViewDiagnozs != null)
                      {
                          MessageBox.Show("Колекція діагнозів :" + ViewDiagnozs[0].nameDiagnoza.ToString());
                      }
                  },
                 (obj) => ViewDiagnozs != null));
            }
        }

        // команда загрузки справочника міжнародний класифікатор захворювань 11
        private RelayCommand? addLoadGrDiagnoz;
        public RelayCommand AddLoadGrDiagnoz
        {
            get
            {
                return addLoadGrDiagnoz ??
                  (addLoadGrDiagnoz = new RelayCommand(obj =>
                  { AddComandAddLoadGrDiagnoz(); }));
            }
        }

        private void AddComandAddLoadGrDiagnoz()
        {
            MapOpisViewModel.ActCompletedInterview = "NameGrDiagnoz";
            WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            MapOpisViewModel.ActCompletedInterview = "";
            if (WindowMen.Diagnozt1.Text.Length != 0)
            { 
               if (selectedDiagnoz == null) selectedDiagnoz = new ModelDiagnoz();
               selectedDiagnoz.icdGrDiagnoz= WindowMen.Diagnozt1.Text;
                GrupDiagnoz = WindowMen.Diagnozt1.Text;
            }
 

        }




        // команда загрузки справочника міжнародний класифікатор захворювань 11
        private RelayCommand? addLoadMkx;
        public RelayCommand AddLoadMkx
        {
            get
            {
                return addLoadMkx ??
                  (addLoadMkx = new RelayCommand(obj =>
                  { AddComandAddLoadMkx(); }));
            }
        }

        private void AddComandAddLoadMkx()
        {
            if (WindowMen.Diagnozt1.Text != "")
            { 
                GrupDiagnoz = KeiIcdGrup = WindowMen.Diagnozt1.Text.Substring(0, WindowMen.Diagnozt1.Text.IndexOf(" ")).Trim();
            }
            MapOpisViewModel.ActCompletedInterview = "KeiIcdGrup";
            WinNsiIcd NewOrder = new WinNsiIcd();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-50;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (selectedDiagnoz == null) selectedDiagnoz = new ModelDiagnoz();
            selectedDiagnoz.keyIcd = WindowMen.Diagnozt4.Text;
            MapOpisViewModel.ActCompletedInterview = "";
        }
        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? findNameMkx;
        public RelayCommand FindNameMkx
        {
            get
            {
                return findNameMkx ??
                  (findNameMkx = new RelayCommand(obj =>
                  { ComandFindNameMkx(); }));
            }
        }

        private void ComandFindNameMkx()
        {
            if (selectedDiagnoz != null)
            {
                if (ViewDiagnozs != null)
                {
                    if (WindowMen.DiagnozTablGrid.SelectedIndex >= 0)
                    {
                        selectedDiagnoz = ViewDiagnozs[WindowMen.DiagnozTablGrid.SelectedIndex];
                        if (loadGrupDiagnoz == false && addboolGrDiagnoz == false)
                        {
                            MapOpisViewModel.ActCompletedInterview = "IcdGrDiagnoz";
                            SelectActivGrupDiagnoz = selectedDiagnoz.keyIcd;
                            SelectedViewDiagnoz = new ModelDiagnoz();
                            WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                            NewOrder.Left = (MainWindow.ScreenWidth / 2);
                            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                            NewOrder.ShowDialog();
                            MapOpisViewModel.ActCompletedInterview = "";
                        }
                        else
                        { 
                            WindowMen.Diagnozt3.Text = WindowMen.LibDiagnozt3.Text = "";
                            
                            selectedInterview = new ModelInterview();
                            selectedInterview.uriInterview = selectedDiagnoz.uriDiagnoza;
                            if (selectedDiagnoz.keyIcd != "")
                            {

                                string json = VeiwModelNsiIcd.controlerNsiIcd + selectedDiagnoz.keyIcd.ToString()+"/0";
                                CallServer.PostServer(VeiwModelNsiIcd.controlerNsiIcd, json, "GETID");
                                //CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                                                
                                string CmdStroka = CallServer.ServerReturn();
                                var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
                                List<ModelIcd> res = result.ModelIcd.ToList();
                                ObservableCollection<ModelIcd> Idinsert = new ObservableCollection<ModelIcd>((IEnumerable<ModelIcd>)res);
                                
                                if (Idinsert != null)
                                {
                                    WindowMen.Diagnozt3.Text = Idinsert[0].name;
                                    WindowMen.LibDiagnozt3.Text = Idinsert[0].name;
                                } 
                            }
                            WindowMen.LibFoldInterv.Visibility = Visibility.Visible;
                            WindowMen.LibCompInterviewLab.Visibility = Visibility.Visible;
                        }
                        
  
                   
                    }
                }
                

            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntevLib;
        public RelayCommand ReadColectionIntevLib
        {
            get
            {
                return readColectionIntevLib ??
                  (readColectionIntevLib = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";
                      string json = pathcontrolerDependency + selectedDiagnoz.kodDiagnoza + "/0/0";
                      CallServer.PostServer(pathcontrolerDependency, json, "GETID");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      ModelDependencyDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelDependencyDiagnoz>(CallServer.ResponseFromServer);
                      if (Idinsert != null) MapOpisViewModel.GetidkodProtokola = Idinsert.kodProtokola;
                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = 600;
                      NewOrder.Top = 130;
                      NewOrder.ShowDialog();

                  }));
            }
        }

        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? selectedListGrDiagnoz;
        public RelayCommand SelectedListGrDiagnoz
        {
            get
            {
                return selectedListGrDiagnoz ??
                  (selectedListGrDiagnoz = new RelayCommand(obj =>
                  { ComandFindNameGrDiagnoz(); }));
            }
        }

        private void ComandFindNameGrDiagnoz()
        {
            if (CheckStatusUser() == false) return;
            //SelectActivGrupDiagnoz = "GrupDiagnoz";  //   
            ActCompletedInterview = "IcdGrDiagnoz";
            WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (WindowMen.Diagnozt1.Text.Length != 0)
            {
                GrupDiagnoz = WindowMen.Diagnozt1.Text;
                
                string jason = controlerViewDiagnoz + "0/" + WindowMen.Diagnozt1.Text + "/0";
                CallServer.PostServer(controlerViewDiagnoz, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]"))
                {
                    ViewDiagnozs = new ObservableCollection<ModelDiagnoz>();
                    WindowMen.DiagnozTablGrid.ItemsSource = ViewDiagnozs;
                    CallServer.BoolFalseTabl();
                }
                else { ObservableViewDiagnoz(CmdStroka); loadGrupDiagnoz = true; }
            }

            ActCompletedInterview = "";
        }

        #endregion
        #endregion

    }
}

