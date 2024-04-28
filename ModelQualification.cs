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
    // Уточнение класификация детализации данного характера или особености жалобы
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

    public partial class ListModelQualification
    {
        [JsonProperty("list")]
        public ModelQualification[] ViewQualification { get; set; }
    }

    public class ModelQualification : BaseViewModel
    {
        private int Id;
        private string KodGroupQualification;
        private string NameQualification;
        private string KodQualification;
        private string IdUser;
        public ModelQualification(int Id = 0, string KodQualification="", string KodGroupQualification = "", string NameQualification = "", string IdUser = "")
        {
            this.Id = Id;
            this.KodQualification = KodQualification;
            this.KodGroupQualification = KodGroupQualification;
            this.NameQualification = NameQualification;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodQualification")]
        public string kodQualification
        {
            get { return KodQualification; }
            set { KodQualification = value; OnPropertyChanged("kodQualification"); }
        }

        [JsonProperty("kodGroupQualification")]
        public string kodGroupQualification
        {
            get { return KodGroupQualification; }
            set { KodGroupQualification = value; OnPropertyChanged("kodGroupQualification"); }
        }
        [JsonProperty("nameQualification")]
        public string nameQualification
        {
            get { return NameQualification; }
            set { NameQualification = value; OnPropertyChanged("nameQualification"); }
        }
        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }
    }
}
