﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BackSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {
        public MainWindow WindowHidden = MainWindow.LinkNameWindow("BackMain");
        private RelayCommand? geustGridLoadHidden;
        public RelayCommand GeustGridLoadHidden
        {
            get
            {
                return geustGridLoadHidden ??
                  (geustGridLoadHidden = new RelayCommand(obj =>
                  {

                      switch (WindowHidden.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                               break;
                          // Закладка пациент
                          case 1:
                              WindowHidden.ProfilPacientGridLoad.Visibility = Visibility.Visible;
                              WindowHidden.ProfilPacientgrAdd.Visibility = Visibility.Visible;
                              WindowHidden.ProfilPacientGridGhange.Visibility = Visibility.Visible;
                              WindowHidden.ProfilPacientSave.Visibility = Visibility.Visible;
                              WindowHidden.ProfilPacientPrint.Visibility = Visibility.Visible;
                              WindowHidden.ProfilPacientDelete.Visibility = Visibility.Visible;
                              switch (WindowHidden.ControlPacient.SelectedIndex)
                              {
                                  case 1:
                                      WindowHidden.ProfilPacientGridLoad.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilPacientGridGhange.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilPacientSave.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilPacientDelete.Visibility = Visibility.Hidden;
                                      // Провести опитування пацієнта
                                      if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") { WindowHidden.ControlPacient.SelectedIndex = 0; return;}
                                      MetodStartPacient();
                                      WindowHidden.ControlPacient.SelectedIndex = 0;
                                      break;
                                  case 2:
                                      WindowHidden.ProfilPacientgrAdd.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilPacientGridGhange.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilPacientSave.Visibility = Visibility.Hidden;
                                      // Завантажити показники проведених інтервью
                                      if (RegUserStatus != "2") if (CheckStatusUser() == false) return;
                                      //CheckLoadKabinetPacient();
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") { WindowHidden.ControlPacient.SelectedIndex = 0; return;}
                                      MethodLoadtableColectionIntevPacient();
                                      break;

                              }
                              break;
                          // Закладка лікар
                          case 2:
                              WindowHidden.GridLikarProfLoad.Visibility = Visibility.Visible;
                              WindowHidden.ProfilLikarGridAdd.Visibility = Visibility.Visible;
                              WindowHidden.ProfilLikarGhange.Visibility = Visibility.Visible;
                              WindowHidden.ProfilLikarSave.Visibility = Visibility.Visible;
                              WindowHidden.ProfilLikarPrint.Visibility = Visibility.Visible;
                              WindowHidden.ProfilLikarDelete.Visibility = Visibility.Visible;
                              switch (WindowHidden.ControlLikar.SelectedIndex)
                              {
                                  case 1:
                                      WindowHidden.GridLikarProfLoad.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilLikarGhange.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilLikarSave.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilLikarDelete.Visibility = Visibility.Hidden;

                                      if (_kodDoctor == "") MetodLoadProfilLikar(); 
                                      if (_kodDoctor == "") { WindowHidden.ControlLikar.SelectedIndex = 0; return; }
                                      MetodInterviewStartLikar();
                                     WindowHidden.ControlLikar.SelectedIndex = 0; 
                                      break;
                                  case 2:
                                      WindowHidden.ProfilLikarGridAdd.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilLikarGhange.Visibility = Visibility.Hidden;
                                      WindowHidden.ProfilLikarSave.Visibility = Visibility.Hidden;
                                      //CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MetodLoadProfilLikar();
                                      if (_kodDoctor == "") { WindowHidden.ControlLikar.SelectedIndex = 0; return; }
                                      MethodLoadtableColectionIntevLikar();
                                      break;

                              }
                              break;
                          // Закладка Администрування
                          case 5:
                              break;
                          // Закладка про програму
                          case 6:
                              break;
                      }

                  }));
            }
        }

        private void CheckLoadKabinetPacient()
        {
            if (loadboolProfilLikar == true && boolSetAccountUser == false)
            {
                MessageOnOffKabinetPacient();
                if (MapOpisViewModel.DeleteOnOff == false) return;
                //ExitCabinetLikar();
            }

        }
    }
}