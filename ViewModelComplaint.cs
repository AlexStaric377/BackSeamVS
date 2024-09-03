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
        // ViewComplaint   модель  Complaint
        // клавиша в окне: ""Жалобы""

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Жалобы"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>
        public static MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
        private bool editboolComplaint = false, addtboolComplaint = false, loadboolComplaint = false;
        public static string[] dictionty =  { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
            "AB","BC","CD","DE","EF","FG","GH","HI","IJ","JK","KL","LM","MN","NO","OP"
            ,"PQ","QR","RS","ST","TU","UV","VW","WX","XY","YZ","AC","AD","AE","AF","AG"
            ,"AH","AI","AJ","AK","AL","AM","AN","AO","AP","AR","AS","AT","AU","AV","AW","AX","AY","AZ"};
        public static string pathComplaint =  "/api/ApiControllerComplaint/";
        public static string IndexAddEdit = "";
        private bool activeditComplaint = false, addboolComplaint = false;
        private string editextComplaint = "";
        public static ModelComplaint selectedComplaint;
        public static ObservableCollection<ModelComplaint> ViewComplaints { get; set; }
        public static List<ModelComplaint> listviews = new List<ModelComplaint>();
        public ModelComplaint SelectedComplaint
        { get { return selectedComplaint; } set { selectedComplaint = value; OnPropertyChanged("SelectedComplaint"); } }
       // конструктор класса
        //public MapOpisViewModel()
        //{
        //    //ComplaintLoadData();                   
        //}

        private void ComplaintLoadData()
        {

            CallServer.PostServer(pathComplaint, pathComplaint, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else { ObservableViewComplaints(CmdStroka); }
        }

        public static void ObservableViewComplaints(string CmdStroka)
        {
            List<ModelComplaint> res = JsonConvert.DeserializeObject<ListModelComplaint>(CmdStroka).ViewComplaint.ToList();
            listviews = res;
            ViewComplaints = new ObservableCollection<ModelComplaint>((IEnumerable<ModelComplaint>)res);
            WindowMen.SimptomTablGrid.ItemsSource = ViewComplaints;
            WindowMen.LoadCompl.Visibility = Visibility.Hidden;
        }

        #region Команды вставки, удаления и редектирования справочника "Жалобы"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>
        // команда добавления нового объекта

        private RelayCommand loadComplaint;
        public RelayCommand LoadCompl
        {
            get
            {
                return loadComplaint ??
                  (loadComplaint = new RelayCommand(obj =>
                  {
                      ComplaintLoadData();
                      loadboolComplaint = true;
                  }));
            }
        }


        private RelayCommand addComplaint;
        public RelayCommand AddComplaint
        {
            get
            {
                return addComplaint ??
                  (addComplaint = new RelayCommand(obj =>
                  {
                      if (loadboolComplaint == false) ComplaintLoadData();
                      AddComand(); }));
            }
        }
        private void AddComand()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolComplaint == false) BoolTrueCompaint();
            else BoolFalseCompaint();
            WindowMen.SimptomTablGrid.SelectedItem = null;
        }

        private void BoolTrueCompaint()
        {
            activeditComplaint = true;
            addboolComplaint = true;
            WindowMen.Simptomt2.IsEnabled = true;
            WindowMen.Simptomt2.Background = Brushes.AntiqueWhite;
            WindowMen.FolderFuter.Visibility = Visibility.Visible;
            WindowMen.SimptomTablGrid.IsEnabled = false;
        }
        private void BoolFalseCompaint()
        {
            activeditComplaint = false;
            addboolComplaint = false;
            WindowMen.Simptomt2.IsEnabled = false;
            WindowMen.Simptomt2.Background = Brushes.White;
            WindowMen.FolderFuter.Visibility = Visibility.Hidden;
            WindowMen.SimptomTablGrid.IsEnabled = true;
        }

        // команда удаления
        private RelayCommand? removeComplaint;
        public RelayCommand RemoveComplaint
        {
            get
            {
                return removeComplaint ??
                  (removeComplaint = new RelayCommand(obj =>
                  {
                      if (selectedComplaint !=null)
                      {
                          if (selectedComplaint.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoRemoveZapis();
                              return;
                          }
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathComplaint + selectedComplaint.id.ToString();
                              CallServer.PostServer(pathComplaint, json, "DELETE");
                              ViewComplaints.Remove(selectedComplaint);
                              selectedComplaint = new ModelComplaint();
                          }
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewComplaints != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editComplaint;
        public RelayCommand? EditComplaint
        {
            get
            {
                return editComplaint ??
                  (editComplaint = new RelayCommand(obj =>
                  {
                      editextComplaint = "";
                      if (selectedComplaint != null)
                      {
                          if (selectedComplaint.idUser != RegIdUser && RegUserStatus != "1")
                          {
                              InfoEditZapis();
                              return;
                          }
                          IndexAddEdit = "editCommand";
                          if (activeditComplaint == false)
                          {
                              BoolTrueCompaint();
                              editextComplaint = WindowMen.Simptomt2.Text.ToString();
                          }
                          else
                          {
                              BoolFalseCompaint();
                              WindowMen.SimptomTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveComplaint;
        public RelayCommand SaveComplaint
        {
            get
            {
                return saveComplaint ??
                  (saveComplaint = new RelayCommand(obj =>
                  {
                      string json = "";
                      
                      if (WindowMen.Simptomt2.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              if (selectedComplaint == null) selectedComplaint = new ModelComplaint();
                              bool maxindex = false;
                              string addindex = ".000";
                              int indexdictionty = 0, Numbkey=0;
                              while (maxindex == false)
                              {
                                  selectedComplaint.keyComplaint = dictionty[indexdictionty] + addindex;
                                  json = pathComplaint + selectedComplaint.keyComplaint+"/0";
                                  CallServer.PostServer(pathComplaint, json, "GETID");
                                  if (CallServer.ResponseFromServer.Contains("[]") == true)
                                  {
                                      selectedComplaint.keyComplaint = dictionty[indexdictionty] + addindex;
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
                              selectedComplaint.name = WindowMen.Simptomt2.Text.ToString().Trim();
                              selectedComplaint.idUser = RegIdUser;
                              // ОБращение к серверу добавляем запись
                              json = JsonConvert.SerializeObject(selectedComplaint);
                              CallServer.PostServer(pathComplaint, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer;
                              ModelComplaint Idinsert = JsonConvert.DeserializeObject<ModelComplaint>(CallServer.ResponseFromServer);
                              int Countins = ViewComplaints != null ? ViewComplaints.Count : 0;
                              ViewComplaints.Insert(Countins, Idinsert);
                              
                          }
                          else
                          { 
                               // ОБращение к серверу измнить корректируемую запись в БД
                               json = JsonConvert.SerializeObject(selectedComplaint);
                               CallServer.PostServer(pathComplaint, json, "PUT");
                               CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                               json = CallServer.ResponseFromServer;
                          }
                          SelectedComplaint = new ModelComplaint();
                          UnloadCmdStroka("Complaint/", json);


                      }
                      else WindowMen.Simptomt2.Text = editextComplaint;
                      BoolFalseCompaint();
                      WindowMen.SimptomTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));
            }
        }

        // команда печати
        RelayCommand? printComplaint;
        public RelayCommand PrintComplaint
        {
            get
            {
                return printComplaint ??
                  (printComplaint = new RelayCommand(obj =>
                  {

                      if (ViewComplaints != null)
                      {
                          MessageBox.Show("Жалоби :" + ViewComplaints[0].name.ToString());
                      }
                  },
                 (obj) => ViewComplaints != null));
            }
        }

        
        // команда печати
        RelayCommand? visibleFeature;
        public RelayCommand VisibleFeature
        {
            get
            {
                return visibleFeature ??
                  (visibleFeature = new RelayCommand(obj =>
                  {
                      WindowMen.FolderFuter.Visibility = Visibility.Visible;
                  },
                 (obj) => ViewComplaints != null));
            }
        }
        // команда  просмотра характера жалобы

        RelayCommand? viewFeature;
        public RelayCommand ViewFeature
        {
            get
            {
                return viewFeature ??
                  (viewFeature = new RelayCommand(obj =>
                  {

                      if (selectedComplaint != null) 
                      {
                          ViewModelNsiFeature.Method = "GETID";
                          string jason = ViewModelNsiFeature.pathFeatureController + "0/" + selectedComplaint.keyComplaint;
                          CallServer.PostServer(ViewModelNsiFeature.pathFeatureController, jason, ViewModelNsiFeature.Method);
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]") == false)
                          {
                              ViewModelNsiFeature.jasonstoka = jason;
                              MapOpisViewModel.ActCompletedInterview = "Complaint";
                              MapOpisViewModel.selectedComplaintname = selectedComplaint.name;
                              WinNsiFeature NewOrder = new WinNsiFeature();
                              NewOrder.Left = (MainWindow.ScreenWidth / 2);
                              NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                              NewOrder.ShowDialog();
                              MapOpisViewModel.ActCompletedInterview = null;

                          } 
                      }

                  },
                 (obj) => ViewComplaints != null));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        #endregion
    }
}