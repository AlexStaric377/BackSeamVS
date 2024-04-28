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
    public partial class ListLikarGrupDiagnoz
    {
        [JsonProperty("list")]
        public LikarGrupDiagnoz[] LikarGrupDiagnoz { get; set; }
    }

    public class LikarGrupDiagnoz
    {
        public int id { get; set; }
        public string kodDoctor { get; set; }
        public string icdGrDiagnoz { get; set; } // код группы МКХ в которую входит диагноз

    }

    public partial class ListModelLikarGrupDiagnoz
    {
        [JsonProperty("list")]
        public ModelLikarGrupDiagnoz[] ModelLikarGrupDiagnoz { get; set; }
    }

    public class ModelLikarGrupDiagnoz : BaseViewModel
    {

        public int Id;
        public string KodDoctor;
        public string IcdGrDiagnoz;


        public ModelLikarGrupDiagnoz(int Id = 0, string KodDoctor = "", string IcdGrDiagnoz = "")
        {
            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.IcdGrDiagnoz = IcdGrDiagnoz;

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
        [JsonProperty("icdGrDiagnoz")]
        public string icdGrDiagnoz
        {
            get { return IcdGrDiagnoz; }
            set { IcdGrDiagnoz = value; OnPropertyChanged("icdGrDiagnoz"); }
        }
    }
}

