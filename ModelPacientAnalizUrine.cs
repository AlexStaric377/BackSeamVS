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
    public partial class ListPacientAnalizUrine
    {
        [JsonProperty("list")]
        public PacientAnalizUrine[] PacientAnalizUrine { get; set; }
    }
    public partial class PacientAnalizUrine
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
        private string Color;   // цвет
        private string Ph; // кислотность
        private string Sg; // плотность
        private string Pro;   //  белок
        private string Glu; // глюкоза
        private string Bil; // билирубин
        private string Uro;   // уробилиноген
        private string Ket; // кетоновые тела
        private string Bld; // эритроциты
        private string Leu;  // лейкоциты
        private string Nit; // соли


        public PacientAnalizUrine(int Id = 0, string KodPacient = "", string DateAnaliza = "", string Color = "",
           string Ph = "", string Sg = "", string Pro = "", string Glu = "", string Bil = "", string Uro = "", string Ket = "",
           string Bld = "", string Leu = "", string Nit = "")
        {
            this.Id = Id;
            this.KodPacient = KodPacient;
            this.DateAnaliza = DateAnaliza;

            this.Color = Color;   // цвет
            this.Ph = Ph; // кислотность
            this.Sg = Sg; // плотность
            this.Pro = Pro;   //  белок
            this.Glu = Glu; // глюкоза
            this.Bil = Bil; // билирубин
            this.Uro = Uro;   // уробилиноген
            this.Ket = Ket; // кетоновые тела
            this.Bld = Bld; // эритроциты
            this.Leu = Leu;  // лейкоциты
            this.Nit = Nit; // соли 
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
        [JsonProperty("color")]
        public string color
        {
            get { return Color; }
            set { Color = value; OnPropertyChanged("color"); }
        }
        [JsonProperty("ph")]
        public string ph
        {
            get { return Ph; }
            set { Ph = value; OnPropertyChanged("ph"); }
        } // давление
        [JsonProperty("sg")]
        public string sg
        {
            get { return Sg; }
            set { Sg = value; OnPropertyChanged("sg"); }
        }  // температура
        [JsonProperty("pro")]
        public string pro
        {
            get { return Pro; }
            set { Pro = value; OnPropertyChanged("pro"); }
        }

        [JsonProperty("glu")]
        public string glu
        {
            get { return Glu; }
            set { Glu = value; OnPropertyChanged("glu"); }
        }
        [JsonProperty("bil")]
        public string bil
        {
            get { return Bil; }
            set { Bil = value; OnPropertyChanged("bil"); }
        }
        [JsonProperty("uro")]
        public string uro
        {
            get { return Uro; }
            set { Uro = value; OnPropertyChanged("uro"); }
        }
        [JsonProperty("ket")]
        public string ket
        {
            get { return Ket; }
            set { Ket = value; OnPropertyChanged("ket"); }
        } // давление
        [JsonProperty("bld")]
        public string bld
        {
            get { return Bld; }
            set { Bld = value; OnPropertyChanged("bld"); }
        }  // температура
        [JsonProperty("leu")]
        public string leu
        {
            get { return Leu; }
            set { Leu = value; OnPropertyChanged("leu"); }
        }

        [JsonProperty("nit")]
        public string nit
        {
            get { return Nit; }
            set { Nit = value; OnPropertyChanged("nit"); }
        }

 
    }
}
