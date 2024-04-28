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
    public partial class ListPacientAnalizKrovi
    {
        [JsonProperty("list")]
        public PacientAnalizKrovi[] PacientAnalizKrovi { get; set; }
    }
    public partial class PacientAnalizKrovi
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private int Id;
        private string KodPacient;
        private string DateAnaliza;
        private string Gender;
        private string Rbc;   // Эритроциты
        private string Hgb; // Гемоглобин
        private string Wbc; // Лейкоциты
        private string Cp;   // Цветовой показатель
        private string Hct; // Гематокрит
        private string Ret; // Ретикулоциты
        private string Plt;   // Тромбоциты
        private string Esr; // СОЭ
        private string Bas; // Базофилы
        private string Eo;  // Эозинофилы
        private string Mot; // Миелоциты
        private string Mtmot; // Метамиелоциты
        private string Neutp;   // Нейтрофилы палочкоядерные
        private string Neuts; // Нейтрофилы сегментоядерные
        private string Lym; // Лимфоциты
        private string Mon;  // Моноциты


        public PacientAnalizKrovi(int Id=0, string KodPacient="", string DateAnaliza="", string Gender="",
           string Rbc="", string Hgb="", string Wbc="", string Cp="", string Hct="", string Ret="", string Plt="",
           string Esr="", string Bas="", string Eo="", string Mot="", string Mtmot="", string Neutp="",
           string Neuts="", string Lym="", string Mon="")
        {
            this.Id = Id;
            this.KodPacient = KodPacient;
            this.DateAnaliza = DateAnaliza;
            this.Gender = Gender;
            this.Rbc = Rbc;   // Эритроциты
            this.Hgb = Hgb; // Гемоглобин
            this.Wbc = Wbc; // Лейкоциты
            this.Cp = Cp;   // Цветовой показатель
            this.Hct = Hct; // Гематокрит
            this.Ret = Ret; // Ретикулоциты
            this.Plt = Plt;   // Тромбоциты
            this.Esr = Esr; // СОЭ
            this.Bas = Bas; // Базофилы
            this.Eo = Eo;  // Эозинофилы
            this.Mot = Mot; // Миелоциты
            this.Mtmot = Mtmot; // Метамиелоциты
            this.Neutp = Neutp;   // Нейтрофилы палочкоядерные
            this.Neuts = Neuts; // Нейтрофилы сегментоядерные
            this.Lym = Lym; // Лимфоциты
            this.Mon = Mon;  // Моноциты        
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodPacient")]
        public string kodPacient
        {
            get { return KodPacient; }
            set { KodPacient = value; OnPropertyChanged("kodPacient"); }
        }
        [JsonProperty("dateAnaliza")]
        public string dateAnaliza
        {
            get { return DateAnaliza; }
            set { DateAnaliza = value; OnPropertyChanged("dateAnaliza"); }
        }
        [JsonProperty("gender")]
        public string gender
        {
            get { return Gender; }
            set { Gender = value; OnPropertyChanged("gender"); }
        }
        [JsonProperty("rbc")]
        public string rbc
        {
            get { return Rbc; }
            set { Rbc = value; OnPropertyChanged("rbc"); }
        } // давление
        [JsonProperty("hgb")]
        public string hgb
        {
            get { return Hgb; }
            set { Hgb = value; OnPropertyChanged("hgb"); }
        }  // температура
        [JsonProperty("wbc")]
        public string wbc
        {
            get { return Wbc; }
            set { Wbc = value; OnPropertyChanged("wbc"); }
        }
        
        [JsonProperty("cp")]
        public string cp
        {
            get { return Cp; }
            set { Cp = value; OnPropertyChanged("cp"); }
        }
        [JsonProperty("hct")]
        public string hct
        {
            get { return Hct; }
            set { Hct = value; OnPropertyChanged("hct"); }
        }
        [JsonProperty("ret")]
        public string ret
        {
            get { return Ret; }
            set { Ret = value; OnPropertyChanged("ret"); }
        }
        [JsonProperty("plt")]
        public string plt
        {
            get { return Plt; }
            set { Plt = value; OnPropertyChanged("plt"); }
        } // давление
        [JsonProperty("esr")]
        public string esr
        {
            get { return Esr; }
            set { Esr = value; OnPropertyChanged("esr"); }
        }  // температура
        [JsonProperty("bas")]
        public string bas
        {
            get { return Bas; }
            set { Bas = value; OnPropertyChanged("bas"); }
        }

        [JsonProperty("eo")]
        public string eo
        {
            get { return Eo; }
            set { Eo = value; OnPropertyChanged("eo"); }
        }

        [JsonProperty("mot")]
        public string mot
        {
            get { return Mot; }
            set { Mot = value; OnPropertyChanged("mot"); }
        }
        [JsonProperty("mtmot")]
        public string mtmot
        {
            get { return Mtmot; }
            set { Mtmot = value; OnPropertyChanged("mtmot"); }
        }
        [JsonProperty("neutp")]
        public string neutp
        {
            get { return Neutp; }
            set { Neutp = value; OnPropertyChanged("neutp"); }
        }
        [JsonProperty("neuts")]
        public string neuts
        {
            get { return Neuts; }
            set { Neuts = value; OnPropertyChanged("neuts"); }
        } // давление
        [JsonProperty("lym")]
        public string lym
        {
            get { return Lym; }
            set { Lym = value; OnPropertyChanged("lym"); }
        }  // температура
        [JsonProperty("mon")]
        public string mon
        {
            get { return Mon; }
            set { Mon = value; OnPropertyChanged("mon"); }
        }
    }
}
