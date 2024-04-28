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
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {


        public static ModelComplaint selectedSetAccountUser;
        public ObservableCollection<ModelComplaint> NsiComplaints { get; set; }
        public static int CountComplaint = 0;

        public ModelComplaint SelectedSetAccountUser
        { get { return selectedSetAccountUser; } set { selectedSetAccountUser = value; OnPropertyChanged("SelectedComplaint"); } }
        // конструктор класса



        // команда открытия окна справочника групп уточнения детализации и  добавления группы уточнения
        private RelayCommand? loadSetAccountUser;
        public RelayCommand LoadSetAccountUser
        {
            get
            {
                return loadSetAccountUser ??
                  (loadSetAccountUser = new RelayCommand(obj =>
                  {
                      NsiComplaint NewOrder = new NsiComplaint();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();

                      /*AddComandAddSetAccountUser();*/
                  }));
            }
            set { loadSetAccountUser = value; OnPropertyChanged("LoadSetAccountUser"); }
        }

        public void AddComandAddSetAccountUser()
        {
            NsiComplaint NewOrder = new NsiComplaint();
            NewOrder.Left = 600;
            NewOrder.Top = 200;
            NewOrder.ShowDialog();

        }

    }
}
