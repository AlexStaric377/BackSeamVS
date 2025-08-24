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

    // Колекция характеров жалоб 
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelMedical
    {

        [JsonProperty("list")]
        public MedicalInstitution[] MedicalInstitution { get; set; }

    }

    public class MedicalInstitution : BaseViewModel
    {
  

        private int Id;
        private string Edrpou;
        private string Name;
        private string PostIndex;
        private string Adres;
        private string Telefon;
        private string Email;
        private string UriwebZaklad;
        private string KodObl;
        private string IdStatus;
        private string KodZaklad;

        public MedicalInstitution(int Id = 0, string Edrpou = "", string Name = "", string PostIndex = "", 
            string Adres ="", string Telefon="", string Email="", string UriwebZaklad="", string KodObl = "", string IdStatus = "", string KodZaklad ="")
        {
            this.Id = Id;
            this.Edrpou = Edrpou;
            this.Name = Name;
            this.PostIndex = PostIndex;
            this.Adres = Adres;
            this.Telefon = Telefon;
            this.Email = Email;
            this.UriwebZaklad = UriwebZaklad;
            this.KodObl = KodObl;
            this.IdStatus = IdStatus;
            this.KodZaklad = KodZaklad;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("edrpou")]
        public string edrpou
        {
            get { return Edrpou; }
            set { Edrpou = value; OnPropertyChanged("edrpou"); }
        }

        [JsonProperty("name")]
        public string name
        { 
          get { return Name; } 
          set { Name = value; OnPropertyChanged("name"); } 
        }

        [JsonProperty("postIndex")]
        public string postIndex
        { 
            get { return PostIndex; }
            set { PostIndex = value; OnPropertyChanged("postIndex"); } 
        }

        [JsonProperty("adres")]
        public string adres
        {
            get { return Adres; }
            set { Adres = value; OnPropertyChanged("adres"); }
        }

        [JsonProperty("telefon")]
        public string telefon
        {
            get { return Telefon; }
            set { Telefon = value; OnPropertyChanged("telefon"); }
        }

        [JsonProperty("email")]
        public string email
        {
            get { return Email; }
            set { Email = value; OnPropertyChanged("email"); }
        }

        [JsonProperty("uriwebZaklad")]
        public string uriwebZaklad
        {
            get { return UriwebZaklad; }
            set { UriwebZaklad = value; OnPropertyChanged("uriwebZaklad"); }
        }
        [JsonProperty("kodobl")]
        public string kodobl
        {
            get { return KodObl; }
            set { KodObl = value; OnPropertyChanged("kodobl"); }
        }

        [JsonProperty("idstatus")]
        public string idstatus
        {
            get { return IdStatus; }
            set { IdStatus = value; OnPropertyChanged("idstatus"); }
        }
        [JsonProperty("kodZaklad")]
        public string kodZaklad
        {
            get { return KodZaklad; }
            set { KodZaklad = value; OnPropertyChanged("kodZaklad"); }
        }
    }

    public partial class ListStatusMedZaklad
    {

        [JsonProperty("list")]
        public StatusMedZaklad[] StatusMedZaklad { get; set; }

    }
    public class StatusMedZaklad : BaseViewModel
    {

        private int Id;
        private string IdStatus;
        private string NameStatus;
        private string TypeStatus;

        public StatusMedZaklad(int Id = 0, string IdStatus = "", string NameStatus = "", string TypeStatus = "")
        {
            this.Id = Id;
            this.IdStatus = IdStatus;
            this.NameStatus = NameStatus;
            this.TypeStatus = TypeStatus;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("idstatus")]
        public string idstatus
        {
            get { return IdStatus; }
            set { IdStatus = value; OnPropertyChanged("idstatus"); }
        }

        [JsonProperty("nameStatus")]
        public string nameStatus
        {
            get { return NameStatus; }
            set { NameStatus = value; OnPropertyChanged("nameStatus"); }
        }

        [JsonProperty("typeStatus")]
        public string typeStatus
        {
            get { return TypeStatus; }
            set { TypeStatus = value; OnPropertyChanged("typeStatus"); }
        }

    }

}
