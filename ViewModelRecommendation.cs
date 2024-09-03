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
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        private bool loadbooltablRecom = false, activModelRecommendation = false;
        public static string controlerModelRecommendation =  "/api/RecommendationController/";
        public static ModelRecommendation selectedRecommendation;
        
        public static ObservableCollection<ModelRecommendation> ModelRecommendations { get; set; }

        public ModelRecommendation SelectedModelRecommendation
        { 
            get { return selectedRecommendation; }
            set { selectedRecommendation = value; OnPropertyChanged("SelectedModelRecommendation"); }
        }

        public static void ObservableModelRecommendation(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelRecommendation>(CmdStroka);
            List<ModelRecommendation> res = result.ViewRecommendation.ToList();
            ModelRecommendations = new ObservableCollection<ModelRecommendation>((IEnumerable<ModelRecommendation>)res);
            WindowMen.RecommendationTablGrid.ItemsSource = ModelRecommendations;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadModelRecommendation;
        public RelayCommand LoadModelRecommendation
        {
            get  {
                return loadModelRecommendation ??
                  (loadModelRecommendation = new RelayCommand(obj =>
                  {
                      if (loadbooltablRecom == false) MethodloadtablRecom();
                  }));
            }
        }


        // команда добавления нового объекта
        
        private RelayCommand addModelRecommendation;
        public RelayCommand AddModelRecommendation
        {
            get { 
                return addModelRecommendation ??
                  (addModelRecommendation = new RelayCommand(obj =>
                  { AddComModelRecommendation(); })); }
        }

        private void AddComModelRecommendation()
        {
            if (loadbooltablRecom == false) MethodloadtablRecom();
            MethodaddcomRecom();
        }
        private void MethodaddcomRecom()
        {
            WindowMen.RecommendationTablGrid.SelectedItem = null;
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activModelRecommendation == false) BoolTrueRecom();
            else BoolFalseRecom();
        }

        private void MethodloadtablRecom()
        {
            WindowMen.LoadRecom.Visibility = Visibility.Hidden;
            CallServer.PostServer(controlerModelRecommendation, controlerModelRecommendation, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelRecommendation(CmdStroka);
            WindowMen.PoiskRecommendation.IsEnabled = true;
            WindowMen.PoiskRecommendation.Background = Brushes.AntiqueWhite;
        }

        private void BoolTrueRecom()
        {
            activModelRecommendation = true;
            WindowMen.Recommendationt2.IsEnabled = true;
            WindowMen.Recommendationt2.Background = Brushes.AntiqueWhite;
            WindowMen.RecommendationTablGrid.IsEnabled = false;
        }

        private void BoolFalseRecom()
        {
            WindowMen.Recommendationt2.IsEnabled = false;
            WindowMen.Recommendationt2.Background = Brushes.White;
            activModelRecommendation = false;
            WindowMen.RecommendationTablGrid.IsEnabled = true;
        }
        // команда удаления
        private RelayCommand? removeModelRecommendation;
        public RelayCommand RemoveModelRecommendation
        {
            get
            {
                return removeModelRecommendation ??
                  (removeModelRecommendation = new RelayCommand(obj =>
                  {
                      if (selectedRecommendation != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerModelRecommendation + selectedRecommendation.id.ToString();
                              CallServer.PostServer(MainWindow.UrlServer, json, "DELETE");
                              ModelRecommendations.Remove(selectedRecommendation);
                              selectedRecommendation = new ModelRecommendation();
                          }
                        BoolFalseRecom();
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ModelRecommendations != null));
            }
        }


        // команда  редактировать
        
       
        private RelayCommand? editModelRecommendation;
        public RelayCommand? EditModelRecommendation
        {
            get
            {
                return editModelRecommendation ??
                  (editModelRecommendation = new RelayCommand(obj =>
                  {
                      if (selectedRecommendation != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (activModelRecommendation == false)BoolTrueRecom();
                          else
                          {
                              BoolFalseRecom();
                              WindowMen.RecommendationTablGrid.SelectedItem = null;
                              SelectedModelRecommendation = new ModelRecommendation();
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveModelRecommendation;
        public RelayCommand SaveModelRecommendation
        {
            get
            {
                return saveModelRecommendation ??
                  (saveModelRecommendation = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if (WindowMen.Recommendationt2.Text.Trim().Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              SelectNewRecom();
                              json = JsonConvert.SerializeObject(selectedRecommendation);
                              CallServer.PostServer(controlerModelRecommendation, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelRecommendation Idinsert = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                              int CountIns = ModelRecommendations != null ? ModelRecommendations.Count : 0;
                              ModelRecommendations.Insert(CountIns, Idinsert);
                              SelectedModelRecommendation = Idinsert;
                              WindowMen.RecommendationTablGrid.ItemsSource = ModelRecommendations;
                          }
                          else 
                          {
                              json = JsonConvert.SerializeObject(selectedRecommendation);
                              CallServer.PostServer(controlerModelRecommendation, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                          }
                          UnloadCmdStroka("Recommendation/", json);
                      }
 
                      WindowMen.RecommendationTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseRecom();
                  }));
            }
        }


        private void SelectNewRecom()
        {
            int indexGroup = 0, setindex = 0;
            if (selectedRecommendation == null) selectedRecommendation = new ModelRecommendation();
            string kodgrup = "";
            if (ModelRecommendations != null)
            {
                kodgrup = ModelRecommendations[0].kodRecommendation;
                indexGroup = Convert.ToInt32(kodgrup.Substring(kodgrup.LastIndexOf(".") + 1, kodgrup.Length - (kodgrup.LastIndexOf(".")+1)));
                for (int i = 0; i < ModelRecommendations.Count; i++)
                {
                    setindex = Convert.ToInt32(ModelRecommendations[i].kodRecommendation.Substring(ModelRecommendations[i].kodRecommendation.LastIndexOf(".") + 1, ModelRecommendations[i].kodRecommendation.Length - (ModelRecommendations[i].kodRecommendation.LastIndexOf(".")+1)));
                    if (indexGroup < setindex) indexGroup = Convert.ToInt32(ModelRecommendations[i].kodRecommendation.Substring(ModelRecommendations[i].kodRecommendation.LastIndexOf(".") + 1, ModelRecommendations[i].kodRecommendation.Length - (ModelRecommendations[i].kodRecommendation.LastIndexOf(".")+1)));
                }
                indexGroup++;
                string _repl = "000000000";
                kodgrup = "REC." + _repl.Substring(0, _repl.Length - indexGroup.ToString().Length) + indexGroup.ToString();
            }
            else { kodgrup = "REC.000000001"; }
            selectedRecommendation.kodRecommendation = kodgrup;
            selectedRecommendation.contentRecommendation = WindowMen.Recommendationt2.Text;
        }


    

        // команда печати
        RelayCommand? printModelRecommendation;
        public RelayCommand PrintModelRecommendation
        {
            get
            {
                return printModelRecommendation ??
                  (printModelRecommendation = new RelayCommand(obj =>
                  {
                      if (ModelRecommendations != null)
                      {
                          MessageBox.Show("Колекція рекомендацій :" + ModelRecommendations[0].contentRecommendation.ToString());
                      }
                  },
                 (obj) => ModelRecommendations != null));
            }
        }

        

        // команда поиска наименования рекомендации
        RelayCommand? sarchRecommendation;
        public RelayCommand SearchRecommendation
        {
            get
            {
                return sarchRecommendation ??
                  (sarchRecommendation = new RelayCommand(obj =>
                  {
                      if (WindowInterv.PoiskRecommendation.Text.Trim() != "")
                      {
                          string jason = controlerModelRecommendation + "0/" + WindowInterv.PoiskRecommendation.Text;
                          CallServer.PostServer(controlerModelRecommendation, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableModelRecommendation(CmdStroka);
                      }
                  }));
            }
        }

    }
    #endregion
    #endregion
}