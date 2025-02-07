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
    public partial class ListModelIcd
    {

        [JsonProperty("list")]
        public ModelIcd[] ModelIcd { get; set; }

    }
    public class ModelIcd : BaseViewModel
    {

        // Международная классификация болезней 11 пересмотра МКБ11 

        private int Id;
        public string KeyIcd;
        private string Name;

        public ModelIcd(int Id = 0, string KeyIcd = "", string Name = "")
        {
            this.Id = Id;
            this.Name = Name;
            this.KeyIcd = KeyIcd;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("keyIcd")]
        public string keyIcd
        {
            get { return KeyIcd; }
            set { KeyIcd = value; OnPropertyChanged("keyIcd"); }
        }

        [JsonProperty("name")]
        public string name
        { get { return Name; } set { Name = value; OnPropertyChanged("name"); } }


    }


}
