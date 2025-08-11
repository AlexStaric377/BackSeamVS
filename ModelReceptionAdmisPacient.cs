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
    public partial class ListAdmissionPatient
    {

        [JsonProperty("list")]
        public AdmissionPatient[] AdmissionPatient { get; set; }

    }


    // список приемов пациентов записавшихся на прием средством СЕАМ 
    public class AdmissionPatient : INotifyPropertyChanged
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
        private string DateVizita;
        private string KodProtokola;
        private string TopictVizita;
        private string KodComplInterv;
        private string KodDiagnoz;
        



        public AdmissionPatient(int Id=0, string KodDoctor="",string KodPacient = "", string DateInterview = "", string DateVizita = "",
           string KodProtokola = "", string TopictVizita = "", string KodComplInterv = "", string KodDiagnoz = "")
        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.KodPacient = KodPacient;
            this.DateVizita = DateVizita;
            this.DateInterview = DateInterview;
            this.KodProtokola = KodProtokola;
            this.TopictVizita = TopictVizita;
            this.KodComplInterv = KodComplInterv;
            this.KodDiagnoz = KodDiagnoz;

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

        [JsonProperty("dateVizita")]
        public string dateVizita
        {
            get { return DateVizita; }
            set { DateVizita = value; OnPropertyChanged("dateVizita"); }
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
        public string kodDiagnoz
        {
            get { return KodDiagnoz; }
            set { KodDiagnoz = value; OnPropertyChanged("kodDiagnoz"); }
        }
    }

 
    // График приемных дней недели и времени доктора

    public class VisitingDays
    {
        public int Id { get; set; }
        public string KodDoctor { get; set; }
        public string DaysOfTheWeek { get; set; }
        public string DateVizita { get; set; }
        public string OnOff { get; set; }

    }

    public partial class ListModelReceptionPatient
    {
        public ModelReceptionPatient[] ModelReceptionPatient { get; set; }

    }

    public class ModelReceptionPatient : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }


        private string DateInterview;
        private string DateVizita;
        private string TopictVizita;
        private string NamePacient;
        private string NameDiagnoz;
        private string NameRecomen;
        private string KodComplInterv;
        private string KodProtokola;
        private string KodPacient;

        public ModelReceptionPatient( string NamePacient = "", string KodPacient = "", string DateInterview = "", string DateVizita = "",
           string KodProtokola = "", string TopictVizita = "", string KodComplInterv = "",string NameDiagnoz="",
           string NameRecomen ="")
        {
           
            this.KodPacient = KodPacient;
            this.DateVizita = DateVizita;
            this.DateInterview = DateInterview;
            this.KodProtokola = KodProtokola;
            this.TopictVizita = TopictVizita;
            this.KodComplInterv = KodComplInterv;
            this.NameDiagnoz = NameDiagnoz;
            this.NameRecomen = NameRecomen;
            this.NamePacient = NamePacient;
        }


        public string kodPacient
        {
            get { return KodPacient; }
            set { KodPacient = value; OnPropertyChanged("kodPacient"); }
        }

        public string dateInterview
        {
            get { return DateInterview; }
            set { DateInterview = value; OnPropertyChanged("dateInterview"); }
        }

        public string dateVizita
        {
            get { return DateVizita; }
            set { DateVizita = value; OnPropertyChanged("dateVizita"); }
        }

        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        public string kodComplInterv
        {
            get { return KodComplInterv; }
            set { KodComplInterv = value; OnPropertyChanged("kodComplInterv"); }
        }

 
        public string topictVizita
        {
            get { return TopictVizita; }
            set { TopictVizita = value; OnPropertyChanged("topictVizita"); }
        }

        public string nameDiagnoz
        {
            get { return NameDiagnoz; }
            set { NameDiagnoz = value; OnPropertyChanged("nameDiagnoz"); }
        }

        public string nameRecomen
        {
            get { return NameRecomen; }
            set { NameRecomen = value; OnPropertyChanged("nameRecomen"); }
        }

        public string namePacient
        {
            get { return NamePacient; }
            set { NamePacient = value; OnPropertyChanged("namePacient"); }
        }

    }
}
