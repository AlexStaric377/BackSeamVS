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
    public partial class ListGrupDiagnoz
    {
        [JsonProperty("list")]
        public GrupDiagnoz[] GrupDiagnoz { get; set; }
    }

    public class GrupDiagnoz
    {
        public int id { get; set; }
        public string icdGrDiagnoz { get; set; }
        public string nameGrDiagnoz { get; set; }
        public string opisDiagnoza { get; set; }
        public string uriDiagnoza { get; set; }
        public string IdUser { get; set; } // код пациента или доктора

    }

    public class GrDiagnozXls
    {
        public string icdGrA { get; set; }
        public string icdGrB { get; set; }
        public string icdGrC { get; set; }
        public string icdGrD { get; set; }
        public string icdGrE { get; set; }
        public string nameGrDiagnoz { get; set; }
        public string IdUser { get; set; } // код пациента или доктора


    }

    public partial class ListModelGrupDiagnoz
    {
        [JsonProperty("list")]
        public ModelGrupDiagnoz[] ModelGrupDiagnoz { get; set; }
    }

    public class ModelGrupDiagnoz : BaseViewModel
    {

        
        public int Id;
        public string IcdGrDiagnoz;
        public string NameGrDiagnoz;
        public string OpisDiagnoza;
        public string UriDiagnoza;
        private string IdUser;


        public ModelGrupDiagnoz(int Id = 0, string IcdGrDiagnoz = "", string NameGrDiagnoz = "",  string OpisDiagnoza = "", string UriDiagnoza = "", string IdUser = "")
        {
            this.Id = Id;
            this.IcdGrDiagnoz = IcdGrDiagnoz;
            this.NameGrDiagnoz = NameGrDiagnoz;
            this.OpisDiagnoza = OpisDiagnoza;
            this.UriDiagnoza = UriDiagnoza;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("icdGrDiagnoz")]
        public string icdGrDiagnoz
        {
            get { return IcdGrDiagnoz; }
            set { IcdGrDiagnoz = value; OnPropertyChanged("icdGrDiagnoz"); }
        }
        [JsonProperty("nameGrDiagnoz")]
        public string nameGrDiagnoz
        {
            get { return NameGrDiagnoz; }
            set { NameGrDiagnoz = value; OnPropertyChanged("nameGrDiagnoz"); }
        }
        [JsonProperty("opisDiagnoza")]
        public string opisDiagnoza
        {
            get { return OpisDiagnoza; }
            set { OpisDiagnoza = value; OnPropertyChanged("opisDiagnoza"); }
        }
        [JsonProperty("uriDiagnoza")]
        public string uriDiagnoza
        {
            get { return UriDiagnoza; }
            set { UriDiagnoza = value; OnPropertyChanged("uriDiagnoza"); }
        }
        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }

}
