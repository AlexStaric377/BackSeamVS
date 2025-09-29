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
    public partial class ListModelDoctor
    {
        [JsonProperty("list")]
        public ModelDoctor[] ModelDoctor { get; set; }
    }

    public partial class ModelDoctor : BaseViewModel
    {



        private int Id;
        private string KodDoctor;
        private string Name;
        private string Surname;
        private string Telefon;
        private string Email;
        private string Edrpou;
        private string Specialnoct;
        private string Napryamok;
        private string UriwebDoctor;
        private string Resume;

        public ModelDoctor(int Id = 0, string KodDoctor = "", string Name = "", string Surname = "",
             string Telefon = "", string Email = "", string Edrpou = "", string Specialnoct = "",
             string Napryamok = "", string UriwebDoctor = "", string Resume = "")

        {
            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.Name = Name;
            this.Surname = Surname;
            this.Telefon = Telefon;
            this.Email = Email;
            this.Edrpou = Edrpou;
            this.Specialnoct = Specialnoct;
            this.Napryamok = Napryamok;
            this.UriwebDoctor = UriwebDoctor;
            this.Resume = Resume;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodDoctor")]
        public string kodDoctor
        {
            get { return KodDoctor; }
            set { KodDoctor = value; OnPropertyChanged("kodDoctor"); }
        }
        [JsonProperty("name")]
        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }
        [JsonProperty("surname")]
        public string surname
        {
            get { return Surname; }
            set { Surname = value; OnPropertyChanged("surname"); }
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
        [JsonProperty("edrpou")]
        public string edrpou
        {
            get { return Edrpou; }
            set { Edrpou = value; OnPropertyChanged("edrpou"); }
        }
        [JsonProperty("specialnoct")]
        public string specialnoct
        {
            get { return Specialnoct; }
            set { Specialnoct = value; OnPropertyChanged("specialnoct"); }
        }
        [JsonProperty("napryamok")]
        public string napryamok
        {
            get { return Napryamok; }
            set { Napryamok = value; OnPropertyChanged("napryamok"); }
        }

        [JsonProperty("uriwebDoctor")]
        public string uriwebDoctor
        {
            get { return UriwebDoctor; }
            set { UriwebDoctor = value; OnPropertyChanged("uriwebDoctor"); }
        }

        [JsonProperty("resume")]
        public string resume
        {
            get { return Resume; }
            set { Resume = value; OnPropertyChanged("resume"); }
        }



    }
    public partial class ModelGridDoctor : BaseViewModel
    {
        private int Id;
        private string KodDoctor;
        private string Name;
        private string Surname;
        private string Telefon;
        private string Email;
        private string Edrpou;
        private string NameZaklad;
        private string AdrZaklad;
        private string Pind;
        private string Specialnoct;
        private string Napryamok;
        private string UriwebDoctor;
        private string Resume;

        public ModelGridDoctor(int Id = 0, string KodDoctor = "", string Name = "", string Surname = "", string NameZaklad = "",
            string AdrZaklad = "", string Pind = "", string Telefon = "", string Email = "", string Edrpou = "", string Specialnoct = "",
             string Napryamok = "", string UriwebDoctor = "", string Resume = "")

        {
            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.Name = Name;
            this.Surname = Surname;
            this.NameZaklad = NameZaklad;
            this.AdrZaklad = AdrZaklad;
            this.Pind = Pind;
            this.Telefon = Telefon;
            this.Email = Email;
            this.Edrpou = Edrpou;
            this.Specialnoct = Specialnoct;
            this.Napryamok = Napryamok;
            this.UriwebDoctor = UriwebDoctor;
            this.Resume = Resume;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        public string kodDoctor
        {
            get { return KodDoctor; }
            set { KodDoctor = value; OnPropertyChanged("kodDoctor"); }
        }
        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }
        public string surname
        {
            get { return Surname; }
            set { Surname = value; OnPropertyChanged("surname"); }
        }
        public string nameZaklad
        {
            get { return NameZaklad; }
            set { NameZaklad = value; OnPropertyChanged("nameZaklad"); }
        }
        public string adrZaklad
        {
            get { return AdrZaklad; }
            set { AdrZaklad = value; OnPropertyChanged("adrZaklad"); }
        }
        public string pind
        {
            get { return Pind; }
            set { Pind = value; OnPropertyChanged("pind"); }
        }
        public string telefon
        {
            get { return Telefon; }
            set { Telefon = value; OnPropertyChanged("telefon"); }
        }
        public string email
        {
            get { return Email; }
            set { Email = value; OnPropertyChanged("email"); }
        }
        public string edrpou
        {
            get { return Edrpou; }
            set { Edrpou = value; OnPropertyChanged("edrpou"); }
        }

        public string specialnoct
        {
            get { return Specialnoct; }
            set { Specialnoct = value; OnPropertyChanged("specialnoct"); }
        }

        public string napryamok
        {
            get { return Napryamok; }
            set { Napryamok = value; OnPropertyChanged("napryamok"); }
        }

        public string uriwebDoctor
        {
            get { return UriwebDoctor; }
            set { UriwebDoctor = value; OnPropertyChanged("uriwebDoctor"); }
        }

        [JsonProperty("resume")]
        public string resume
        {
            get { return Resume; }
            set { Resume = value; OnPropertyChanged("resume"); }
        }


        // команда закрытия окна
        RelayCommand? checkLikarTel;
        public RelayCommand CheckLikarTel
        {
            get
            {
                return checkLikarTel ??
                  (checkLikarTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Doctort6, 12);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilLikarTel;
        public RelayCommand CheckProfilLikarTel
        {
            get
            {
                return checkProfilLikarTel ??
                  (checkProfilLikarTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Likart6, 12);
                  }));
            }
        }
        // команда закрытия окна
        RelayCommand? checkLikarPind;
        public RelayCommand CheckLikarPind
        {
            get
            {
                return checkLikarPind ??
                  (checkLikarPind = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Doctort5, 5);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilLikarPind;
        public RelayCommand CheckProfilLikarPind
        {
            get
            {
                return checkProfilLikarPind ??
                  (checkProfilLikarPind = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Likart5, 5);
                  }));
            }
        }


    }

}
