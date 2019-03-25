using MosaicServeur.Main;
using Serveur.Controllers;
using Serveur.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MosaicServeur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FrmListenerController _frmListenerController;
        private ClientsListView _clientsListView = new ClientsListView();
        public static MainWindow instance;

        public MainWindow()
        {
            instance = this;
            // >> LISTENER >>
            _frmListenerController = new FrmListenerController();
            ListenerState.startListen = false;
            if (ListenerState.autoListen == true)
            {
                ListenerState.startListen = true;
                _frmListenerController.listen(ListenerState.listenPort, ListenerState.IPv6Support);
            }
            // << LISTENER <<
            InitializeComponent();
            GridSettings.Children.Clear();
            GridSettings.Children.Add(new ClientsListView());
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(_clientsListView);
                    break;
                case 1:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(new Settings(_frmListenerController));
                    break;
                case 2:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(new Builder());
                    break;
                default:
                    break;
            }
        }

        public void setListeningStatus(string status)
        {           
            statusBarListening.Dispatcher.BeginInvoke(new Action(delegate
            {
                lblListening.Text = status;
            }));
        }
    }
}
