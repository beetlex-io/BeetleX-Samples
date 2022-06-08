using BeetleX.EventArgs;
using BeetleX.Samples.Tcp.PushFile.Messages;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography.Xml;

namespace BeetleX.Samples.Tcp.PushFile
{
    class Program : ServerHandlerBase
    {
        static IServer mServer;

        static ConcurrentDictionary<string, FileTransfer> mFiles = new ConcurrentDictionary<string, FileTransfer>(StringComparer.OrdinalIgnoreCase);

        static void Main(string[] args)
        {
            mServer = SocketFactory.CreateTcpServer(new Program(), new ProtobufPacket());
            mServer.Options.LogLevel = LogType.Warring;
            mServer.Options.BufferSize = 1024 * 8;
            mServer.Open();
            System.Threading.Thread.Sleep(-1);
        }

        protected override void OnReceiveMessage(IServer server, ISession session, object message)
        {
            if (message is FileContentBlock block)
            {
                mFiles.TryGetValue(block.FileName, out FileTransfer value);
                if (block.Index == 0)
                {
                    if (value != null)
                    {
                        value.Dispose();
                    }
                    value = new FileTransfer(block.FileName);
                    mFiles[block.FileName] = value;
                }
                value.Stream.Write(block.Data, 0, block.Data.Length);
                if (block.Eof)
                {
                    value.Dispose();
                    mFiles.TryRemove(block.FileName, out value);
                }
            }
            base.OnReceiveMessage(server, session, message);
        }
    }
}
