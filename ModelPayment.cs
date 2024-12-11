using System;
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

namespace BackSeam
{

    // статистика оплат вртості пакетів послуг з Диференційної діагностики стану нездужання людини
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com


    public class ModelPayment : BaseViewModel
    {
        public int Id { get; set; }
        public string KeyClient { get; set; } // код пациента или доктора
        public string NameClient { get; set; }
        public string DatePayment { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        public decimal Suma { get; set; }
        public string KeyPrice { get; set; }
        public string NamePrice { get; set; }
        public decimal PriceQuantity { get; set; }
        
        public string Telefon { get; set; }


        public ModelPayment(int Id = 0, string KeyClient = "", string NameClient="", string DatePayment = "", decimal Suma = 0m, string KeyPrice = "", string NamePrice = "", decimal PriceQuantity =0m, string Telefon = "")

        {
            this.Id = Id;
            this.KeyClient = KeyClient;
            this.NameClient = NameClient;
            this.DatePayment = DatePayment;
            this.Suma = Suma;
            this.KeyPrice = KeyPrice;
            this.NamePrice = NamePrice;
            this.PriceQuantity = PriceQuantity;
            this.Telefon = Telefon;
        }

        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        public string keyClient
        {
            get { return KeyClient; }
            set { KeyClient = value; OnPropertyChanged("keyClient"); }
        }

        public string nameClient
        {
            get { return NameClient; }
            set { NameClient = value; OnPropertyChanged("nameClient"); }
        }
        public string datePayment
        {
            get { return DatePayment; }
            set { DatePayment = value; OnPropertyChanged("datePayment"); }
        }
        public decimal suma
        {
            get { return Suma; }
            set { Suma = value; OnPropertyChanged("suma"); }
        }
        public string keyPrice
        {
            get { return KeyPrice; }
            set { KeyPrice = value; OnPropertyChanged("keyPrice"); }
        }

        public string namePrice
        {
            get { return NamePrice; }
            set { NamePrice = value; OnPropertyChanged("namePrice"); }
        }
        public decimal priceQuantity
        {
            get { return PriceQuantity; }
            set { PriceQuantity = value; OnPropertyChanged("priceQuantity"); }
        }
        public string telefon
        {
            get { return Telefon; }
            set { Telefon = value; OnPropertyChanged("telefon"); }
        }
    }

    public partial class ListPayment
    {

        [JsonProperty("list")]
        public Payment[] Payment { get; set; }

    }
    public class Payment
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("keyClient")]
        public string keyClient { get; set; }

        [JsonProperty("datePayment")]
        public string datePayment { get; set; }

        [JsonProperty("suma")]
        public decimal suma { get; set; }

        [JsonProperty("keyPrice")]
        public string keyPrice { get; set; }

        [JsonProperty("telefon")]
        public string telefon { get; set; }

    }


    // Типы и коды доступа пользователей
    public partial class ListNsiPayment
    {

        [JsonProperty("list")]
        public NsiPayment[] NsiPayment { get; set; }

    }
    public class NsiPayment
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("keyClient")]
        public string keyClient { get; set; }

        [JsonProperty("datePayment")]
        public string datePayment { get; set; }

        [JsonProperty("suma")]
        public decimal suma { get; set; }

        [JsonProperty("keyPrice")]
        public string keyPrice { get; set; }

        [JsonProperty("telefon")]
        public string telefon { get; set; }

    }
}
