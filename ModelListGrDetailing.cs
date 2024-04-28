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
    // Список групп детализации Групи деталізацій
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

    public partial class ListModelListGrDetailing
    {

        [JsonProperty("list")]
        public ModelListGrDetailing[] ViewListGrDetailing { get; set; }

    }
    public class ModelListGrDetailing : BaseViewModel
    {

        private int Id;
        private string KeyGrDetailing;
        private string NameGrup;
        private string IdUser;


        public ModelListGrDetailing(int Id = 0, string KeyGrDetailing ="", string NameGrup = "", string IdUser = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameGrup = NameGrup;
            this.IdUser = IdUser;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyGrDetailing")]
        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }

        [JsonProperty("nameGrup")]
        public string nameGrup
        {
            get { return NameGrup; }
            set { NameGrup = value; OnPropertyChanged("nameGrup"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }

 
    }
}
