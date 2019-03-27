using MosaicServeur.Main;
using Serveur.Controllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MosaicServeur
{
    /// <summary>
    /// Logique d'interaction pour Builder.xaml
    /// </summary>
    public partial class Builder : UserControl
    {
        private FrmBuilderController buildercontroller = new FrmBuilderController();

        public Builder()
        {
            InitializeComponent();
            txtMutex.Text = buildercontroller.getUniqueMutex(18);
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            var i = (ClientsListView.connectedClients() + 1).ToString();
            txtClientTag.Text = string.Format("Client {0}", int.Parse(i) < 10 ? "0" + i : i);
            lblLogDir.Foreground = new SolidColorBrush(Colors.LightGray);
            txtLogDir.IsEnabled = false;
            chkKeyLogger.IsChecked = false;
            chkDirHidden.IsEnabled = false;
        }

        private void btnTabControl(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(0 + (150 * index), 20, 0, 0);

            switch (index)
            {
                case 0:
                    tcSample.SelectedIndex = 0;
                    break;
                case 1:
                    tcSample.SelectedIndex = 1;
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }       

        private void btnBuild(object sender, RoutedEventArgs e)
        {
            buildercontroller.create_stub(txtHost.Text, txtPort.Text, txtMutex.Text, txtRecoTries.Text);
        }

        private void btnMutex(object sender, RoutedEventArgs e)
        {
            txtMutex.Text = buildercontroller.getUniqueMutex(18);
        }

        private void chkKeyLoggerEvent(object sender, RoutedEventArgs e)
        {
            if (chkKeyLogger.IsChecked.Value == true)
            {
                txtLogDir.IsEnabled = true;
                chkDirHidden.IsEnabled = true;
                lblLogDir.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                txtLogDir.IsEnabled = false;
                chkDirHidden.IsEnabled = false;
                chkDirHidden.IsChecked = false;
                lblLogDir.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}
