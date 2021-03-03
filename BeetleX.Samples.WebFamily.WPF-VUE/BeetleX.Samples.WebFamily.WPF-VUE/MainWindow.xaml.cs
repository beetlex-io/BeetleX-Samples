using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.VueExtend;
using BeetleX.WebFamily;
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

namespace BeetleX.Samples.WebFamily.WPF_VUE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Controller]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WebHost host;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            host = new WebHost();
            host.Setting(o =>
            {
                o.SetDebug();
                o.Port = 8082;
                o.LogLevel = EventArgs.LogType.Error;
                o.WriteLog = true;
                o.LogToConsole = true;
            })
            .Initialize((http,vue,rec) =>
            {
                //注册程序集中所有控制器
                //s.Register(typeof(MainWindow).Assembly);
                //把当前窗体注册为​控制器
                http.ActionFactory.Register(this);
                //注册Vue文件资源
                rec.AddAssemblies(typeof(MainWindow).Assembly);
                //s.GetWebFamily().AddScript("echarts.js"); //添加javascript文件
                //s.GetWebFamily().AddCss("website.css"); //添加css文件
                vue.Debug();
            }).Completed(s =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (webView != null && webView.CoreWebView2 != null)
                    {
                        webView.CoreWebView2.Navigate("http://localhost:8082/");
                    }
                });
            });
            host.Run(true);
        }

        public object Hello(string name)
        {
            return $"hello {name} {DateTime.Now}";
        }
    }
}
