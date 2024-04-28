using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;

namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class ViewModelInfoDocx
    {
        public static OpenFileDialog openFileDialog = new OpenFileDialog();
        string originalfilename = System.IO.Path.GetFullPath(openFileDialog.FileName);
        public  ViewModelInfoDocx()
            {
            openFileDialog.FileName = @"D:\BackSeam\Images\BdDbSeam.docx";
            if (openFileDialog.ShowDialog() == true )
                {
            // Open document 
        

                if (openFileDialog.CheckFileExists && new[] { ".docx", ".doc", ".txt", ".rtf" }.Contains(System.IO.Path.GetExtension(originalfilename).ToLower()))
                {
                    Microsoft.Office.Interop.Word.Application wordObject = new Microsoft.Office.Interop.Word.Application();
                    object File = originalfilename;
                    object nullobject = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.Word.Application wordobject = new Microsoft.Office.Interop.Word.Application();
                    wordobject.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
                    Microsoft.Office.Interop.Word._Document docs = wordObject.Documents.Open(ref File, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject, ref nullobject);
                    docs.ActiveWindow.Selection.WholeStory();
                    docs.ActiveWindow.Selection.Copy();

                    docs.Close(ref nullobject, ref nullobject, ref nullobject);
                    wordobject.Quit(ref nullobject, ref nullobject, ref nullobject);


                    MessageBox.Show("file loaded");
                }
        } 
  
}
  
    }
}
