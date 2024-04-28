using System;
using System.Windows;
using System.Windows.Input;
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
using Newtonsoft.Json.Converters;
using System.Windows.Media;

namespace BackSeam
{

    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ListModelComplaint
    {

        [JsonProperty("list")]
        public ModelComplaint[] ViewComplaint { get; set; }

    }

  
    public class ModelComplaint : BaseViewModel
    {

        private int Id;
        private string KeyComplaint;
        private string Name;
        private string IdUser;

        public ModelComplaint(int Id = 0, string KeyComplaint = "", string Name = "", string IdUser="")
        {
            this.Id = Id;
            this.KeyComplaint = KeyComplaint;
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


        [JsonProperty("name")]
        public string name
        { get { return Name; }
          set { Name = value; OnPropertyChanged("name"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }


     }

}
