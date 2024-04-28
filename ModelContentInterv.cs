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
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelContentInterv
    {

        [JsonProperty("list")]
        public ModelContentInterv[] ModelContentInterv { get; set; }

    }

    public partial class ModelContentInterv : BaseViewModel
    {
        private int Id;
        private string KodProtokola;
        private string DetailsInterview;
        private string KodDetailing;
        private int Numberstr;
        private string IdUser;

        public ModelContentInterv(int Id = 0, int Numberstr=0, string KodProtokola = "", string KodDetailing="", string DetailsInterview = "", string NametInterview = "", string IdUser = "")
        {
            this.Id = Id;
            this.KodProtokola = KodProtokola;
            this.KodDetailing = KodDetailing;
            this.DetailsInterview = DetailsInterview;
            this.Numberstr = Numberstr;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        [JsonProperty("numberstr")]
        public int numberstr
        {
            get { return Numberstr; }
            set { Numberstr = value; OnPropertyChanged("numberstr"); }
        }

        [JsonProperty("kodDetailing")]
        public string kodDetailing
        {
            get { return KodDetailing; }
            set { KodDetailing = value; OnPropertyChanged("kodDetailing"); }
        }

        [JsonProperty("detailsInterview")]
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
        }
        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }
}
