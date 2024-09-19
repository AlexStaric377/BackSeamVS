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

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {

        //  ОБработка команд сопровождения справочника областей

        #region Обработка событий и команд вставки, удаления и редектирования справочника "областей"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех областей из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowSob = MainWindow.LinkNameWindow("BackMain");
        public static bool editboolSob = false, addboolSob = false, loadboolSob = false;
        
        public static string pathcontrolerSob = "/api/SobController/";
        public ViewModelSob selectedSob;
        public static ViewModelSob selectedViewModelSob;
       
        public static ObservableCollection<ViewModelSob> ViewSobs { get; set; }
        public ViewModelSob SelectedViewModelSob
        {
            get { return selectedViewModelSob; }
            set { selectedViewModelSob = value; OnPropertyChanged("SelectedViewModelSob"); }
        }


        public static void ObservableViewSob(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelSob>(CmdStroka);
            List<ViewModelSob> res = result.ViewModelSob.ToList();
            ViewSobs = new ObservableCollection<ViewModelSob>((IEnumerable<ViewModelSob>)res);
            WindowSob.SobTablGrid.ItemsSource = ViewSobs;
            loadboolSob = true;

        }


        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadVeiwModelSob;
        public RelayCommand LoadSob
        {
            get
            {
                return loadVeiwModelSob ??
                  (loadVeiwModelSob = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadtabSob();
                  }));
            }
        }


        // команда добавления нового объекта

        private RelayCommand addVeiwModelSob;
        public RelayCommand AddSob
        {
            get
            {
                return addVeiwModelSob ??
                  (addVeiwModelSob = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComVeiwModelSob(); }));
            }
        }

        private void AddComVeiwModelSob()
        {
            if (loadboolSob == false) MethodloadtabSob();
            MethodaddcomSob();
        }

        private void MethodaddcomSob()
        {
            WindowSob.SobTablGrid.SelectedItem = null;
            selectedSob = new ViewModelSob();
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolSob == false) ModelSobtrue();
            else ModelSobfalse();
        }


        private void MethodloadtabSob()
        {
            WindowSob.LoadSob.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerSob, pathcontrolerSob, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewSob(CmdStroka);
        }

        // команда  редактировать
        private bool activeditVeiwModelSob = false;
        private RelayCommand? editVeiwModelSob;
        public RelayCommand? EditSob
        {
            get
            {
                return editVeiwModelSob ??
                  (editVeiwModelSob = new RelayCommand(obj =>
                  {
                      if (WindowSob.SobTablGrid.SelectedIndex >= 0)
                      {
                          selectedSob = ViewSobs[WindowSob.SobTablGrid.SelectedIndex];
                          IndexAddEdit = "editCommand";
                          if (editboolSob == false) ModelSobtrue();
                          else
                          {
                              ModelSobfalse();
                              WindowSob.SobTablGrid.SelectedItem = null;
                          }
                      }
    
                  }));
            }
        }

        // команда удаления
        private RelayCommand? removeVeiwModelSob;
        public RelayCommand RemoveSob
        {
            get
            {
                return removeVeiwModelSob ??
                  (removeVeiwModelSob = new RelayCommand(obj =>
                  {
                    if(WindowSob.SobTablGrid.SelectedIndex>=0)
                      {
                           SelectedRemove();
                           // Видалення данных 
                           if (MapOpisViewModel.DeleteOnOff == true)
                           {
                              selectedSob = ViewSobs[WindowSob.SobTablGrid.SelectedIndex];
                              string json = pathcontrolerSob + selectedSob.id.ToString();
                              CallServer.PostServer(pathcontrolerSob, json, "DELETE");
                              ViewSobs.Remove(selectedSob);
                              selectedSob = new ViewModelSob();
                           }
                      }
  
                      IndexAddEdit = "";
                  },
                 (obj) => ViewSobs != null));
            }
        }


        // команда сохранить редактирование
        RelayCommand? saveVeiwModelSob;
        public RelayCommand SaveSob
        {
            get
            {
                return saveVeiwModelSob ??
                  (saveVeiwModelSob = new RelayCommand(obj =>
                  {
                      if (WindowSob.SobPind.Text.Trim().Length != 0 & WindowSob.Sobnamepunkt.Text.Trim().Length != 0)
                      {
                          selectedSob.pind = Convert.ToInt32(WindowSob.SobPind.Text);
                          selectedSob.nameObl = WindowSob.SobnameObl.Text;
                          selectedSob.nameRajon = WindowSob.SobnameRajon.Text;
                          selectedSob.namepunkt = WindowSob.Sobnamepunkt.Text;
                          
                          var json = JsonConvert.SerializeObject(selectedSob);
                          string Method = IndexAddEdit == "addCommand" ? "POST" : "PUT";
                          CallServer.PostServer(pathcontrolerSob, json, Method);
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer;
                          UnloadCmdStroka("Sob/", json);
                          ViewModelSob Idinsert = JsonConvert.DeserializeObject<ViewModelSob>(json);
                          int CountInsert = ViewSobs != null ? ViewSobs.Count : 0;
                          if (IndexAddEdit == "addCommand")
                          { 
                              ViewSobs.Insert(CountInsert, Idinsert);
                              WindowSob.SobTablGrid.ItemsSource = ViewSobs;
                          } 
                          
                      }
                      ModelSobfalse();

                  }));
            }
        }
        private void ModelSobfalse()
        {
            addboolSob = false;
            editboolSob = false;
            WindowSob.SobPind.IsEnabled = false;
            WindowSob.SobPind.Background = Brushes.White;
            WindowSob.SobnameObl.IsEnabled = false;
            WindowSob.SobnameObl.Background = Brushes.White;
            WindowSob.SobnameRajon.IsEnabled = false;
            WindowSob.SobnameRajon.Background = Brushes.White;
            WindowSob.Sobnamepunkt.IsEnabled = false;
            WindowSob.Sobnamepunkt.Background = Brushes.White;
            WindowSob.SobTablGrid.IsEnabled = true;

        }

        private void ModelSobtrue()
        {
            addboolSob = true;
            editboolSob = true;
            WindowSob.SobPind.IsEnabled = true;
            WindowSob.SobPind.Background = Brushes.AntiqueWhite;
            WindowSob.SobnameObl.IsEnabled = true;
            WindowSob.SobnameObl.Background = Brushes.AntiqueWhite;
            WindowSob.SobnameRajon.IsEnabled = true;
            WindowSob.SobnameRajon.Background = Brushes.AntiqueWhite;
            WindowSob.Sobnamepunkt.IsEnabled = true;
            WindowSob.Sobnamepunkt.Background = Brushes.AntiqueWhite;
            WindowSob.SobTablGrid.IsEnabled = false;

        }
        // команда печати
        RelayCommand? printVeiwModelSob;
        public RelayCommand PrintSob
        {
            get
            {
                return printVeiwModelSob ??
                  (printVeiwModelSob = new RelayCommand(obj =>
                  {
                           MessageBox.Show("Довідник областей :" + ViewSobs[0].nameObl.ToString());
                  },
                 (obj) => ViewSobs != null));
            }
        }

        

        // Выбор назви області
        private RelayCommand? searchSob;
        public RelayCommand SearchSob
        {
            get
            {
                return searchSob ??
                  (searchSob = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      if (WindowSob.PoiskSob.Text.Trim() != "")
                      {
                          string jason = pathcontrolerSob + "0/0/" + WindowSob.PoiskSob.Text;
                          CallServer.PostServer(pathcontrolerSob, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableViewSob(CmdStroka);
                      }

                  }));
            }
        }
    }
    #endregion
}
