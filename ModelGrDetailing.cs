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
    // Список содержимого групп детализаций т.е. какя детализация входит в соства каждой групы
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

    public partial class ListModelGrDetailing
    {

        [JsonProperty("list")]
        public ModelGrDetailing[] ViewGrDetailing { get; set; }

    }

   

    public class ModelGrDetailing : BaseViewModel
    {

        private int Id;
        private string KeyGrDetailing;
        private string KodGroupQualification;
        private string NameGrDetailing;
        private string KodDetailing;
        private string IdUser;

        public ModelGrDetailing(int Id = 0, string KeyGrDetailing = "", string KodDetailing="", string KodGroupQualification = "", string NameGrDetailing = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.KodGroupQualification = KodGroupQualification;
            this.NameGrDetailing = NameGrDetailing;
            this.KodDetailing = KodDetailing;
            this.IdUser = IdUser;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodDetailing")]
        public string kodDetailing
        {
            get { return KodDetailing; }
            set { KodDetailing = value; OnPropertyChanged("kodDetailing"); }
        }
        [JsonProperty("keyGrDetailing")]
        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }
        [JsonProperty("kodGroupQualification")]
        public string kodGroupQualification
        {
            get { return KodGroupQualification; }
            set { KodGroupQualification = value; OnPropertyChanged("kodGroupQualification"); }
        }
        [JsonProperty("nameGrDetailing")]
        public string nameGrDetailing
        {
            get { return NameGrDetailing; }
            set { NameGrDetailing = value; OnPropertyChanged("nameGrDetailing"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }
    // Содержание груповых детализаций грид экранной формы вывода данных объеденены  модели
    // ViewGrDetailing и ViewGroupQualification
    public partial class ListDetailingQualification
    {
        [JsonProperty("list")]
        public ViewDetailingQualification[] ViewDetailingQualification { get; set; }
    }
    public class ViewDetailingQualification : BaseViewModel
    {

        private int Id;
        private string KeyGrDetailing;
        private string IdQualification;
        private string NameGrup;
        private string NameGroupQualification;
        private string NnameGrDetailing;
        private string IdUser;
        public ViewDetailingQualification(int Id = 0, string KeyGrDetailing = "", string IdQualification = "", string NameGrup = "", string NameGroupQualification = "", string NnameGrDetailing = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.IdQualification = IdQualification;
            this.NameGrup = NameGrup;
            this.NameGroupQualification = NameGroupQualification;
            this.NnameGrDetailing = NnameGrDetailing;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }
        [JsonProperty("idQualification")]
        public string idQualification
        {
            get { return IdQualification; }
            set { IdQualification = value; OnPropertyChanged("idQualification"); }
        }
        [JsonProperty("nameGrup")]
        public string nameGrup
        {
            get { return NameGrup; }
            set { NameGrup = value; OnPropertyChanged("nameGrup"); }
        }
        [JsonProperty("nameGroupQualification")]
        public string nameGroupQualification
        {
            get { return NameGroupQualification; }
            set { NameGroupQualification = value; OnPropertyChanged("nameGroupQualification"); }
        }
        [JsonProperty("nameGrDetailing")]
        public string nameGrDetailing
        {
            get { return NnameGrDetailing; }
            set { NnameGrDetailing = value; OnPropertyChanged("nameGrDetailing"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }
}
