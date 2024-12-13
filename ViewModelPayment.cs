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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

namespace BackSeam
{
    // ViewModelPayment  Прайс стоимости услуг по Диференційна діагностика стану нездужання людини
    // клавиша в окне:  Адміністрування

    public partial class MapOpisViewModel : BaseViewModel
    {
        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка  из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowPayment = MainWindow.LinkNameWindow("BackMain");
        private bool editboolPayment = false, addboolPayment = false, loadboolPayment = false;
        public static string pathcontPayment = "/api/ControllerPayment/";
        public static ModelPayment selectedModelPayment;
        public static Payment selectedPayment;

        public static ObservableCollection<ModelPayment> ViewModelPayments { get; set; }
        public static ObservableCollection<Payment> ViewPayments { get; set; }

        public Payment SelectedPayment
        {
            get { return selectedPayment; }
            set { selectedPayment = value; OnPropertyChanged("SelectedPayment"); }
        }

        public ModelPayment SelectedModelPayment
        {
            get { return selectedModelPayment; }
            set { selectedModelPayment = value; OnPropertyChanged("SelectedModelPayment"); }
        }
        public static void ObservableViewPayments(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListPayment>(CmdStroka);
            List<Payment> res = result.Payment.ToList();
            ViewPayments = new ObservableCollection<Payment>((IEnumerable<Payment>)res);
            LoadViewModelPayments();
            WindowPayment.PaymentTablGrid.ItemsSource = ViewModelPayments;
        }

        private static void LoadViewModelPayments()
        {
            ViewModelPayments = new ObservableCollection<ModelPayment>();
            foreach (Payment payment in ViewPayments)
            {
                selectedModelPayment = new ModelPayment();
                selectedModelPayment.id = payment.id;
                selectedModelPayment.keyClient = payment.keyClient;
                selectedModelPayment.keyPrice = payment.keyPrice;
                selectedModelPayment.datePayment = payment.datePayment;
                selectedModelPayment.suma = payment.suma;
                selectedModelPayment.telefon = payment.telefon;
                SelectNameClient();
                SelectNamePrice();
                ViewModelPayments.Add(selectedModelPayment);
            }
        }
        private static void SelectNameClient()
        {
            string jason = pathcontPrice + selectedModelPayment.keyPrice;
            CallServer.PostServer(pathcontPrice, pathcontPrice, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            json = CallServer.ResponseFromServer;
            Price Idinsert = JsonConvert.DeserializeObject<Price>(CallServer.ResponseFromServer);
            selectedModelPayment.namePrice = Idinsert.namePrice;
            selectedModelPayment.priceQuantity = Idinsert.priceQuantity;
        }

        private static void SelectNamePrice()
        {
            string jason = pathcontrolerPacient + selectedModelPayment.keyClient;
            CallServer.PostServer(pathcontrolerPacient, pathcontrolerPacient, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            json = CallServer.ResponseFromServer;
            ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
            selectedModelPayment.nameClient = "Тел."+Idinsert.login + " " + Idinsert.name+" "+ Idinsert.surname;
            
        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadPayment;
        public RelayCommand LoadPayment
        {
            get
            {
                return loadPayment ??
                  (loadPayment = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadPayment();
                  }));
            }
        }

        // команда добавления нового объекта

        private RelayCommand? addPayment;
        public RelayCommand AddPayment
        {
            get
            {
                return addPayment ??
                  (addPayment = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComandPayment(); }));
            }
        }

        private void AddComandPayment()
        {

            NewModelPayment();
            if (loadboolPayment == false)
            {
                MethodLoadPayment();
                if (boolSetAccountUser == false) return;
            }
            MethodaddcomPayment();
        }

