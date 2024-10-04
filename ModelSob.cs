using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;
// Управление вводом-выводом
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class ListModelSob
    {
        [JsonProperty("list")]
        public ViewModelSob[] ViewModelSob { get; set; }
    }

    public class ModelSob
    {
        public int Id { get; set; }
        public string KodObl { get; set; }
        public string NameObl { get; set; }
        public string NameRajon { get; set; }
        public string Namepunkt { get; set; }
        public int Piple { get; set; }
        public string Pind { get; set; }
    }

    public class ViewModelSob : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private int Id;
        private string KodObl;
        private string NameObl;
        private string NameRajon;
        private string Namepunkt;
        private int Piple;
        private string Pind;


        public ViewModelSob(int Id=0, string KodObl="", string NameObl="", string NameRajon="", string Namepunkt="", int Piple=0, string Pind="")
        {
            this.KodObl = KodObl;
            this.Id = Id;
            this.NameObl = NameObl;
            this.NameRajon = NameRajon;
            this.Namepunkt = Namepunkt;
            this.Piple = Piple;
            this.Pind = Pind;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodObl")]
        public string kodObl
        {
            get { return KodObl; }
            set { KodObl = value; OnPropertyChanged("kodObl"); }
        }

        [JsonProperty("nameObl")]
        public string nameObl
        {
            get { return NameObl; }
            set { NameObl = value; OnPropertyChanged("nameObl"); }
        }

        [JsonProperty("nameRajon")]
        public string nameRajon
        {
            get { return NameRajon; }
            set { NameRajon = value; OnPropertyChanged("nameRajon"); }
        }

        [JsonProperty("namepunkt")]
        public string namepunkt
        {
            get { return Namepunkt; }
            set { Namepunkt = value; OnPropertyChanged("namepunkt"); }
        }

        [JsonProperty("piple")]
        public int piple
        {
            get { return Piple; }
            set { Piple = value; OnPropertyChanged("piple"); }
        }

        [JsonProperty("pind")]
        public string pind
        {
            get { return Pind; }
            set { Pind = value; OnPropertyChanged("pind"); }
        }
    }
}
