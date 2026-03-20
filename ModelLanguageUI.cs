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
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;


namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ListModelLanguageUI
    {

        [JsonProperty("list")]
        public ModelLanguageUI[] ModelLanguageUI { get; set; }

    }
    public class ModelLanguageUI : BaseViewModel
    {

        public int Id;
        public string KeyLang;
        public string Name;

        public ModelLanguageUI(int Id = 0, string KeyLang = "", string Name = "")
        {
            this.Id = Id;
            this.KeyLang = KeyLang;
            this.Name = Name;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyLang")]
        public string keyLang
        {
            get { return KeyLang; }
            set { KeyLang = value; OnPropertyChanged("keyLang"); }
        }


        [JsonProperty("name")]
        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }
    }
}
