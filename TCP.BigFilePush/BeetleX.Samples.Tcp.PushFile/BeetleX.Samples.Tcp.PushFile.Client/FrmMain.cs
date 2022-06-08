using BeetleX.Buffers;
using BeetleX.Clients;
using BeetleX.Samples.Tcp.PushFile.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeetleX.Samples.Tcp.PushFile.Client
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private AsyncTcpClient mClient;

        private DateTime mStart;

        private void cmdSelectFile_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtError.Text = "";
                    string filename = openFileDialog1.FileName;
                    var reader = new FileReader(filename);
                    mClient["file"] = reader;
                    var block = reader.Next();
                    block.Completed = OnCompleted;
                    mClient.Send(block);
                    cmdSelectFile.Enabled = false;
                    progressBar1.Maximum = reader.Pages;
                    mStart = DateTime.Now;
                }
            }
            catch (Exception e_)
            {
                MessageBox.Show(e_.Message);
            }
        }

        private void OnCompleted(FileContentBlock e)
        {
            this.BeginInvoke(new Action(() =>
            {

                var reader = (FileReader)mClient["file"];
                txtError.ForeColor = Color.Green;
                if (!reader.Completed)
                {
                    Task.Run(() =>
                    {
                        var block = reader.Next();
                        block.Completed = OnCompleted;
                        mClient.Send(block);
                    });
                    txtError.Text = "传输中...";
                }
                else
                {
                  
                    txtError.Text = $"文件传输完成！[{DateTime.Now-mStart}]";
                    cmdSelectFile.Enabled = true;
                }
                progressBar1.Value = reader.Index;
            }));

        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            BufferPool.BUFFER_SIZE = 1024 * 8;
            mClient = SocketFactory.CreateClient<AsyncTcpClient, ProtobufClientPacket>("localhost", 9090);
            mClient.ClientError = (o, err) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    txtError.ForeColor = Color.Red;
                    txtError.Text = err.Error.Message;
                    cmdSelectFile.Enabled = true;
                }));
            };
        }
    }
}
