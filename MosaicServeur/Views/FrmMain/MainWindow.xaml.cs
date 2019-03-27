using MosaicServeur.Main;
using Serveur.Controllers;
using Serveur.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MosaicServeur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FrmListenerController _frmListenerController;
        private ClientsListView _clientsListView;
        private Builder _builder;
        private Settings _settings;
        public static MainWindow instance;
        BrushConverter bc = new BrushConverter();

        public MainWindow()
        {
            instance = this;
            InitializeComponent();            
            GridSettings.Children.Clear();
            GridSettings.Children.Add(_clientsListView = new ClientsListView());
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            // >> Listener >>
            _frmListenerController = new FrmListenerController();
            ListenerState.startListen = false;
            if (ListenerState.autoListen == true)
            {
                ListenerState.startListen = true;
                _frmListenerController.listen(ListenerState.listenPort, ListenerState.IPv6Support);
            }
            // >> UserControls >>
            _settings = new Settings(_frmListenerController);
            _builder = new Builder();
        }
        
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            ItemHome.Background     = (Brush)bc.ConvertFrom("#FF222222");
            ItemSettings.Background = (Brush)bc.ConvertFrom("#FF222222");
            ItemBuilder.Background  = (Brush)bc.ConvertFrom("#FF222222");


            switch (index)
            {
                case 0:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(_clientsListView);
                    ItemHome.Background = (Brush)bc.ConvertFrom("#4A9EF5");
                    break;
                case 1:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(_settings);
                    ItemSettings.Background = (Brush)bc.ConvertFrom("#4A9EF5");
                    break;
                case 2:
                    GridSettings.Children.Clear();
                    GridSettings.Children.Add(_builder);
                    ItemBuilder.Background = (Brush)bc.ConvertFrom("#4A9EF5");
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
