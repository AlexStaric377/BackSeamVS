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
    // Детализация особености жалобы
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelRegistrationAppointment
    {

        [JsonProperty("list")]
        public ModelRegistrationAppointment[] ModelRegistrationAppointment { get; set; }

    }


    // список приемов пациентов записавшихся на прием средством СЕАМ 
    public class ModelRegistrationAppointment : INotifyPropertyChanged
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
        private string KodDoctor;
        private string KodPacient;
        private string DateInterview;
        private string DateDoctor;
        private string KodProtokola;
        private string TopictVizita;
        private string KodComplInterv;


        public ModelRegistrationAppointment(int Id = 0, string KodDoctor = "", string KodPacient = "", string DateInterview = "", string DateDoctor = "",
           string KodProtokola = "", string TopictVizita = "", string KodComplInterv = "")
        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.KodPacient = KodPacient;
            this.DateDoctor = DateDoctor;
            this.DateInterview = DateInterview;
            this.KodProtokola = KodProtokola;
            this.TopictVizita = TopictVizita;
            this.KodComplInterv = KodComplInterv;

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

        [JsonProperty("kodPacient")]
        public string kodPacient
        {
            get { return KodPacient; }
            set { KodPacient = value; OnPropertyChanged("kodPacient"); }
        }

        [JsonProperty("dateInterview")]
        public string dateInterview
        {
            get { return DateInterview; }
            set { DateInterview = value; OnPropertyChanged("dateInterview"); }
        }

        [JsonProperty("dateDoctor")]
        public string dateDoctor
        {
            get { return DateDoctor; }
            set { DateDoctor = value; OnPropertyChanged("dateDoctor"); }
        }

        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }


        [JsonProperty("kodComplInterv")]
        public string kodComplInterv
        {
            get { return KodComplInterv; }
            set { KodComplInterv = value; OnPropertyChanged("kodComplInterv"); }
        }

        [JsonProperty("topictVizita")]
        public string topictVizita
        {
            get { return TopictVizita; }
            set { TopictVizita = value; OnPropertyChanged("topictVizita"); }


        }

    }

}
