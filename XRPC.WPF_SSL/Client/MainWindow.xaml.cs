using BeetleX.XRPC.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = await dataService.List();
                lstEmployees.ItemsSource = data;
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private IDataService dataService;

        private XRPCClient XRPCClient;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XRPCClient = new XRPCClient("localhost", 9090, "test");
            XRPCClient.CertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
            XRPCClient.Options.ParameterFormater = new JsonPacket();
            dataService = XRPCClient.Create<IDataService>();
        }

        private async void CmdLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var token = await dataService.Login(txtName.Text, txtPwd.Text);
                txtToken.Content = token;
                ((IHeader)dataService).Header["token"] = token;
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
