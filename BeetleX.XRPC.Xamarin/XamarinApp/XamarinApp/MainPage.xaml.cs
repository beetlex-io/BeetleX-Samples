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
            mClient = new BeetleX.XRPC.Clients.XRPCClient("192.168.1.18", 9090);
            mClient.Options.ParameterFormater = new JsonPacket();
            mUser = mClient.Create<IUser>();
            mClient.AddDelegate<Func<Task<string>>>(() =>
            {
                return Task.FromResult($"{Environment.OSVersion} {DateTime.Now}");
            });
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
            while (true)
            {
                var result = await mGetTime();
                txtResult.Text = result;
                await Task.Delay(1000);
            }
        }
    }
}
