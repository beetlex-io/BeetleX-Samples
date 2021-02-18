using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //定义XRPC SSL客户端
            mClient = new XRPCClient("192.168.1.18", 9090, "beetlex");
            mClient.CertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
            mClient.Options.ParameterFormater = new JsonPacket();
            //创建接口远程调用代理
            mUser = mClient.Create<IUser>();
            //定义委托给服务端调用
            mClient.AddDelegate<Func<Task<string>>>(() =>
            {
                return Task.FromResult($"{Environment.OSVersion} {DateTime.Now}");
            });
            //创建对应服务端的远程委托代理
            mGetTime = mClient.Delegate<Func<Task<string>>>();
        }

        private Func<Task<string>> mGetTime;

        private static IUser mUser;

        private static BeetleX.XRPC.Clients.XRPCClient mClient;

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(userName.Text))
                {
                    await DisplayAlert("Error", "Enter you name!", "OK");
                    return;
                }
                //登陆
                var result = await mUser.Login(userName.Text, userPwd.Text);
                this.layoutGrid.IsVisible = false;
                this.cmdLogin.IsVisible = false;
                OnGetTime();
            }
            catch (Exception e_)
            {
                await DisplayAlert("Error", e_.Message, "OK");
            }
        }

        private async Task OnGetTime()
        {
            try
            {
                //定时获取服务和时间信息
                while (true)
                {
                    var result = await mGetTime();
                    txtResult.Text = result;
                    await Task.Delay(1000);
                }
            }
            catch (Exception e_)
            {
                await DisplayAlert("Error", e_.Message, "OK");
                this.layoutGrid.IsVisible = true;
                this.cmdLogin.IsVisible = true;
            }

        }
    }
}
