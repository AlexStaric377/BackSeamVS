using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace BackSeam
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            if (!InstanceCheck())
            {
                // нажаловаться пользователю и завершить процесс
                Environment.Exit(0);
            }
        }

        // держим в переменной, чтобы сохранить владение им до конца пробега программы
        static Mutex? InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            var mutex = new Mutex(true, "BackSeam", out isNew);
            if (isNew)
                InstanceCheckMutex = mutex;
            else
                mutex.Dispose(); // отпустить mutex сразу
            return isNew;
        }
    }
}
