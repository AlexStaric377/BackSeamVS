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
        public static bool activVeiwModelIcd = false, loadbooltablIcd = false;
        public static string controlerIcd =  "/api/IcdController/";
        public static ModelIcd selectedIcd;

        public static ObservableCollection<ModelIcd> VeiwModelIcds { get; set; }

        public ModelIcd SelectedModelIcd
        { get { return selectedIcd; } set { selectedIcd = value; OnPropertyChanged("SelectedModelIcd"); } }

        public static void ObservableVeiwModelIcd(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
            List<ModelIcd> res = result.ModelIcd.ToList();
            VeiwModelIcds = new ObservableCollection<ModelIcd>((IEnumerable<ModelIcd>)res);
            WindowMen.IcdTablGrid.ItemsSource = VeiwModelIcds;
            loadbooltablIcd = true;
            MetodSelectMkx();
        }

        private static void MetodSelectMkx()
        {
            CallServer.PostServer(ViewModelMedicalGrDiagnoz.controlerGrDiagnoz, ViewModelMedicalGrDiagnoz.controlerGrDiagnoz, "GET");
            if (CallServer.ResponseFromServer.Contains("[]") == true) return;
            CmdStroka = CallServer.ServerReturn();
            var GrDiagnoz = JsonConvert.DeserializeObject<ListModelMedGrupDiagnoz>(CmdStroka);
            List<ModelMedGrupDiagnoz> grupDiagnoz = GrDiagnoz.ModelMedGrupDiagnoz.ToList();
            ViewModelNsiMedZaklad.GrupMedZaklads = new ObservableCollection<ModelMedGrupDiagnoz>((IEnumerable<ModelMedGrupDiagnoz>)grupDiagnoz);
 

            foreach (ModelMedGrupDiagnoz modelGrupDiagnoz in ViewModelNsiMedZaklad.GrupMedZaklads)
            {
                if (modelGrupDiagnoz.icdKey == null)
                {
                    string jason = controlerIcd + "0/" + modelGrupDiagnoz.icdGrDiagnoz;
                    CallServer.PostServer(controlerIcd, jason, "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]") == false)
                    {
                        var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
                        List<ModelIcd> res = result.ModelIcd.ToList();
                        VeiwModelIcds = new ObservableCollection<ModelIcd>((IEnumerable<ModelIcd>)res);
                        modelGrupDiagnoz.icdKey = VeiwModelIcds[0].keyIcd;
                        json = JsonConvert.SerializeObject(modelGrupDiagnoz);
                        CallServer.PostServer(ViewModelMedicalGrDiagnoz.controlerGrDiagnoz, json, "PUT");
                    }
                }

            }
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadVeiwModelIcd;
        public RelayCommand LoadVeiwModelIcd
        {
            get
            {
                return loadVeiwModelIcd ??
                  (loadVeiwModelIcd = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodloadtabIcd();
                  }));
            }
        }


        // команда добавления нового объекта
 
        private RelayCommand addVeiwModelIcd;
        public RelayCommand AddVeiwModelIcd
        {
            get
            {
                return addVeiwModelIcd ??
                  (addVeiwModelIcd = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComVeiwModelIcd(); }));
            }
        }

        private void AddComVeiwModelIcd()
        {
            if (loadbooltablIcd == false) MethodloadtabIcd();
            MethodaddcomIcd();
        }

        private void MethodaddcomIcd()
        {
            WindowMen.IcdTablGrid.SelectedItem = null;
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (activVeiwModelIcd == false) ModelIcdtrue();
            else ModelIcdfalse();
        }


        private void MethodloadtabIcd()
        {
            WindowMen.Loadmkx.Visibility = Visibility.Hidden;
            CallServer.PostServer(controlerIcd, controlerIcd, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else  ObservableVeiwModelIcd(CmdStroka);
        }

        // команда удаления
        private RelayCommand? removeVeiwModelIcd;
        public RelayCommand RemoveVeiwModelIcd
        {
            get
            {
                return removeVeiwModelIcd ??
                  (removeVeiwModelIcd = new RelayCommand(obj =>
                  {
                      if (selectedIcd !=null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = controlerIcd + selectedIcd.id.ToString();
                            CallServer.PostServer(controlerIcd, json, "DELETE");
                            VeiwModelIcds.Remove(selectedIcd);
                            selectedIcd = new ModelIcd();
                          }
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => VeiwModelIcds != null));
            }
        }


        // команда  редактировать
        private bool activeditVeiwModelIcd = false;
        private RelayCommand? editVeiwModelIcd;
        public RelayCommand? EditVeiwModelIcd
        {
            get
            {
                return editVeiwModelIcd ??
                  (editVeiwModelIcd = new RelayCommand(obj =>
                  {
                      if (selectedIcd != null && selectedIcd.id != 0)
                      {
                          IndexAddEdit = "editCommand";
                          if (activeditVeiwModelIcd == false)
                          {
                              ModelIcdtrue();
                          }
                          else
                          {
                              ModelIcdfalse();
                              WindowMen.IcdTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveVeiwModelIcd;
        public RelayCommand SaveVeiwModelIcd
        {
            get
            {
                return saveVeiwModelIcd ??
                  (saveVeiwModelIcd = new RelayCommand(obj =>
                  {
                      
                      if (WindowMen.Icdt2.Text.Trim().Length != 0 & WindowMen.Icdt3.Text.Trim().Length != 0)
                      {
                          if (selectedIcd == null)
                          {
                              selectedIcd = new ModelIcd();
                              selectedIcd.id = 0;
                              selectedIcd.keyIcd = WindowMen.Icdt2.Text.ToString();
                              selectedIcd.name = WindowMen.Icdt3.Text.ToString();
                          }
                          var json = JsonConvert.SerializeObject(selectedIcd);
                          string Method = IndexAddEdit == "addCommand" ? "POST" : "PUT";
                          CallServer.PostServer(controlerIcd, json, Method);
                          CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                          json = CallServer.ResponseFromServer;

                          UnloadCmdStroka("Icd/", json);
                          MethodloadtabIcd();

                      }
                      ModelIcdfalse();
                      IndexAddEdit = "";
                      SelectedModelIcd = new ModelIcd();
                      WindowMen.IcdTablGrid.SelectedItem = null;
                  }));
            }
        }

        private void ModelIcdfalse()
        {
            activVeiwModelIcd = false;
            activeditVeiwModelIcd = false;
            WindowMen.Icdt2.IsEnabled = false;
            WindowMen.Icdt2.Background = Brushes.White;
            WindowMen.Icdt3.IsEnabled = false;
            WindowMen.Icdt3.Background = Brushes.White;
            WindowMen.FolderGrNapryamok.Visibility = Visibility.Hidden;
            WindowMen.IcdTablGrid.IsEnabled = true;
            WindowMen.BorderLoadIcd.IsEnabled = true;
            WindowMen.BorderGhangeIcd.IsEnabled = true;
            WindowMen.BorderDeleteIcd.IsEnabled = true;
            WindowMen.BorderPrintIcd.IsEnabled = true;
            WindowMen.BorderAddIcd.IsEnabled = true;
        }

        private void ModelIcdtrue()
        {
            activVeiwModelIcd = true;
            activeditVeiwModelIcd = true;
            WindowMen.Icdt2.IsEnabled = true;
            WindowMen.Icdt2.Background = Brushes.AntiqueWhite;
            WindowMen.Icdt3.IsEnabled = true;
            WindowMen.Icdt3.Background = Brushes.AntiqueWhite;
            WindowMen.FolderGrNapryamok.Visibility = Visibility.Visible;
            WindowMen.IcdTablGrid.IsEnabled = false;

            if (IndexAddEdit == "addCommand")
            {
                WindowMen.BorderLoadIcd.IsEnabled = false;
                WindowMen.BorderGhangeIcd.IsEnabled = false;
                WindowMen.BorderDeleteIcd.IsEnabled = false;
                WindowMen.BorderPrintIcd.IsEnabled = false;
            }
            if (IndexAddEdit == "editCommand")
            {
                WindowMen.BorderLoadIcd.IsEnabled = false;
                WindowMen.BorderAddIcd.IsEnabled = false;
                WindowMen.BorderDeleteIcd.IsEnabled = false;
                WindowMen.BorderPrintIcd.IsEnabled = false;
            }

        }
        // команда печати
        RelayCommand? printVeiwModelIcd;
        public RelayCommand PrintVeiwModelIcd
        {
            get
            {
                return printVeiwModelIcd ??
                  (printVeiwModelIcd = new RelayCommand(obj =>
                  {
                      if (VeiwModelIcds != null)
                      {
                          MessageBox.Show("Міжнародний класифікатор МКХ11 :" + VeiwModelIcds[0].name.ToString());
                      }
                  },
                 (obj) => VeiwModelIcds != null));
            }
        }



        
        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? selectedDiagnozMKX;
        public RelayCommand SelectedDiagnozMKX
        {
            get
            {
                return selectedDiagnozMKX ??
                  (selectedDiagnozMKX = new RelayCommand(obj =>
                  { ComandFindNameDiagnozMKX(); }));
            }
        }

        private void ComandFindNameDiagnozMKX()
        {
            MapOpisViewModel.GrupDiagnoz = "";
            WinNsiIcd NewOrder = new WinNsiIcd();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-50;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (WindowMen.Diagnozt4.Text.Length != 0)
            {
                string tmpkod = WindowMen.Diagnozt4.Text.Substring(0, WindowMen.Diagnozt4.Text.Length - 1);
                string tmpkod1 = tmpkod.Substring(tmpkod.LastIndexOf(".") + 1, tmpkod.Length - tmpkod.LastIndexOf(".") - 1);
                if (tmpkod.Length - tmpkod1.Length >= 3)
                {
                    WindowMen.Icdt2.Text = WindowMen.Diagnozt4.Text + "01.";
                }
                else
                { 
                    int number = Convert.ToInt32(tmpkod.Substring(tmpkod.LastIndexOf(".")+1, tmpkod.Length- tmpkod.LastIndexOf(".")-1))+1;
                    string addtext = number>=10? Convert.ToString(number):"0"+ Convert.ToString(number) + ".";
                    WindowMen.Icdt2.Text = tmpkod.Substring(0, tmpkod.LastIndexOf(".")+1)+ addtext;
                }
                
            }

        }

        
        private RelayCommand? searchIcd;
        public RelayCommand SearchIcd
        {
            get
            {
                return searchIcd ??
                  (searchIcd = new RelayCommand(obj =>
                  {
                      MetodSearchIcd();
                  }));
            }
        }

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyIcd;
        public RelayCommand CheckKeyIcd
        {
            get
            {
                return checkKeyIcd ??
                  (checkKeyIcd = new RelayCommand(obj =>
                  {
                      MetodSearchIcd();
                  }));
            }
        }


        private void MetodSearchIcd()
        {
            if (CheckStatusUser() == false) return;
            if (WindowMen.PoiskIcd.Text.Trim() != "")
            {
                string jason = controlerIcd  + WindowInterv.PoiskIcd.Text+ "/0";
                CallServer.PostServer(controlerIcd, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]"))
                {
                    jason = controlerIcd  + "0/"+ WindowInterv.PoiskIcd.Text;
                    CallServer.PostServer(controlerIcd, jason, "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]"))CallServer.BoolFalseTabl();
                    else ObservableVeiwModelIcd(CmdStroka);
                }    
                else ObservableVeiwModelIcd(CmdStroka);
            }
        }
        #endregion
        #endregion

    }
}
