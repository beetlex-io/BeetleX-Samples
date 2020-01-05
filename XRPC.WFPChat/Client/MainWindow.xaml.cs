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
    public partial class MainWindow : Window, IUser
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Task Login(string name)
        {
            AddMessage(name, "login");
            return Task.CompletedTask;
        }

        public Task Exit(string name)
        {
            AddMessage(name, "exit");
            return Task.CompletedTask;
        }

        public Task Talk(string name, string message)
        {
            AddMessage(name, message);
            return Task.CompletedTask;
        }

        private BeetleX.XRPC.Clients.XRPCClient mClient;

        private IUser mUser;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mClient = new BeetleX.XRPC.Clients.XRPCClient("192.168.2.18", 9090);
            mClient.Options.ParameterFormater = new JsonPacket();
            mClient.Register<IUser>(this);
            mUser = mClient.Create<IUser>();
            txtMessages.Document.Blocks.Clear();
        }

        private void AddMessage(string name, string message)
        {
            this.Dispatcher.Invoke(() =>
            {
                Paragraph paragraph = new Paragraph();
                //run
                Run run = new Run() { Text = $" {name} ", Background = new SolidColorBrush(Colors.DarkCyan) };

                paragraph.LineHeight = 8;
                paragraph.Inlines.Add(run);
                run = new Run() { Text = $" [{DateTime.Now}]" };
                paragraph.Inlines.Add(run);
                txtMessages.Document.Blocks.Add(paragraph);

                paragraph = new Paragraph();
                paragraph.LineHeight = 8;
                run = new Run() { Text = $"   {message}" };
                paragraph.Inlines.Add(run);
                txtMessages.Document.Blocks.Add(paragraph);
                txtMessages.ScrollToEnd();
            });
        }

        private async void CmdLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("请输入登录名称!");
                    return;
                }
                await mUser.Login(txtName.Text);
                MessageBox.Show("登陆成功!");
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private async void CmdTalk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await mUser.Talk(null, txtTalk.Text);
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }


    }
}
