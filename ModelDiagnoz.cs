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
    // справочник диагнозов
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListDiagnoz
    {
        [JsonProperty("list")]
        public Diagnoz[] Diagnoz { get; set; }
    }

    public class Diagnoz 
    {
        public int id;
        public string kodDiagnoza { get; set; }
        public string nameDiagnoza { get; set; }
        public string opisDiagnoza { get; set; }
        public string uriDiagnoza { get; set; }
        public string keyIcd { get; set; }
        public string IcdGrDiagnoz { get; set; }
        public string IdUser { get; set; } // код пациента или доктора
    }

        public partial class ListModelDiagnoz
    {
        [JsonProperty("list")]
        public ModelDiagnoz[] ModelDiagnoz { get; set; }
    }


    public class ModelDiagnoz : BaseViewModel
    {

        public int Id;
        public string KodDiagnoza;
        public string NameDiagnoza;
        public string OpisDiagnoza;
        public string UriDiagnoza;
        public string KeyIcd; // код Международная классификация болезней 11 пересмотра 
        public string IcdGrDiagnoz;
        private string IdUser;

        public ModelDiagnoz(int Id = 0, string KodDiagnoza = "", string NameDiagnoza = "", string KeyIcd = "", string OpisDiagnoza = "", string UriDiagnoza = "", string IcdGrDiagnoz="", string IdUser = "")
        {
            this.Id = Id;
            this.KodDiagnoza = KodDiagnoza;
            this.NameDiagnoza = NameDiagnoza;
            this.OpisDiagnoza = OpisDiagnoza;
            this.UriDiagnoza = UriDiagnoza;
            this.KeyIcd = KeyIcd;
            this.IcdGrDiagnoz = IcdGrDiagnoz;
            this.IdUser = IdUser;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodDiagnoza")]
        public string kodDiagnoza
        {
            get { return KodDiagnoza; }
            set { KodDiagnoza = value; OnPropertyChanged("kodDiagnoza"); }
        }
        [JsonProperty("nameDiagnoza")]
        public string nameDiagnoza
        {
            get { return NameDiagnoza; }
            set { NameDiagnoza = value; OnPropertyChanged("nameDiagnoza"); }
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
        [JsonProperty("keyIcd")]
        public string keyIcd
        {
            get { return KeyIcd; }
            set { KeyIcd = value; OnPropertyChanged("keyIcd"); }
        }

        [JsonProperty("icdGrDiagnoz")]
        public string icdGrDiagnoz
        {
            get { return IcdGrDiagnoz; }
            set { IcdGrDiagnoz = value; OnPropertyChanged("icdGrDiagnoz"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }
}
