using Serveur.Controllers;
using Serveur.Controllers.Server;
using Serveur.Models;
using Serveur.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public static ClientsListView instance;
        private FrmMainController _frmMainController;
        private FrmListenerController _frmListenerController;
        private int selectedRow;
        private readonly object _clientsLock = new object();

        public ClientsListView()
        {
            instance = this;
            _frmMainController = new FrmMainController();
            // >> LISTENER >>
            _frmListenerController = new FrmListenerController();
            ListenerState.startListen = false;
            if (ListenerState.autoListen == true)
            {
                ListenerState.startListen = true;
                _frmListenerController.listen(ListenerState.listenPort, ListenerState.IPv6Support);
            }
            InitializeComponent();
            ClientMosaic.DvgUpdater += dgvUpdater;

            //var MyItem = new MyItem { Ip= "salut", Name="çava", AccType = "bien", Country="oupas?",  Os="vraiment", Status="bien"};
            //lvClients.Items.Add(new MyItem { Ip = "salut", Name = "çava", AccType = "bien", Country = "oupas?", Os = "vraiment", Status = "bien" });
        }

        private void testFrm(object sender, RoutedEventArgs e)
        {
            FrmRemoteDesktop frmRd = new FrmRemoteDesktop();
            frmRd.ShowDialog();
        }

        // System Event
         

        private void SysInfoMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void FileMgMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void TaskMgMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void StartupMgMenuItem(object sender, RoutedEventArgs e)
        {

        }

        private void RunasMenuItem(object sender, RoutedEventArgs e)
        {

        }

        // Spying Event










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
                            break;
                        case false:
                            removeClientFromListView(client.Key);
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
                //ListViewItem lvi = new ListViewItem(new string[]
                //{
                //    client.endPoint.ToString().Split(':')[0], client.value.name,
                //    client.value.accountType, client.value.country, client.value.operatingSystem, "Connected"
                //})
                //{ Tag = client };


                lvClients.Dispatcher.BeginInvoke(new Action(delegate
                {
                    lock (_lockClients)
                    {
                        lvClients.Items.Add(new MyItem { Ip = client.endPoint.ToString().Split(':')[0], Name = client.value.name, AccType = client.value.accountType,
                            Country = client.value.country, Os = client.value.operatingSystem, Status = "Connected" });
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
                        foreach (ListViewItem lvi in lvClients.Items.Cast<ListViewItem>()
                            .Where(lvi => lvi != null && client.Equals(lvi.Tag)))
                        {
                           // lvi.Remove();
                            break;
                        }
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }


            //statusBarListening.Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    //lblListening.Text = status;
            //}));

        }

        /// :: GET CLIENT FROM DATAGRIDVIEW :: ///
        //public ClientMosaic getClient()
        //{
        //    //return (ClientMosaic)lvClients.SelectedItems[0].Tag;
        //}

    }

    public class MyItem
    {
        public string Ip { get; set; }

        public string Name { get; set; }

        public string AccType { get; set; }

        public string Country { get; set; }

        public string Os { get; set; }

        public string Status { get; set; }

    }
}