        private void MethodaddcomPayment()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolPayment == false) BoolTruePayment();
            else BoolFalsePayment();
            WindowPayment.PaymentTablGrid.SelectedItem = null;

        }

        private void MethodLoadPayment()
        {
            //RegStatusUser = "Адміністратор";
            WindowPayment.LoadPayment.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontPayment, pathcontPayment, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewPayments(CmdStroka);
            loadboolPayment = true;
        }


        private void BoolTruePayment()
        {
            addboolPayment = true;
            editboolPayment = true;
            WindowPayment.FoldSelectUser.Visibility = Visibility.Visible;
            WindowPayment.FoldSelectPaket.Visibility = Visibility.Visible;
            WindowPayment.PaymentDatePicker.IsEnabled = true;
            //WindowPayment.Paymentt4.IsEnabled = true;
            //WindowPayment.Paymentt4.Background = Brushes.AntiqueWhite;
            WindowPayment.Paymentt2.IsEnabled = true;
            WindowPayment.Paymentt2.Background = Brushes.AntiqueWhite;

        }

        private void BoolFalsePayment()
        {
            addboolPayment = false;
            editboolPayment = false;
            WindowPayment.FoldSelectUser.Visibility = Visibility.Hidden;
            WindowPayment.FoldSelectPaket.Visibility = Visibility.Hidden;
            WindowPayment.PaymentDatePicker.IsEnabled = false;
            //WindowPayment.Paymentt4.IsEnabled = false;
            //WindowPayment.Paymentt4.Background = Brushes.White;
            WindowPayment.Paymentt2.IsEnabled = false;
            WindowPayment.Paymentt2.Background = Brushes.White;

        }

        // команда удаления
        private RelayCommand? removePayment;
        public RelayCommand RemovePayment
        {
            get
            {
                return removePayment ??
                  (removePayment = new RelayCommand(obj =>
                  {
                      if (selectedPayment != null)
                      {
                          SelectedRemove();
                          // Видалення данных о гостях, пациентах, докторах, учетных записях
                          if (MapOpisViewModel.DeleteOnOff == true)
                          {
                              string json = pathcontPayment + selectedPayment.id.ToString();
                              CallServer.PostServer(pathcontPayment, json, "DELETE");
                              ViewPayments.Remove(selectedPayment);
                          }
                      }
                      BoolFalsePayment();
                  },
                 (obj) => ViewPayments != null));
            }
        }


        // команда  редактировать
        private RelayCommand? editPayment;
        public RelayCommand? EditPayment
        {
            get
            {
                return editPayment ??
                  (editPayment = new RelayCommand(obj =>
                  {
                      if (selectedPayment != null)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolPayment == false)
                          {
                              BoolTruePayment();
                          }
                          else
                          {
                              BoolFalsePayment();
                              WindowPayment.PaymentTablGrid.SelectedItem = null;
                              IndexAddEdit = "";
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? savePayment;
        public RelayCommand SavePayment
        {
            get
            {
                return savePayment ??
                  (savePayment = new RelayCommand(obj =>
                  {
                      BoolFalsePayment();
                      if (WindowPayment.Paymentt1.Text.Length != 0 && WindowPayment.Paymentt3.Text.Length != 0 && WindowPayment.Paymentt2.Text.Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              //  формирование кода Detailing по значениею группы выранного храктера жалобы
                              //SelectNewNsiPayment();
                              selectedPayment.datePayment = WindowPayment.Paymentt4.Text;
                              string json = JsonConvert.SerializeObject(selectedPayment);
                              CallServer.PostServer(pathcontPayment, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              Payment Idinsert = JsonConvert.DeserializeObject<Payment>(CallServer.ResponseFromServer);
                              if (ViewPayments == null)
                              {
                                  ViewPayments = new ObservableCollection<Payment>();
                                  ViewPayments.Add(Idinsert);
                              }
                              else
                              { ViewPayments.Insert(ViewPayments.Count, Idinsert); }
                              SelectedPayment = Idinsert;
                          }
                          else
                          {
                              string json = JsonConvert.SerializeObject(selectedPayment);
                              CallServer.PostServer(pathcontPayment, json, "PUT");
                          }
                          WindowPayment.PaymentTablGrid.ItemsSource = ViewPayments;
                      }
                      WindowPayment.PaymentTablGrid.SelectedItem = null;
                      IndexAddEdit = "";

                  }));

            }
        }


        // команда печати
        RelayCommand? printPayment;
        public RelayCommand PrintPayment
        {
            get
            {
                return printPayment ??
                  (printPayment = new RelayCommand(obj =>
                  {
                      WindowPayment.PaymentTablGrid.SelectedItem = null;
                      if (ViewModelPayments != null)
                      {
                          MessageBox.Show("Користувач :" + ViewModelPayments[0].nameClient.ToString());
                      }
                  },
                 (obj) => ViewModelPayments != null));
            }
        }


        // команда выбора пакета услуг
        RelayCommand? selectPaket;
        public RelayCommand SelectPaket
        {
            get
            {
                return selectPaket ??
                  (selectPaket = new RelayCommand(obj =>
                  {
                      NewModelPayment();
                      selectedPrice = new Price();
                      CallViewProfilLikar = "WinNsiPacient";
                      WinNsiPrice NewOrder = new WinNsiPrice();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      CallViewProfilLikar = "";

                      if (selectedPrice.keyPrice != "")
                      {
                          SelectedModelPayment.keyPrice = selectedPrice.keyPrice;
                          SelectedModelPayment.namePrice = selectedPrice.keyPrice + ": " + selectedPrice.namePrice;
                          SelectedModelPayment.priceQuantity = selectedPrice.priceQuantity;

                      }
                  }));

            }
        }

        // команда выбора пользователя оплатившего услугу
        RelayCommand? selectUser;
        public RelayCommand SelectUser
        {
            get
            {
                return selectUser ??
                  (selectUser = new RelayCommand(obj =>
                  {
                      NewModelPayment();
                      CallViewProfilLikar = "WinNsiPacient";
                      WinNsiPacient NewOrder = new WinNsiPacient();
                      NewOrder.ShowDialog();
                      CallViewProfilLikar = "";
                      string CmdStroka = CallServer.ServerReturn();
                      if (WindowPayment.LikarIntert3.Text != "")
                      {
                          SelectedModelPayment.keyClient = selectedModelPayment.keyClient = MapOpisViewModel.namePacient.Substring(0, MapOpisViewModel.namePacient.IndexOf(":")).Trim();
                          WindowPayment.Paymentt1.Text = SelectedModelPayment.nameClient = selectedModelPayment.nameClient = "Тел." + WindowPayment.AccountUsert2.Text + " " + WindowPayment.AccountUsert5.Text.Substring(WindowPayment.AccountUsert5.Text.IndexOf(":"), WindowPayment.AccountUsert5.Text.Length - WindowPayment.AccountUsert5.Text.IndexOf(":"));
                      }

                  }));

            }
        }

        private void NewModelPayment()
        {
            if (selectedPayment == null) selectedPayment = new Payment();
            if (SelectedPayment == null) SelectedPayment = new Payment();
            if (selectedModelPayment == null) selectedModelPayment = new ModelPayment();
            if (SelectedModelPayment == null) SelectedModelPayment = new ModelPayment();
        }

        #endregion

    }
    #endregion
}
