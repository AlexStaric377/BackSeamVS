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

        // ViewModelPrice  Прайс стоимости услуг по Диференційна діагностика стану нездужання людини
        // клавиша в окне:  Адміністрування


        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка  из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowPrice = MainWindow.LinkNameWindow("BackMain");
        private bool editboolPrice = false, addboolPrice = false, loadboolPrice = false;
        private string edittextPrice = "";
        public static string pathcontPrice = "/api/ControllerPrice/";
        public static ModelPrice selectedModelPrice;
        public static Price selectedPrice;

        public static ObservableCollection<ModelPrice> ViewModelPrices { get; set; }
        public static ObservableCollection<Price> ViewPrices { get; set; }

        public Price SelectedPrice
        {
            get { return selectedPrice; }
            set { selectedPrice = value; OnPropertyChanged("SelectedPrice"); }
        }
        public static void ObservableViewPrices(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListPrice>(CmdStroka);
            List<Price> res = result.Price.ToList();
            ViewPrices = new ObservableCollection<Price>((IEnumerable<Price>)res);
            WindowPrice.PriceTablGrid.ItemsSource = ViewPrices;
        }


        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadPrice;
        public RelayCommand LoadPrice
        {
            get
            {
                return loadPrice ??
                  (loadPrice = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadPrice();
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addPrice;
        public RelayCommand AddPrice
        {
            get
            {
                return addPrice ??
                  (addPrice = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandPrice(); }));
            }
        }

        private void AddComandPrice()
        {
            selectedPrice = new Price();
            SelectedPrice = selectedPrice;
            if (loadboolPrice == false)
            {
                MethodLoadPrice();
                if (boolSetAccountUser == false) return;
            }
            MethodaddcomPrice();
        }

        private void MethodaddcomPrice()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolPrice == false) BoolTruePrice();
            else BoolFalsePrice();
            WindowPrice.PriceTablGrid.SelectedItem = null;

        }

        private void MethodLoadPrice()
        {
            //RegStatusUser = "Адміністратор";
            WindowPrice.LoadPrice.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontPrice, pathcontPrice, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) { ViewPrices = new ObservableCollection<Price>(); CallServer.BoolFalseTabl(); }
            else ObservableViewPrices(CmdStroka);
            loadboolPrice = true;
        }


        private void BoolTruePrice()
        {
            addboolPrice = true;
            editboolPrice = true;
            WindowPrice.Pricet1.IsEnabled = true;
            WindowPrice.Pricet1.Background = Brushes.AntiqueWhite;
            WindowPrice.Pricet4.IsEnabled = true;
            WindowPrice.Pricet4.Background = Brushes.AntiqueWhite;
            WindowPrice.Pricet2.IsEnabled = true;
            WindowPrice.Pricet2.Background = Brushes.AntiqueWhite;
 
        }

        private void BoolFalsePrice()
        {
            addboolPrice = false;
            editboolPrice = false;
            WindowPrice.Pricet1.IsEnabled = false;
            WindowPrice.Pricet1.Background = Brushes.White;
            WindowPrice.Pricet4.IsEnabled = false;
            WindowPrice.Pricet4.Background = Brushes.White;
            WindowPrice.Pricet2.IsEnabled = false;
            WindowPrice.Pricet2.Background = Brushes.White;

        }

        // команда удаления
        private RelayCommand? removePrice;
        public RelayCommand RemovePrice
        {
            get
            {
                return removePrice ??
                  (removePrice = new RelayCommand(obj =>
                  {
                      if (selectedPrice != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathcontPrice + selectedPrice.id.ToString();
                              CallServer.PostServer(pathcontPrice, json, "DELETE");
                              ViewPrices.Remove(selectedPrice);
                          }
                      }
                      BoolFalsePrice();
                  },
                 (obj) => ViewPrices != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editPrice;
        public RelayCommand? EditPrice
        {
            get
            {
                return editPrice ??
                  (editPrice = new RelayCommand(obj =>
                  {
                      if (selectedPrice != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolPrice == false)
                          {
                              BoolTruePrice();
                          }
                          else
                          {
                              BoolFalsePrice();
                              WindowPrice.PriceTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? savePrice;
        public RelayCommand SavePrice
        {
            get
            {
                return savePrice ??
                  (savePrice = new RelayCommand(obj =>
                  {
                      BoolFalsePrice();
                      if (WindowPrice.Pricet1.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              SelectNewNsiPrice();
                              string json = JsonConvert.SerializeObject(selectedPrice);
                              CallServer.PostServer(pathcontPrice, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              Price Idinsert = JsonConvert.DeserializeObject<Price>(CallServer.ResponseFromServer);
                              if (ViewPrices == null)
                              {
                                  ViewPrices = new ObservableCollection<Price>();
                                  ViewPrices.Add(Idinsert);
                              }
                              else
                              { ViewPrices.Insert(ViewPrices.Count, Idinsert); }
                              SelectedPrice = Idinsert;
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedPrice);
                              CallServer.PostServer(pathcontPrice, json, "PUT");
                          }
                          WindowPrice.PriceTablGrid.ItemsSource = ViewPrices;
                      }
                      WindowPrice.PriceTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));

            }
        }

        public void SelectNewNsiPrice()
        {

            if (selectedPrice == null) selectedPrice = new Price();
            if (ViewPrices != null)
            {
                int _keyPriceindex = 0, setindex = 0;
                _keyPriceindex = Convert.ToInt32(ViewPrices[0].keyPrice);
                for (int i = 0; i < ViewPrices.Count; i++)
                {
                    setindex = Convert.ToInt32(ViewPrices[i].keyPrice);
                    if (_keyPriceindex < setindex) _keyPriceindex = setindex;
                }
                _keyPriceindex++;
                selectedPrice.keyPrice = _keyPriceindex.ToString();
            }
            else { selectedPrice.keyPrice = "1"; }

            selectedPrice.namePrice = WindowDetailing.Pricet1.Text.ToString();
            //selectedPrice.keyPrice = WindowDetailing.Pricet3.Text.ToString();
            selectedPrice.quantityDays = Convert.ToInt32(WindowDetailing.Pricet4.Text);
            selectedPrice.priceQuantity = Convert.ToDecimal(WindowDetailing.Pricet2.Text);

        }

        // команда печати
        RelayCommand? printPrice;
        public RelayCommand PrintPrice
        {
            get
            {
                return printPrice ??
                  (printPrice = new RelayCommand(obj =>
                  {
                      WindowPrice.PriceTablGrid.SelectedItem = null;
                      if (ViewPrices != null)
                      {
                          MessageBox.Show("Обліковий запис :" + ViewPrices[0].namePrice.ToString());
                      }
                  },
                 (obj) => ViewPrices != null));
            }
        }


        #endregion
        #endregion
    }
}
