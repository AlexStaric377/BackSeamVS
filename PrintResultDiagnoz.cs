using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
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
// Управление вводом-выводом
using System.IO;
using System.IO.Compression;
using System.Threading;

using System.Data;
//using System.Management;
using System.ServiceProcess;

//using System.Drawing;
//using System.Drawing.Printing;
using System.Printing;
// Ссылка в проекте MSV2010 добовляется ...
//using System.Drawing.Text;

using System.Globalization;

namespace BackSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

        public static void PrintDiagnoz()
        {
            MapOpisViewModel.ModelCall = "";
            MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
            ViewModelCreatInterview.LoadCreatInterview();


            FlowDocument doc = CreateFlowDocument(); // new FlowDocument(new Paragraph(new Run("Результат опитування")));
            doc.Name = "FlowDoc";
            PrintDialog printDlg = new PrintDialog();

            // Create a FlowDocument dynamically.  
            bool? isPrinted = printDlg.ShowDialog();
            if (isPrinted != true) return;
               

            // Print a specific page range within the document.
            try
            {
                // Ширина страницы
                doc.PageWidth = printDlg.PrintableAreaWidth;
                doc.PagePadding = new Thickness(25);
                doc.ColumnGap = 0;
                doc.ColumnWidth = doc.PageWidth - doc.PagePadding.Left - doc.PagePadding.Right;

             

                // Создать IDocumentPaginatorSource из FlowDocument  

                IDocumentPaginatorSource idpSource = doc;

                // Вызов метода PrintDocument для отправки документа на принтер  

                printDlg.PrintDocument(idpSource.DocumentPaginator, "Документ");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        /// Этот метод создает динамический FlowDocument. Вы можете добавить к этому что угодно  
        /// FlowDocument, который вы хотите отправить на принтер  
        /// </резюме>  
        /// <возвраты></возвраты>  
        /// Метод CreateFlowDocument в листинге 2 создает и возвращает объект FlowDocument.
        /// Объекты Paragraph, Section, Underline представляют абзац, раздел и подчеркивание документа.
        /// Здесь вы можете добавить столько содержимого в свой FlowDocument.
        private static FlowDocument CreateFlowDocument()
        {
            // Создаем потоковый документ
            FlowDocument doc = new FlowDocument();
            // Создаем раздел 
            Section sec = new Section();
            // печать реузльтата диагностики и протокола орпроса
            if (PrintComplInterview != true)
            { 
                // Создаем первый абзац
                Paragraph p0 = new Paragraph(new Run(" " ));
                // Добавляем абзац в раздел       
                sec.Blocks.Add(p0);
                
                p0 = new Paragraph(new Run("Диференційна діагностика стану нездужання людини - SEAM"));
                p0.FontSize = 14;
                p0.FontStyle = FontStyles.Normal;
                p0.TextAlignment = TextAlignment.Center;
                p0.Foreground = Brushes.Black;
                sec.Blocks.Add(p0);

                p0 = new Paragraph(new Run("Інформація за результатом опитування " ));
                p0.FontSize = 14;
                p0.FontStyle = FontStyles.Normal;
                p0.FontWeight = FontWeights.Bold;
                p0.TextAlignment = TextAlignment.Center;
                p0.Foreground = Brushes.Black;
                sec.Blocks.Add(p0);
                p0 = new Paragraph(new Run( MapOpisViewModel._pacientName));
                p0.FontSize = 14;
                p0.FontStyle = FontStyles.Normal;
                p0.FontWeight = FontWeights.Bold;
                p0.TextAlignment = TextAlignment.Center;
                p0.Foreground = Brushes.Black;
                sec.Blocks.Add(p0);

                p0 = new Paragraph(new Run("Дата опитування: " + MapOpisViewModel.modelColectionInterview.dateInterview));
                p0.FontSize = 14;
                p0.FontStyle = FontStyles.Normal;
                p0.FontWeight = FontWeights.Bold;
                p0.TextAlignment = TextAlignment.Center;
                p0.Foreground = Brushes.Black;
                 // Добавляем абзац в раздел       
                sec.Blocks.Add(p0);

                Paragraph p1 = new Paragraph(new Run("Назва опитування: " + MapOpisViewModel.modelColectionInterview.nameInterview));
                p1.FontSize = 14;
                p1.FontStyle = FontStyles.Normal;
                p1.TextAlignment = TextAlignment.Left;
                p1.Foreground = Brushes.Black;
                p1.Margin = new Thickness(10);
                sec.Blocks.Add(p1);

                p1 = new Paragraph(new Run("Попередній діагноз: " + MapOpisViewModel.NameDiagnoz));
                p1.FontSize = 14;
                p1.Margin = new Thickness(10);
                sec.Blocks.Add(p1);
                p1 = new Paragraph(new Run("Рекомендації: " + MapOpisViewModel.NameRecomendaciya));
                p1.FontSize = 14;
                p1.Margin = new Thickness(10);
                sec.Blocks.Add(p1);

                p1 = new Paragraph(new Run("Опис попереднього діагнозу: " + MapOpisViewModel.OpistInterview));
                p1.FontSize = 14;
                p1.Margin = new Thickness(10);
                sec.Blocks.Add(p1);

                p1 = new Paragraph(new Run("Опис в інтернеті: " + MapOpisViewModel.UriInterview));
                p1.FontSize = 14;
                p1.Margin = new Thickness(10);
                sec.Blocks.Add(p1);

                // Добавляем раздел в FlowDocument 
                doc.Blocks.Add(sec);            
            }
            // печать только протокола орпроса

            Section sec1 = new Section();
            Paragraph p2 = new Paragraph(new Run("Протокол опитування"));

            p2.FontStyle = FontStyles.Normal;
            p2.FontWeight = FontWeights.Bold;
            p2.TextAlignment = TextAlignment.Center;
            p2.Foreground = Brushes.Black;
            sec1.Blocks.Add(p2);

            foreach (ModelContentInterv modelContentInterv in ViewModelCreatInterview.ContentIntervs)
            {
                p2 = new Paragraph(new Run(modelContentInterv.detailsInterview));
                p2.FontSize = 14;
                p2.Margin = new Thickness(10);
                p2.FontStyle = FontStyles.Normal;
                p2.TextAlignment = TextAlignment.Left;
                p2.Foreground = Brushes.Black;
                p2.FontWeight = FontWeights.Normal;
                sec1.Blocks.Add(p2);
            }
            Underline underline = new Underline();
            underline.Inlines.Add(new Run("Кінець інформації за результатом опитування.      Контактний телефон:         "));
            p2 = new Paragraph(new Run(" "));
            p2.Inlines.Add(underline);
            p2.FontSize = 14;
            p2.FontStyle = FontStyles.Normal;
            p2.FontWeight = FontWeights.Normal;
            p2.TextAlignment = TextAlignment.Left;
            p2.Foreground = Brushes.Black;
            sec1.Blocks.Add(p2);

            doc.Blocks.Add(sec1);
            return doc;
        }
    }
}
