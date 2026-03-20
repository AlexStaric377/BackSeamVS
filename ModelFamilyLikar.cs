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
    public partial class ListFamilyLikar
    {
        [JsonProperty("list")]
        public FamilyLikar[] FamilyLikar { get; set; }
    }


    // Жизнь пациента и взаимодействие с врачами
    // 
    public class FamilyLikar : BaseViewModel
    {
        
    
        public int Id { get; set; }
        public string KodPacient { get; set; }
        public string KodDoctor { get; set; }
        public DateTime Datestart { get; set; }
        public DateTime Dateend { get; set; }
        public int Numberrequests { get; set; }
        public int Numberdiagnoz { get; set; }
    }

    public class ListModelFamilyLikar
    {

        [JsonProperty("list")]
        public ModelFamilyLikar[] ModelFamilyLikar { get; set; }

    }
    public class ModelFamilyLikar : BaseViewModel
    {

        private int Id;
        private string KodDoctor;
        private string KodPacient;
        private DateTime Datestart;
        private DateTime Dateend;
        private int Numberrequests;
        private int Numberdiagnoz;
        private string NameDoctor;
        private string NamePacient;

    

        public ModelFamilyLikar(int Id = 0, string KodDoctor = "", string KodPacient = "",
            int Numberrequests = 0, int Numberdiagnoz = 0,  string NameDoctor = "", string NamePacient = "", DateTime Dateend = new DateTime(), DateTime Datestart = new DateTime())
        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.KodPacient = KodPacient;
            this.Datestart = Datestart;
            this.Dateend = Dateend;
            this.Numberrequests = Numberrequests;
            this.Numberdiagnoz = Numberdiagnoz;
            this.NameDoctor = NameDoctor;
            this.NamePacient = NamePacient;
  
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


        [JsonProperty("datestart")]
        public DateTime datestart
        {
            get { return Datestart; }
            set { Datestart = value; OnPropertyChanged("datestart"); }
        }

        [JsonProperty("dateend")]
        public DateTime dateend
        {
            get { return Dateend; }
            set { Dateend = value; OnPropertyChanged("dateend"); }
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
        
       

    }
}
