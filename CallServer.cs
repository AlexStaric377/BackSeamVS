using System;
using System.Windows;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.ServiceProcess;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;




namespace BackSeam
{
    /// "Диференційна діагностика стану нездужання людини-SEAM" 
    /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
    public class CallServer
    {

        public static WebResponse result = null;
        public static WebRequest reqPOST = null;
        public static Stream sendStream = null;
        public static Stream ReceiveStream = null;
        public static StreamReader StrRead = null;
        public static string ResponseFromServer = "", ExMessage = "", UnloadString="";
        public static int ReturnCodeServer = 0;
        public static string UrlAdres = "", _UrlAdres = "", UrlAdresId = "";
        

        // Процедура обмена сообщением с кассовым сервером. 
        public static void PostServer(string Adres = "", string CmdStroka = "", string Method = "",int IdRec=0)
        {
            int indrestapiok = 0;
            bool restapiok = false;
            if (_UrlAdres == "") _UrlAdres = MapOpisViewModel.ConfigBuild();
            UrlAdres = _UrlAdres + Adres;
            UrlAdresId = _UrlAdres + CmdStroka;
            ResponseFromServer = "";
            ReturnCodeServer = 0;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UrlAdres);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            HttpResponseMessage response = null;
            while (restapiok == false)
            { 
               try
               {
                    switch (Method)
                    {
                        case "GET":
                            //MessageBox.Show(CmdStroka);
                            response = client.GetAsync(UrlAdres).Result;
                            break;
                        case "GETID":
                            response = client.GetAsync(UrlAdresId).Result;
                            break;
                        case "POST":
                            var stringContent = new StringContent(CmdStroka, Encoding.UTF8, "application/json");
                            response = client.PostAsync(UrlAdres, stringContent).Result;
                            break;
                        case "PUT":
                            stringContent = new StringContent(CmdStroka, Encoding.UTF8, "application/json");
                            response = client.PutAsync(UrlAdres, stringContent).Result;
                        
                            break;
                        case "DELETE":
                            response = client.DeleteAsync(CmdStroka).Result;
                            break;
 
                    }
                    restapiok = true;
               }
               catch
               {
                    indrestapiok++;
                    if (indrestapiok >= 5)
                    {
                        if (response == null)
                        {
                            FalseServerGet(UrlAdresId);
                            ResponseFromServer = "[]";
                        }
                        else
                        {
                            string SuccessCode = response.IsSuccessStatusCode.ToString();
                            string Headers = response.Headers.ToString();
                            string Request = response.RequestMessage.ToString();
                            string Reason = response.ReasonPhrase.ToString();
                            decimal status = ((decimal)response.StatusCode);
                            MainWindow.TextName = status.ToString();
                            ResponseFromServer = response.Content.ReadAsStringAsync().Result;
                            FalseServerGet("Код завершення: " + MainWindow.TextName + " Рядок данних: " + UrlAdresId);
                        }
                        restapiok = true;
                        return;
                    }
 
                    //Environment.Exit(0);
               }            
            }
            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    ResponseFromServer = response.Content.ReadAsStringAsync().Result;
                    ResponseFromServer = ResponseFromServer.Contains("[") ? ResponseFromServer : "[" + ResponseFromServer + "]";
                }
                else
                {
                    string SuccessCode = response.IsSuccessStatusCode.ToString();
                    string Headers = response.Headers.ToString();
                    string Request = response.RequestMessage.ToString();
                    string Reason = response.ReasonPhrase.ToString();
                    decimal status = ((decimal)response.StatusCode);
                    MainWindow.TextName = status.ToString();
                    ResponseFromServer = "[]";
                    if (MainWindow.TextName.Contains("404") == false)
                    {
                        FalseServerGet("Код завершення: " + MainWindow.TextName + " Рядок данних: " + UrlAdresId);
                        //Environment.Exit(0);
                    }
                }
            }
            client.Dispose();
        }

 

        public static string ServerReturn()
        {
            int startindex = ResponseFromServer.Contains("value") ? ResponseFromServer.IndexOf("value") + 7 : 0;
            int lengthstr = startindex == 0 ? ResponseFromServer.Length : ResponseFromServer.Length - startindex - 1;
            string CmdStroka = "{\"list\":" + ResponseFromServer.Substring(startindex, lengthstr) + "}";
            return CmdStroka;
        }

        public static void BoolFalseTabl()
        {
            if (MapOpisViewModel.boolVisibleMessage == true) return;
            MainWindow.MessageError = "Завантаженний довідник не містить записів" + Environment.NewLine +
            "Для продовження роботи введіть інформаційні записи.";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 5);
            NewOrder.ShowDialog();

        }

        public static void FalseServerGet(string ErrStroka ="")
        {
            MainWindow.MessageError = "Сервер не відповідає, " + Environment.NewLine + "Похибка при зверненні." + Environment.NewLine +
             ErrStroka + Environment.NewLine + "Необхідно перевірити з'єднання з сервером або стан БД";
            MessageError NewOrder = new MessageError(MainWindow.MessageError, 2, 10);
            NewOrder.ShowDialog();
            
        }
    }
}
