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
        public static MainWindow WindowLanguageUI = MainWindow.LinkNameWindow("BackMain");
        private bool editboolLanguageUI = false, addtboolLanguageUI = false, loadboolLanguageUI = false;

        public static string pathLanguageUI = "/api/LanguageUIController/";
       
        private bool activeditLanguageUI = false, addboolLanguageUI = false;
        private string editextLanguageUI = "";
        public static ModelLanguageUI selectedLanguageUI;
        public static ObservableCollection<ModelLanguageUI> ViewLanguageUIs { get; set; }
        public ModelLanguageUI SelectedLanguageUI
        { get { return selectedLanguageUI; } set { selectedLanguageUI = value; OnPropertyChanged("SelectedLanguageUI"); } }


        public static void ObservableModelLanguageUIs(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelLanguageUI>(CmdStroka);
            List<ModelLanguageUI> res = result.ModelLanguageUI.ToList();
            ViewLanguageUIs = new ObservableCollection<ModelLanguageUI>((IEnumerable<ModelLanguageUI>)res);
            WindowLanguageUI.LanguageUITablGrid.ItemsSource = ViewLanguageUIs;

        }

        #region Команды вставки, удаления и редектирования справочника "мова інтерфейсу"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadLanguageUI;
        public RelayCommand LoadLanguageUI
        {
            get
            {
                return loadLanguageUI ??
                  (loadLanguageUI = new RelayCommand(obj =>
                  {
                      if (loadboolLanguageUI == false) MethodLoadtableLanguageUI();
                  }));
            }
        }

        // команда добавления нового объекта
        private RelayCommand? addLanguageUI;
        public RelayCommand AddLanguageUI
        {
            get
            {
                return addLanguageUI ??
                  (addLanguageUI = new RelayCommand(obj =>
                  { AddComandLanguageUI(); }));
            }
        }

        private void AddComandLanguageUI()
        {
            if (loadboolLanguageUI == false) MethodLoadtableLanguageUI();
            MethodaddcomLanguageUI();
        }

        private void MethodLoadtableLanguageUI()
        {
            WindowLanguageUI.Loadspeak.Visibility = Visibility.Hidden;
            ActCompletedInterview = "";
            CallServer.PostServer(pathLanguageUI, pathLanguageUI, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelLanguageUIs(CmdStroka);
        }

        private void MethodaddcomLanguageUI()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addtboolLanguageUI == false) BoolTrueLanguageUI();
            else BoolFalseLanguageUI();
            WindowLanguageUI.LanguageUITablGrid.SelectedItem = null;

        }


        private void BoolTrueLanguageUI()
        {
            addtboolLanguageUI = true;
            editboolLanguageUI = true;
            WindowLanguageUI.UIt3.IsEnabled = true;
            WindowLanguageUI.UIt3.Background = Brushes.AntiqueWhite;
            
        }

        private void BoolFalseLanguageUI()
        {
            addtboolLanguageUI = false;
            editboolLanguageUI = false;
            WindowLanguageUI.UIt3.IsEnabled = false;
            WindowLanguageUI.UIt3.Background = Brushes.White;
                        
        }
        // команда удаления
        private RelayCommand? removeLanguageUI;
        public RelayCommand RemoveLanguageUI
        {
            get
            {
                return removeLanguageUI ??
                  (removeLanguageUI = new RelayCommand(obj =>
                  {
                      if (selectedLanguageUI != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathLanguageUI + selectedLanguageUI.id.ToString();
                              CallServer.PostServer(pathLanguageUI, json, "DELETE");
                              IndexAddEdit = "remove";
                              ViewLanguageUIs.Remove(selectedLanguageUI);
                              WindowLanguageUI.LanguageUITablGrid.ItemsSource = ViewLanguageUIs;
                              BoolFalseLanguageUI();
                              IndexAddEdit = "";
                              WindowLanguageUI.LanguageUITablGrid.SelectedItem = null;
                          }
                      }

                  },
                 (obj) => ViewLanguageUIs != null));
            }
        }

        // команда  редактировать
        private RelayCommand? editLanguageUI;
        public RelayCommand? EditLanguageUI
        {
            get
            {
                return editLanguageUI ??
                  (editLanguageUI = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (editboolLanguageUI == false & selectedLanguageUI != null)
                      {
                          BoolTrueLanguageUI();
                      }
                      else
                      {
                          BoolFalseLanguageUI();
                          IndexAddEdit = "";
                          WindowLanguageUI.LanguageUITablGrid.SelectedItem = null;
                      }

                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveLanguageUI;
        public RelayCommand SaveLanguageUI
        {
            get
            {
                return saveLanguageUI ??
                  (saveLanguageUI = new RelayCommand(obj =>
                  {
                      string json = "";

                      if (WindowLanguageUI.UIt3.Text.Length != 0)
                      {
                          //  формирование кода Feature по значениею группы выранной жалобы
                          
                          if (IndexAddEdit == "addCommand")
                          {

                              SelectNewLanguageUI();
                              // ОБращение к серверу добавляем запись
                              json = JsonConvert.SerializeObject(selectedLanguageUI);
                              CallServer.PostServer(pathLanguageUI, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelLanguageUI Idinsert = JsonConvert.DeserializeObject<ModelLanguageUI>(CallServer.ResponseFromServer);
                              if (Idinsert == null)
                              {
                                  return;
                              }

                              if (ViewLanguageUIs == null) ViewLanguageUIs = new ObservableCollection<ModelLanguageUI>();
                              ViewLanguageUIs.Add(selectedLanguageUI);
                              WindowLanguageUI.LanguageUITablGrid.ItemsSource = ViewLanguageUIs;
                          }
                          else
                          {
                              // ОБращение к серверу измнить корректируемую запись в БД
                              json = JsonConvert.SerializeObject(selectedLanguageUI);
                              CallServer.PostServer(pathLanguageUI, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("LanguageUI/", json);
                      }
                      BoolFalseLanguageUI();
                      IndexAddEdit = "";
                      WindowLanguageUI.LanguageUITablGrid.SelectedItem = null;

                  }));
            }
        }


        public void SelectNewLanguageUI()
        {
            selectedLanguageUI = new  ModelLanguageUI();
            selectedLanguageUI.name = WindowLanguageUI.UIt3.Text;
            if (ViewLanguageUIs != null && ViewLanguageUIs.Count >0)
            {
                int _keyLanguageUIindex = 0, setindex = 0;
                _keyLanguageUIindex = Convert.ToInt32(ViewLanguageUIs[0].keyLang.Substring(ViewLanguageUIs[0].keyLang.LastIndexOf(".") + 1, ViewLanguageUIs[0].keyLang.Length - (ViewLanguageUIs[0].keyLang.LastIndexOf(".") + 1)));
                for (int i = 0; i < ViewLanguageUIs.Count; i++)
                {
                    setindex = Convert.ToInt32(ViewLanguageUIs[i].keyLang.Substring(ViewLanguageUIs[i].keyLang.LastIndexOf(".") + 1, ViewLanguageUIs[i].keyLang.Length - (ViewLanguageUIs[i].keyLang.LastIndexOf(".") + 1)));
                    if (_keyLanguageUIindex < setindex) _keyLanguageUIindex = setindex;
                }
                _keyLanguageUIindex++;
                string _repl = "000000000";
                selectedLanguageUI.keyLang = "LNG." + _repl.Substring(0, _repl.Length - _keyLanguageUIindex.ToString().Length) + _keyLanguageUIindex.ToString();
            }
            else
            {
                ViewLanguageUIs = new ObservableCollection<ModelLanguageUI>();
                selectedLanguageUI.keyLang = "LNG.000000001";
            }

        }

        // команда печати
        RelayCommand? printLanguageUI;
        public RelayCommand PrintLanguageUI
        {
            get
            {
                return printLanguageUI ??
                  (printLanguageUI = new RelayCommand(obj =>
                  {
                          MessageBox.Show("Мова інтерфуйсу :" + ViewLanguageUIs[0].name.ToString());
                  },
                 (obj) => ViewLanguageUIs != null));
            }
        }
        #endregion
        
    }
}
