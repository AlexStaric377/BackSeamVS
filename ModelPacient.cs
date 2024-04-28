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
    public partial class ListModelPacient
    {

        [JsonProperty("list")]
        public ModelPacient[] ViewPacient { get; set; }

    }
    public class ModelPacient : BaseViewModel 
    {

        // команда закрытия окна
        RelayCommand? checkKeyTextTel;
        public RelayCommand CheckKeyTextTel
        {
            get
            {
                return checkKeyTextTel ??
                  (checkKeyTextTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Pacientt8, 12);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilTel;
        public RelayCommand CheckProfilTel
        {
            get
            {
                return checkProfilTel ??
                  (checkProfilTel = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.PacientProfilt8, 12);
                  }));
            }
        }
        // команда закрытия окна
        RelayCommand? checkKeyTextPind;
        public RelayCommand CheckKeyTextPind
        {
            get
            {
                return checkKeyTextPind ??
                  (checkKeyTextPind = new RelayCommand(obj =>
                  {
                          IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.Pacientt13, 5);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkProfilPind;
        public RelayCommand CheckProfilPind
        {
            get
            {
                return checkProfilPind ??
                  (checkProfilPind = new RelayCommand(obj =>
                  {
                      IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowMen.PacientProfilt13, 5);
                  }));
            }
        }
        private int Id;
        private string KodPacient;
        private string IdCabinet;
        private int Age;
        private decimal Weight;
        private int Growth;
        private string Gender;
        private string Tel;
        private string Email;
        private string Name;
        private string Surname;
        private string Login;
        private string Password;
        private string Pind;
        private string Profession;


        public ModelPacient(int Id = 0, string KodPacient = "", string IdCabinet = "", int Age = 0, decimal Weight = 0, int Growth = 0, string Gender = "",
             string Tel = "", string Email = "", string Name = "", string Surname = "", string Login = "", string Password = "", string Pind = "", string Profession="")
        {
            this.Id = Id;
            this.KodPacient = KodPacient;
            this.IdCabinet = IdCabinet;
            this.Age = Age;
            this.Weight = Weight;
            this.Growth = Growth;
            this.Gender = Gender;
            this.Tel = Tel;
            this.Email = Email;
            this.Name = Name;
            this.Surname = Surname;
            this.Login = Login;
            this.Password = Password;
            this.Pind = Pind;
            this.Profession = Profession;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodPacient")]
        public string kodPacient
        {
            get { return KodPacient; }
            set { KodPacient = value; OnPropertyChanged("kodPacient"); }
        }
        [JsonProperty("idCabinet")]
        public string idCabinet
        {
            get { return IdCabinet; }
            set { IdCabinet = value; OnPropertyChanged("idCabinet"); }
        }

        [JsonProperty("age")]
        public int age
        {
            get { return Age; }
            set { Age = value; OnPropertyChanged("age"); }
        }

        [JsonProperty("weight")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal weight
        {
            get { return Weight; }
            set { Weight = value; OnPropertyChanged("weight"); }
        }

        [JsonProperty("growth")]
        public int growth
        { 
            get { return Growth; }
            set { Growth = value; OnPropertyChanged("growth"); }
        }

        [JsonProperty("gender")]
        public string gender
        {
            get { return Gender; }
            set { Gender = value; OnPropertyChanged("gender"); }
        }

 
        [JsonProperty("tel")]
        public string tel
        {
            get { return Tel; }
            set { Tel = value; OnPropertyChanged("tel"); }
        }
        [JsonProperty("email")]
        public string email
        {
            get { return Email; }
            set { Email = value; OnPropertyChanged("email"); }
        }
        [JsonProperty("name")]
        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }
        [JsonProperty("surname")]
        public string surname
        {
            get { return Surname; }
            set { Surname = value; OnPropertyChanged("surname"); }
        }
        [JsonProperty("login")]
        public string login
        {
            get { return Login; }
            set { Login = value; OnPropertyChanged("login"); }
        }
        [JsonProperty("password")]
        public string password
        {
            get { return Password; }
            set { Password = value; OnPropertyChanged("password"); }
        }
        [JsonProperty("pind")]
        public string pind
        {
            get { return Pind; }
            set { Pind = value; OnPropertyChanged("pind"); }
        }
        [JsonProperty("profession")]
        public string profession
        {
            get { return Profession; }
            set { Profession = value; OnPropertyChanged("profession"); }
        }

        //private string _namePacient;
        //public string NamePacient 
        //{ 
        //    get => _namePacient;
        //    set 
        //    {
        //        _namePacient = value;
        //        OnPropertyChanged(nameof(NamePacient));
        //    } 
        //}

        //private string _surnamePacient;
        //public string SurNamePacient
        //{
        //    get => _surnamePacient;
        //    set
        //    {
        //        _surnamePacient = value;
        //        OnPropertyChanged(nameof(SurNamePacient));
        //    }
        //}

        //private string _telefonPacient;
        //public string TelefonPacient
        //{
        //    get => _telefonPacient;
        //    set
        //    {
        //        _telefonPacient = value;
        //        OnPropertyChanged(nameof(TelefonPacient));
        //    }
        //}

    }

    public class SelectMan : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public static List<string> Units { get; set; } = new List<string> { "чол.", "жін." };

        private string _SelectedUnit;
        public string SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedUnit = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUnit)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewUnit(_SelectedUnit);
            }
        }

        public void SetNewUnit(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("BackMain");
            WindowMen.Pacientt7.Text = selected == "0" ? "чол." : "жін.";
        }   
    }


}
