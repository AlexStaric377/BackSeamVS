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

    // Рекомендации о дальнейших действиях, в том числе, и по адресному обращению для оказания медицинской помощи к врачам - специалистам медицинских учреждений.

    public partial class ListRecommendation
    {
        [JsonProperty("list")]
        public Recommendation[] Recommendation { get; set; }
    }

    public class Recommendation 
    {
        public int Id { get; set; }
        public string KodRecommendation { get; set; }
        public string ContentRecommendation { get; set; }
    }


        public partial class ListModelRecommendation
    {
        [JsonProperty("list")]
        public ModelRecommendation[] ViewRecommendation { get; set; }
    }


    public class ModelRecommendation : INotifyPropertyChanged
    {
        public int Id;
        public string KodRecommendation;
        public string ContentRecommendation;


        public ModelRecommendation(int Id = 0, string KodRecommendation = "", string ContentRecommendation = "")
        {
            this.Id = Id;
            this.KodRecommendation = KodRecommendation;
            this.ContentRecommendation = ContentRecommendation;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodRecommendation")]
        public string kodRecommendation
        {
            get { return KodRecommendation; }
            set { KodRecommendation = value; OnPropertyChanged("kodRecommendation"); }
        }
        [JsonProperty("contentRecommendation")]
        public string contentRecommendation
        {
            get { return ContentRecommendation; }
            set { ContentRecommendation = value; OnPropertyChanged("contentRecommendation"); }
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
