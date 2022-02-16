using BeetleX;
using BeetleX.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TCP.Forward
{
    public class ForwardServer : BeetleX.ServerHandlerBase
    {
        private List<BeetleX.Clients.AsyncTcpClient> mClients = new List<BeetleX.Clients.AsyncTcpClient>();

        private BeetleX.IServer mServer;

        private Channel<ArraySegment<byte>> mBufferChannel;
        public ForwardServer(string host, int port)
        {
            ServerOptions option = new ServerOptions();
            option.DefaultListen.Host = host;
            option.DefaultListen.Port = port;
            mServer = BeetleX.SocketFactory.CreateTcpServer(this, null, option);
        }

        public async Task<ForwardServer> Push(string host, int port)
        {
            var clinet = BeetleX.SocketFactory.CreateClient<BeetleX.Clients.AsyncTcpClient>(host, port);
            await clinet.Connect();
            mClients.Add(clinet);
            return this;
        }

        public void Run()
        {
            mBufferChannel = Channel.CreateUnbounded<ArraySegment<byte>>();
            mServer.Open();
            Task.Run(OnReadChannel);
        }

        private async Task OnReadChannel() {
            var reader = mBufferChannel.Reader;
            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var data))
                {
                    foreach (var client in mClients) {
                        var pipestream = client.Stream.ToPipeStream();
                        pipestream.Write(data.Array, data.Offset, data.Count);
                        client.Stream.Flush();
                    }
                    System.Buffers.ArrayPool<byte>.Shared.Return(data.Array);
                }
            }
        }

        public override void SessionReceive(IServer server, SessionReceiveEventArgs e)
        {
            base.SessionReceive(server, e);
            var pipeStream = e.Stream.ToPipeStream();
            int length = (int)pipeStream.Length;
            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(length);
            pipeStream.Read(buffer, 0, length);
            mBufferChannel.Writer.WriteAsync(new ArraySegment<byte>(buffer, 0, length));
        }
    }
}
