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
    public partial class ListModelDetailing
    {

        [JsonProperty("list")]
        public ModelDetailing[] ViewDetailing { get; set; }

    }

 
    public class ModelDetailing : BaseViewModel
    {

        private int Id;
        private string KeyFeature;
        private string KeyGrDetailing;
        private string NameDetailing;
        private string KodDetailing;
        private string IdUser;


        public ModelDetailing(int Id = 0, string KeyFeature = "", string KodDetailing="", string KeyGrDetailing = "", string NameDetailing = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyFeature = KeyFeature;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameDetailing = NameDetailing;
            this.KodDetailing = KodDetailing;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyFeature")]
        public string keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
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

        [JsonProperty("nameDetailing")]
        public string nameDetailing
        { get { return NameDetailing; } set { NameDetailing = value; OnPropertyChanged("nameDetailing"); } }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }
    }

    // Модель экранной формы объеденяющая ViewDetailing и Feature
    public partial class ListViewDetailingFeature
    {

        [JsonProperty("list")]
        public ViewDetailingFeature[] ViewDetailingFeature { get; set; }

    }


    public class ViewDetailingFeature : BaseViewModel
    {

        private int Id;
        private string KodDetailing;
        private string KeyFeature;
        private string NameFeature;
        private string KeyGrDetailing;
        private string NameDetailing;
        private string NameGrDetailing;
        private string IdUser;
        private string KodComplaint;
        private string NameComplaint;



        public ViewDetailingFeature(int Id = 0, string KodDetailing = "",string KeyFeature = "",  string NameFeature = "", string KeyGrDetailing = "", string NameDetailing = "", string NameGrDetailing = "", string IdUser = "", string KodComplaint="", string NameComplaint = "")
        {
            this.Id = Id;
            this.KodDetailing = KodDetailing;
            this.KeyFeature = KeyFeature;
            this.NameFeature = NameFeature;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameDetailing = NameDetailing;
            this.NameGrDetailing = NameGrDetailing;
            this.IdUser = IdUser;
            this.KodComplaint = KodComplaint;
            this.NameComplaint = NameComplaint;


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
        [JsonProperty("keyFeature")]
        public string keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
        }

        [JsonProperty("nameFeature")]
        public string nameFeature
        { get { return NameFeature; } set { NameFeature = value; OnPropertyChanged("nameFeature"); } }

        [JsonProperty("keyGrDetailing")]
        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }

        [JsonProperty("nameDetailing")]
        public string nameDetailing
        { get { return NameDetailing; } set { NameDetailing = value; OnPropertyChanged("nameDetailing"); } }

        [JsonProperty("nameGrDetailing")]
        public string nameGrDetailing
        { get { return NameGrDetailing; } set { NameGrDetailing = value; OnPropertyChanged("nameGrDetailing"); } }


        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

        public string kodComplaint
        {
            get { return KodComplaint; }
            set { KodComplaint = value; OnPropertyChanged("kodComplaint"); }
        }
        public string nameComplaint
        { get { return NameComplaint; } set { NameComplaint = value; OnPropertyChanged("nameComplaint"); } }
    }

}
