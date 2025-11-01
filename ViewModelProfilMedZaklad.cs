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
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace BackSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {
        //  модель MedicalInstitution
        //  клавиша в окне: "Довідник мед закладів"

        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех закладів из БД
        /// через механизм REST.API
        /// </summary>
        private static MainWindow WindowProfilMedical = MainWindow.LinkNameWindow("BackMain");
               
        public static string controlerProfilMedical = "/api/MedicalInstitutionController/";
        public static MedicalInstitution selectedProfilMedical;

        public static ObservableCollection<MedicalInstitution> VeiwModelProfilMedicals { get; set; }

        public MedicalInstitution SelectedProfilMedical
        { get { return selectedProfilMedical; } set { selectedProfilMedical = value; OnPropertyChanged("SelectedProfilMedical"); } }

        public static void ObservableVeiwProfilMedical(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelMedical>(CmdStroka);
            List<MedicalInstitution> res = result.MedicalInstitution.ToList();
            VeiwModelProfilMedicals = new ObservableCollection<MedicalInstitution>((IEnumerable<MedicalInstitution>)res);
            MetodAddNameStatusProfil();
            WindowProfilMedical.GuestMedicalTablGrid.ItemsSource = VeiwModelProfilMedicals;
        }


        public static void MetodAddNameStatusProfil()
        {
            int indexrepl = 0;
            foreach (MedicalInstitution medicalInstitution in VeiwModelProfilMedicals)
            {
                if (medicalInstitution.kodZaklad == null) NewkodZaklad();

                string json = controlerStatusZaklad + medicalInstitution.idstatus;
                CallServer.PostServer(controlerStatusZaklad, json, "GETID");
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                StatusMedZaklad Idinsert = JsonConvert.DeserializeObject<StatusMedZaklad>(CallServer.ResponseFromServer);
                VeiwModelProfilMedicals[indexrepl].idstatus += ":" + Idinsert.nameStatus;
                indexrepl++;
            }

        }


        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadVeiwProfilMedical;
        public RelayCommand LoadVeiwProfilMedical
        {
            get
            {
                return loadVeiwProfilMedical ??
                  (loadVeiwProfilMedical = new RelayCommand(obj =>
                  {
                      MethodloadProfilMedical();
                  }));
            }
        }

        private void MethodloadProfilMedical()
        {
            WindowProfilMedical.GuestFolderWorkzaklad.Visibility = Visibility.Visible;
            WindowProfilMedical.GuestFolderStatuszaklad.Visibility = Visibility.Visible;
            CallServer.PostServer(controlerProfilMedical, controlerProfilMedical+"0/0/0/5", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableVeiwProfilMedical(CmdStroka);
        }

        
            private RelayCommand? visibleFolderProfil;
        public RelayCommand VisibleFolderProfil
        {
            get
            {
                return visibleFolderProfil ??
                  (visibleFolderProfil = new RelayCommand(obj =>
                  {
                      if (VeiwModelProfilMedicals != null && WindowProfilMedical.GuestMedicalTablGrid.SelectedIndex >= 0)
                      {
                          
                          selectedProfilMedical = VeiwModelProfilMedicals[WindowProfilMedical.GuestMedicalTablGrid.SelectedIndex];
                          SelectedProfilMedical = selectedProfilMedical;
                          EdrpouMedZaklad = WindowMedical.GuestMedicalt2.Text;
                      }

                  }));
            }
        }

        #endregion
    }
}
