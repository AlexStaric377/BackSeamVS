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
    // Вартість пакетів послуг з Диференційної діагностики стану нездужання людини
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com


    public class ModelPrice : BaseViewModel
    {
        private int Id { get; set; }
        private string KeyPrice { get; set; }
        private int QuantityDays { get; set; }
        [Column(TypeName = "decimal(14,2)")]
        private decimal PriceQuantity { get; set; }
        private string NamePrice { get; set; }


        public ModelPrice(int Id = 0, string KeyPrice = "", int QuantityDays = 0,  decimal PriceQuantity = 0m, string NamePrice="")
            
        {
            this.Id = Id;
            this.KeyPrice = KeyPrice;
            this.QuantityDays = QuantityDays;
            this.PriceQuantity = PriceQuantity;
            this.NamePrice = NamePrice;
        }

        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        public string keyPrice
        {
            get { return KeyPrice; }
            set { KeyPrice = value; OnPropertyChanged("keyPrice"); }
        }
        public int quantityDays
        {
            get { return QuantityDays; }
            set { QuantityDays = value; OnPropertyChanged("quantityDays"); }
        }
        public decimal priceQuantity
        {
            get { return PriceQuantity; }
            set { PriceQuantity = value; OnPropertyChanged("priceQuantity"); }
        }
        public string namePrice
        {
            get { return NamePrice; }
            set { NamePrice = value; OnPropertyChanged("namePrice"); }
        }
    }

    public partial class ListPrice
    {

        [JsonProperty("list")]
        public Price[] Price { get; set; }

    }
    public class Price
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("keyPrice")]
        public string keyPrice { get; set; }

        [JsonProperty("quantityDays")]
        public int quantityDays { get; set; }

        [JsonProperty("priceQuantity")]
        public decimal priceQuantity { get; set; }

        [JsonProperty("namePrice")]
        public string namePrice { get; set; }

    }


    // Типы и коды доступа пользователей
    public partial class ListNsiPrice
    {

        [JsonProperty("list")]
        public NsiPrice[] NsiPrice { get; set; }

    }
    public class NsiPrice
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("keyPrice")]
        public string keyPrice { get; set; }

        [JsonProperty("quantityDays")]
        public int quantityDays { get; set; }

        [JsonProperty("priceQuantity")]
        public decimal priceQuantity { get; set; }

        [JsonProperty("namePrice")]
        public string namePrice { get; set; }

    }

}

