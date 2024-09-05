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
    public partial class MapOpisViewModel : INotifyPropertyChanged
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
                  { AddComListGroupDeliting(); }));
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
            if (addboolListGrDet == false) BoolTrueGroupDetailing();
            else BoolFalseGroupDetailing();
            WindowMen.GrDetailingTablGrid.SelectedItem = null;
        }

        private void MethodloadtablListGrDet()
        {
            WindowMen.LoadListGrdel.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerListGrDet, pathcontrolerListGrDet, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableListViewGroupDeliting(CmdStroka);
            WindowMen.PoiskGrDetailing.IsEnabled = true;
            WindowMen.PoiskGrDetailing.Background = Brushes.AntiqueWhite;
        }
        
        private void BoolTrueGroupDetailing()
        {
            addboolListGrDet = true;
            editboolListGrDet = true;
            WindowMen.GrDetailingt2.IsEnabled = true ;
            WindowMen.GrDetailingt2.Background = Brushes.AntiqueWhite;
            WindowMen.GrDetailingTablGrid.IsEnabled = false;
        }

        private void BoolFalseGroupDetailing()
        {
            addboolListGrDet = false;
            editboolListGrDet = false;
            WindowMen.GrDetailingt2.IsEnabled =  false ;
            WindowMen.GrDetailingt2.Background = Brushes.White;
            WindowMen.FolderGrDetailing.Visibility = Visibility.Hidden;
            WindowMen.GrDetailingTablGrid.IsEnabled = true;
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
                      if (selectedListGroupDeliting != null)
                      {
                          if (selectedListGroupDeliting.idUser != RegIdUser && selectedComplaint.idUser != "Admin")
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
                      BoolFalseGroupDetailing();
                        if (WindowMen.GrDetailingt2.Text.Length != 0)
                        {
                            if (IndexAddEdit == "addCommand")
                            {
                                //  формирование кода Detailing по значениею группы выранного храктера жалобы
                                SelectNewGroupDetailing();
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
                        //else WindowMen.GrDetailingt2.Text = edittextDetailing;
                      WindowMen.GrDetailingTablGrid.SelectedItem = null;
                      IndexAddEdit = "";


                  }));
            }
        }

        private void SelectNewGroupDetailing()
        {
            if (selectedListGroupDeliting == null) selectedListGroupDeliting = new ModelListGrDetailing();
            //selectedListGroupDeliting.keyGrDetailing = dictiontygr[ViewListGrDetailings.Count] + ".000"; ;
            selectedListGroupDeliting.nameGrup = WindowMen.GrDetailingt2.Text;
            selectedListGroupDeliting.idUser = RegIdUser;
            bool maxindex = false;
            string addindex = ".000";
            int indexdictionty = 0, Numbkey = 0;
            while (maxindex == false)
            {
                selectedListGroupDeliting.keyGrDetailing = dictiontygr[indexdictionty] + addindex;
                string json = pathcontrolerListGrDet + selectedListGroupDeliting.keyGrDetailing;
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
            selectedListGroupDeliting.id = 0;
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
                      if (ViewListGrDetailings != null)
                      {
                          if (WindowMen.PoiskGrDetailing.Text.Trim() != "")
                          {
                              string jason = pathcontrolerListGrDet + "0/" + WindowMen.PoiskGrDetailing.Text;
                              CallServer.PostServer(pathcontrolerListGrDet, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                              else ObservableListViewGroupDeliting(CmdStroka);
                          }
                      }
                  }));
            }
        }

        #endregion
        #endregion
    }
}
