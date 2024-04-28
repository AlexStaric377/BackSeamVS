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
    // Список групп Квалификации
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelGroupQualification
    {
        [JsonProperty("list")]
        public ModelGroupQualification[] ViewGroupQualification { get; set; }
    }

    public class ModelGroupQualification : BaseViewModel
    {

        private int Id;
        private string KodGroupQualification;
        private string NameGroupQualification;
        private string IdUser;

        public ModelGroupQualification(int Id = 0, string KodGroupQualification = "", string NameGroupQualification = "", string IdUser = "")
        {
            this.Id = Id;
            this.KodGroupQualification = KodGroupQualification;
            this.NameGroupQualification = NameGroupQualification;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodGroupQualification")]
        public string kodGroupQualification
        {
            get { return KodGroupQualification; }
            set { KodGroupQualification = value; OnPropertyChanged("kodGroupQualification"); }
        }

        [JsonProperty("nameGroupQualification")]
        public string nameGroupQualification
        {
            get { return NameGroupQualification; }
            set { NameGroupQualification = value; OnPropertyChanged("nameGroupQualification"); }
        }
        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }
}
