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
    public class ViewModelColectionAnalizUrine : BaseViewModel
    {

        private static WinColectionAnalizUrine WinAnalizUrine = MainWindow.LinkMainWindow("WinColectionAnalizUrine");
        private bool loadboolAnalizUrine = false, addboolAnalizUrine = false, editboolAnalizUrine = false;
        public static string pathcontrollerAnalizUrine = "/api/PacientAnalizUrineController/";
        public string KodProtokola = "", IndexAddEdit = "";
        public static PacientAnalizUrine selectedPacientAnalizUrine;
        public static ObservableCollection<PacientAnalizUrine> ViewPacientAnalizUrines { get; set; }
        public PacientAnalizUrine SelectedPacientAnalizUrine
        { get { return selectedPacientAnalizUrine; } set { selectedPacientAnalizUrine = value; OnPropertyChanged("SelectedPacientAnalizUrine"); } }
        // конструктор класса

        public ViewModelColectionAnalizUrine()
        {
            LoadStanAnalizUrine();
            loadboolAnalizUrine = true;
        }

        public static void LoadStanAnalizUrine()
        {
            CallServer.PostServer(pathcontrollerAnalizUrine, pathcontrollerAnalizUrine + MapOpisViewModel._pacientProfil + "/0", "GETID"); // 
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionPacientAnalizUrine(CmdStroka);
        }

        public static void ObservablelColectionPacientAnalizUrine(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListPacientAnalizUrine>(CmdStroka);
            List<PacientAnalizUrine> res = result.PacientAnalizUrine.ToList();
            ViewPacientAnalizUrines = new ObservableCollection<PacientAnalizUrine>((IEnumerable<PacientAnalizUrine>)res);
        }


        // команда добавления данных
        private RelayCommand? addAnalizUrine;
        public RelayCommand AddAnalizUrine
        {
            get
            {
                return addAnalizUrine ??
                  (addAnalizUrine = new RelayCommand(obj =>
                  {
                      if (loadboolAnalizUrine == false) LoadStanAnalizUrine();
                      IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
                      if (addboolAnalizUrine == false) BoolTrueAnalizUrine();
                      else BoolFalseAnalizUrine();
                      WinAnalizUrine.ColectionAnalizUrineTablGrid.SelectedItem = null;
                  }));
            }
        }


        private void BoolTrueAnalizUrine()
        {
            addboolAnalizUrine = true;
            editboolAnalizUrine = true;
 
            WinAnalizUrine.DateAnaliz.IsReadOnly = false;
            WinAnalizUrine.DateAnaliz.Background = Brushes.AntiqueWhite;


            WinAnalizUrine.BoxCL.IsReadOnly = false;
            WinAnalizUrine.BoxCL.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxpH.IsReadOnly = false;
            WinAnalizUrine.BoxpH.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxSG.IsReadOnly = false;
            WinAnalizUrine.BoxSG.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxPRO.IsReadOnly = false;
            WinAnalizUrine.BoxPRO.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxGLU.IsReadOnly = false;
            WinAnalizUrine.BoxGLU.Background = Brushes.AntiqueWhite;

            WinAnalizUrine.BoxBIL.IsReadOnly = false;
            WinAnalizUrine.BoxBIL.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxURO.IsReadOnly = false;
            WinAnalizUrine.BoxURO.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxKET.IsReadOnly = false;
            WinAnalizUrine.BoxKET.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxBLD.IsReadOnly = false;
            WinAnalizUrine.BoxBLD.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxLEU.IsReadOnly = false;
            WinAnalizUrine.BoxLEU.Background = Brushes.AntiqueWhite;
            WinAnalizUrine.BoxNIT.IsReadOnly = false;
            WinAnalizUrine.BoxNIT.Background = Brushes.AntiqueWhite;

        }

        private void BoolFalseAnalizUrine()
        {
            addboolAnalizUrine = false;
            editboolAnalizUrine = false;
            WinAnalizUrine.DateAnaliz.IsReadOnly = true;
            WinAnalizUrine.DateAnaliz.Background = Brushes.Transparent;
            WinAnalizUrine.BoxCL.IsReadOnly = true;
            WinAnalizUrine.BoxCL.Background = Brushes.Transparent;
            WinAnalizUrine.BoxpH.IsReadOnly = true;
            WinAnalizUrine.BoxpH.Background = Brushes.Transparent;
            WinAnalizUrine.BoxSG.IsReadOnly = true;
            WinAnalizUrine.BoxSG.Background = Brushes.Transparent;
            WinAnalizUrine.BoxPRO.IsReadOnly = true;
            WinAnalizUrine.BoxPRO.Background = Brushes.Transparent;
            WinAnalizUrine.BoxGLU.IsReadOnly = true;
            WinAnalizUrine.BoxGLU.Background = Brushes.Transparent;
            WinAnalizUrine.BoxBLD.IsReadOnly = true;
            WinAnalizUrine.BoxBLD.Background = Brushes.Transparent;
            WinAnalizUrine.BoxBIL.IsReadOnly = true;
            WinAnalizUrine.BoxBIL.Background = Brushes.Transparent;
            WinAnalizUrine.BoxURO.IsReadOnly = true;
            WinAnalizUrine.BoxURO.Background = Brushes.Transparent;
            WinAnalizUrine.BoxKET.IsReadOnly = true;
            WinAnalizUrine.BoxKET.Background = Brushes.Transparent;
            WinAnalizUrine.BoxLEU.IsReadOnly = true;
            WinAnalizUrine.BoxLEU.Background = Brushes.Transparent;
            WinAnalizUrine.BoxNIT.IsReadOnly = true;
            WinAnalizUrine.BoxNIT.Background = Brushes.Transparent;
            IndexAddEdit = "";
        }



        // команда корректировки данных
        private RelayCommand? editAnalizUrine;
        public RelayCommand EditAnalizUrine
        {
            get
            {
                return editAnalizUrine ??
                  (editAnalizUrine = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (editboolAnalizUrine == false)
                      {
                          BoolTrueAnalizUrine();
                      }
                      else
                      {
                          BoolFalseAnalizUrine();
                          WinAnalizUrine.ColectionAnalizUrineTablGrid.SelectedItem = null;
                          IndexAddEdit = "";
                      }
                  }));
            }
        }


        // команда удаления данных
        private RelayCommand? deleteAnalizUrine;
        public RelayCommand DeleteAnalizUrine
        {
            get
            {
                return deleteAnalizUrine ??
                  (deleteAnalizUrine = new RelayCommand(obj =>
                  {
                      MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви дійсно бажаєте стерти облікові данні?";
                      MapOpisViewModel.SelectedRemove();
                      if (MapOpisViewModel.DeleteOnOff == true)
                      {
                          if (selectedPacientAnalizUrine.id != 0)
                          {
                              string json = pathcontrollerAnalizUrine + selectedPacientAnalizUrine.id.ToString();
                              CallServer.PostServer(pathcontrollerAnalizUrine, json, "DELETE");
                              ViewPacientAnalizUrines.Remove(selectedPacientAnalizUrine);
                              selectedPacientAnalizUrine = new PacientAnalizUrine();
                          }
                      }

                  }));
            }
        }


        // команда сохранения данных
        private RelayCommand? saveAnalizUrine;
        public RelayCommand SaveAnalizUrine
        {
            get
            {
                return saveAnalizUrine ??
                  (saveAnalizUrine = new RelayCommand(obj =>
                  {
                      if (WinAnalizUrine.BoxCL.Text.Length != 0 && WinAnalizUrine.BoxpH.Text.Length != 0
                      && WinAnalizUrine.BoxSG.Text.Length != 0 && WinAnalizUrine.DateAnaliz.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewPacientAnalizUrine();
                              string json = JsonConvert.SerializeObject(selectedPacientAnalizUrine);
                              CallServer.PostServer(pathcontrollerAnalizUrine, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              PacientAnalizUrine Idinsert = JsonConvert.DeserializeObject<PacientAnalizUrine>(CallServer.ResponseFromServer);
                              if (ViewPacientAnalizUrines == null)
                              {
                                  ViewPacientAnalizUrines = new ObservableCollection<PacientAnalizUrine>();
                                  ViewPacientAnalizUrines.Add(Idinsert);
                              }
                              else
                              { ViewPacientAnalizUrines.Insert(ViewPacientAnalizUrines.Count, Idinsert); }
                              SelectedPacientAnalizUrine = Idinsert;
                              LoadStanAnalizUrine();
                              WinAnalizUrine.ColectionAnalizUrineTablGrid.ItemsSource = ViewPacientAnalizUrines;
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedPacientAnalizUrine);
                              CallServer.PostServer(pathcontrollerAnalizUrine, json, "PUT");
                          }
                      }
                      BoolFalseAnalizUrine();
                      WinAnalizUrine.ColectionAnalizUrineTablGrid.SelectedItem = null;
                      IndexAddEdit = "";


                  }));
            }
        }

        public void SelectNewPacientAnalizUrine()
        {
            selectedPacientAnalizUrine = new PacientAnalizUrine();
            selectedPacientAnalizUrine.kodPacient = MapOpisViewModel._pacientProfil;
            selectedPacientAnalizUrine.dateAnaliza = WinAnalizUrine.DateAnaliz.Text.ToString();
            selectedPacientAnalizUrine.color = WinAnalizUrine.BoxCL.Text.ToString();
            selectedPacientAnalizUrine.ph = WinAnalizUrine.BoxpH.Text.ToString();
            selectedPacientAnalizUrine.sg = WinAnalizUrine.BoxSG.Text.ToString();
            selectedPacientAnalizUrine.pro = WinAnalizUrine.BoxPRO.Text.ToString();
            selectedPacientAnalizUrine.glu = WinAnalizUrine.BoxGLU.Text.ToString();
            selectedPacientAnalizUrine.bil = WinAnalizUrine.BoxBIL.Text.ToString();
            selectedPacientAnalizUrine.uro = WinAnalizUrine.BoxURO.Text.ToString();
            selectedPacientAnalizUrine.ket = WinAnalizUrine.BoxKET.Text.ToString();
            selectedPacientAnalizUrine.leu = WinAnalizUrine.BoxLEU.Text.ToString();
            selectedPacientAnalizUrine.nit = WinAnalizUrine.BoxNIT.Text.ToString();

        }

 
        private RelayCommand? onVisibleObjAnalizUrine;
        public RelayCommand OnVisibleObjAnalizUrine
        {
            get
            {
                return onVisibleObjAnalizUrine ??
                  (onVisibleObjAnalizUrine = new RelayCommand(obj =>
                  {

                      if (WinAnalizUrine.ColectionAnalizUrineTablGrid.SelectedIndex == -1) return;
                      if (ViewPacientAnalizUrines != null)
                      {

                          selectedPacientAnalizUrine = ViewPacientAnalizUrines[WinAnalizUrine.ColectionAnalizUrineTablGrid.SelectedIndex];
                          WinAnalizUrine.DateAnaliz.Text = selectedPacientAnalizUrine.dateAnaliza;
 
                      }
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? closeResult;
        public RelayCommand CloseAnalizUrine
        {
            get
            {
                return closeResult ??
                  (closeResult = new RelayCommand(obj =>
                  {
                      WinColectionAnalizUrine WindowCls = MainWindow.LinkMainWindow("WinColectionAnalizUrine");
                      WindowCls.Close();
                  }));
            }
        }
    }
}
