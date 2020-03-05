using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
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

        XRPCClient mClient;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mClient = new XRPCClient("localhost", 9090);
            mClient.Options.ParameterFormater = new JsonPacket();
            mClient.AddDelegate<Action<DateTime>>(SetTime);
            comboEmployees.ItemsSource = from a in await mClient.Delegate<ListEmployees>()() select new { a.EmployeeID, Name = $"{a.FirstName} {a.LastName}" };
            comboxCustomer.ItemsSource = await mClient.Delegate<ListCustomers>()();
            lstOrders.ItemsSource = await mClient.Delegate<ListOrders>()(0, null);
        }

        private void SetTime(DateTime time)
        {
            this.Dispatcher.BeginInvoke(new Action<DateTime>(t =>
            {
                this.txtTime.Content = t.ToString();
            }), time);
        }

        private async void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            lstOrders.ItemsSource = await mClient.Delegate<ListOrders>()(
                comboEmployees.SelectedValue!=null?(int)comboEmployees.SelectedValue:0, 
                comboxCustomer.SelectedValue!=null?(string)comboxCustomer.SelectedValue:null
                );
        }
    }
}
