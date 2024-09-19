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
using System.Diagnostics;

namespace BackSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        /// 
        public static MainWindow WindowGrupDiagnoz = MainWindow.LinkNameWindow("BackMain");
        private static bool loadboolGrupDiagnoz = false, activeditViewGroupDiagnoz = false;
        public static string controlerGrupDiagnoz = "/api/GrupDiagnozController/";
        private ModelGrupDiagnoz selectedViewGrupDiagnoz;

        public static ObservableCollection<ModelGrupDiagnoz> ViewGrupDiagnozs { get; set; }
       
        public ModelGrupDiagnoz SelectedViewGrupDiagnoz
        { get { return selectedViewGrupDiagnoz; } set { selectedViewGrupDiagnoz = value; OnPropertyChanged("SelectedViewGrupDiagnoz"); } }

        public static void ObservableViewGrupDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGrupDiagnoz>(CmdStroka);
            List<ModelGrupDiagnoz> res = result.ModelGrupDiagnoz.ToList();
            ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>((IEnumerable<ModelGrupDiagnoz>)res);
            WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x=> x.icdGrDiagnoz);
            loadboolGrupDiagnoz = true;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>



        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadViewGrupDiagnoz;
        public RelayCommand LoadViewGrupDiagnoz
        {
            get
            {
                return loadViewGrupDiagnoz ??
                  (loadViewGrupDiagnoz = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadGrupDiagnoz();
                  }));
            }
        }


        // команда добавления нового объекта
        bool activViewGrupDiagnoz = false;
        private RelayCommand addViewGrupDiagnoz;
        public RelayCommand AddViewGrupDiagnoz
        {
            get
            {
                return addViewGrupDiagnoz ??
                  (addViewGrupDiagnoz = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComViewGrupDiagnoz(); }));
            }
        }

        private void AddComViewGrupDiagnoz()
        {
            if (loadboolGrupDiagnoz == false) MethodLoadGrupDiagnoz();
            MethodAddGrupDiagnoz();

        }

        private void MethodAddGrupDiagnoz()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            
            if (activViewGrupDiagnoz == false) BoolTrueGrupDiagnoz();
            else BoolFalseGrupDiagnoz();
            WindowMen.GrDiagnozTablGrid.SelectedItem = null;

        }

        private void MethodLoadGrupDiagnoz()
        {
            WindowMen.GrDiagnozload.Visibility = Visibility.Hidden;
            ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>();
            CallServer.PostServer(controlerGrupDiagnoz, controlerGrupDiagnoz, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewGrupDiagnoz(CmdStroka);

        }


        private void BoolTrueGrupDiagnoz()
        {
            if (IndexAddEdit == "addCommand")
            { 
                WindowMen.GrDiagnozt1.IsEnabled = true;
                WindowMen.GrDiagnozt1.Background = Brushes.AntiqueWhite;           
            }
            WindowMen.GrDiagnozt2.IsEnabled = true;
            WindowMen.GrDiagnozt2.Background = Brushes.AntiqueWhite;
            WindowMen.GrDiagnozt5.IsEnabled = true;
            WindowMen.GrDiagnozt5.Background = Brushes.AntiqueWhite;
            WindowMen.GrDiagnozt6.IsEnabled = true;
            WindowMen.GrDiagnozt6.Background = Brushes.AntiqueWhite;
            WindowMen.FolderGroupDiagnoz.Visibility = Visibility.Visible;
            WindowMen.FolderGrMKX.Visibility = Visibility.Visible;
            WindowMen.GrDiagnozTablGrid.IsEnabled = false;
            activViewGrupDiagnoz = true;

            
        }

        private void BoolFalseGrupDiagnoz()
        {
            WindowMen.GrDiagnozt1.IsEnabled = false;
            WindowMen.GrDiagnozt1.Background = Brushes.White;
            WindowMen.GrDiagnozt2.IsEnabled = false;
            WindowMen.GrDiagnozt2.Background = Brushes.White;
            WindowMen.GrDiagnozt5.IsEnabled = false;
            WindowMen.GrDiagnozt5.Background = Brushes.White;
            WindowMen.GrDiagnozt6.IsEnabled = false;
            WindowMen.GrDiagnozt6.Background = Brushes.White;
            WindowMen.FolderGroupDiagnoz.Visibility = Visibility.Hidden;
            WindowMen.FolderGrMKX.Visibility = Visibility.Hidden;
            WindowMen.GrDiagnozTablGrid.IsEnabled = true;
            activViewGrupDiagnoz = false;
        }

        // команда удаления
        private RelayCommand? removeViewGroupDiagnoz;
        public RelayCommand RemoveViewGroupDiagnoz
        {
            get
            {
                return removeViewGroupDiagnoz ??
                  (removeViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      if (selectedViewGrupDiagnoz != null)
                      {
                          if (selectedViewGrupDiagnoz.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerGrupDiagnoz + selectedViewGrupDiagnoz.id.ToString();
                              CallServer.PostServer(controlerGrupDiagnoz, json, "DELETE");
                              ViewGrupDiagnozs.Remove(selectedViewGrupDiagnoz);
                              WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x => x.icdGrDiagnoz);

                              selectedViewGrupDiagnoz = new  ModelGrupDiagnoz();
                              BoolFalseQualification();
                              WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                          }
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewGrupDiagnozs != null));
            }
        }


        // команда  редактировать
       
        private RelayCommand? editViewGroupDiagnoz;
        public RelayCommand? EditViewGroupDiagnoz
        {
            get
            {
                return editViewGroupDiagnoz ??
                  (editViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (selectedViewGrupDiagnoz != null)
                      {
                          if (selectedViewGrupDiagnoz.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          if (activeditViewGroupDiagnoz == false)
                          {
                              BoolTrueGrupDiagnoz();
                              activeditViewGroupDiagnoz = true;
                          }
                          else
                          {
                              BoolFalseGrupDiagnoz();
                              activeditViewGroupDiagnoz = false;
                              WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveViewGroupDiagnoz;
        public RelayCommand SaveViewGroupDiagnoz
        {
            get
            {
                return saveViewGroupDiagnoz ??
                  (saveViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if (WindowMen.GrDiagnozt2.Text.Trim().Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              selectedViewGrupDiagnoz.idUser = RegIdUser;
                              json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              CallServer.PostServer(controlerGrupDiagnoz, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                              ViewGrupDiagnozs.Add(Idinsert);
                              WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x => x.icdGrDiagnoz);
                          }
                          else
                          {
                              json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              CallServer.PostServer(controlerGrupDiagnoz, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("GrupDiagnoz/", json);
                      }
                      
                      WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseGrupDiagnoz();

                  }));
            }
        }

 
        // команда печати
        RelayCommand? printViewGrupDiagnoz;
        public RelayCommand PrintViewGrupDiagnoz
        {
            get
            {
                return printViewGrupDiagnoz ??
                  (printViewGrupDiagnoz = new RelayCommand(obj =>
                  {
                     MessageBox.Show("Груповання діагнозів :" + ViewGrupDiagnozs[0].nameGrDiagnoz.ToString());
                     
                  },
                 (obj) => ViewGrupDiagnozs != null));
            }
        }

        
        // команда загрузки справочника міжнародний класифікатор захворювань 11
        private RelayCommand? addLoadGrMkx;
        public RelayCommand AddLoadGrMkx
        {
            get
            {
                return addLoadGrMkx ??
                  (addLoadGrMkx = new RelayCommand(obj =>
                  { ComandAddLoadGrMkxMkx(); }));
            }
        }

        private void ComandAddLoadGrMkxMkx()
        {
            WinNsiIcd NewOrder = new WinNsiIcd();
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (WindowMen.Diagnozt4.Text != "")
            {

                if (selectedViewGrupDiagnoz == null) selectedViewGrupDiagnoz = new ModelGrupDiagnoz();
                WindowMen.GrDiagnozt1.Text = WindowMen.Diagnozt4.Text;
                WindowMen.GrDiagnozt2.Text = WindowMen.Diagnozt3.Text;
                selectedViewGrupDiagnoz.icdGrDiagnoz = WindowMen.Diagnozt4.Text;
                selectedViewGrupDiagnoz.nameGrDiagnoz = WindowMen.Diagnozt3.Text;
            }
 
            
        }


        

        // Выбор названия интервью диагностики 
        private RelayCommand? searchGrDiagnoz;
        public RelayCommand SearchGrDiagnoz
        {
            get
            {
                return searchGrDiagnoz ??
                  (searchGrDiagnoz = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      if (WindowGrupDiagnoz.PoiskGrDiagnoz.Text.Trim() != "")
                      { 
                          string jason = controlerGrupDiagnoz + "0/" + WindowGrupDiagnoz.PoiskGrDiagnoz.Text;
                          CallServer.PostServer(Interviewcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableViewGrupDiagnoz(CmdStroka);                     
                      }

                  }));
            }
        }
    }
    #endregion
    #endregion
}
