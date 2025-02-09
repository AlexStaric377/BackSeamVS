﻿using System;
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
        // ViewListGrDetailing модель GrDetailing
        // клавиша в окне: "Групы детализации"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы детализации"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>
        /// 

        private bool editboolListGrDet = false, addboolListGrDet = false, loadboolListGrDet = false;
        public static string pathcontrolerListGrDet =  "/api/ControllerListGroupDetail/";
        public static ModelListGrDetailing selectedListGroupDeliting;
        public static string[] dictiontygr = { "YAA", "YBB", "YCC", "YDD", "YEE", "YFF", "YGG", "YHH", "YII", "YJJ", "YKK", "YLL", "YMM", "YNN", "YOO", "YPP", "YQQ", "YRR", "YSS", "YTT", "YUU", "YVV", "YWW", "YXX", "YYY", "YZZ", "YAAB", "YBBC", "YCCD", "YDDE" };


        public static ObservableCollection<ModelListGrDetailing> ViewListGrDetailings { get; set; }
        public static int CountListGroupDeliting = 0;
        
        public ModelListGrDetailing SelectedListGroupDeliting
        { get { return selectedListGroupDeliting; } set { selectedListGroupDeliting = value; OnPropertyChanged("SelectedListGroupDeliting"); } }

        public static void ObservableListViewGroupDeliting(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelListGrDetailing>(CmdStroka);
            List<ModelListGrDetailing> res = result.ViewListGrDetailing.ToList();
            ViewListGrDetailings = new ObservableCollection<ModelListGrDetailing>((IEnumerable<ModelListGrDetailing>)res);
            WindowMen.GrDetailingTablGrid.ItemsSource = ViewListGrDetailings;

        }

        #region Команды вставки, удаления и редектирования справочника "Жалобы"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadListGrDetailing;
        public RelayCommand LoadListGrDetailing
        {
            get
            {
                return loadListGrDetailing ??
                  (loadListGrDetailing = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadtablListGrDet();
                  }));
            }
        }

        // команда добавления нового объекта
        private RelayCommand addListGroupDeliting;
        public RelayCommand AddListGroupDeliting
        {
            get
            {
                return addListGroupDeliting ??
                  (addListGroupDeliting = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComListGroupDeliting(); }));
            }
        }

        private void AddComListGroupDeliting()
        {
            if (loadboolListGrDet == false) MethodloadtablListGrDet();
            MethodaddcomListGrDet();
        }

        private void MethodaddcomListGrDet()
        {
            IndexAddEdit = "addCommand";
            selectedListGroupDeliting = new ModelListGrDetailing();
            SelectedListGroupDeliting = selectedListGroupDeliting;
            if (addboolListGrDet == false) { BoolTrueGroupDetailing(); SelectNewGroupDetailing(); } 
            else BoolFalseGroupDetailing();

        }

        private void MethodloadtablListGrDet()
        {
            WindowMen.LoadListGrdel.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerListGrDet, pathcontrolerListGrDet, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableListViewGroupDeliting(CmdStroka);

        }
        
        private void BoolTrueGroupDetailing()
        {
            addboolListGrDet = true;
            editboolListGrDet = true;
            WindowMen.GrDetailingt2.IsEnabled = true ;
            WindowMen.GrDetailingt2.Background = Brushes.AntiqueWhite;
            WindowMen.GrDetailingTablGrid.IsEnabled = false;
            if (IndexAddEdit == "addCommand")
            {
                WindowMen.BorderLoadListGrDetailing.IsEnabled = false;
                WindowMen.BorderGhangeListGrDetailing.IsEnabled = false;
                WindowMen.BorderDeleteListGrDetailing.IsEnabled = false;
                WindowMen.BorderPrintListGrDetailing.IsEnabled = false;
            }
            if (IndexAddEdit == "editCommand")
            {
                WindowMen.BorderLoadListGrDetailing.IsEnabled = false;
                WindowMen.BorderAddListGrDetailing.IsEnabled = false;
                WindowMen.BorderDeleteListGrDetailing.IsEnabled = false;
                WindowMen.BorderPrintListGrDetailing.IsEnabled = false;
            }
        }

        private void BoolFalseGroupDetailing()
        {
            addboolListGrDet = false;
            editboolListGrDet = false;
            WindowMen.GrDetailingt2.IsEnabled =  false ;
            WindowMen.GrDetailingt2.Background = Brushes.White;
            WindowMen.FolderGrDetailing.Visibility = Visibility.Hidden;
            WindowMen.GrDetailingTablGrid.IsEnabled = true;
            WindowMen.BorderLoadListGrDetailing.IsEnabled = true;
            WindowMen.BorderGhangeListGrDetailing.IsEnabled = true;
            WindowMen.BorderDeleteListGrDetailing.IsEnabled = true;
            WindowMen.BorderPrintListGrDetailing.IsEnabled = true;
            WindowMen.BorderAddListGrDetailing.IsEnabled = true;
        }

        // команда удаления
        private RelayCommand? removeListGroupDeliting;
        public RelayCommand RemoveListGroupDeliting
        {
            get
            {
                return removeListGroupDeliting ??
                  (removeListGroupDeliting = new RelayCommand(obj =>
                  {
                     if (selectedListGroupDeliting !=null)
                     {
                          if (selectedListGroupDeliting.idUser != RegIdUser && selectedComplaint.idUser != "Admin")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathcontrolerListGrDet + selectedListGroupDeliting.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ViewListGrDetailings.Remove(selectedListGroupDeliting);
                              selectedListGroupDeliting = new ModelListGrDetailing();
                              BoolFalseGroupDetailing();
                              IndexAddEdit = "";
                          }
                     }
                      
                  },
                  (obj) => ViewListGrDetailings != null)); 
            }
        }

        // команда  редактировать
        private RelayCommand? editListGroupDeliting;
        public RelayCommand? EditListGroupDeliting
        {
            get
            {
                return editListGroupDeliting ??
                  (editListGroupDeliting = new RelayCommand(obj =>
                  {
                      if (selectedListGroupDeliting != null && selectedListGroupDeliting.id != 0)
                      {
                          if (selectedListGroupDeliting.idUser != RegIdUser ) //&& selectedComplaint.idUser != "Admin"
                          {
                              InfoEditZapis();
                              return;
                          }
                          IndexAddEdit = "editCommand";
                          if (editboolListGrDet == false)
                          {
                              BoolTrueGroupDetailing();
                              //edittextDetailing = WindowMen.GrDetailingt2.Text.ToString();
                          }
                          else
                          {
                              BoolFalseGroupDetailing();
                              WindowMen.GrDetailingTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveListGroupDeliting;
        public RelayCommand SaveListGroupDeliting
        {
            get
            {
                return saveListGroupDeliting ??
                  (saveListGroupDeliting = new RelayCommand(obj =>
                  {
                      string json = "";
 
                        if (WindowMen.GrDetailingt2.Text.Length != 0)
                        {
                            if (IndexAddEdit == "addCommand")
                            {
                                //  формирование кода Detailing по значениею группы выранного храктера жалобы
   
                                selectedListGroupDeliting.id = 0;
                                selectedListGroupDeliting.nameGrup = WindowMen.GrDetailingt2.Text;
                                selectedListGroupDeliting.idUser = RegIdUser;
                                json = JsonConvert.SerializeObject(selectedListGroupDeliting);
                                CallServer.PostServer(pathcontrolerListGrDet, json, "POST");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                json = CallServer.ResponseFromServer;
                                ModelListGrDetailing Idinsert = JsonConvert.DeserializeObject<ModelListGrDetailing>(CallServer.ResponseFromServer);
                                int Countins = ViewListGrDetailings != null ? ViewListGrDetailings.Count : 0;
                                ViewListGrDetailings.Insert(Countins, Idinsert);
                                SelectedListGroupDeliting = Idinsert;
                            }
                            else
                            {
                                json = JsonConvert.SerializeObject(selectedListGroupDeliting);
                                CallServer.PostServer(pathcontrolerListGrDet, json, "PUT");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("ListGrDetailing/", json);
                      }
                      WindowMen.GrDetailingTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseGroupDetailing();

                  }));
            }
        }

        private void SelectNewGroupDetailing()
        {
            if (selectedListGroupDeliting == null) selectedListGroupDeliting = new ModelListGrDetailing();
             
            bool maxindex = false;
            string addindex = ".000";
            int indexdictionty = 0, Numbkey = 0;
            while (maxindex == false)
            {
                selectedListGroupDeliting.keyGrDetailing = dictiontygr[indexdictionty] + addindex;
                string json = pathcontrolerListGrDet + selectedListGroupDeliting.keyGrDetailing +"/0";
                CallServer.PostServer(pathcontrolerListGrDet, json, "GETID");
                if (CallServer.ResponseFromServer.Contains("[]") == true)
                {
                    selectedListGroupDeliting.keyGrDetailing = dictiontygr[indexdictionty] + addindex;
                    maxindex = true;
                    break;
                }
                Numbkey++;
                if (Numbkey == 1000)
                {
                    Numbkey = 1;
                    indexdictionty++;
                }
                addindex = addindex.Length - Numbkey.ToString().Length > 0 ? addindex.Substring(0, addindex.Length - Numbkey.ToString().Length) + Numbkey.ToString() : "";
            }           
        }


        // команда печати
        RelayCommand? printListGroupDeliting;
        public RelayCommand PrintListGroupDeliting
        {
            get
            {
                return printListGroupDeliting ??
                  (printListGroupDeliting = new RelayCommand(obj =>
                  {
                      if (ViewListGrDetailings != null) 
                      {
                          MessageBox.Show("Жалоби :" + ViewListGrDetailings[0].nameGrup.ToString());
                      }
                  },
                 (obj) => ViewListGrDetailings != null)); //ViewListGrDetailings.Count > 0
            }
        }

        // команда открытия окна содержащего группу детализации характера жалобы  
        private RelayCommand? viewGrDetailing;
        public RelayCommand ViewGrDetailing
        {
            get
            {
                return viewGrDetailing ??
                  (viewGrDetailing = new RelayCommand(obj =>
                  {
                      if (selectedListGroupDeliting != null)
                      {
                          if (selectedListGroupDeliting.keyGrDetailing!=null && selectedListGroupDeliting.keyGrDetailing!="")
                          { 
                              string pathcontroller =  "/api/GrDetalingController/"; 
                              string jason = pathcontroller + "0/" + selectedListGroupDeliting.keyGrDetailing + "/0";
                              CallServer.PostServer(pathcontroller, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  MapOpisViewModel.ActCompletedInterview = "GrDetailing";
                                  WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                                  NewOrder.Left = (MainWindow.ScreenWidth / 2);
                                  NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                                  NewOrder.ShowDialog();
                                  MapOpisViewModel.ActCompletedInterview = null;

                              }
                          }
                          
                          

                      }

                  }));
            }
        }


        
        // команда выбора групы детализации
        RelayCommand? selectGrDeteling;
        public RelayCommand SelectGrDeteling
        {
            get
            {
                return selectGrDeteling ??
                  (selectGrDeteling = new RelayCommand(obj =>
                  {
                      if (ViewListGrDetailings != null)
                      {
                          WindowMen.FolderGrDetailing.Visibility = Visibility.Visible;
                      }
                  })); 
            }
        }

        

        // команда поиска групы детализации
        RelayCommand? searchGrDetailing;
        public RelayCommand SearchGrDetailing
        {
            get
            {
                return searchGrDetailing ??
                  (searchGrDetailing = new RelayCommand(obj =>
                  {
                      MetodDetailingEnter();

                  }));
            }
        }

        

       RelayCommand? checkKeyTextDetailing;
        public RelayCommand CheckKeyTextDetailing
        {
            get
            {
                return checkKeyTextDetailing ??
                  (checkKeyTextDetailing = new RelayCommand(obj =>
                  {
                      MetodDetailingEnter();
                  }));
            }
        }
        public void MetodDetailingEnter()
        {

            if (CheckStatusUser() == false) return;
            if (WindowMen.PoiskGrDetailing.Text.Trim() != "")
            {
                string jason = pathcontrolerListGrDet + "0/" + WindowMen.PoiskGrDetailing.Text;
                CallServer.PostServer(pathcontrolerListGrDet, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableListViewGroupDeliting(CmdStroka);
            }
        }
        #endregion
        #endregion
    }
}
