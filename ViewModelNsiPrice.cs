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
using System.Windows.Controls;
namespace BackSeam
{
    public class ViewModelNsiPrice : BaseViewModel
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        private WinNsiPrice WindowNsiPrice = MainWindow.LinkMainWindow("WinNsiPrice");
        public static string pathcontPrice = "/api/ControllerPrice/";
        private NsiPrice selectedNsiPrice;
        public static ObservableCollection<NsiPrice> VeiwNsiPrices { get; set; }

        public NsiPrice SelectedNsiPrice
        { get { return selectedNsiPrice; } set { selectedNsiPrice = value; OnPropertyChanged("SelectedNsiPrice"); } }
        // конструктор класса
        public ViewModelNsiPrice()
        {
            CallServer.PostServer(pathcontPrice, pathcontPrice, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewNsiPrice(CmdStroka);
        }

        public static void ObservableViewNsiPrice(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListNsiPrice>(CmdStroka);
            List<NsiPrice> res = result.NsiPrice.ToList();
            VeiwNsiPrices = new ObservableCollection<NsiPrice>((IEnumerable<NsiPrice>)res);
        }


        // команда закрытия окна
        RelayCommand? closeNsiPrice;
        public RelayCommand CloseNsiPrice
        {
            get
            {
                return closeNsiPrice ??
                  (closeNsiPrice = new RelayCommand(obj =>
                  {
                      WindowNsiPrice.Close();
                  }));
            }
        }

        // Выбор названия интервью диагностики 
        private RelayCommand? searchPrice;
        public RelayCommand SearchPrice
        {
            get
            {
                return searchPrice ??
                  (searchPrice = new RelayCommand(obj =>
                  {
                      MetodPriceEnter();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? selectedPrice;
        public RelayCommand SelectedPrice
        {
            get
            {
                return selectedPrice ??
                  (selectedPrice = new RelayCommand(obj =>
                  {
                      MainWindow Windowmain = MainWindow.LinkNameWindow("BackMain");
                      if (selectedNsiPrice != null)
                      {
                          MapOpisViewModel.selectedPrice.keyPrice = selectedNsiPrice.keyPrice.ToString();
                          MapOpisViewModel.selectedPrice.namePrice = selectedNsiPrice.namePrice.ToString();
                          MapOpisViewModel.selectedPrice.priceQuantity = selectedNsiPrice.priceQuantity;
                          MapOpisViewModel.selectedPrice.quantityDays = selectedNsiPrice.quantityDays;
                          WindowNsiPrice.Close();
                      }

                  }));
            }
        }

        RelayCommand? checkKeyText;
        public RelayCommand CheckKeyText
        {
            get
            {
                return checkKeyText ??
                  (checkKeyText = new RelayCommand(obj =>
                  {
                      MetodPriceEnter();
                  }));
            }
        }
        public void MetodPriceEnter()
        {

            if (WindowNsiPrice.PoiskPrice.Text.Trim() != "")
            {
                string jason = pathcontPrice + WindowNsiPrice.PoiskPrice.Text;
                CallServer.PostServer(pathcontPrice, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewNsiPrice(CmdStroka);
                WindowNsiPrice.TablPrice.ItemsSource = VeiwNsiPrices;
            }
        }
    }
 
 
}
