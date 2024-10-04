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
using Microsoft.WindowsAPICodePack.Dialogs;
using ClosedXML.Excel;
using System.Threading;


namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public partial class MapOpisViewModel : BaseViewModel
    {
        // ViewModelUploadDb - Загрузка таблиц из внешних текстовых файлов
        // клавиша в окне:  Завантажити

        #region Обработка событий и команд вигрузки
        /// <summary>
        /// Стркутура: Команды, 
        /// 
        /// </summary> 

        public static MainWindow WindowUpload = MainWindow.LinkNameWindow("BackMain");
        public static string upLoadstroka = "", FilePath="", strokajson = "", Method="POST";
        public static bool loadPathTab = false;
        public static int resultid =0;


        // Команда установления пути к архивным  файлам  из которых идет загрузка в БД

        public static IEnumerable<ModelSob> EnumerateSobs(string xlsxpath)
        {
            //var xlsSobs = new ModelSob();
            // Открываем книгу
            var workbook = new XLWorkbook(xlsxpath);
                // Берем в ней первый лист
            var worksheet = workbook.Worksheets.Worksheet(1);
            // Перебираем диапазон нужных строк
            int countsob = workbook.Worksheets.Count();
            for (int row = 2; row <= 17803; ++row)
            {
                // По каждой строке формируем объект
                var xlsSobs = new ModelSob
                {
                    KodObl = worksheet.Cell(row, 1).GetValue<string>(),
                        NameObl = worksheet.Cell(row, 2).GetValue<string>(),
                        NameRajon = worksheet.Cell(row, 3).GetValue<string>(),
                        Namepunkt = worksheet.Cell(row, 4).GetValue<string>(),
                        Piple = worksheet.Cell(row, 5).GetValue<int>(),
                        Pind = worksheet.Cell(row, 6).GetValue<string>(),
                };
                    
                 // И возвращаем его
                 yield return xlsSobs;
            }
            
        }

        public static IEnumerable<GrDiagnozXls> EnumerateGrupDiagnozs(string xlsxpath)
        {
            //var xlsSobs = new ModelSob();
            // Открываем книгу
            var workbook = new XLWorkbook(xlsxpath);
            // Берем в ней первый лист
            var worksheet = workbook.Worksheets.Worksheet(1);
            // Перебираем диапазон нужных строк
            int countsob = workbook.Worksheets.Count();
            for (int row = 1; row <= 17803; ++row)
            {
                // По каждой строке формируем объект
                var xlsGrupDiagnozs = new GrDiagnozXls
                {
                    icdGrA = worksheet.Cell(row, 1).GetValue<string>(),
                    icdGrB = worksheet.Cell(row, 2).GetValue<string>(),
                    icdGrC = worksheet.Cell(row, 3).GetValue<string>(),
                    icdGrD = worksheet.Cell(row, 4).GetValue<string>(),
                    icdGrE = worksheet.Cell(row, 5).GetValue<string>(),
                    nameGrDiagnoz = worksheet.Cell(row, 6).GetValue<string>(),
                };

                // И возвращаем его
                yield return xlsGrupDiagnozs;
            }

        }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                loadPathTab = true;
                return true;
            }
            return false;
        }


        private RelayCommand? setPathComand;
        public RelayCommand SetPathComand
        {
            get
            {
                return setPathComand ??
                  (setPathComand = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      RegStatusUser = "Адміністратор";
                      if (boolSetAccountUser == false)
                      {
                          if (RegSetAccountUser() == false) return;
                      }
                      loadPathTab = false;
                      var dlg = new System.Windows.Forms.FolderBrowserDialog();
                      dlg.Description = "Шлях розташування архівних файлів";
                      System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                      if (result == System.Windows.Forms.DialogResult.OK)
                      {
                         FilePath = dlg.SelectedPath + @"\";
                         WindowUpload.UploadBdTextBox.Text = FilePath.ToString();
                         WindowUnload.UpTabl.Content= listtablbd[0];
                      }
                  }));
            }
        }
        // команда выбора отдельной таблицы для загрузки
        private RelayCommand? setPathTabComand;
        public RelayCommand SetPathTabComand
        {
            get
            {
                return setPathTabComand ??
                  (setPathTabComand = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      RegStatusUser = "Адміністратор";
                      if (boolSetAccountUser == false)
                      {
                          if (RegSetAccountUser() == false) return;
                      }

                      if(OpenFileDialog() == false) return;
                      WindowUpload.TabUploadBdTextBox.Text = FilePath.ToString();
                      
                      int countSob = listtablbd.Count();
                      string xlxTabl = FilePath.Substring(FilePath.LastIndexOf(@"\")+1, FilePath.LastIndexOf(".")-(FilePath.LastIndexOf(@"\") + 1));
                      for (int index = 0; index < countSob; index++)
                      {
                          if (listtablbd[index].ToUpper().Contains(xlxTabl.ToUpper()) == true)
                          {
                              itemtable = index;
                              break;
                          }
                      }
                      if (itemtable == 0)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ім'я вказаного файлу не входить до складу імен таблиць БД.";
                          SelectedMessageOk(5);
                      }
                      WindowUnload.UpTabl.Content = listtablbd[itemtable];
                  }));
            }
        }


        // Команда  загрузки файлов  в БД
        private RelayCommand? upLoadComand;
        
        public RelayCommand UpLoadComand
        {
            get
            {
                return upLoadComand ??
                  (upLoadComand = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      if (boolSetAccountUser == true)
                      {
                          WindowUpload.LoadButton.Width += 30;
                          WindowUpload.LoadLabel.Width += 30;
                          WindowUpload.LoadButton.Background = Brushes.Green;
                          WindowUpload.LoadLabel.Background = Brushes.Green;
                          WindowUpload.LoadLabel.Foreground = Brushes.Wheat;
                          WindowUpload.LoadLabel.Content = "Завантажується";
                          WinStrokaProgressBar ProgressBar = new WinStrokaProgressBar();
                          ProgressBar.Show();
                          // загрузка екселевского файла SOB справочник областей в БД
                          if (loadPathTab == true)
                          {
                              if (itemtable == 0)
                              {
                                  MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ім'я вказаного файлу не входить до складу імен таблиць БД.";
                                  SelectedMessageOk(5);
                                  return;
                              }
 
 
                              //ProgressBar.Close();

                              string icdGrA = "", icdGrB = "", icdGrC = "", icdGrD = "", icdGrE = "";
                              DeleteId.Deleteid(controler[itemtable], deldstroka[itemtable]);

                              if (FilePath.Contains("Sob") == true)
                              { 
                                  var xlsTabls = EnumerateSobs(FilePath);
                                  foreach (var Tabl in xlsTabls)
                                  {
                                      json = JsonConvert.SerializeObject(Tabl);
                                      CallServer.PostServer(controler[itemtable], json, "POST");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      json = CallServer.ResponseFromServer;
                                      if (json.Contains("[]") == true)
                                      {
                                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Виникла помилка при завантажені записів в таблицю." + Environment.NewLine + FilePath;
                                          SelectedFalseLogin(4);
                                      }
                                  }
                              }
                              if (FilePath.Contains("GrupDiagnoz") == true )
                              {
                                  var xlsTabls = EnumerateGrupDiagnozs(FilePath);
                                  foreach (var Tabl in xlsTabls)
                                  {
                                      if (Tabl.nameGrDiagnoz == "") break;
                                      if (icdGrB != Tabl.icdGrB && Tabl.icdGrB != "")
                                      {
                                          icdGrB = Tabl.icdGrB + ".";
                                          icdGrC = "";
                                          icdGrD = "";


                                      }
                                      if (icdGrC != Tabl.icdGrC && Tabl.icdGrC != "")
                                      {
                                          icdGrC = Tabl.icdGrC + ".";
                                          icdGrD = "";

                                      }
                                      if (Tabl.icdGrB != "" || Tabl.icdGrC != "")
                                      {
                                              GrupDiagnoz grupDiagnoz = new GrupDiagnoz();
                                              grupDiagnoz.icdGrDiagnoz = icdGrB + icdGrC;
                                              grupDiagnoz.nameGrDiagnoz = Tabl.nameGrDiagnoz;
                                              grupDiagnoz.opisDiagnoza = "";
                                              grupDiagnoz.uriDiagnoza = "";

                                              json = JsonConvert.SerializeObject(grupDiagnoz);
                                              CallServer.PostServer(controler[itemtable], json, "POST");
                                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                              json = CallServer.ResponseFromServer;
                                              if (json.Contains("[]") == true)
                                              {
                                                  MainWindow.MessageError = "Увага!" + Environment.NewLine + "Виникла помилка при завантажені записів в таблицю." + Environment.NewLine + FilePath;
                                                  SelectedFalseLogin(4);
                                              }
                                          WindowUpload.LineUpLoad.Width += 0.25;
                                          ProgressBar = new WinStrokaProgressBar();
                                          ProgressBar.Show();
                                          ProgressBar.Close();
                                      }
                                      
                                  }
 

                                  //WindowUpload.LoadButton.Background = 
                              }
                              if ( FilePath.Contains("Icd") == true)
                              {
                                  var xlsTabls = EnumerateGrupDiagnozs(FilePath);
                                  foreach (var Tabl in xlsTabls)
                                  {
                                      if (Tabl.nameGrDiagnoz == "") break;
                                      if (icdGrA != Tabl.icdGrA && Tabl.icdGrA != "")
                                      {
                                          icdGrA = Tabl.icdGrA + ".";
                                          icdGrB = "";
                                          icdGrC = "";
                                          icdGrD = "";
                                          
                                      }
                                      if (icdGrB != Tabl.icdGrB && Tabl.icdGrB != "")
                                      { 
                                          icdGrB = Tabl.icdGrB + ".";
                                          icdGrC = "";
                                          icdGrD = "";
                                         

                                      }
                                      if (icdGrC != Tabl.icdGrC && Tabl.icdGrC != "")
                                      {
                                          icdGrC = Tabl.icdGrC + ".";
                                          icdGrD = "";
                                         
                                      }
                                      if (icdGrD != Tabl.icdGrD && Tabl.icdGrD != "")
                                      {
                                          icdGrD = Tabl.icdGrD + ".";
                                          
                                      }
                
                                      ModelIcd modelIcd = new ModelIcd();
                                      modelIcd.name = Tabl.nameGrDiagnoz;
                                      modelIcd.keyIcd = icdGrA + icdGrB + icdGrC + icdGrD + icdGrE;
                                      json = JsonConvert.SerializeObject(modelIcd);
                                      CallServer.PostServer(controler[itemtable], json, "POST");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      json = CallServer.ResponseFromServer;
                                      if (json.Contains("[]") == true)
                                      {
                                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Виникла помилка при завантажені записів в таблицю." + Environment.NewLine + FilePath;
                                          SelectedFalseLogin(4);
                                      }
                                  }
                              }
                             

                          }
                          else
                          { 
                              string[] fileLines = { };
                              int itemtable = 0;
                              UpLoadDb upLoadDb = new UpLoadDb();
                              BaseUpload[] arrayUpload = { new UploadComplaint(), new UploadFeature(), new UploadDetailing(),
                                new UploadListGrDetailing(), new UploadGrDetailing(), new UploadListGroupQualification(),
                                new UploadQualification(), new UploadDiagnoz(), new UploadRecommendation(), new UploadInterview(), new UploadContentInterv(),
                                new UploadColectionInterview(), new UploadCompletedInterview(), new UploadDependencyDiagnoz(), new UploadIcd(),
                                new UploadMedicalInstitution(), new UploadDoctor(), new UploadPacient(), new UploadAccountUser(), new UploadNsiStatusUser(),
                                new UploadSob(), new UploadGrupDiagnoz(),new UploadMedGrupDiagnoz(), new UploadLikarGrupDiagnoz()};
                              foreach (var item in arrayUpload)
                              {
                                  OutFile = FilePath + listtablbd[itemtable] + ".json";
                                  if ((System.IO.File.Exists(OutFile)))
                                  {

                                      DeleteId.Deleteid(controler[itemtable], deldstroka[itemtable]);
                                      
                                      Encoding code = Encoding.Default;
                                      fileLines = System.IO.File.ReadAllLines(OutFile, code);
                                      foreach (string stroka in fileLines)
                                      {
                                          upLoadstroka = stroka;
                                          upLoadDb.FireUpLoad(item);
                                          CallServer.PostServer(controler[itemtable], strokajson, Method);
                                      }
                                      WindowUnload.UpTabl.Content = listtablbd[itemtable+1];
                                      WindowUnload.LineUpLoad.Width += 10;
                                      ProgressBar = new WinStrokaProgressBar();
                                      ProgressBar.Show();
                                      ProgressBar.Close();
                                      //MainWindow.MessageError = "Увага!" + Environment.NewLine + "Завантаження таблиці: "+ listtablbd[itemtable] + Environment.NewLine+ " завершено.";
                                      //SelectedMessageOk(1);
                                  }
                                  itemtable++;
                                  
                              }
                              WindowUnload.UpTabl.Content = "";


                          }
                          WindowUpload.LoadButton.Background = WindowUpload.LoadCopy.Background;
                          WindowUpload.LoadLabel.Background = WindowUpload.LoadCopy.Background;
                          WindowUpload.LoadLabel.Foreground = Brushes.Black;
                          WindowUpload.LoadLabel.Content = "Завантажити";
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Завантаження бази даних завершено!.";
                          SelectedMessageOk(4);
                      }
                      else
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не зареєстровані як Адміністратор!.";
                          SelectedMessageOk(4);
                      }
                  }));
            }
        }

        public class UpLoadDb
        {
            public void FireUpLoad(BaseUpload baseUpload)
            {
                baseUpload.UploadTable();
            }
        }

        public abstract class BaseUpload
        {
             // абстрактный метод
            public abstract void UploadTable();
        }

        public class DeleteId
        {
            public static void Deleteid(string path = "", string stroka = "")
            {
                json = path + stroka;
                CallServer.PostServer(path, json, "DELETE");
                //if (CallServer.ResponseFromServer.Contains("[]") == true)
                //{
                //    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Виникла помилка при видалені записів в таблиці." + Environment.NewLine + path;
                //    SelectedFalseLogin(4);
                //}
 
                
            }
        }

        // Complaint 1
        public class UploadComplaint : BaseUpload
        {
            public override void UploadTable()
            {
                ModelComplaint result = JsonConvert.DeserializeObject<ModelComplaint>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result); 
            }
        }

        // Feature 2
        public class UploadFeature : BaseUpload
        {
            public override void UploadTable()
            {
                
                ModelFeature result = JsonConvert.DeserializeObject<ModelFeature>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Detailing 3
        class UploadDetailing : BaseUpload
        {
            public override void UploadTable()
            {
                ModelDetailing result = JsonConvert.DeserializeObject<ModelDetailing>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }
        // GrDetailing 4
        class UploadListGrDetailing : BaseUpload
        {
            public override void UploadTable()
            {
                ModelListGrDetailing result = JsonConvert.DeserializeObject<ModelListGrDetailing>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // GrDetailing 5 
        class UploadGrDetailing : BaseUpload
        {
            public override void UploadTable()
            {
                ModelGrDetailing result = JsonConvert.DeserializeObject<ModelGrDetailing>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }
        // ListGroupQualification 6
        class UploadListGroupQualification : BaseUpload
        {
            public override void UploadTable()
            {
                ModelGroupQualification result = JsonConvert.DeserializeObject<ModelGroupQualification>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }
        // Qualification 7
        class UploadQualification : BaseUpload
        {
            public override void UploadTable()
            {
                ModelQualification result = JsonConvert.DeserializeObject<ModelQualification>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }
        // Diagnoz 8
        class UploadDiagnoz : BaseUpload
        {
            public override void UploadTable()
            {
                ModelDiagnoz result = JsonConvert.DeserializeObject<ModelDiagnoz>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Recommendation 9
        class UploadRecommendation : BaseUpload
        {
            public override void UploadTable()
            {
                ModelRecommendation result = JsonConvert.DeserializeObject<ModelRecommendation>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Interview 10
        class UploadInterview : BaseUpload
        {
            public override void UploadTable()
            {
                ModelInterview result = JsonConvert.DeserializeObject<ModelInterview>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // ContentInterv 11
        class UploadContentInterv : BaseUpload
        {
            public override void UploadTable()
            {
                ModelContentInterv result = JsonConvert.DeserializeObject<ModelContentInterv>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // ColectionInterview 12
        class UploadColectionInterview : BaseUpload
        {
            public override void UploadTable()
            {
                ModelColectionInterview result = JsonConvert.DeserializeObject<ModelColectionInterview>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // CompletedInterview 13
        class UploadCompletedInterview : BaseUpload
        {
             public override void UploadTable()
            {
                ModelCompletedInterview result = JsonConvert.DeserializeObject<ModelCompletedInterview>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // DependencyDiagnoz 14
        class UploadDependencyDiagnoz : BaseUpload
        {
            public override void UploadTable()
            {
                ModelDependencyDiagnoz result = JsonConvert.DeserializeObject<ModelDependencyDiagnoz>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // DependencyDiagnoz 15
        class UploadIcd : BaseUpload
        {
            public override void UploadTable()
            {
                ModelIcd result = JsonConvert.DeserializeObject<ModelIcd>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // MedicalInstitution 16
        class UploadMedicalInstitution : BaseUpload
        {
            public override void UploadTable()
            {
                MedicalInstitution result = JsonConvert.DeserializeObject<MedicalInstitution>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Doctor 17
        class UploadDoctor : BaseUpload
        {
            public override void UploadTable()
            {
                ModelDoctor result = JsonConvert.DeserializeObject<ModelDoctor>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);

            }
        }

        // Pacient 18
        class UploadPacient : BaseUpload
        {
            public override void UploadTable()
            {
                ModelPacient result = JsonConvert.DeserializeObject<ModelPacient>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);

            }
        }

        // Pacient 19
        class UploadAccountUser : BaseUpload
        {
            public override void UploadTable()
            {
                AccountUser result = JsonConvert.DeserializeObject<AccountUser>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Pacient 20
        class UploadNsiStatusUser : BaseUpload
        {
            public override void UploadTable()
            {
                NsiStatusUser result = JsonConvert.DeserializeObject<NsiStatusUser>(upLoadstroka);
                result.id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }
        }

        // Pacient 21
        class UploadSob : BaseUpload
        {
            public override void UploadTable()
            {
                ModelSob result = JsonConvert.DeserializeObject<ModelSob>(upLoadstroka);
                result.Id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }

        }

        class UploadGrupDiagnoz : BaseUpload
        {
            public override void UploadTable()
            {
                ModelGrupDiagnoz result = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(upLoadstroka);
                result.Id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }

        }

        class UploadMedGrupDiagnoz : BaseUpload
        {
            public override void UploadTable()
            {
                ModelMedGrupDiagnoz result = JsonConvert.DeserializeObject<ModelMedGrupDiagnoz>(upLoadstroka);
                result.Id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }

        }

        class UploadLikarGrupDiagnoz : BaseUpload
        {
            public override void UploadTable()
            {
                ModelLikarGrupDiagnoz result = JsonConvert.DeserializeObject<ModelLikarGrupDiagnoz>(upLoadstroka);
                result.Id = 0;
                strokajson = JsonConvert.SerializeObject(result);
            }

        }
        #endregion

    }


}

