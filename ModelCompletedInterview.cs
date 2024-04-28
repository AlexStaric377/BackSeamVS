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
    public partial class ListModelCompletedInterview
    {

        [JsonProperty("list")]
        public ModelCompletedInterview[] ModelCompletedInterview { get; set; }

    }
    public partial class ModelCompletedInterview : INotifyPropertyChanged
    {
        private int Id;
        private string KodComplInterv;
        private string DetailsInterview;
        private string KodDetailing;
        private int Numberstr;

        public ModelCompletedInterview(int Id = 0, int Numberstr = 0, string KodComplInterv = "", string KodDetailing = "", string DetailsInterview = "", string NametInterview = "")
        {
            this.Id = Id;
            this.KodComplInterv = KodComplInterv;
            this.KodDetailing = KodDetailing;
            this.DetailsInterview = DetailsInterview;
            this.Numberstr = Numberstr;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodComplInterv")]
        public string kodComplInterv
        {
            get { return KodComplInterv; }
            set { KodComplInterv = value; OnPropertyChanged("kodComplInterv"); }
        }

        [JsonProperty("numberstr")]
        public int numberstr
        {
            get { return Numberstr; }
            set { Numberstr = value; OnPropertyChanged("numberstr"); }
        }

        [JsonProperty("kodDetailing")]
        public string kodDetailing
        {
            get { return KodDetailing; }
            set { KodDetailing = value; OnPropertyChanged("kodDetailing"); }
        }

        [JsonProperty("detailsInterview")]
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }





}
