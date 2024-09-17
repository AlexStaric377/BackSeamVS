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
    public partial class MapOpisViewModel : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        /// 
        public static MainWindow WindowDefaultLanguageUI = MainWindow.LinkNameWindow("BackMain");
        public static string DefaultLanguage = "";


        #region Команды вставки, удаления и редектирования справочника "мова інтерфейсу"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadLanguageDefault;
        public RelayCommand LoadLanguageDefault
        {
            get
            {
                return loadLanguageDefault ??
                  (loadLanguageDefault = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadDefaultLanguageUI();
                  }));
            }
        }



        private void MethodLoadDefaultLanguageUI()
        {
            WindowDefaultLanguageUI.LoadLanguageUI.Visibility = Visibility.Hidden;

            CallServer.PostServer(pathLanguageUI, pathLanguageUI, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelLanguageUIs(CmdStroka);
            WindowDefaultLanguageUI.LanguageUIGrid.ItemsSource = ViewLanguageUIs;
            WindowDefaultLanguageUI.LanguageUIt1.Text = MainWindow.SelectLanguageUI;
        }

        // команда сохранить редактирование
        RelayCommand? saveDefaultLanguageUI;
        public RelayCommand SaveDefaultLanguageUI
        {
            get
            {
                return saveDefaultLanguageUI ??
                  (saveDefaultLanguageUI = new RelayCommand(obj =>
                  {

                      MainWindow.SelectLanguageUI = ViewLanguageUIs[WindowDefaultLanguageUI.LanguageUIGrid.SelectedIndex].name;
                      WindowDefaultLanguageUI.LanguageUIt1.Text = MainWindow.SelectLanguageUI;
                      WindowDefaultLanguageUI.LanguageUIGrid.SelectedItem = null;
                      WarningMessageLoadLanguageUI();
                      Environment.Exit(0);

                  }));
            }
            #endregion
        }
    }
}
