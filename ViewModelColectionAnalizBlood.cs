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
    public class ViewModelColectionAnalizBlood : BaseViewModel
    {

        private static WinColectionAnalizBlood WinAnalizBlood = MainWindow.LinkMainWindow("WinColectionAnalizBlood");
        private bool loadboolAnalizBlood = false, addboolAnalizBlood = false, editboolAnalizBlood =false;
        public static string pathcontrollerAnalizBlood = "/api/PacientAnalizKroviController/";
        public string KodProtokola = "", IndexAddEdit="";
        public static PacientAnalizKrovi selectedPacientAnalizKrovi;
        public static ObservableCollection<PacientAnalizKrovi> ViewPacientAnalizKrovis { get; set; }
        public PacientAnalizKrovi SelectedPacientAnalizKrovi
        { get { return selectedPacientAnalizKrovi; } set { selectedPacientAnalizKrovi = value; OnPropertyChanged("SelectedPacientAnalizKrovi"); } }
        // конструктор класса

        public ViewModelColectionAnalizBlood()
        {
            LoadStanAnalizBlood();
            loadboolAnalizBlood = true;
        }

        public static void LoadStanAnalizBlood()
        { 
            CallServer.PostServer(pathcontrollerAnalizBlood, pathcontrollerAnalizBlood + MapOpisViewModel._pacientProfil + "/0", "GETID"); // 
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionPacientAnalizKrovi(CmdStroka);
        }

        public static void ObservablelColectionPacientAnalizKrovi(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListPacientAnalizKrovi>(CmdStroka);
            List<PacientAnalizKrovi> res = result.PacientAnalizKrovi.ToList();
            ViewPacientAnalizKrovis = new ObservableCollection<PacientAnalizKrovi>((IEnumerable<PacientAnalizKrovi>)res);
        }


        // команда добавления данных
        private RelayCommand? addAnalizBlood;
        public RelayCommand AddAnalizBlood
        {
            get
            {
                return addAnalizBlood ??
                  (addAnalizBlood = new RelayCommand(obj =>
                  {
                    if (loadboolAnalizBlood == false) LoadStanAnalizBlood();
                      IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
                        if (addboolAnalizBlood == false) BoolTrueAnalizBlood();
                        else BoolFalseAnalizBlood();
                        WinAnalizBlood.ColectionAnalizBloodTablGrid.SelectedItem = null;
                  }));
            }
        }

 
        private void BoolTrueAnalizBlood()
        {
            addboolAnalizBlood = true;
            editboolAnalizBlood = true;
            if (IndexAddEdit == "addCommand")
            { 
                WinAnalizBlood.DateAnaliz.IsReadOnly = false;
                WinAnalizBlood.DateAnaliz.Background = Brushes.AntiqueWhite;           
            }

            WinAnalizBlood.BoxNamerbc.IsReadOnly = false;
            WinAnalizBlood.BoxNamerbc.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.BoxNamehgb.IsReadOnly = false;
            WinAnalizBlood.BoxNamehgb.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.Boxwbc.IsReadOnly = false;
            WinAnalizBlood.Boxwbc.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.BoxNamecp.IsReadOnly = false;
            WinAnalizBlood.BoxNamecp.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.Texthct.IsReadOnly = false;
            WinAnalizBlood.Texthct.Background = Brushes.AntiqueWhite;

            WinAnalizBlood.TextRet.IsReadOnly = false;
            WinAnalizBlood.TextRet.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.Textplt.IsReadOnly = false;
            WinAnalizBlood.Textplt.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.Textceo.IsReadOnly = false;
            WinAnalizBlood.Textceo.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextBAS.IsReadOnly = false;
            WinAnalizBlood.TextBAS.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextEO.IsReadOnly = false;
            WinAnalizBlood.TextEO.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextNEUTp.IsReadOnly = false;
            WinAnalizBlood.TextNEUTp.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextNEUTs.IsReadOnly = false;
            WinAnalizBlood.TextNEUTs.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextLYM.IsReadOnly = false;
            WinAnalizBlood.TextLYM.Background = Brushes.AntiqueWhite;
            WinAnalizBlood.TextMON.IsReadOnly = false;
            WinAnalizBlood.TextMON.Background = Brushes.AntiqueWhite;

    }

    private void BoolFalseAnalizBlood()
    {
            addboolAnalizBlood = false;
            editboolAnalizBlood = false;
            WinAnalizBlood.DateAnaliz.IsReadOnly = true;
            WinAnalizBlood.DateAnaliz.Background = Brushes.Transparent;
            WinAnalizBlood.BoxNamerbc.IsReadOnly = true;
            WinAnalizBlood.BoxNamerbc.Background = Brushes.Transparent;
            WinAnalizBlood.BoxNamehgb.IsReadOnly = true;
            WinAnalizBlood.BoxNamehgb.Background = Brushes.Transparent;
            WinAnalizBlood.Boxwbc.IsReadOnly = true;
            WinAnalizBlood.Boxwbc.Background = Brushes.Transparent;
            WinAnalizBlood.BoxNamecp.IsReadOnly = true;
            WinAnalizBlood.BoxNamecp.Background = Brushes.Transparent;
            WinAnalizBlood.Texthct.IsReadOnly = true;
            WinAnalizBlood.Texthct.Background = Brushes.Transparent;

            WinAnalizBlood.TextRet.IsReadOnly = true;
            WinAnalizBlood.TextRet.Background = Brushes.Transparent;
            WinAnalizBlood.Textplt.IsReadOnly = true;
            WinAnalizBlood.Textplt.Background = Brushes.Transparent;
            WinAnalizBlood.Textceo.IsReadOnly = true;
            WinAnalizBlood.Textceo.Background = Brushes.Transparent;
            WinAnalizBlood.TextBAS.IsReadOnly = true;
            WinAnalizBlood.TextBAS.Background = Brushes.Transparent;
            WinAnalizBlood.TextEO.IsReadOnly = true;
            WinAnalizBlood.TextEO.Background = Brushes.Transparent;
            WinAnalizBlood.TextNEUTp.IsReadOnly = true;
            WinAnalizBlood.TextNEUTp.Background = Brushes.Transparent;
            WinAnalizBlood.TextNEUTs.IsReadOnly = true;
            WinAnalizBlood.TextNEUTs.Background = Brushes.Transparent;
            WinAnalizBlood.TextLYM.IsReadOnly = true;
            WinAnalizBlood.TextLYM.Background = Brushes.Transparent;
            WinAnalizBlood.TextMON.IsReadOnly = true;
            WinAnalizBlood.TextMON.Background = Brushes.Transparent;
            IndexAddEdit = "";
        }



    // команда корректировки данных
    private RelayCommand? editAnalizBlood;
        public RelayCommand EditAnalizBlood
        {
            get
            {
                return editAnalizBlood ??
                  (editAnalizBlood = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (editboolAnalizBlood == false)
                      {
                          BoolTrueAnalizBlood();
                      }
                      else
                      {
                          BoolFalseAnalizBlood();
                          WinAnalizBlood.ColectionAnalizBloodTablGrid.SelectedItem = null;
                          IndexAddEdit = "";
                      }
                  }));
            }
        }


        // команда удаления данных
        private RelayCommand? deleteAnalizBlood;
        public RelayCommand DeleteAnalizBlood
        {
            get
            {
                return deleteAnalizBlood ??
                  (deleteAnalizBlood = new RelayCommand(obj =>
                  {
                      MainWindow.MessageError = "Увага!" + Environment.NewLine +"Ви дійсно бажаєте стерти облікові данні?";
                      MapOpisViewModel.SelectedRemove();
                      if (MapOpisViewModel.DeleteOnOff == true)
                      { 
                          if (selectedPacientAnalizKrovi.id != 0)
                          {
                              string json = pathcontrollerAnalizBlood + selectedPacientAnalizKrovi.id.ToString();
                              CallServer.PostServer(pathcontrollerAnalizBlood, json, "DELETE");
                              ViewPacientAnalizKrovis.Remove(selectedPacientAnalizKrovi);
                              selectedPacientAnalizKrovi = new  PacientAnalizKrovi();
                          }                    
                      }
 
                  }));
            }
        }


        // команда сохранения данных
        private RelayCommand? saveAnalizBlood;
        public RelayCommand SaveAnalizBlood
        {
            get
            {
                return saveAnalizBlood ??
                  (saveAnalizBlood = new RelayCommand(obj =>
                  {
                      if (WinAnalizBlood.BoxNamerbc.Text.Length != 0 && WinAnalizBlood.BoxNamehgb.Text.Length != 0
                      && WinAnalizBlood.TextLYM.Text.Length != 0 && WinAnalizBlood.Boxwbc.Text.Length != 0 && WinAnalizBlood.DateAnaliz.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewPacientAnalizBlood();
                              string json = JsonConvert.SerializeObject(selectedPacientAnalizKrovi);
                              CallServer.PostServer(pathcontrollerAnalizBlood, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              PacientAnalizKrovi Idinsert = JsonConvert.DeserializeObject<PacientAnalizKrovi>(CallServer.ResponseFromServer);
                              if (ViewPacientAnalizKrovis == null)
                              {
                                  ViewPacientAnalizKrovis = new ObservableCollection<PacientAnalizKrovi>();
                                  ViewPacientAnalizKrovis.Add(Idinsert);
                              }
                              else
                              { ViewPacientAnalizKrovis.Insert(ViewPacientAnalizKrovis.Count, Idinsert); }
                              SelectedPacientAnalizKrovi = Idinsert;
                              LoadStanAnalizBlood();
                              WinAnalizBlood.ColectionAnalizBloodTablGrid.ItemsSource = ViewPacientAnalizKrovis;
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedPacientAnalizKrovi);
                              CallServer.PostServer(pathcontrollerAnalizBlood, json, "PUT");
                          }
                      }
                      BoolFalseAnalizBlood();
                      WinAnalizBlood.ColectionAnalizBloodTablGrid.SelectedItem = null;
                      IndexAddEdit = "";


                  }));
            }
        }

        public void SelectNewPacientAnalizBlood()
        {
            selectedPacientAnalizKrovi = new  PacientAnalizKrovi();
            selectedPacientAnalizKrovi.kodPacient = MapOpisViewModel._pacientProfil;
            selectedPacientAnalizKrovi.dateAnaliza = WinAnalizBlood.DateAnaliz.Text.ToString();
            selectedPacientAnalizKrovi.gender = MapOpisViewModel._pacientGender;
            selectedPacientAnalizKrovi.rbc = WinAnalizBlood.BoxNamerbc.Text.ToString();
            selectedPacientAnalizKrovi.hgb = WinAnalizBlood.BoxNamehgb.Text.ToString();
            selectedPacientAnalizKrovi.wbc= WinAnalizBlood.Boxwbc.Text.ToString();
            selectedPacientAnalizKrovi.cp = WinAnalizBlood.BoxNamecp.Text.ToString();
            selectedPacientAnalizKrovi.hct = WinAnalizBlood.Texthct.Text.ToString();
            selectedPacientAnalizKrovi.ret = WinAnalizBlood.TextRet.Text.ToString();
            selectedPacientAnalizKrovi.plt = WinAnalizBlood.Textplt.Text.ToString();
            selectedPacientAnalizKrovi.esr = WinAnalizBlood.Textceo.Text.ToString();
            selectedPacientAnalizKrovi.bas = WinAnalizBlood.TextBAS.Text.ToString();
            selectedPacientAnalizKrovi.eo= WinAnalizBlood.TextEO.Text.ToString();
            selectedPacientAnalizKrovi.neutp = WinAnalizBlood.TextNEUTp.Text.ToString();
            selectedPacientAnalizKrovi.neuts = WinAnalizBlood.TextNEUTs.Text.ToString();
            selectedPacientAnalizKrovi.lym = WinAnalizBlood.TextLYM.Text.ToString();
            selectedPacientAnalizKrovi.mon = WinAnalizBlood.TextMON.Text.ToString();

        }

        private RelayCommand? onVisibleObjAnalizBlood;
        public RelayCommand OnVisibleObjAnalizBlood
        {
            get
            {
                return onVisibleObjAnalizBlood ??
                  (onVisibleObjAnalizBlood = new RelayCommand(obj =>
                  {

                      if (WinAnalizBlood.ColectionAnalizBloodTablGrid.SelectedIndex == -1) return;
                      if (ViewPacientAnalizKrovis != null)
                      {
                          
                          selectedPacientAnalizKrovi = ViewPacientAnalizKrovis[WinAnalizBlood.ColectionAnalizBloodTablGrid.SelectedIndex];
                          WinAnalizBlood.DateAnaliz.Text= selectedPacientAnalizKrovi.dateAnaliza;
                      }
                  }));
            }
        }
        
        // команда закрытия окна
        RelayCommand? closeResult;
        public RelayCommand CloseAnalizBlood
        {
            get
            {
                return closeResult ??
                  (closeResult = new RelayCommand(obj =>
                  {
                      WinColectionAnalizBlood WindowCls = MainWindow.LinkMainWindow("WinColectionAnalizBlood");
                      WindowCls.Close();
                  }));
            }
        }
    }
}
