using Serveur.Controllers;
using Serveur.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MosaicServeur
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private int _numValue = 0;

        private FrmListenerController _frmListenerController;

        public Settings(FrmListenerController frmListenerController)
        {
           _frmListenerController = frmListenerController;
            InitializeComponent();
            txtPort.Text = _numValue.ToString();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            txtPort.Text = ListenerState.listenPort.ToString();

            if (ListenerState.startListen == true)
            {
                btnListen.Content = "Stop listening";
            }

            chkStartupConnections.IsChecked = ListenerState.autoListen;
            chkPopupNotification.IsChecked = ListenerState.showPopup;
            chkIPv6.IsChecked = ListenerState.IPv6Support;
        }

        private void btnListening(object sender, RoutedEventArgs e)
        {
            if (btnListen.Content.ToString() == "Start listening")
            {
                ListenerState.listenPort = Convert.ToInt32(txtPort.Text);
                ListenerState.startListen = true;
                _frmListenerController.listen(int.Parse(txtPort.Text), chkIPv6.IsChecked.Value);
                btnListen.Content = "Stop listening";
            }
            else
            {
                ListenerState.startListen = false;
                _frmListenerController.stopListening();
                btnListen.Content = "Start listening";
            }
        }

        private void btnSave(object sender, RoutedEventArgs e)
        {
            ListenerState.autoListen = chkStartupConnections.IsChecked.Value;
            ListenerState.showPopup = chkPopupNotification.IsChecked.Value;
            ListenerState.IPv6Support = chkIPv6.IsChecked.Value;
        }

        //  NumPort EVENT
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtPort.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPort == null)
            {
                return;
            }

            if (!int.TryParse(txtPort.Text, out _numValue))
                txtPort.Text = _numValue.ToString();
        }
    }
}
