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
    // Колекция характеров жалоб 
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelFeature
    {

        [JsonProperty("list")]
        public ModelFeature[] ModelFeature { get; set; }

    }

    public class ModelFeature : BaseViewModel
    {

        private int Id;
        private string KeyComplaint;
        private string KeyFeature;
        private string Name;
        private string IdUser;

        public ModelFeature(int Id = 0, string KeyComplaint = "", string KeyFeature = "", string Name = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyComplaint = KeyComplaint;
            this.KeyFeature = KeyFeature;
            this.Name = Name;
            this.IdUser = IdUser;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyComplaint")]
        public string keyComplaint
        { get { return KeyComplaint; } 
          set { KeyComplaint = value; OnPropertyChanged("keyComplaint"); }
        }

        [JsonProperty("KeyFeature")]
        public string keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
        }

        [JsonProperty("name")]
        public string name
        { get { return Name; } set { Name = value; OnPropertyChanged("name"); } }


        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }

    public partial class ListViewFeatureComplaint
    {

        [JsonProperty("list")]
        public ViewFeatureComplaint[] ViewFeatureComplaint { get; set; }

    }

    public class ViewFeatureComplaint : BaseViewModel
    {

        private int Id;
        private string KeyFeature;
        private string KeyComplaint;
        private string NameFeature;
        private string NameComplaint;
        private string IdUser;

        public ViewFeatureComplaint(int Id = 0, string KeyComplaint = "", string KeyFeature = "", string NameFeature = "", string NameComplaint = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyComplaint = KeyComplaint;
            this.KeyFeature = KeyFeature;
            this.NameFeature = NameFeature;
            this.NameComplaint = NameComplaint;
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

        [JsonProperty("keyComplaint")]
        public string keyComplaint
        {
            get { return KeyComplaint; }
            set { KeyComplaint = value; OnPropertyChanged("keyComplaint"); }
        }


        [JsonProperty("nameFeature")]
        public string nameFeature
        { get { return NameFeature; } set { NameFeature = value; OnPropertyChanged("nameFeature"); } }

        [JsonProperty("nameComplaint")]
        public string nameComplaint
        { get { return NameComplaint; } set { NameComplaint = value; OnPropertyChanged("nameComplaint"); } }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

    }

}
