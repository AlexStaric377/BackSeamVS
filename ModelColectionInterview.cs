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
    public class ListColectionInterview
    {

        [JsonProperty("list")]
        public ColectionInterview[] ColectionInterview { get; set; }

    }
    public class ColectionInterview
    {
        public int id { get; set; }
        public string kodDoctor { get; set; }
        public string kodPacient { get; set; }
        public string dateInterview { get; set; }
        public string dateDoctor { get; set; }
        public string kodProtokola { get; set; }
        public string detailsInterview { get; set; }
        public string resultDiagnoz { get; set; }
        public string kodComplInterv { get; set; }
        public string nameInterview { get; set; }

    }

    public class ListModelColectionInterview
    {

        [JsonProperty("list")]
        public ModelColectionInterview[] ModelColectionInterview { get; set; }

    }
    public class ModelColectionInterview : BaseViewModel
    {

        private int Id;
        private string KodDoctor;
        private string KodPacient;
        private string DateInterview;
        private string DateDoctor;
        private string KodProtokola;
        private string DetailsInterview;
        private string ResultDiagnoz;
        private string NameDiagnoz;
        private string NameRecomen;
        private string NameDoctor;
        private string NamePacient;
        private string KodComplInterv;
        private string NameInterview;

        public ModelColectionInterview(int Id = 0, string KodDoctor = "", string KodPacient = "", string DateInterview = "",
            string DateDoctor = "", string KodProtokola = "", string DetailsInterview = "", string ResultDiagnoz = "",
            string NameDiagnoz = "", string NameRecomen = "", string NameDoctor = "", string NamePacient = "", string KodComplInterv = "", string NameInterview = "")
        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.KodPacient = KodPacient;
            this.DateDoctor = DateDoctor;
            this.DateInterview = DateInterview;
            this.KodProtokola = KodProtokola;
            this.DetailsInterview = DetailsInterview;
            this.ResultDiagnoz = ResultDiagnoz;
            this.NameDiagnoz = NameDiagnoz;
            this.NameRecomen = NameRecomen;
            this.NameDoctor = NameDoctor;
            this.NamePacient = NamePacient;
            this.KodComplInterv = KodComplInterv;
            this.NameInterview = NameInterview;
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

        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        [JsonProperty("detailsInterview")]
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
        }

        [JsonProperty("resultDiagnoz")]
        public string resultDiagnoz
        {
            get { return ResultDiagnoz; }
            set { ResultDiagnoz = value; OnPropertyChanged("resultDiagnoz"); }
        }

        [JsonProperty("nameDiagnoz")]
        public string nameDiagnoz
        {
            get { return NameDiagnoz; }
            set { NameDiagnoz = value; OnPropertyChanged("nameDiagnoz"); }
        }
        [JsonProperty("nameRecomen")]
        public string nameRecomen
        {
            get { return NameRecomen; }
            set { NameRecomen = value; OnPropertyChanged("nameRecomen"); }
        }
        [JsonProperty("nameDoctor")]
        public string nameDoctor
        {
            get { return NameDoctor; }
            set { NameDoctor = value; OnPropertyChanged("nameDoctor"); }
        }
        [JsonProperty("namePacient")]
        public string namePacient
        {
            get { return NamePacient; }
            set { NamePacient = value; OnPropertyChanged("namePacient"); }
        }
        [JsonProperty("kodComplInterv")]
        public string kodComplInterv
        {
            get { return KodComplInterv; }
            set { KodComplInterv = value; OnPropertyChanged("kodComplInterv"); }
        }

        [JsonProperty("nameInterview")]
        public string nameInterview
        {
            get { return NameInterview; }
            set { NameInterview = value; OnPropertyChanged("nameInterview"); }
        }

        [JsonProperty("dateDoctor")]
        public string dateDoctor
        {
            get { return DateDoctor; }
            set { DateDoctor = value; OnPropertyChanged("dateDoctor"); }
        }

    }

    // модель статистики количества диагнозов установленных врачем за текущий квартал. 
    public class ModelColectionDiagnozInterview : BaseViewModel
    {

        private int Id;
        private string KodDoctor;
        private string KodPacient;
        private string KodProtokola;
        private string NameDiagnoz;
        private string NamePacient;
        private int    QuanntityDiagnoz;


        public ModelColectionDiagnozInterview(int Id = 0, string KodDoctor = "", string KodPacient = "", string KodProtokola = "",
            string NameDiagnoz = "", string NamePacient = "", int QuanntityDiagnoz = 0)
        { 

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.KodPacient = KodPacient;
            this.KodProtokola = KodProtokola;
            this.NameDiagnoz = NameDiagnoz;
            this.NamePacient = NamePacient;
            this.QuanntityDiagnoz = QuanntityDiagnoz;

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


        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }


        [JsonProperty("nameDiagnoz")]
        public string nameDiagnoz
        {
            get { return NameDiagnoz; }
            set { NameDiagnoz = value; OnPropertyChanged("nameDiagnoz"); }
        }

        [JsonProperty("namePacient")]
        public string namePacient
        {
            get { return NamePacient; }
            set { NamePacient = value; OnPropertyChanged("namePacient"); }
        }

        [JsonProperty("quanntityDiagnoz")]
        public int quanntityDiagnoz
        {
            get { return QuanntityDiagnoz; }
            set { QuanntityDiagnoz = value; OnPropertyChanged("quanntityDiagnoz"); }
        }
    }    

}
