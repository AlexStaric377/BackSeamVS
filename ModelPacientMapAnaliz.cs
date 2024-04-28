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
    public partial class ListPacientMapAnaliz
    {
        [JsonProperty("list")]
        public PacientMapAnaliz[] PacientMapAnaliz { get; set; }
    }

    public partial class PacientMapAnaliz
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
        private string Pulse;
        private string Pressure;
        private string Temperature;
        private string ResultAnaliza;

        public PacientMapAnaliz(int Id = 0, string KodPacient = "", string DateAnaliza = "", string Pulse = "",
            string Pressure = "", string Temperature = "", string ResultAnaliza = "")
        {

            this.Id = Id;
            this.KodPacient = KodPacient;
            this.DateAnaliza = DateAnaliza;
            this.Pulse = Pulse;
            this.Pressure = Pressure;
            this.Temperature = Temperature;
            this.ResultAnaliza = ResultAnaliza;
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
        [JsonProperty("pulse")]
        public string pulse
        {
            get { return Pulse; }
            set { Pulse = value; OnPropertyChanged("pulse"); }
        }
        [JsonProperty("pressure")]
        public string pressure
        {
            get { return Pressure; }
            set { Pressure = value; OnPropertyChanged("pressure"); }
        } // давление
        [JsonProperty("temperature")]
        public string temperature
        {
            get { return Temperature; }
            set { Temperature = value; OnPropertyChanged("temperature"); }
        }  // температура
        [JsonProperty("resultAnaliza")]
        public string resultAnaliza
        {
            get { return ResultAnaliza; }
            set { ResultAnaliza = value; OnPropertyChanged("resultAnaliza"); }
        }
    
        

    }
}
